using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCObjectManipulator : MonoBehaviour
{
     [SerializeField]
    private NPCMover _npcMover;
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private NPCSoundHandler _npcSoundHandler;
    [SerializeField]
    private NPCDataReceiver _npcDataReceiver;
    [SerializeField]
    private NPCEffectHandler _npcEffectHandler;
    private UIHandler _uiHandler;
    [SerializeField]
    ItemHandler _itemHandler;
    [SerializeField]
    private float _initialDestroyPower = 1.0f;
    public float InitialDestroyPower => _initialDestroyPower;
    private float _destroyPower;
    [SerializeField]
    private GameObject _predictCubes;
    private bool _hasBlock = false;
    public bool HasBlock => _hasBlock;
    private bool _animSwing = false;
    public bool AnimSwing => _animSwing;
    private Vector3 _insPos;
    private Vector3 _insBigPos;

    [SerializeField]
    GameObject[] _herosPrefab = new GameObject[2];
    [SerializeField]
    GameObject[] _bigPrefab = new GameObject[2];
    [SerializeField]
    GameObject[] _cPrefab = new GameObject[2];
    private Quaternion[] _insQuaternion = { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0) };
    private Vector3 _upPadding = new Vector3(0f,0.5f,0f);
    private bool _animBreak = false;
    public bool AnimBreak => _animBreak;
    private BlockBehaviour _currentBlock;
    [SerializeField] 
    private float _npcReach = 1.0f;
    [SerializeField]
    private NPCItemHandler _npcItemHandler;
    private ItemC _itemC;
     //public bool _hitBlock = false;
    void Start()
    {
        _destroyPower = _initialDestroyPower;
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
    }

    void Update()
    {
        if (!_myPV.isMine) return;
        if(!_npcDataReceiver.IsActiveGame) return;
      
        BreakBlock();
        CreateBlock();
        if (_hasBlock) 
        {
            _insPos = new Vector3((int)transform.position.x, (int)transform.position.y + 0.25f, -1.0f);
            _predictCubes.transform.position = _insPos;
        }
    }


    //オブジェクト生成
    public void CreateBlock()
    {
        //ブロックを持ってれば処理を行う
        if (!_hasBlock) return;
        if(_animSwing) return;
        //swingAnim再生
        _animSwing = true;
        _hasBlock = false;
        _predictCubes.SetActive(false);
        _insBigPos = _insPos;
        _insBigPos.y += 1.0f;
        _npcSoundHandler.CreateBlockSE();
        StartCoroutine(GenerateBlock());
    }

    private IEnumerator GenerateBlock()
    {
        yield return new WaitForSeconds(0.4f);
        _animSwing = false;
        //アイテムBを持っていたら巨大ブロック一回だけ生成
        if (_itemHandler._HasItemB && _npcItemHandler.UseItemState == 0)
        {
            //アイテムB微調整
            _myPV.RPC(nameof(SyncCreateBig), PhotonTargets.MasterClient, _insBigPos, _npcDataReceiver.MineTeamID, _npcDataReceiver.EnemyTeamID);
            _npcItemHandler.UsedItem();
            _itemHandler.ItemEffectB();
        }
        //ItemCBlock生成
        else if (_itemHandler._HasItemC && _npcItemHandler.UseItemState == 1)
        {
            _myPV.RPC(nameof(SyncCreateItemC), PhotonTargets.MasterClient, _insPos, _npcDataReceiver.MineTeamID, _npcDataReceiver.EnemyTeamID);
            _npcItemHandler.UsedItem();
            _itemHandler.ItemEffectC();
        }
        else
        {
            _myPV.RPC(nameof(SyncCreateHeros), PhotonTargets.MasterClient, _insPos, _npcDataReceiver.MineTeamID, _npcDataReceiver.EnemyTeamID);
        }
    }

    // ネットワーク上のキューブ生成
    [PunRPC]
    private void SyncCreateBig(Vector3 pos, int mineTeam, int enemyTeam)
    {
        GameObject insObj = PhotonNetwork.Instantiate(_bigPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        insObj.transform.parent = _npcDataReceiver.InsCubeParent[enemyTeam];
    }

    [PunRPC]
    private void SyncCreateItemC(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        GameObject insObj = PhotonNetwork.Instantiate(_cPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        insObj.transform.parent = _npcDataReceiver.InsCubeParent[enemyTeam];
    }

    [PunRPC]
    private void SyncCreateHeros(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        GameObject insObj = PhotonNetwork.Instantiate(_herosPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        insObj.transform.parent = _npcDataReceiver.InsCubeParent[enemyTeam];
    }

    //オブジェクト破壊
    public void BreakBlock()
    {
        CreateBlock();
        _animBreak = false;
        if (_hasBlock) return;
        _animBreak = true;
        Vector3 direction = transform.forward;
        direction.Normalize();
        Ray ray = new Ray(transform.position + _upPadding, direction);
        
        Debug.DrawRay(transform.position + _upPadding, direction, Color.red, 1.0f);

        if (!Physics.Raycast(ray, out RaycastHit hit, _npcReach)) return;

        _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
        if(_currentBlock == null) return;
        _currentBlock.DecreceGage();

        int objID = _currentBlock.DestroyBlock(_destroyPower);

        if (objID == UIHandler._ambrassID || objID == UIHandler._herosID)
        {
            //UIに保持しているブロックを表示する処理
            //_uiHandler.BlockImage(objID);
            if (hit.collider.CompareTag("ItemCBlock"))
            {
                SetItemC(hit.collider);
                ProcessItemCBlockEffect();
            }
            //ブロック破壊SE;
            _npcSoundHandler.BreakBlockSE();
            _predictCubes.SetActive(true);
            _hasBlock = true;
            if (!_itemHandler._HasItemA || !_itemHandler._HasItemB || !_itemHandler._HasItemC) 
            {
                //_itemHandler.StackBlock(objID);
                _itemHandler.CreateItem();
            }
        }
    }

    public void ProcessItemCBlockEffect()
    {
        int effectID = _itemHandler.ChoseEffectC();
        switch (effectID)
        {
            case 1:
                _npcMover.ChangeMoveSpeed(0.0f);
                //スタンエフェクト再生
                _npcEffectHandler.ChangeStan(true);
                Invoke("FinishItemC", _itemHandler._ItemCEffectTime);
                break;
            case 2:
                _itemC.Break4();
                break;
        }
    }

    public void SetItemC(Collider target)
    {
        _itemC = target.GetComponent<ItemC>();
    }

    void FinishItemC()
    {
        _npcMover.ChangeMoveSpeed(_npcMover.InitialSpeed);
        //スタンエフェクト停止
        _npcEffectHandler.ChangeStan(false);
    }

    // 移動速度の変更
    public void ChangeDestroyPower(float newPower)
    {
        _destroyPower = newPower;
    }
}

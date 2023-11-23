using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private AmbrasPoolHandler _ambrasPoolHandler;
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private float _playerSpeed;
    private float _usePlayerSpeed;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private float _jumprayrength;
    [SerializeField]
    private float _destroyPower = 1.0f;
    private float _useDestroyPower;
    //アイテムCブロックのプレハブ
    private GameObject _itemCBlock;
    [SerializeField]
    PoolHandler _poolHandler;

    [SerializeField]
    ItemHandler _itemHandler;
    [SerializeField] 
    ItemC _itemC;
    [SerializeField]
    GameObject _herosPrefab;
    [SerializeField]
    GameObject _bigPrefab;
    [SerializeField]
    GameObject _cPrefab;
    //private Transform _cubeParent;

    BlockBehaviour _currentBlock;
    HerosBehaviour _herosBehaviour,_bigBehaviour,_cBehaviour;
    private Rigidbody _rb;
    private bool _isJump,_isHead = false;
    private bool _hasBlock = false;
    private int _mineTeam,_enemyTeam;
    private bool _developMode = false;
    private WaitForSeconds _waitTime;
    private Transform[] _cubeParentTeam = new Transform[2];
    private Quaternion[] _insQuaternion = {Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0)};

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _usePlayerSpeed = _playerSpeed;
        _useDestroyPower = _destroyPower;
        _waitTime = new WaitForSeconds(_itemHandler._ItemAEffectTime);
    }

    public void SetParameter(Transform parent1, Transform parent2, int thisTeam, bool isDevelop)
    {
       
        _cubeParentTeam[0] = parent1;
        _cubeParentTeam[1] = parent2;
        _mineTeam = thisTeam;
        _enemyTeam = 1 - _mineTeam;
        _developMode = isDevelop;
        ChangeBlockID(thisTeam);
    }
    //IDからブロックの移動方向決定
    public void ChangeBlockID(int thisTeam)
    {
        HerosBehaviour _herosBehaviour = _herosPrefab.GetComponent<HerosBehaviour>();
        HerosBehaviour _bigBehaviour = _bigPrefab.GetComponent<HerosBehaviour>();
        HerosBehaviour _cBehaviour = _cPrefab.GetComponent<HerosBehaviour>();
       _herosBehaviour.SetID(thisTeam);
       _bigBehaviour.SetID(thisTeam);
       _cBehaviour.SetID(thisTeam);
    }

    void Update()
    {
        if (!_myPV.isMine && !_developMode) return;

        BreakBlock();
        CreateBlock();
        //アイテム生成
        _itemHandler.CreateItem();
        Item();
        Jump();
        JudgeDeath();     
    }
    void FixedUpdate()
    {
        if (!_myPV.isMine && !_developMode) return;
        PlayerCtrl();
    }

    //プレイヤーの移動
    void PlayerCtrl()
    {
        float inputX = Input.GetAxis("Horizontal");
        Debug.Log(inputX);
        if(inputX == 0) return;

        // カメラの方向を考慮して移動ベクトルを作成
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        Vector3 right = Camera.main.transform.right;
        right.y = 0.0f;
        right.Normalize();

        Vector3 movement = new Vector3(inputX, 0, 0); 

        _rb.MovePosition(transform.position + movement * _usePlayerSpeed * Time.deltaTime);

        // プレイヤーの向きを移動ベクトルに向ける
        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1080.0f * Time.deltaTime);

    }

    void JudgeDeath()
    {
        Ray ray = new Ray(transform.position ,new Vector3(0,_jumprayrength,0));
        RaycastHit _hitHead;
        if (Physics.Raycast(ray, out _hitHead,_jumprayrength)) _isHead =true;
        else _isHead = false;
        //頭のRayと足のRayが両方ぶつかっていたら死亡
        if(!_isJump && _isHead)
        {
            // Debug.Log("死亡");
        }
    }

    void Jump()
    {
        // Jump handling
        //後で綺麗にします
        Ray ray = new Ray(transform.position,new Vector3(0,-_jumprayrength,0));
        RaycastHit hit;
        Debug.DrawRay(transform.position,Vector3.down * _jumprayrength, Color.red, 0.1f); 
        if (Physics.Raycast(ray, out hit,_jumprayrength))  _isJump = false;
        else _isJump = true;

        if (Input.GetKeyDown(KeyCode.Space) && _isJump == false)
        {         
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            _isJump = true;
        }
    }

    //オブジェクト破壊
    public void BreakBlock()
    {
        if (_hasBlock || !Input.GetMouseButton(0)) return;

        Vector3 direction = transform.forward;
        direction.Normalize();
        Ray ray = new Ray(transform.position, direction);

        Debug.DrawRay(transform.position, ray.direction * 0.5f, Color.red, 1.0f);

        if (!Physics.Raycast(ray, out RaycastHit hit) || !IsBlock(hit.collider)) return;

        _currentBlock = hit.collider.GetComponent<BlockBehaviour>();

        if (hit.collider.CompareTag("ItemCBlock"))
        {
            _itemC = hit.collider.GetComponent<ItemC>();
            ProcessItemCBlockEffect();
        }

        int objID = _currentBlock.DestroyBlock(_useDestroyPower, _developMode);

        if (objID == 1 || objID == 2)
        {
            _hasBlock = true;
            _itemHandler.StackBlock(objID);
        }
    }

    private bool IsBlock(Collider collider)
    {
        return collider.CompareTag("Ambras") || collider.CompareTag("Heros") || collider.CompareTag("ItemCBlock");
    }

    private void ProcessItemCBlockEffect()
    {
        int effectID = _itemHandler.ChoseEffectC();

        switch (effectID)
        {
            case 1:
                _itemC.EffectStan(ref _usePlayerSpeed);
                Invoke("FinishItemC", _itemHandler._ItemCEffectTime);
                break;
            case 2:
                _itemC.Break4();
                break;
        }
    }


    //オブジェクト生成
    public void CreateBlock()
    {
        //ブロックを持ってれば処理を行う
        if (_hasBlock == false) return;
        if (!Input.GetMouseButtonDown(1)) return;

        Vector3 insPos = new Vector3((int)transform.position.x, (int)transform.position.y, -1.0f);
        Vector3 insBigPos = new Vector3((int)transform.position.x, (int)transform.position.y  + 0.75f, -1.0f);
        GameObject insObj;
        if (_developMode)
        {
            //アイテムBを持っていたら巨大ブロック一回だけ生成
            if (_itemHandler._HasItemB)
            {
                //アイテムB微調整
                insObj = Instantiate(_itemHandler._BigBlock, insBigPos, _insQuaternion[_mineTeam]);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
                _itemHandler.ItemEffectB();
            }
            //ItemCBlock生成
            else if(_itemHandler._HasItemC)
            {
                _itemHandler.ItemEffectC();
                insObj = Instantiate(_itemHandler._ItemCBlock, insPos, _insQuaternion[_mineTeam]);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
                // Debug.Log("せいせい");
            }
            else
            {
                insObj = Instantiate(_herosPrefab, insPos, _insQuaternion[_mineTeam]);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
            }
        }
        else 
        {
            //アイテムBを持っていたら巨大ブロック一回だけ生成
            if (_itemHandler._HasItemB)
            {
                //アイテムB微調整
                insObj = PhotonNetwork.Instantiate(_itemHandler._BigBlock.name, insBigPos, _insQuaternion[_mineTeam], 0);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
                _itemHandler.ItemEffectB();
            }
            //ItemCBlock生成
            else if(_itemHandler._HasItemC)
            {
                _itemHandler.ItemEffectC();
                insObj = PhotonNetwork.Instantiate(_itemHandler._ItemCBlock.name, insPos, _insQuaternion[_mineTeam], 0);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
            }
            else
            {
                insObj = PhotonNetwork.Instantiate(_herosPrefab.name, insPos, _insQuaternion[_mineTeam], 0);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
            }

        }
        // 仮置き
        _hasBlock = false;
    }

    //アイテムを使う
    public void Item()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (_itemHandler._HasItemA == true)
        {
            StartCoroutine(FinishItemA());
            _itemHandler.ItemEffectA(ref _useDestroyPower, ref _usePlayerSpeed);
        }
    }

    IEnumerator FinishItemA()
    {
        yield return _waitTime;
        _usePlayerSpeed = _playerSpeed;
        _useDestroyPower = _destroyPower;
    }

    void FinishItemC()
    {
        Debug.Log("スタン解除");
        _usePlayerSpeed = _playerSpeed;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
    // [SerializeField] 
    // ItemHandler _itemHandler;
    private GameObject _herosPrefab;
    //アイテムCブロックのプレハブ
    private GameObject _itemCBlock;

    [SerializeField]
    ItemHandler _itemHandler;
    [SerializeField] 
    ItemC _itemC;
    private Transform _cubeParent;

    BlockBehaviour _currentBlock;
    private Rigidbody _rb;
    private bool _isJump = false;
    private bool _hasBlock = false;

    private float _enemyPos;
    private bool _developMode = false;
    private WaitForSeconds _waitTime;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cubeParent = GameObject.FindWithTag("CubeParent").transform;
        _myPV = GetComponent<PhotonView>();
        _usePlayerSpeed = _playerSpeed;
        _useDestroyPower = _destroyPower;
        _waitTime = new WaitForSeconds(_itemHandler._ItemAEffectTime);
    }

    public void SetParameter(GameObject heros, bool isDevelop)
    {
        _herosPrefab = heros;
        _developMode = isDevelop;
    }

    void Update()
    {
        if (!_myPV.isMine && !_developMode) return;

        BreakBlock();
        CreateBlock();
        //マウスクリックでアイテム生成
        if (Input.GetMouseButtonDown(2)) _itemHandler.CreateItem();
        Item();
        Jump();     
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
        
        if(inputX == 0) return;

        // カメラの方向を考慮して移動ベクトルを作成
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        Vector3 right = Camera.main.transform.right;
        right.y = 0.0f;
        right.Normalize();

        Vector3 movement = inputX * right;

        _rb.MovePosition(transform.position + movement * _usePlayerSpeed * Time.deltaTime);

        // プレイヤーの向きを移動ベクトルに向ける
        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1080.0f * Time.deltaTime);
        

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
        //ブロックを持ってなければ処理を行う
        if (_hasBlock || !Input.GetMouseButton(0)) return;
        Vector3 direction = transform.forward; // プレイヤーの向いている方向を取得
        direction.Normalize();
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(transform.position, ray.direction * 1, Color.red, 1.0f); // 長さ３０、赤色で５秒間可視化
        if (Physics.Raycast(ray, out hit))
        {
            _currentBlock = null;

            if (hit.collider.CompareTag("Ambras") || 
                hit.collider.CompareTag("Heros") || 
                hit.collider.CompareTag("ItemCBlock"))
            {
                if (_currentBlock == null) _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
                if(hit.collider.CompareTag("ItemCBlock"))_itemC =  hit.collider.GetComponent<ItemC>();
                int _objID = _currentBlock.DestroyBlock(_useDestroyPower, _developMode);
                // objIDを利用してUI表示  
                if (_objID == 1 || _objID == 2)
                {
                    //ItemCBlockを破壊した際に効果発動
                    if(hit.collider.CompareTag("ItemCBlock")) 
                    {           
                        int _effectid = _itemHandler.ChoseEffectC();
                        switch(_effectid){
                            //敵スタン
                            case 1:
                            _itemC.EffectStan(ref _usePlayerSpeed);
                            Invoke("FinishItemC", _itemHandler._ItemCEffectTime);
                            break;
                            //周囲4マスのブロックを破壊
                            case 2:
                            _itemC.Break4();
                            break;
                        }
                        Debug.Log("Chakai");
                    }
                    _hasBlock = true;
                    _itemHandler.StackBlock(_objID);
                }
                return;
            }
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
                insObj = Instantiate(_itemHandler._BigBlock, insBigPos, Quaternion.identity, _cubeParent);
                _itemHandler.ItemEffectB();
            }
            else if(_itemHandler._HasItemC)
            {
                // insObj = Instantiate(_itemCBlock, insPos, Quaternion.identity, _cubeParent);
                // Debug.Log("せいせい");
            }
            else
            {
                insObj = Instantiate(_herosPrefab, insPos, Quaternion.identity, _cubeParent);
            }
        }
        else insObj = PhotonNetwork.Instantiate(_herosPrefab.name, insPos, Quaternion.identity, 0);
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

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

    [SerializeField]
    ItemHandler _itemHandler;
    private Transform _cubeParent;

    BlockBehaviour _currentBlock;

    //x.z軸方向の入力を保存
    private float _input_x, _input_z;
    private Rigidbody _rb;
    private bool _isJump = false;
    private bool _hasBlock = false;

    private float _enemyPos;
    private bool _developMode = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cubeParent = GameObject.FindWithTag("CubeParent").transform;
        _myPV = GetComponent<PhotonView>();
        _usePlayerSpeed = _playerSpeed;
        _useDestroyPower = _destroyPower;
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

        _input_x = Input.GetAxis("Horizontal");
        _input_z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(_input_x, 0.0f, _input_z);


        _rb.MovePosition(transform.position + movement * _usePlayerSpeed * Time.deltaTime);

        // Handle player rotation based on the input
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720.0f * Time.deltaTime);
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
        //ブロックを持ってなければ処理を行う
        if (_hasBlock == true) return;
        Vector3 direction = transform.forward; // プレイヤーの向いている方向を取得
        if (!Input.GetMouseButton(0)) return;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;
        Debug.DrawRay(transform.position, ray.direction * 30, Color.red, 1.0f); // 長さ３０、赤色で５秒間可視化
        if (!Physics.Raycast(ray, out hit))
        {
            _currentBlock = null;
            return;
        }

        if (hit.collider.CompareTag("Ambras") || hit.collider.CompareTag("Heros"))
        {
            if (_currentBlock == null) _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
            int _objID = _currentBlock.DestroyBlock(_useDestroyPower);
            // objIDを利用してUI表示  
            if (_objID == 1 || _objID == 2)
            {
                _hasBlock = true;
                _itemHandler.StackBlock(_objID);
            }
            return;
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
             Invoke("FinishItemA", _itemHandler._ItemAEffectTime);
        _itemHandler.ItemEffectA(ref _useDestroyPower, ref _usePlayerSpeed);
        }
        else if(_itemHandler._HasItemC == true)
        {
            _itemHandler.ChoseEffectC(ref _usePlayerSpeed);
            //_itemHandler.ItemEffectC(ref _usePlayerSpeed);
             //スタン時間
            //Invoke("FinishItemC", _itemHandler._ItemCEffectTime);
        }
    }

    void FinishItemA()
    {
        _usePlayerSpeed = _playerSpeed;
        _useDestroyPower = _destroyPower;
    }

    void FinishItemC()
    {
        Debug.Log("スタン解除");
        _usePlayerSpeed = _playerSpeed;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PhotonView _myPV;
    [SerializeField] 
    private float _playerSpeed;
    [SerializeField] 
    private float _jumpPower;
    [SerializeField]
    private float _destroyPower = 1.0f;
    // [SerializeField] 
    // ItemHandler _itemHandler;
    [SerializeField] 
    GameObject _herosPrefab;

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
    }

    public void SetParameter(float posZ, bool isDevelop)
    {
        _enemyPos = posZ;
        _developMode = isDevelop;
    }

    void Update()
    {
        if (!_myPV.isMine) return;

        BreakBlock();
        CreateBlock();
        
        if (Input.GetMouseButtonDown(2)) _itemHandler.CreateItem();
        if (Input.GetMouseButtonDown(0)) _itemHandler.UseItem();

        // Jump handling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);            
        }
    }

    void FixedUpdate()
    {
        PlayerCtrl();
    }

    //プレイヤーの移動
    void PlayerCtrl()
    {
        
        _input_x = Input.GetAxis("Horizontal");
        _input_z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(_input_x, 0.0f, _input_z);
 

        _rb.MovePosition(transform.position + movement * _playerSpeed * Time.deltaTime);

        // Handle player rotation based on the input
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720.0f * Time.deltaTime);
        }

    }

    //オブジェクト破壊
    public void BreakBlock()
    {
        //ブロックを持ってなければ処理を行う
        if(_hasBlock == true) return;
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
            if(_currentBlock == null) _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
            int _objID = _currentBlock.DestroyBlock(_destroyPower);
            // objIDを利用してUI表示  
            if(_objID == 1 ||_objID == 2)
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
        if(_hasBlock == false) return;
        if (!Input.GetMouseButtonDown(1)) return;

        Vector3 insPos = new Vector3 ((int)transform.position.x,(int)transform.position.y, -1.0f);
        GameObject insObj;
        if(_developMode) insObj = Instantiate(_herosPrefab, insPos, Quaternion.identity, _cubeParent);
        else insObj = PhotonNetwork.Instantiate(_herosPrefab.name, insPos, Quaternion.identity, 0);
        // 仮置き
        insObj.GetComponent<HerosBehaviour>().SetTargetPos(_enemyPos);
        _hasBlock = false;
        
    }

    //アイテムを使う
    public void UseItem()
    {
        //_destroyPower = ItemEffectA();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] BlockBehaviour _blockBehaviour;
    [SerializeField] ItemHandler _itemHandler;
    [SerializeField] GameObject BlockPrefab;
    [SerializeField] GameObject[] _generatePos;

    //x.z軸方向の入力を保存
    private float _input_x, _input_z;
    Vector3 _pos, _angle;
    private Rigidbody _rb;
    private bool _isJump = false;
    private bool _hasBlock = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        // _pos = transform.position;
        _angle = transform.eulerAngles;
    }

    void Update()
    {
       
        BreakBlock();
        CreateBlock();
        
        if (Input.GetMouseButtonDown(2))
        {
            _itemHandler.UseItem();
            
        }
    }

    void FixedUpdate()
    {
        PlayerCtrl();
    }

    //プレイヤーの移動
    public void PlayerCtrl()
    {
        
        _input_x = Input.GetAxis("Horizontal");
        //_input_z = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(_input_x, 0.0f, 0.0f);
 

        _rb.MovePosition(transform.position + movement * _playerSpeed * Time.deltaTime);

        // Handle player rotation based on the input
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720.0f * Time.deltaTime);
        }

        // Jump handling
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
            
        }
        //下記の移動は壁貫通のため廃棄
        // // 座標を取得
        // _pos = transform.position;
        // _angle = transform.eulerAngles;
        // //左移動
        // if (Input.GetKey(KeyCode.A))
        // {
        //     _pos.z += _playerSpeed * Time.deltaTime;    // z座標へ0.01加算
        //     _angle.y = 0.0f;
        //     transform.position = _pos;  // 入力から座標を再代入
        //     transform.eulerAngles = _angle;  // 入力から座標を再代入                
        // }
        // //右移動
        // else if (Input.GetKey(KeyCode.D))
        // {
        //     _pos.z -= _playerSpeed * Time.deltaTime;
        //     _angle.y = 180.0f;
        //     transform.position = _pos;  // 入力から座標を再代入
        //     transform.eulerAngles = _angle;  // 入力から座標を再代入
        // }
        //  //奥移動
        //  if(Input.GetKey(KeyCode.D))
        // {
        //     _pos.x += _playerSpeed * Time.deltaTime; 
        //     _angle.y = 90.0f;
        //     transform.position = _pos;  // 入力から座標を再代入
        //     transform.eulerAngles = _angle;  // 入力から座標を再代入
        // }
        // //手前移動
        // else if(Input.GetKey(KeyCode.A))
        // {
        //     _pos.x -= _playerSpeed * Time.deltaTime; 
        //     _angle.y = 270.0f;
        //     transform.position = _pos;  // 入力から座標を再代入
        //     transform.eulerAngles = _angle;  // 入力から座標を再代入
        // }
        // //ジャンプ
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        // //     if (_isJump == false)
        // //     {
        //         _rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
        //         Debug.Log("Jump");
        //     //     _isJump = true;
        //     // }
        // }
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Ground"))
    //     {
    //         _isJump = false;
    //     }
    // }

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
        if (!Physics.Raycast(ray, out hit)) return;

        if (hit.collider.gameObject.tag == "Block")
        {
            BlockBehaviour _blockBehaviour = hit.collider.GetComponent<BlockBehaviour>();
            int _objID = _blockBehaviour.DestroyBlock();
            _itemHandler.StackBlock(_objID);
           
            //0または1でない値を弾く（BlockBehaviourの DestroyBlock()参照）
            if(_objID == -1) return;
            if(_objID == 1 || _objID == 2)
            _hasBlock = true;
        }
        else
        {

        }

    }

    //オブジェクト
    public void CreateBlock()
    {
        //ブロックを持ってれば処理を行う
        if(_hasBlock == false) return;
        if (!Input.GetMouseButtonDown(1)) return;
        for (int i = -7 ; i < 8 ; i++)
        {
            if(transform.position.x >= i &&  transform.position.x <= i + 1)
            {
                //_pos.x >= 0 &&  _pos.x <= 1のときBLockを０の位置に生成
                Instantiate(BlockPrefab,_generatePos[i + 7].transform.position,Quaternion.identity);
                 _hasBlock = false;
                break;
            }
        }
        
    }

    //アイテムを使う、スタックリセット
    // public void UseItem()
    // {
    //      if (!Input.GetMouseButtonDown(2)) return;
    //      Debug.Log("アイテムを使用");
    // }
}

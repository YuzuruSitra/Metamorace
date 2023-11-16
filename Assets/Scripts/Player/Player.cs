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

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
         _pos = transform.position;
        _angle = transform.eulerAngles;
    }

    void Update()
    {
        PlayerCtrl();
        ObjectAction();
        CreateBlock();
        if (Input.GetMouseButtonDown(2))
        {
            _itemHandler.SelectItem();
        }
    }

    //プレイヤーの移動
    public void PlayerCtrl()
    {
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
         //奥移動
         if(Input.GetKey(KeyCode.D))
        {
            _pos.x += _playerSpeed * Time.deltaTime; 
            _angle.y = 90.0f;
            transform.position = _pos;  // 入力から座標を再代入
            transform.eulerAngles = _angle;  // 入力から座標を再代入
        }
        //手前移動
        else if(Input.GetKey(KeyCode.A))
        {
            _pos.x -= _playerSpeed * Time.deltaTime; 
            _angle.y = 270.0f;
            transform.position = _pos;  // 入力から座標を再代入
            transform.eulerAngles = _angle;  // 入力から座標を再代入
        }
        //ジャンプ
        if (Input.GetKeyDown(KeyCode.Space))
        {
        //     if (_isJump == false)
        //     {
                _rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
                Debug.Log("Jump");
            //     _isJump = true;
            // }
        }
    }

    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Ground"))
    //     {
    //         _isJump = false;
    //     }
    // }

    //オブジェクト破壊
    public void ObjectAction()
    {
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
        }
        else
        {

        }

    }


    public void CreateBlock()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if( _pos.x >= 0 &&  _pos.x <= 1)
        {
            //_pos.x >= 0 &&  _pos.x <= 1のときBLockを０の位置に生成
            Instantiate(BlockPrefab,_generatePos[0].transform.position,Quaternion.identity);
        }
         if( _pos.x >= 3 &&  _pos.x <= 4)
        {
            //_pos.x >= 0 &&  _pos.x <= 1のときBLockを０の位置に生成
            Instantiate(BlockPrefab,_generatePos[1].transform.position,Quaternion.identity);
        }
    }

    //アイテムを使う
    public void UseItem()
    {

    }
}

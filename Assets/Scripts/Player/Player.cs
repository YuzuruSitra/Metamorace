using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _jumpPower;
    [SerializeField] BlockBehaviour _blockBehaviour;
    //x.z軸方向の入力を保存
    private float _input_x,_input_z;
    Vector3 _pos,_angle;
    private Rigidbody _rb;
    private bool _isJump = false;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();   
    }

    void Update()
    {
        PlayerCtrl();
        ObjectAction();
        if(Input.GetMouseButtonDown(0))
        {
            //objectreference.CreateBlock(transform);
        }
    }

    //プレイヤーの移動
    public void PlayerCtrl()
    {
        // 座標を取得
         _pos = transform.position;
         _angle = transform.eulerAngles;
        //左移動
        if(Input.GetKey(KeyCode.A))
        {
            _pos.z += _playerSpeed * Time.deltaTime;    // z座標へ0.01加算
            _angle.y = 0.0f;  
            transform.position = _pos;  // 入力から座標を再代入
            transform.eulerAngles = _angle;  // 入力から座標を再代入                
        }
         //右移動
        else if(Input.GetKey(KeyCode.D))
        {
            _pos.z -= _playerSpeed * Time.deltaTime;
            _angle.y = 180.0f;
            transform.position = _pos;  // 入力から座標を再代入
            transform.eulerAngles = _angle;  // 入力から座標を再代入
        }
         //奥移動
        else if(Input.GetKey(KeyCode.W))
        {
            _pos.x += _playerSpeed * Time.deltaTime; 
            _angle.y = 90.0f;
            transform.position = _pos;  // 入力から座標を再代入
            transform.eulerAngles = _angle;  // 入力から座標を再代入
        }
        //手前移動
        else if(Input.GetKey(KeyCode.S))
        {
            _pos.x -= _playerSpeed * Time.deltaTime; 
            _angle.y = 270.0f;
            transform.position = _pos;  // 入力から座標を再代入
            transform.eulerAngles = _angle;  // 入力から座標を再代入
        }
        //ジャンプ
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(_isJump == false)
            {
                _rb.AddForce(transform.up * _jumpPower, ForceMode.Impulse);
                Debug.Log("Jump");
                _isJump = true;
            }
        }

        


        //変更前の移動
        // //Horizontal、水平、横方向のイメージ
        // _input_x = Input.GetAxis("Horizontal");
        // //Vertical、垂直、縦方向のイメージ
        // _input_z = Input.GetAxis("Vertical");

        // //移動の向きなど座標関連はVector3で扱う
        // Vector3 velocity = new Vector3(_input_x, 0, _input_z);
        // //ベクトルの向きを取得
        // Vector3 direction = velocity.normalized;

        // //移動距離を計算
        // float distance = _playerSpeed * Time.deltaTime;
        // //移動先を計算
        // Vector3 destination = transform.position + direction * distance;

        // //移動先に向けて回転
        // transform.LookAt(destination);
        // //移動先の座標を設定
        // transform.position = destination;

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground")) {
			_isJump = false;
		}
    }

    //オブジェクト破壊
    public void ObjectAction()
    {
        Vector3 direction = transform.forward; // プレイヤーの向いている方向を取得
        if(!Input.GetMouseButton(0)) return;
        
        Ray ray = new Ray(transform.position,direction);
        RaycastHit hit;
        Debug.DrawRay(transform.position, ray.direction * 30, Color.red, 1.0f); // 長さ３０、赤色で５秒間可視化
        if(Physics.Raycast(ray,out hit))
        {
            if(hit.collider.gameObject.tag == "Block") 
            {
                BlockBehaviour _blockBehaviour = hit.collider.GetComponent<BlockBehaviour>();
                 _blockBehaviour.DestroyBlock();
            }
            else{
                    
            }

        }
        
    }

    //アイテムを使う
    public void UseItem()
    {

    }
}

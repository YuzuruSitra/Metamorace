using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] Objectreference objectreference;

    //x軸方向の入力を保存
    private float _input_x;
    //z軸方向の入力を保存
    private float _input_z;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerCtrl();
        ObjectAction();
        if(Input.GetMouseButtonDown(0))
        {
            objectreference.CreateBlock(transform);
        }
    }

    //プレイヤーの移動
    public void PlayerCtrl()
    {
        //Horizontal、水平、横方向のイメージ
        _input_x = Input.GetAxis("Horizontal");
        //Vertical、垂直、縦方向のイメージ
        _input_z = Input.GetAxis("Vertical");

        //移動の向きなど座標関連はVector3で扱う
        Vector3 velocity = new Vector3(_input_x, 0, _input_z);
        //ベクトルの向きを取得
        Vector3 direction = velocity.normalized;

        //移動距離を計算
        float distance = _playerSpeed * Time.deltaTime;
        //移動先を計算
        Vector3 destination = transform.position + direction * distance;

        //移動先に向けて回転
        transform.LookAt(destination);
        //移動先の座標を設定
        transform.position = destination;

    }

    //オブジェクト破壊
    public void ObjectAction()
    {
        Vector3 direction = new Vector3(1, 0, 0); // X軸方向を表すベクトル:仮置きの終点
        if(Input.GetMouseButton(0))
        {
        Ray ray = new Ray(transform.position,direction);
        RaycastHit hit;
        Debug.DrawRay(transform.position, ray.direction * 30, Color.red, 1.0f); // 長さ３０、赤色で５秒間可視化
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.gameObject.tag == "Block") 
                {
                    Objectreference objectreference = hit.collider.GetComponent<Objectreference>();
                    objectreference.DestroyBlock();
                }

            }
        }
    }

    //アイテムを使う
    public void UseItem()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player_Wait : MonoBehaviour
{
    private PhotonView _myPV;
    [SerializeField]
    private float _playerSpeed;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private float _jumprayrength;

    private Rigidbody _rb;
    private bool _isJump,_isHead = false;
    private bool _isReady = false;
    public bool IsReady => _isReady;
    [SerializeField]
    private int _selectTeam;
    public int SelectTeam => _selectTeam;
    public event Action<bool> OnReadyChanged;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _myPV = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!_myPV.isMine) return;
        Jump();   
    }
    void FixedUpdate()
    {
        if (!_myPV.isMine) return;
        PlayerCtrl();
    }

    // 準備完了
    private void ChangeState(bool newState)
    {
        _isReady = newState;
        OnReadyChanged?.Invoke(newState);
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
        _rb.MovePosition(transform.position + movement * _playerSpeed * Time.deltaTime);

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

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Team1Area"))
        {
            _selectTeam = 0;
            ChangeState(true);
        }
        if(other.CompareTag("Team2Area"))
        {
            _selectTeam = 1;
            ChangeState(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Team1Area"))
        {
            ChangeState(false);
        }
        if(other.CompareTag("Team2Area"))
        {
            ChangeState(false);
        }
    }

}

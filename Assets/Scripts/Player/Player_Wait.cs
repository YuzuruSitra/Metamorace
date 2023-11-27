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
    private int _playerID;
    public int PlayerID => _playerID;

    [SerializeField]
    Animator _playerAnim;
    private bool _animJump = false;
    private bool _animWalk = false;
    private bool _animIdole = false;
   private SoundHandler _soundHandler;
   [SerializeField] AudioClip jump;

    void Start()
    {
         _soundHandler = SoundHandler.InstanceSoundHandler;
        _rb = GetComponent<Rigidbody>();
        _myPV = GetComponent<PhotonView>();
    }

    public void SetID(int id)
    {
        _playerID = id;
    }

    void Update()
    {
        if (!_myPV.isMine) return;
        Jump();   
        AnimSelect();
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
        if(inputX == 0)
        {
            _animIdole = true;
            _animWalk = false;
            Debug.Log(_animIdole);
            return;
        }
        _animIdole = false;
        _animWalk = true;
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
        //if(_animSwing) return;
        float raypos = 0.45f;
        float rayheight = 0.1f;
        // Jump handling
        bool _isJump = CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, -raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, -raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, raypos), Vector3.down));
        if (Input.GetKeyDown(KeyCode.Space) && _isJump)
        {
            //ジャンプSE鳴らす
            _soundHandler.PlaySE(jump);
            _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
        }
    }

    private bool CheckAndJump(Ray ray)
    {
        RaycastHit hit;
        // Debug.DrawRay(transform.position, Vector3.down * _jumprayrength, Color.red, 0.1f); 
        Debug.DrawRay(ray.origin, ray.direction * _jumprayrength, Color.red, 0.1f);
        if (Physics.Raycast(ray, out hit, _jumprayrength))
        {
            _isJump = false;
            _animJump = false;
            return true; // 地面に当たっている場合はtrueを返す
        }
        else
        {
            _isJump = true;
            _animJump = true;
            return false; // 地面に当たっていない場合はfalseを返す
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

    private void AnimSelect()
    {
        if (_animIdole)
        {
           _playerAnim.SetBool("_isJump", false);
            _playerAnim.SetBool("_isIdole", true);
            _playerAnim.SetBool("_isWalk", false);
            _playerAnim.SetFloat("MoveSpeed", 0.0f);
        }
        else if(_animJump)
        {
            _playerAnim.SetBool("_isJump", true);
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", false);
        }
        else if(_animWalk)
        {
            _playerAnim.SetBool("_isJump", false);           
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", true);
            _playerAnim.SetFloat("MoveSpeed", 1.0f);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    [SerializeField]
    private NPCDataReceiver _npcDataReceiver;
    [SerializeField]
    private NPCSoundHandler _npcSoundHandler;
    [SerializeField]
    private NPCCheckAround _npcCheckAround;
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    private float _initialSpeed = 5.0f;
    public float InitialSpeed => _initialSpeed;
    private float _npcSpeed;
    [SerializeField]
    private float _frontRayRength = 0.51f;
    [SerializeField]
    private float _initialjumpPower = 30.0f;
    private float _JumpPower;
    [SerializeField]
    private float _jumprayrength = 0.15f;
    private bool _isMoving;
    public bool IsMoving => _isMoving;
    private bool _onGround;
    public bool OnGround => _onGround;
    private Vector3 _upPadding = new Vector3(0f, 0.5f, 0f);
    bool Istest = true;
    float inputX = 0.0f;

    float JumpInterval;

    float _verticalRayOffset = 2.0f;
    float _vericalAvoidRay = 6.0f;

    bool _overBlock = false;
    public bool _OverBlock => _overBlock;
     bool _depthBlock = false;
    public bool _DepthBlock => _depthBlock;
    float _wallInterval = 0;
    float _directionfixingtime = 0;
    float SwitchDirectInterval;
    [SerializeField]
    float MinRandomTime = 3.0f, MaxRandomTime = 6.0f;

    void Start()
    {
        if(!_myPV.isMine) 
        {
            _rb.useGravity = false;
            _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
            return;
        }
        _npcSpeed = _initialSpeed;
        _JumpPower = _initialjumpPower;
    }

    void Update()
    {
        if (!_myPV.isMine) return;
        if(!_npcDataReceiver.IsActiveGame) return;
        AvoidBlock();
        
        float raypos = 0.45f;
        float rayheight = 0.1f;
        // Jump handling
        _onGround = CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, -raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(raypos, rayheight, -raypos), Vector3.down)) ||
                        CheckAndJump(new Ray(transform.position + new Vector3(-raypos, rayheight, raypos), Vector3.down));
    }

    void FixedUpdate()
    {
        if (!_myPV.isMine) return;
        if(!_npcDataReceiver.IsActiveGame) return;
        //PlayerCtrl();
    }

    public void NPCIdle()
    {
        inputX = 0;
    }

    //プレイヤーの移動
    public void NPCCtrl()
    {
        //if(_animSwing) return;
        TestCoroutine();

        _isMoving = true;
        //Debug.Log(inputX);

        Vector3 movement = new Vector3(inputX, 0, 0);
        // プレイヤーの向きを移動ベクトルに向ける
        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1080.0f * Time.deltaTime);
        if (CheckFront(new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward * _frontRayRength))) return;
        // カメラの方向を考慮して移動ベクトルを作成
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        Vector3 right = Camera.main.transform.right;
        right.y = 0.0f;
        right.Normalize();
        _rb.MovePosition(transform.position + movement * _npcSpeed * Time.deltaTime);
    }

    void TestCoroutine()
    {
        //ブロックのある方向
        string BlockDirection = _npcCheckAround.CheckArroundBlock();
        //プルプル防止


        SwitchDirectInterval -= Time.deltaTime;
        _wallInterval -= Time.deltaTime;
        _directionfixingtime -= Time.deltaTime;
        bool IsWall = _npcCheckAround.CheckWall();
        if (IsWall && _wallInterval < 0)
        {
            //Debug.Log("IsWall");
            inputX = -inputX;
            _wallInterval = 1.0f;
        }
        else
        {
            switch (BlockDirection)
            {
                case "Right":
                    inputX = 1.0f;
                    //Debug.Log(BlockDirection);
                    break;
                case "Left":
                    inputX = -1.0f;
                    // Debug.Log(BlockDirection);
                    break;
                case "Null":
                    //ランダム左右移動
                    // Debug.Log(BlockDirection);
                    if (SwitchDirectInterval < 0)
                    {
                        // Debug.Log(BlockDirection);
                        inputX = -inputX;
                        SwitchDirectInterval = Random.Range(MinRandomTime, MaxRandomTime);
                    }
                    break;
            }


        }
    }

    private bool CheckFront(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * _frontRayRength, Color.blue, 0.1f);
        if (Physics.Raycast(ray, out hit, _frontRayRength))
            return true;
        else
            return false;
    }

    public void Jump()
    {
        JumpInterval -= Time.deltaTime;
        // if(_animSwing) return;
        if (_onGround && JumpInterval < 0)
        {
            JumpInterval = 1.0f;
            //ジャンプSE鳴らす
            _npcSoundHandler.PlayJumpSE();
            _rb.AddForce(Vector3.up * _JumpPower, ForceMode.Impulse);
        }
    }

    // 地上にいるか判定
    private bool CheckAndJump(Ray ray)
    {
        RaycastHit hit;
        // Debug.DrawRay(ray.origin, ray.direction * _jumprayrength, Color.red, 0.1f);
        if (Physics.Raycast(ray, out hit, _jumprayrength)) return true;
        else return false;
    }

    // 移動速度の変更
    public void ChangeMoveSpeed(float newSpeed)
    {
        _npcSpeed = newSpeed;
    }

    public void AvoidBlock()
    {
       
    }
}

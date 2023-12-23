using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMover : MonoBehaviour
{
    [SerializeField]
    private NPCDataReceiver _npcDataReceiver;
    [SerializeField]
    private NPCSoundHandler _npcSoundHandler;
    // [SerializeField]
    // private PhotonView _myPV;
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
    [SerializeField]
    private float _npcWallReach = 1.0f;
    bool Istest = true;
    float inputX = 0.0f;

    float JumpInterval;

    float _verticalRayOffset = 2.0f;
    float _vericalAvoidRay = 6.0f;

    bool _overBlock = false;
    public bool _OverBlock => _overBlock;
     float _wallInterval = 0;

    void Start()
    {
        // if(!_myPV.isMine) 
        // {
        //     _rb.useGravity = false;
        //     _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        //     return;
        // }
        _npcSpeed = _initialSpeed;
        _JumpPower = _initialjumpPower;
    }

    void Update()
    {
        // if (!_myPV.isMine) return;
        // if(!_npcDataReceiver.IsActiveGame) return;
        //AvoidBlock();
        RayCheck();
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
        // if (!_myPV.isMine) return;
        // if(!_npcDataReceiver.IsActiveGame) return;
        //PlayerCtrl();
    }


    //プレイヤーの移動
    public void PlayerCtrl()
    {
        //if(_animSwing) return;

        if (Istest)
        {
            StartCoroutine(TestCoroutine());
        }
        _isMoving = true;

        if (inputX == 0)
        {
            _isMoving = false;
            return;
        }

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

    public void RayCheck()
    {
        //壁検出Ray
        Vector3 direction = transform.forward;
        direction.Normalize();
        _wallInterval -= Time.deltaTime;
        Ray ray = new Ray(transform.position + _upPadding, direction);
        Debug.DrawRay(transform.position + _upPadding, direction, Color.green, 0.3f);
        if (Physics.Raycast(ray, out RaycastHit hit, _npcWallReach))
        {
            if (hit.collider.CompareTag("Wall") && _wallInterval <= 0)
            {
                inputX = -inputX;
                _wallInterval = 1.0f;
            }
        }
        //頭上検出Ray
        //頭上にブロックがあればよける、両方がブロックで挟まれている場合はジャンプ
        Vector3 rayOrigin = transform.position + Vector3.up * _verticalRayOffset;
        Ray rayover = new Ray(rayOrigin, Vector3.up);
        Debug.DrawRay(rayover.origin, rayover.direction * _vericalAvoidRay, Color.green);

        if (Physics.Raycast(rayover, out RaycastHit hitblock, _vericalAvoidRay) && OnGround)
        {
            if (hitblock.collider.CompareTag("Ambras"))
            {
                _overBlock = true;
                Debug.Log("Avoid");
            }
            else _overBlock = false;
        }
    }

    IEnumerator TestCoroutine()
    {
        if (Istest)
        {
            Istest = false;
            if (inputX == 1.0f)
            {
                inputX = -1.0f;
            }
            else
            {
                inputX = 1.0f;
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 6.0f));// 適宜範囲を調整してください  
            Istest = true;
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
            JumpInterval = 2.0f;
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

        {
            Debug.Log("Avoid");
        }

    }
}

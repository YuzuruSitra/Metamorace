using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private AmbrasPoolHandler _ambrasPoolHandler;
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private float _playerSpeed;
    private float _usePlayerSpeed;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private float _jumprayrength;
    [SerializeField]
    private float _frontRayREngth = 0.51f;
    [SerializeField]
    private float _destroyPower = 1.0f;
    private float _useDestroyPower;
    //アイテムCブロックのプレハブ
    private GameObject _itemCBlock;
    [SerializeField]
    PoolHandler _poolHandler;

    [SerializeField]
    ItemHandler _itemHandler;
    ItemC _itemC;
    [SerializeField]
    GameObject[] _herosPrefab = new GameObject[2];
    [SerializeField]
    GameObject[] _bigPrefab = new GameObject[2];
    [SerializeField]
    GameObject[] _cPrefab = new GameObject[2];
    [SerializeField]
    Animator _playerAnim, _stanEffect;
    //private Transform _cubeParent;
    [SerializeField] GameObject _staneffect;
    [SerializeField] GameObject _saiyaeffect;
    [SerializeField] float stanpos;

    BlockBehaviour _currentBlock;
    HerosBehaviour _herosBehaviour, _bigBehaviour, _cBehaviour;
    private Rigidbody _rb;
    private bool _isJump, _isHead = false;
    private bool _hasBlock = false;
    private int _mineTeam, _enemyTeam;
    [SerializeField]
    private bool _developMode = false;
    private WaitForSeconds _waitTime;
    private Transform[] _cubeParentTeam = new Transform[2];
    private Quaternion[] _insQuaternion = { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0) };
    float inputX = 0;
    UIHandler _uiHandler;
    private bool _isDead = false;
    public bool IsDead => _isDead;
    private bool _isGame = false;
    [SerializeField] float _playerReach;
    [SerializeField] AudioClip jump, breakBlock, createBlock;
    private SoundHandler _soundHandler;

    void Start()
    {
        _soundHandler = SoundHandler.InstanceSoundHandler;
        _rb = GetComponent<Rigidbody>();
        _usePlayerSpeed = _playerSpeed;
        _useDestroyPower = _destroyPower;
        _waitTime = new WaitForSeconds(_itemHandler._ItemAEffectTime);
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
    }

    public void SetGameState(bool isGame)
    {
        _isGame = isGame;
    }

    public void SetParameter(Transform parent1, Transform parent2, int thisTeam, bool isDevelop)
    {

        _cubeParentTeam[0] = parent1;
        _cubeParentTeam[1] = parent2;
        _mineTeam = thisTeam;
        _enemyTeam = 1 - _mineTeam;
        _developMode = isDevelop;
    }

    void Update()
    {
         //エフェクト移動させてる
        if (transform.position.z < 0 &&_saiyaeffect.activeSelf) _saiyaeffect.transform.position = transform.position + new Vector3(0, 0.5f, -stanpos);
        else _saiyaeffect.transform.position = transform.position + new Vector3(0, 0.5f, stanpos);
        if (!_myPV.isMine && !_developMode) return;
        if (!_isGame) return;
        BreakBlock();
        CreateBlock();
        //アイテム生成
        _itemHandler.CreateItem();
        Item();
        Jump();
        JudgeVerticalDeath();
        JudgeHorizontalDeath();
    }
    void FixedUpdate()
    {
        if (!_myPV.isMine && !_developMode) return;
        if (!_isGame) return;
        PlayerCtrl();
    }

    //プレイヤーの移動
    void PlayerCtrl()
    {   
        inputX = 0.0f;
        //チーム1とチーム2で操作反転
        if (transform.position.z < 0)
        {
            if (Input.GetKey("d")) inputX = 1.0f;
            if (Input.GetKey("a")) inputX = -1.0f;
        }
        else
        {
            if (Input.GetKey("d")) inputX = -1.0f;
            if (Input.GetKey("a")) inputX = 1.0f;
        }
        if (Input.GetKey("a") && Input.GetKey("d")) inputX = 0.0f;
        if (inputX == 0) return;

        Vector3 movement = new Vector3(inputX, 0, 0);
        // プレイヤーの向きを移動ベクトルに向ける
        Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 1080.0f * Time.deltaTime);
        if (CheckFront(new Ray(transform.position + new Vector3(0, 0.5f, 0), transform.forward * _frontRayREngth))) return;
        // カメラの方向を考慮して移動ベクトルを作成
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0.0f;
        forward.Normalize();
        Vector3 right = Camera.main.transform.right;
        right.y = 0.0f;
        right.Normalize();
        _rb.MovePosition(transform.position + movement * _usePlayerSpeed * Time.deltaTime);

    }

    private bool CheckFront(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * _frontRayREngth, Color.blue, 0.1f);
        if (Physics.Raycast(ray, out hit, _frontRayREngth))
            return true;
        else
            return false;
    }

    // 縦方向の死亡判定
    void JudgeVerticalDeath()
    {
        Ray ray = new Ray(transform.position + new Vector3(0f, 2.0f, 0f), Vector3.up);
        RaycastHit _hitHead;
        if (Physics.Raycast(ray, out _hitHead, _jumprayrength)) _isHead = true;
        else _isHead = false;
        //頭のRayと足のRayが両方ぶつかっていたら死亡
        Debug.DrawRay(ray.origin, ray.direction * _jumprayrength, Color.green); // レイを描画
        if (!_isJump && _isHead)
        {
            StartCoroutine(DeathCoroutine("Vertical"));
        }
    }

    // 横方向の死亡判定
    void JudgeHorizontalDeath()
    {
        Ray ray;
        if (_mineTeam == 0) ray = new Ray(transform.position + Vector3.up, new Vector3(0f, 0f, -1f));
        else ray = new Ray(transform.position, new Vector3(0f, 0f, 1f));
        Debug.DrawRay(transform.position + Vector3.up, new Vector3(0f, 0f, 1f) * _jumprayrength, Color.blue, 1.0f);

        RaycastHit _hit;
        if (Physics.Raycast(ray, out _hit, _jumprayrength))
        {
            StartCoroutine(DeathCoroutine("Horizontal"));
        }
    }

    private IEnumerator DeathCoroutine(string direction)
    {
        // 死亡アニメーション
        if (direction == "Vertical") _playerAnim.SetBool("_isVDeath", true);
        else _playerAnim.SetBool("_isHDeath", true);

        //////////////////
        yield return new WaitForSeconds(2.0f);
        _isDead = true;
    }

    void Jump()
    {
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
            _playerAnim.SetBool("_isJump", true);
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
            _playerAnim.SetBool("_isJump", false);
            return true; // 地面に当たっている場合はtrueを返す
        }
        else
        {
            _isJump = true;
            return false; // 地面に当たっていない場合はfalseを返す
        }
    }


    //オブジェクト破壊
    public void BreakBlock()
    {
        if (_hasBlock || !Input.GetMouseButton(0)) return;

        Vector3 direction = transform.forward;
        direction.Normalize();
        Ray ray = new Ray(transform.position + Vector3.up, direction);

        Debug.DrawRay(transform.position, ray.direction * 0.5f, Color.red, 1.0f);

        if (!Physics.Raycast(ray, out RaycastHit hit, _playerReach) || !IsBlock(hit.collider)) return;

        _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
        //対象ブロックの体力参照
        //float _objHealth = _currentBlock._ObjHealth;
        _currentBlock.DecreceGage();

        int objID = _currentBlock.DestroyBlock(_useDestroyPower);

        if (objID == 1 || objID == 2)
        {
            if (hit.collider.CompareTag("ItemCBlock"))
            {
                _itemC = hit.collider.GetComponent<ItemC>();
                ProcessItemCBlockEffect();
            }
            //ブロック破壊SE;
            _soundHandler.PlaySE(breakBlock);
            _hasBlock = true;
            _itemHandler.StackBlock(objID);
        }
    }

    private bool IsBlock(Collider collider)
    {
        return collider.CompareTag("Ambras") || collider.CompareTag("Heros") || collider.CompareTag("ItemCBlock");
    }

    private void ProcessItemCBlockEffect()
    {
        int effectID = _itemHandler.ChoseEffectC();
        switch (effectID)
        {
            case 1:
                _itemC.EffectStan(ref _usePlayerSpeed);
                //スタンエフェクト再生
                if (transform.position.z < 0) _staneffect.transform.position = transform.position + new Vector3(0, 0, -stanpos);
                else _staneffect.transform.position = transform.position + new Vector3(0, 0, stanpos);

                _stanEffect.SetBool("Stan", true);
                _playerAnim.SetBool("_isStan", true);
                Invoke("FinishItemC", _itemHandler._ItemCEffectTime);
                break;
            case 2:
                _itemC.Break4();
                break;
        }
    }


    //オブジェクト生成
    public void CreateBlock()
    {
        //ブロックを持ってれば処理を行う
        if (_hasBlock == false) return;
        if (!Input.GetMouseButtonDown(1)) return;
        //swingAnim再生
        _playerAnim.SetBool("_isSwing", true);
       
        Vector3 insPos = new Vector3((int)transform.position.x, (int)transform.position.y, -1.0f);
        Vector3 insBigPos = new Vector3((int)transform.position.x, (int)transform.position.y + 0.75f, -1.0f);
        GameObject insObj;
        if (_developMode)
        {
            _soundHandler.PlaySE(createBlock);
            //アイテムBを持っていたら巨大ブロック一回だけ生成
            if (_itemHandler._HasItemB)
            {
                //アイテムB微調整
                insObj = Instantiate(_bigPrefab[_mineTeam], insBigPos, _insQuaternion[_mineTeam]);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
                _itemHandler.ItemEffectB();
            }
            //ItemCBlock生成
            else if (_itemHandler._HasItemC)
            {
                _itemHandler.ItemEffectC();
                insObj = Instantiate(_cPrefab[_mineTeam], insPos, _insQuaternion[_mineTeam]);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
            }
            else
            {
                insObj = Instantiate(_herosPrefab[_mineTeam], insPos, _insQuaternion[_mineTeam]);
                insObj.transform.parent = _cubeParentTeam[_enemyTeam];
            }
        }
        else
        {
            //アイテムBを持っていたら巨大ブロック一回だけ生成
            if (_itemHandler._HasItemB)
            {
                //アイテムB微調整
                _myPV.RPC(nameof(SyncCreateBig), PhotonTargets.All, insBigPos, _mineTeam, _enemyTeam);
                _itemHandler.ItemEffectB();
            }
            //ItemCBlock生成
            else if (_itemHandler._HasItemC)
            {
                _itemHandler.ItemEffectC();
                _myPV.RPC(nameof(SyncCreateItemC), PhotonTargets.All, insPos, _mineTeam, _enemyTeam);
            }
            else
            {
                _myPV.RPC(nameof(SyncCreateHeros), PhotonTargets.All, insPos, _mineTeam, _enemyTeam);
            }

        }
        // 仮置き
        _hasBlock = false;
    }

    // ネットワーク上のキューブ生成
    [PunRPC]
    private void SyncCreateBig(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        GameObject insObj = PhotonNetwork.Instantiate(_bigPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        insObj.transform.parent = _cubeParentTeam[enemyTeam];
    }

    [PunRPC]
    private void SyncCreateItemC(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        GameObject insObj = PhotonNetwork.Instantiate(_cPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        insObj.transform.parent = _cubeParentTeam[enemyTeam];
    }

    [PunRPC]
    private void SyncCreateHeros(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        GameObject insObj = PhotonNetwork.Instantiate(_herosPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        insObj.transform.parent = _cubeParentTeam[enemyTeam];
    }

    //アイテムを使う
    public void Item()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (_itemHandler._HasItemA == true)
        {
            //さいやエフェクト再生
            _saiyaeffect.SetActive(true);
            _itemHandler.ItemEffectA(ref _useDestroyPower, ref _usePlayerSpeed);
            StartCoroutine(FinishItemA());


        }
    }

    IEnumerator FinishItemA()
    {
        yield return _waitTime;
        _usePlayerSpeed = _playerSpeed;
        _useDestroyPower = _destroyPower;
        _saiyaeffect.SetActive(false);
    }

    void FinishItemC()
    {
        _usePlayerSpeed = _playerSpeed;
        //スタンエフェクト停止
        _stanEffect.SetBool("Stan", false);
        _playerAnim.SetBool("_isStan", false);
    }
}

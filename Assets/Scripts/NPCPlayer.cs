using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPlayer : MonoBehaviour
{
    private AmbrasPoolHandler _ambrasPoolHandler;
    // [SerializeField]
    // private PhotonView _myPV;
    // [SerializeField]
    // private PhotonTransformView _myPTV;
    [SerializeField]
    private float _playerSpeed;
    private float _usePlayerSpeed;
    [SerializeField]
    private float _jumpPower;
    private float _useJumpPower;
    [SerializeField]
    private float _jumprayrength;
    [SerializeField]
    private float _frontRayREngth = 0.51f;
    [SerializeField]
    private float _destroyPower = 1.0f;
    private float _useDestroyPower;
    [SerializeField]
    ItemHandler _itemHandler;
    ItemC _itemC;
    // [SerializeField]
    // GameObject[] _herosPrefab = new GameObject[2];
    // [SerializeField]
    // GameObject[] _bigPrefab = new GameObject[2];
    // [SerializeField]
    // GameObject[] _cPrefab = new GameObject[2];
    [SerializeField]
    Animator _playerAnim, _stanEffect;
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
    //UIHandler _uiHandler;
    private bool _isDead = false;
    public bool IsDead => _isDead;
    private bool _isGame = false;
    private int _useItemState = -1;
    [SerializeField] float _playerReach;
    [SerializeField] AudioClip jump, breakBlock, createBlock;
    private SoundHandler _soundHandler;
    private bool _animJump = false;
    private bool _animWalk = false;
    private bool _animSwing = false;
    private bool _animVDeat = false;
    private bool _animHDeat = false;
    private bool _animStan = false;
    private bool _animIdole = false;
    private bool _animBreak = false;
    private bool _hasItem = false;
    private Vector3 _upPadding = new Vector3(0f, 0.5f, 0f);
    Vector3 _insPos;
    Vector3 _insBigPos;
    [SerializeField]
    private GameObject _predictCubes;
    bool tset = true;
    bool Block = false;
    public enum NPCState
    {
        WAIT,			//行動を一旦停止
        MOVE,			//移動
        CREATE,		//生成
        BREAK,	//破壊
        IDLE,			//待機
        AVOID,		//回避
    }
    public NPCState npcState = NPCState.MOVE;
    void Start()
    {
        // if(!_myPV.isMine) return;
        _soundHandler = SoundHandler.InstanceSoundHandler;
        _rb = GetComponent<Rigidbody>();
        _usePlayerSpeed = _playerSpeed;
        _useJumpPower = _jumpPower;
        _useDestroyPower = _destroyPower;
        _waitTime = new WaitForSeconds(_itemHandler._ItemAEffectTime);
        // _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
    }

    // public void SetGameState(bool isGame)
    // {
    //     _isGame = isGame;
    // }

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
        //NPC
       
        //Debug.Log(npcState);
        switch (npcState)
        {
            case NPCState.MOVE:
                PlayerCtrl();
                break;
            case NPCState.BREAK:
                BreakBlock();
                break;
            case NPCState.CREATE:
                CreateBlock();
                break;
        }
        //エフェクト移動させてる
        if (transform.position.z < 0 && _saiyaeffect.activeSelf) _saiyaeffect.transform.position = transform.position + new Vector3(0, 0.5f, -stanpos);
        else _saiyaeffect.transform.position = transform.position + new Vector3(0, 0.5f, stanpos);
        //  PlayerCtrl();
         // BreakBlock();
        // if (!_myPV.isMine && !_developMode) return;
        //if (!_isGame) return;
        //BreakBlock();
        // CreateBlock();
        // Item();
        // Jump();
        // JudgeVerticalDeath();
        // JudgeHorizontalDeath();
        AnimSelecter();
        if (_hasBlock)
        {
            _insPos = new Vector3((int)transform.position.x, (int)transform.position.y + 0.25f, -1.0f);
            _predictCubes.transform.position = _insPos;
        }
    }
    void FixedUpdate()
    {
        // if (!_myPV.isMine && !_developMode) return;
        //if (!_isGame) return;
        // PlayerCtrl();
    }

    //プレイヤーの移動
    void PlayerCtrl()
    {
        if (_animSwing) return;
        //inputX = 0.0f;
        
        if (tset)
        {
            StartCoroutine(TestCoroutine());
        }
        if(Block)
        {
            npcState = NPCState.BREAK;
        }

        if (inputX == 0)
        {
            _animIdole = true;
            _animWalk = false;
            return;
        }
        _animIdole = false;
        _animWalk = true;

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
        //スムーズな同期のためにPhotonTransformViewに速度値を渡す
        Vector3 velocity = _rb.velocity;
        // _myPTV.SetSynchronizedValues(velocity, 0); 
    }

    IEnumerator TestCoroutine()
    {
        if (tset)
        {
            tset = false;
            if (inputX == 1.0f)
            {
                inputX = -1.0f;
            }
            else
            {
                inputX = 1.0f;
            }
            yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));// 適宜範囲を調整してください  
            tset = true;
        }
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
        if (_mineTeam == 0) ray = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(0f, 0f, -1f));
        else ray = new Ray(transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(0f, 0f, 1f));
        Debug.DrawRay(transform.position + new Vector3(0f, 0.5f, 0f), new Vector3(0f, 0f, 1f) * _jumprayrength, Color.blue, 1.0f);

        RaycastHit _hit;
        if (Physics.Raycast(ray, out _hit, _jumprayrength))
        {
            StartCoroutine(DeathCoroutine("Horizontal"));
        }
    }

    private IEnumerator DeathCoroutine(string direction)
    {
        // 死亡アニメーション
        if (direction == "Vertical") _animVDeat = true;
        else _animHDeat = true;

        //////////////////
        yield return new WaitForSeconds(2.0f);
        _isDead = true;
    }

    void Jump()
    {
        if (_animSwing) return;
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
            _rb.AddForce(Vector3.up * _useJumpPower, ForceMode.Impulse);
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


    //オブジェクト破壊
    public void BreakBlock()
    {
        _animBreak = false;
        if (_hasBlock) return;
        _animBreak = true;
        Vector3 direction = transform.forward;
        direction.Normalize();
        Ray ray = new Ray(transform.position + _upPadding, direction);

        Debug.DrawRay(transform.position + _upPadding, direction, Color.red, 0.3f);
         Debug.Log("b");
        if (!Physics.Raycast(ray, out RaycastHit hit, _playerReach)) return;
        Debug.Log("c");
        _currentBlock = hit.collider.GetComponent<BlockBehaviour>();
        //対象ブロックの体力参照
        //float _objHealth = _currentBlock._ObjHealth;
        _currentBlock.DecreceGage();

        int objID = _currentBlock.DestroyBlock(_useDestroyPower);

        if (objID == UIHandler._ambrassID || objID == UIHandler._herosID)
        {
            //UIに保持しているブロックを表示する処理
            // _uiHandler.BlockImage(objID);
            if (hit.collider.CompareTag("ItemCBlock"))
            {
                _itemC = hit.collider.GetComponent<ItemC>();
                ProcessItemCBlockEffect();
            }
            //ブロック破壊SE;
            // _soundHandler.PlaySE(breakBlock);
            _predictCubes.SetActive(true);
            _hasBlock = true;
            if (!_itemHandler._HasItemA || !_itemHandler._HasItemB || !_itemHandler._HasItemC)
            {
                _itemHandler.StackBlock(objID);
                _itemHandler.CreateItem();
            }
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
                _itemC.EffectStan(ref _usePlayerSpeed, ref _useJumpPower);
                //スタンエフェクト再生
                if (transform.position.z < 0) _staneffect.transform.position = transform.position + new Vector3(0, 0, -stanpos);
                else _staneffect.transform.position = transform.position + new Vector3(0, 0, stanpos);

                _stanEffect.SetBool("Stan", true);
                _animStan = true;
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
        if (!_hasBlock) return;
        if (_animSwing) return;
        if (!Input.GetMouseButtonDown(0)) return;
        //swingAnim再生
        _animSwing = true;
        _hasBlock = false;
        _predictCubes.SetActive(false);
        _insBigPos = _insPos;
        _insBigPos.y += 1.0f;
        _soundHandler.PlaySE(createBlock);
        Invoke("InsSwingObj", 0.4f);
    }

    void InsSwingObj()
    {
        _animSwing = false;
        //アイテムBを持っていたら巨大ブロック一回だけ生成
        if (_itemHandler._HasItemB && _useItemState == 0)
        {
            //アイテムB微調整
            // _myPV.RPC(nameof(SyncCreateBig), PhotonTargets.All, _insBigPos, _mineTeam, _enemyTeam);
            _useItemState = -1;
            _itemHandler.ItemEffectB();
        }
        //ItemCBlock生成
        else if (_itemHandler._HasItemC && _useItemState == 1)
        {
            // _myPV.RPC(nameof(SyncCreateItemC), PhotonTargets.All, _insPos, _mineTeam, _enemyTeam);
            _useItemState = -1;
            _itemHandler.ItemEffectC();
        }
        else
        {
            // _myPV.RPC(nameof(SyncCreateHeros), PhotonTargets.All, _insPos, _mineTeam, _enemyTeam);
        }
        //_uiHandler.ResetBlockImage();

    }

    // ネットワーク上のキューブ生成
    [PunRPC]
    private void SyncCreateBig(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        // GameObject insObj = PhotonNetwork.Instantiate(_bigPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        //insObj.transform.parent = _cubeParentTeam[enemyTeam];
    }

    [PunRPC]
    private void SyncCreateItemC(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        // GameObject insObj = PhotonNetwork.Instantiate(_cPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        // insObj.transform.parent = _cubeParentTeam[enemyTeam];
    }

    [PunRPC]
    private void SyncCreateHeros(Vector3 pos, int mineTeam, int enemyTeam)
    {
        if (!PhotonNetwork.isMasterClient) return;
        // GameObject insObj = PhotonNetwork.Instantiate(_herosPrefab[mineTeam].name, pos, _insQuaternion[mineTeam], 0);
        // insObj.transform.parent = _cubeParentTeam[enemyTeam];
    }

    //アイテムを使う
    public void Item()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (_itemHandler._HasItemA == true)
        {
            //さいやエフェクト再生
            _saiyaeffect.SetActive(true);
            _itemHandler.ItemEffectA(ref _useDestroyPower, ref _usePlayerSpeed);
            //ブロック画像nullにする
            //_uiHandler.ResetStackImage();
            StartCoroutine(FinishItemA());
        }
        else if (_itemHandler._HasItemC == true && _hasBlock)
        {
            //  アイテムUI削除、所持ブロックをCに変更
            //_uiHandler.ItemUI(UIHandler._itemCID);
            // _uiHandler.ResetItemImage();    
            //ブロック画像nullにする
            //_uiHandler.ResetStackImage();
            _useItemState = 1;
        }
        else if (_itemHandler._HasItemB == true && _hasBlock)
        {
            //  アイテムUI削除、所持ブロックをBに変更
            // _uiHandler.ItemUI(UIHandler._itemBID);
            //_uiHandler.ResetItemImage();    
            //ブロック画像nullにする
            //_uiHandler.ResetStackImage();
            _useItemState = 0;
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
        _useJumpPower = _jumpPower;
        //スタンエフェクト停止
        _stanEffect.SetBool("Stan", false);
        _animStan = false;
    }

    // 時短につき良くない実装
    private void AnimSelecter()
    {
        if (_animVDeat)
        {
            _playerAnim.SetBool("_isVDeath", true);
            _playerAnim.SetBool("_isHDeath", false);
            _playerAnim.SetBool("_isStan", false);
            _playerAnim.SetBool("_isSwing", false);
            _playerAnim.SetBool("_isJump", false);
            _playerAnim.SetBool("_isBreak", false);
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", false);
        }
        else if (_animHDeat)
        {
            _playerAnim.SetBool("_isVDeath", false);
            _playerAnim.SetBool("_isHDeath", true);
            _playerAnim.SetBool("_isStan", false);
            _playerAnim.SetBool("_isSwing", false);
            _playerAnim.SetBool("_isJump", false);
            _playerAnim.SetBool("_isBreak", false);
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", false);
        }
        else if (_animStan)
        {
            _playerAnim.SetBool("_isVDeath", false);
            _playerAnim.SetBool("_isHDeath", false);
            _playerAnim.SetBool("_isStan", true);
            _playerAnim.SetBool("_isSwing", false);
            _playerAnim.SetBool("_isJump", false);
            _playerAnim.SetBool("_isBreak", false);
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", false);
        }
        else if (_animSwing)
        {
            _playerAnim.SetBool("_isVDeath", false);
            _playerAnim.SetBool("_isHDeath", false);
            _playerAnim.SetBool("_isStan", false);
            _playerAnim.SetBool("_isSwing", true);
            _playerAnim.SetBool("_isJump", false);
            _playerAnim.SetBool("_isBreak", false);
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", false);
        }
        else if (_animJump)
        {
            _playerAnim.SetBool("_isVDeath", false);
            _playerAnim.SetBool("_isHDeath", false);
            _playerAnim.SetBool("_isStan", false);
            _playerAnim.SetBool("_isSwing", false);
            _playerAnim.SetBool("_isJump", true);
            _playerAnim.SetBool("_isBreak", false);
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", false);
        }
        else if (_animIdole)
        {
            if (_animBreak)
            {
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", true);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", false);
                _playerAnim.SetFloat("MoveSpeed", 0.0f);
            }
            else
            {
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", true);
                _playerAnim.SetBool("_isWalk", false);
                _playerAnim.SetFloat("MoveSpeed", 0.0f);
            }
        }
        else if (_animWalk)
        {
            _playerAnim.SetBool("_isVDeath", false);
            _playerAnim.SetBool("_isHDeath", false);
            _playerAnim.SetBool("_isStan", false);
            _playerAnim.SetBool("_isSwing", false);
            _playerAnim.SetBool("_isJump", false);
            _playerAnim.SetBool("_isBreak", false);
            _playerAnim.SetBool("_isIdole", false);
            _playerAnim.SetBool("_isWalk", true);
            _playerAnim.SetFloat("MoveSpeed", 1.0f);
        }
    }

}

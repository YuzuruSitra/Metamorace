using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;
    private PlayerDataReceiver _playerDataReceiver;
    private NPCDataReceiver _npcDataReceiver;
    private PlayerDeathDetector _playerDeathDetector;
    [SerializeField] 
    private ColorManager _colorManager;
    [SerializeField] 
    private GameObject _blockManagerPrefab;
    private BlockManager _blockManager;
    [SerializeField] 
    private CamManager _camManager;
    [SerializeField] 
    private float _TimeLimit;
    public float _timeLimit => _TimeLimit;
    private UIHandler _uiHandler;
    [SerializeField] 
    private GameObject[] _playerPrefab = new GameObject[4];
    [SerializeField]
    private GameObject[] _npcPrefab = new GameObject[1];
    private int _teamID;
    private int _playerID,_npcID;
    public const float TEAM1_POS_Z = -3.0f;
    public const float TEAM2_POS_Z = 2f;
    public const float TEAM1_BIG_POS_Z = -2.0f;
    public const float TEAM2_BIG_POS_Z = 1.0f;
    public const int MinPosX = -7;
    public const int MaxPosX = 8;
    private const int MAX_POS_X = MaxPosX - MinPosX;
    public const int INS_POS_Y = 11;
    public const int MAX_POS_Y = 8;
    private const int FIELD_SIZE = MAX_POS_X * MAX_POS_Y;
    public bool DevelopeMode;
    public int DevelopeTeamID;
    private Coroutine _coroutineCalc;
    [SerializeField] 
    private float _calcInterval = 2.5f;
    private WaitForSeconds _calcWaitTime;
    [SerializeField]
    private Transform _cubeParentTeam1, _cubeParentTeam2;
    private bool _isGame = false;
    private bool _oneTime = true;
    int shareTeam1result, shareTeam2result;
    private int _joinPlayerCount = 0;
    private int _currentPlayerCount = 0;
    bool IsOnce = false;
    private SoundHandler _soundHandler;
    [SerializeField] 
    private AudioClip battleBGM;
    [SerializeField] 
    private AudioClip countDown,gameStart,gameEnd,drumroll;
    private string[] _memberNames;
    private int[] _memberTeamIDs;

    void Start()
    {
        _soundHandler = SoundHandler.InstanceSoundHandler;
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        _calcWaitTime = new WaitForSeconds(_calcInterval);
        SceneManager.sceneLoaded += OnLoadedScene;
        _soundHandler.PlayBGM(battleBGM);
        if (DevelopeMode)
            HandleDevelopmentMode();
        else
            HandleProductionMode();
    }

    public void SetInfo(int team, int id, int maxPlayer, string[] names, int[] teams,int npcnum)
    {
        _teamID = team;
        _playerID = id;
        _currentPlayerCount = maxPlayer + npcnum;
        _memberNames = names;
        _memberTeamIDs = teams;
    }

    // 開発モードの初期化処理
    private void HandleDevelopmentMode()
    {
        _teamID = DevelopeTeamID;
        SetupPlayer(_teamID == 0 ? TEAM1_POS_Z : TEAM2_POS_Z);
        SetupNPC(_teamID == 0 ? TEAM1_POS_Z : TEAM2_POS_Z);
        _coroutineCalc = StartCoroutine(CalcCubeShare());
    }

    // 製品モードの初期化処理
    private void HandleProductionMode()
    {
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Launcher");
            return;
        }

        SetupPhotonPlayer(_teamID == 0 ? TEAM1_POS_Z : TEAM2_POS_Z);
        SetupPhotonNPC(_teamID == 0 ? TEAM1_POS_Z : TEAM2_POS_Z);
        if (PhotonNetwork.isMasterClient) _coroutineCalc = StartCoroutine(CalcCubeShare());
    }

    // ローカルプレイヤーのセットアップ
    private void SetupPlayer(float myPosZ)
    {
        SetupBlockManager();
        GameObject player = Instantiate(_playerPrefab[_playerID], new Vector3(0f, 1.25f, myPosZ), Quaternion.identity);
        _camManager.SetPlayer(player, _teamID);
        _playerDataReceiver = player.transform.GetChild(0).gameObject.GetComponent<PlayerDataReceiver>();
        _playerDeathDetector = player.transform.GetChild(0).gameObject.GetComponent<PlayerDeathDetector>();
        _playerDataReceiver.SetTeamID(_teamID);
        _playerDataReceiver.SetInsCubeParent(_cubeParentTeam1, _cubeParentTeam2);
    }
     // ローカルNPCのセットアップ
    private void SetupNPC(float myPosZ)
    {
        SetupBlockManager();
        GameObject npc = Instantiate(_npcPrefab[_npcID], new Vector3(0f, 1.25f, myPosZ), Quaternion.identity);
        //_camManager.SetPlayer(player, _teamID);
        _npcDataReceiver = npc.transform.GetChild(0).gameObject.GetComponent<NPCDataReceiver>();
        //_playerDeathDetector = player.transform.GetChild(0).gameObject.GetComponent<PlayerDeathDetector>();
        _npcDataReceiver.SetTeamID(_teamID);
        _npcDataReceiver.SetInsCubeParent(_cubeParentTeam1, _cubeParentTeam2);
    }

    // ネットワークプレイヤーのセットアップ
    private void SetupPhotonPlayer(float myPosZ)
    {
        if (PhotonNetwork.isMasterClient) SetupPhotonBlockManager();

        GameObject player = PhotonNetwork.Instantiate(_playerPrefab[_playerID].name, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity, 0);
        _myPV.RPC(nameof(JoinPlayer), PhotonTargets.All);
        _camManager.SetPlayer(player, _teamID);
        _playerDataReceiver = player.transform.GetChild(0).gameObject.GetComponent<PlayerDataReceiver>();
        _playerDeathDetector = player.transform.GetChild(0).gameObject.GetComponent<PlayerDeathDetector>();
        _playerDataReceiver.SetTeamID(_teamID);
        _playerDataReceiver.SetInsCubeParent(_cubeParentTeam1, _cubeParentTeam2);
    }

     // ネットワークNPCのセットアップ
    private void SetupPhotonNPC(float myPosZ)
    {
        if (PhotonNetwork.isMasterClient) SetupPhotonBlockManager();

        GameObject npc = PhotonNetwork.Instantiate(_npcPrefab[_npcID].name, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity, 0);
        _myPV.RPC(nameof(JoinPlayer), PhotonTargets.All);
       // _camManager.SetPlayer(player, _teamID);
        _npcDataReceiver = npc.transform.GetChild(0).gameObject.GetComponent<NPCDataReceiver>();
        //_playerDeathDetector = npc.transform.GetChild(0).gameObject.GetComponent<NPCDeathDetector>();
        _npcDataReceiver.SetTeamID(_teamID);
        _npcDataReceiver.SetInsCubeParent(_cubeParentTeam1, _cubeParentTeam2);
    }

    // プレイヤーの参加数
    [PunRPC]
    private void JoinPlayer()
    {
        _joinPlayerCount ++;
    }

    // ローカル用ブロックマネージャーのセットアップ
    private void SetupBlockManager()
    {
        GameObject blockManagerObj = Instantiate(_blockManagerPrefab, Vector3.zero, Quaternion.identity);
        _blockManager = blockManagerObj.GetComponent<BlockManager>();
        _blockManager.SetParam(DevelopeMode, _cubeParentTeam1, _cubeParentTeam2);
    }

    // ネットワーク用ブロックマネージャーのセットアップ
    private void SetupPhotonBlockManager()
    {
        GameObject blockManagerObj = PhotonNetwork.Instantiate(_blockManagerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        _blockManager = blockManagerObj.GetComponent<BlockManager>();
        _blockManager.SetParam(DevelopeMode, _cubeParentTeam1, _cubeParentTeam2);
    }

    void Update()
    {

        if(!_isGame)
        {
            if (!_oneTime) return;
            if (!DevelopeMode)
            {
                if (PhotonNetwork.isMasterClient)
                {
                    if (_joinPlayerCount >= _currentPlayerCount)
                    {
                        // 情報を共有
                        _myPV.RPC(nameof(SendTeamInfo), PhotonTargets.All);

                        // ゲーム開始
                        _myPV.RPC(nameof(LaunchGame), PhotonTargets.All);
                    }
                }
            }
            else
            {
                StartCoroutine(StartGame());
            }
        }
        else
        {
            ReduceTimeLimit();

            // ゲーム終了処理
            if (_timeLimit <= 0 || _playerDeathDetector.IsPlayerDeath)
            {
                _soundHandler.PlaySE(gameEnd); 
                if(!DevelopeMode) 
                {            
                    _isGame = false;
                    _myPV.RPC(nameof(FinishMasterGame), PhotonTargets.MasterClient,  _playerDeathDetector.IsPlayerDeath, _teamID);
                }
                else
                {
                    _isGame = false;
                    _playerDataReceiver.SetGameState(_isGame);
                    _blockManager.SetGameState(_isGame);
                    // 死んだプレイヤーのチームを取得して勝敗を判定
                    int winTeam = 1 - _teamID;
                    // 占有率の取得
                    int shareTeam1 = _blockManager.CalcCubeShare1(FIELD_SIZE);
                    int shareTeam2 = _blockManager.CalcCubeShare2(FIELD_SIZE);
                    _uiHandler.ShowCalc(shareTeam1,shareTeam2);
                    _uiHandler.ShowResult(shareTeam1,shareTeam2, _playerDeathDetector.IsPlayerDeath,_teamID);
                    //_uiHandler.ResultInfo(_memberNames);
                }
            }
        }
    }

    [PunRPC]
    private void SendTeamInfo()
    {
        // UIハンドラーへ値を渡す処理
        _uiHandler.SetNames(_memberNames, _memberTeamIDs);
    }

    // ゲーム開始処理
    [PunRPC]
    private void LaunchGame()
    {
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        _uiHandler.ShowLimitTime(_TimeLimit);
        _uiHandler.ShowCalc(0,0);
        _oneTime = false;
        // UIの更新
        yield return new WaitForSeconds(3.0f);
        //ホイッスル
        _soundHandler.PlaySE(gameStart);
        _isGame = true;
        _playerDataReceiver.SetGameState(_isGame);
        if (PhotonNetwork.isMasterClient) _blockManager.SetGameState(_isGame);
    }

    // ゲーム終了準備マスタークライアントのみの処理
    [PunRPC]
    private void FinishMasterGame(bool isDead, int team)
    {
        _blockManager.SetGameState(false);
        // 占有率の取得
        int shareTeam1 = _blockManager.CalcCubeShare1(FIELD_SIZE);
        int shareTeam2 = _blockManager.CalcCubeShare2(FIELD_SIZE);
        _myPV.RPC(nameof(FinishGame), PhotonTargets.All, isDead, team, shareTeam1, shareTeam2);
    }

    // 全クライアントの終了処理
    [PunRPC]
    private void FinishGame(bool isDead, int team, int shareTeam1, int shareTeam2)
    {
        _isGame = false;
        _playerDataReceiver.SetGameState(_isGame);
        // 死んだプレイヤーのチームを取得して勝敗を判定
        StartCoroutine(ShowResultUI(isDead, team, shareTeam1, shareTeam2));
    }

    IEnumerator ShowResultUI(bool isDead, int team, int shareTeam1, int shareTeam2)
    {
        yield return new WaitForSeconds(2.5f);
        int winTeam;
        if (isDead) winTeam = 1 - team;
        else winTeam = CalcWinTeam(shareTeam1, shareTeam2);
        // UIの更新
        _uiHandler.ShowCalc(shareTeam1,shareTeam2);
        _uiHandler.ResultInfo(_memberNames, _memberTeamIDs, winTeam);
        _uiHandler.ShowResult(shareTeam1, shareTeam2, isDead, winTeam);
    }

    // ルームへ戻る処理
    public void BackPrivateRoom()
    {
        if (!DevelopeMode) 
            _myPV.RPC(nameof(ReturnRoom), PhotonTargets.All);
        else
            SceneManager.LoadScene("Master_Wait");
    }

    [PunRPC]
    private void ReturnRoom()
    {
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("Master_Wait");
    }

    private void OnLoadedScene( Scene i_scene, LoadSceneMode i_mode )
    {
        PhotonNetwork.isMessageQueueRunning = true;
    }

    // 制限時間の減少処理
    void ReduceTimeLimit()
    {
        _TimeLimit -= Time.deltaTime;
        _uiHandler.ShowLimitTime(_TimeLimit);
        if(_TimeLimit < 10 && !IsOnce)
        {
            IsOnce = true;
            _soundHandler.PlaySE(countDown);
        }  
    }

    // オブジェクトの占有率を計算するコルーチン
    IEnumerator CalcCubeShare()
    {
        while (true)
        {
            yield return _calcWaitTime;
            int shareTeam1 = _blockManager.CalcCubeShare1(FIELD_SIZE);
            int shareTeam2 = _blockManager.CalcCubeShare2(FIELD_SIZE);

            _uiHandler.ShowCalc(shareTeam1,shareTeam2);
        }
    }

    int CalcWinTeam(int shareTeam1, int shareTeam2)
    {
        int winTeam = -1;
        if (shareTeam1 < shareTeam2) winTeam = 0;
        if (shareTeam1 > shareTeam2) winTeam = 1;
        return winTeam;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
    private GameObject _playerPrefab;
    [SerializeField] 
    private GameObject[] _herosPrefab;
    [SerializeField] 
    private GameObject _itemCBlock;
    private int _teamID;
    private int _playerID;
    public const float TEAM1_POS_Z = -2.5f;
    public const float TEAM2_POS_Z = 1.5f;
    public const int MinPosX = -7;
    public const int MaxPosX = 8;
    private const int MAX_POS_X = MaxPosX - MinPosX;
    public const int MAX_POS_Y = 8;
    private const int FIELD_SIZE = MAX_POS_X * MAX_POS_Y;
    public bool DevelopeMode;
    public int DevelopeTeamID;
    private Coroutine _coroutineCalc;
    [SerializeField] private float _calcInterval = 2.5f;
    private WaitForSeconds _calcWaitTime;
    [SerializeField]
    private Transform _cubeParentTeam1, _cubeParentTeam2;

    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        _calcWaitTime = new WaitForSeconds(_calcInterval);

        if (DevelopeMode)
            HandleDevelopmentMode();
        else
            HandleProductionMode();
    }

    public void SetInfo(int team, int id)
    {
        _teamID = team;
        _playerID = id;
    }

    // 開発モードの初期化処理
    private void HandleDevelopmentMode()
    {
        _teamID = DevelopeTeamID;
        SetupPlayer(_teamID == 0 ? TEAM1_POS_Z : TEAM2_POS_Z, _herosPrefab[_teamID]);
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

        SetupPhotonPlayer(_teamID == 0 ? TEAM1_POS_Z : TEAM2_POS_Z, _herosPrefab[_teamID]);

        if (PhotonNetwork.isMasterClient) _coroutineCalc = StartCoroutine(CalcCubeShare());
    }

    // ローカルプレイヤーのセットアップ
    private void SetupPlayer(float myPosZ, GameObject heros)
    {
        SetupBlockManager();
        GameObject player = Instantiate(_playerPrefab, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity);
        _camManager.SetPlayer(player, _teamID);
        int enemyTeam = 1 - _teamID;
        player.GetComponent<Player>().SetParameter(heros, _cubeParentTeam1, _cubeParentTeam2, enemyTeam,DevelopeMode);
    }

    // ネットワークプレイヤーのセットアップ
    private void SetupPhotonPlayer(float myPosZ, GameObject heros)
    {
        if (PhotonNetwork.isMasterClient) SetupPhotonBlockManager();

        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity, 0);
        _camManager.SetPlayer(player, _teamID);
        int enemyTeam = 1 - _teamID;
        player.GetComponent<Player>().SetParameter(heros, _cubeParentTeam1, _cubeParentTeam2, enemyTeam, DevelopeMode);
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
        //ReduceTimeLimit();
    }

    // 制限時間の減少処理
    void ReduceTimeLimit()
    {
        _TimeLimit -= 1;
        _uiHandler.ShowLimitTime(_TimeLimit);
    }

    // オブジェクトの占有率を計算するコルーチン
    IEnumerator CalcCubeShare()
    {
        while (true)
        {
            yield return _calcWaitTime;
            int shareTeam1 = _blockManager.CalcCubeShare1(FIELD_SIZE);
            int shareTeam2 = _blockManager.CalcCubeShare2(FIELD_SIZE);
            Debug.Log("Team1 : " + shareTeam1 + " Team2 : " + shareTeam2);
            _uiHandler.ShowCalc(shareTeam1,shareTeam2);
        }
    }
}

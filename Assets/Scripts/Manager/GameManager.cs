using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private ColorManager _colorManager;
    [SerializeField] 
    private CamManager _camManager;
    [SerializeField] 
    private BlockManager _blockManager;
    [SerializeField] 
    private float _TimeLimit;
    public float _timeLimit => _TimeLimit;
    private UIHandler _uiHandler;
    [SerializeField] 
    private GameObject _playerPrefab;
    // 0...Team1 1...Team2
    [SerializeField]
    private GameObject[] _herosPrefab;
    private int _teamID;
    public const float TEAM1_POS_Z = -2.5f;
    public const float TEAM2_POS_Z = 1.5f;
    public bool DevelopeMode;
    public int DevelopeTeamID;

    void Start()
    {
        //_uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        //InvokeRepeating("ReduceTimeLimit", 0, 1);

        if (DevelopeMode) HandleDevelopmentMode();
        else HandleProductionMode();
    }

    private void HandleDevelopmentMode()
    {
        _teamID = DevelopeTeamID;
        if (_teamID == 0)  SetupPlayer(TEAM1_POS_Z, _herosPrefab[0]);
        else SetupPlayer(TEAM2_POS_Z, _herosPrefab[1]);
    }

    private void HandleProductionMode()
    {
        if (!PhotonNetwork.connected)
        {
            SceneManager.LoadScene("Launcher");
            return;
        }

        TeamHandler teamHandler = TeamHandler.InstanceTeamHandler;
        _teamID = teamHandler.TeamID;

        if (_teamID == 0)  SetupPhotonPlayer(TEAM1_POS_Z, _herosPrefab[0]);
        else SetupPhotonPlayer(TEAM2_POS_Z, _herosPrefab[1]);
        
    }

    private void SetupPlayer(float myPosZ, GameObject heros)
    {
        _blockManager.SetParam(myPosZ, DevelopeMode);
        GameObject player = Instantiate(_playerPrefab, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity);
        _camManager.SetPlayer(player);
        player.GetComponent<Player>().SetParameter(heros, DevelopeMode);
    }

    private void SetupPhotonPlayer(float myPosZ, GameObject heros)
    {
        _blockManager.SetParam(myPosZ, DevelopeMode);
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity, 0);
        _camManager.SetPlayer(player);
        player.GetComponent<Player>().SetParameter(heros, DevelopeMode);
    }

    void Update()
    {
        //ReduceTimeLimit();
    }

    void ReduceTimeLimit()
    {
        _TimeLimit -= 1;
        _uiHandler.ShowLimitTime(_TimeLimit);
    }
}
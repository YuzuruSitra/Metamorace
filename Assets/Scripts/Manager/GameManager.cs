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
    private int _teamID;
    private const float TEAM1_POS_Z = -2.5f;
    private const float TEAM2_POS_Z = 1.5f;
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
        if (_teamID == 0)  SetupPlayer(TEAM1_POS_Z, TEAM2_POS_Z);
        else SetupPlayer(TEAM2_POS_Z, TEAM1_POS_Z);
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

        if (_teamID == 0)  SetupPhotonPlayer(TEAM1_POS_Z, TEAM2_POS_Z);
        else SetupPhotonPlayer(TEAM2_POS_Z, TEAM1_POS_Z);
        
    }

    private void SetupPlayer(float myPosZ, float enemyPosZ)
    {
        _blockManager.SetParam(myPosZ);
        GameObject player = Instantiate(_playerPrefab, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity);
        _camManager.SetPlayer(player);
        player.GetComponent<Player>().SetEnemyPosZ(enemyPosZ);
    }

    private void SetupPhotonPlayer(float myPosZ, float enemyPosZ)
    {
        _blockManager.SetParam(myPosZ);
        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f, 1.25f, myPosZ), Quaternion.identity, 0);
        _camManager.SetPlayer(player);
        player.GetComponent<Player>().SetEnemyPosZ(enemyPosZ);
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
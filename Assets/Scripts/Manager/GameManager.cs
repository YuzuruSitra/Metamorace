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
    //ゲームの制限時間
    [SerializeField] 
    private float _TimeLimit;
    public float _timeLimit => _TimeLimit;
    UIHandler _uiHandler;
    [SerializeField]
    GameObject _playerPrefab; 
    private int _teamID;
    private const float TEAM1_POS_Z = -2.5f;
    private const float TEAM2_POS_Z = 1.5f;

    void Start()
    {
        //_uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        //制限時間を減らす
        //InvokeRepeating("ReduceTimeLimit", 0,1);
        if (!PhotonNetwork.connected)   //Phootnに接続されていなければ
        {
            SceneManager.LoadScene("Launcher"); //ログイン画面に戻る
            return; 
        }

        TeamHandler teamHandler = TeamHandler.InstanceTeamHandler;
        _teamID = teamHandler.TeamID;
        
        //Photonに接続していれば自プレイヤーを生成
        if(_teamID == 0) 
        {
            _blockManager.SetParam(TEAM1_POS_Z);
            GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f, 1.25f, TEAM1_POS_Z), Quaternion.identity, 0);
            _camManager.SetPlayer(player);
            player.GetComponent<Player>().SetEnemyPosZ(TEAM2_POS_Z);
        }
        else 
        {
            _blockManager.SetParam(TEAM2_POS_Z);
            GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, new Vector3(0f, 1.25f, TEAM2_POS_Z), Quaternion.identity, 0);
            _camManager.SetPlayer(player);
            player.GetComponent<Player>().SetEnemyPosZ(TEAM1_POS_Z);
        }
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

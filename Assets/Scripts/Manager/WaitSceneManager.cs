using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitSceneManager : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;

    [SerializeField]
    private GameObject _playerPrefab;
    private Player_Wait _playerWait;
    private int _playerCount = 0;
    private float _transitionTime = 2.0f;
    private WaitForSeconds _waitTime;
    public bool DebugMode;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.inRoom)
        {
            SceneManager.LoadScene("Launcher");
            return; 
        }
        SceneManager.sceneLoaded += OnLoadedScene;
        _waitTime = new WaitForSeconds(_transitionTime);

        //Photonに接続していれば自プレイヤーを生成
        GameObject Player = PhotonNetwork.Instantiate(this._playerPrefab.name, new Vector3(0f, 1.5f, 0f), Quaternion.identity, 0);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        _playerCount = players.Length;

        _playerWait = Player.GetComponent<Player_Wait>();
        _playerWait.OnReadyChanged += CheckIn;
        _playerWait.SetID(_playerCount);
    }

    // Update is called once per frame
    void CheckIn(bool state)
    {
        if (!state) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        _playerCount = players.Length;

        int team1 = 0;
        int team2 = 0;

        for (int i = 0; i < _playerCount; i++)
        {
            Player_Wait playerWait = players[i].GetComponent<Player_Wait>();
            // エリア外の人がいたら処理を返す
            if (!playerWait.IsReady) return;

            if (playerWait.SelectTeam == 0) team1++;
            else if (playerWait.SelectTeam == 1) team2++;
        }

        if (DebugMode)
            _myPV.RPC(nameof(SendScene), PhotonTargets.All, _playerCount);
        else
            // プレイヤーが2人以上で、Team1とTeam2に均等に割り振られ、全員が準備完了ならシーン遷移
            if (_playerCount >= 2 && team1 == team2 && team1 + team2 == _playerCount) _myPV.RPC(nameof(SendScene), PhotonTargets.All, _playerCount);
    }

    [PunRPC]
    private IEnumerator SendScene(int playerCount)
    {
        _playerCount = playerCount;
        PhotonNetwork.isMessageQueueRunning = false;
        yield return _waitTime;
        PhotonNetwork.LoadLevel("Master_Battle");
    }

    private void OnLoadedScene( Scene i_scene, LoadSceneMode i_mode )
    {
        if(i_scene.name != "Master_Battle") return;
        PhotonNetwork.isMessageQueueRunning = true;
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().SetInfo(_playerWait.SelectTeam, _playerWait.PlayerID, _playerCount);
    }

}

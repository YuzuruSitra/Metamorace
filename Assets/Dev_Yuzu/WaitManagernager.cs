using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitManagernager : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;

    [SerializeField]
    private GameObject _playerPrefab;
    private Player_Wait _playerWait;
    private int _playerCount;
    private float _transitionTime = 2.0f;
    private WaitForSeconds _waitTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.connected)   //Phootnに接続されていなければ
        {
            SceneManager.LoadScene("Launcher"); //ログイン画面に戻る
            return; 
        }
        SceneManager.sceneLoaded += OnLoadedScene;
        _waitTime = new WaitForSeconds(_transitionTime);

        //Photonに接続していれば自プレイヤーを生成
        GameObject Player = PhotonNetwork.Instantiate(this._playerPrefab.name, new Vector3(0f, 1.5f, 0f), Quaternion.identity, 0);
        _playerWait = Player.GetComponent<Player_Wait>();
        _playerWait.OnReadyChanged += CheckIn;
    }

    // Update is called once per frame
    void CheckIn(bool state)
    {
        if(!state) return;
        // タグが"Player"のオブジェクトを検索して配列に格納
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        // 配列の長さ（要素の数）を取得
        _playerCount = players.Length;
        int team1 = 0;
        int team2 = 0;
        for (int i = 0; i < _playerCount; i++)
        {
            Player_Wait playerWait = players[i].GetComponent<Player_Wait>();
            if (playerWait.SelectTeam == 0) team1 ++;
            else team2 ++;
            if (!playerWait.IsReady) return;
        }
        if (_playerCount <= 2 && team1 == 1 && team2 == 1) _myPV.RPC(nameof(SendScene), PhotonTargets.All);
        else if (team1 == 2 && team2 == 2) _myPV.RPC(nameof(SendScene), PhotonTargets.All);
        

    }

    [PunRPC]
    private IEnumerator SendScene()
    {
        yield return _waitTime;
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.LoadLevel("Master_Battle");
    }

    private void OnLoadedScene( Scene i_scene, LoadSceneMode i_mode )
    {
        PhotonNetwork.isMessageQueueRunning = true;
        GameObject.Find("GameManager").GetComponent<GameManager>().SetTeam(_playerWait.SelectTeam);
    }
}

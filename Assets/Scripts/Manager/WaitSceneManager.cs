using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitSceneManager : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;

    [SerializeField]
    private GameObject[] _playerPrefab = new GameObject[4];
    private Player_Wait _playerWait;
    private float _transitionTime = 1.0f;
    private WaitForSeconds _waitTime;
    public bool DebugMode;
    private string[] _memberList = new string[4];
    [SerializeField]
    private WaitUIHandler _waitUIHandler;
    // 入室した番号
    int _playerNum;
    
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
        _playerNum = PhotonNetwork.playerList.Length - 1;
        if (_playerNum >= _playerPrefab.Length) _playerNum = 3;
        GameObject Player = PhotonNetwork.Instantiate(this._playerPrefab[_playerNum].name, new Vector3(24.0f, -15.0f, 84.0f), Quaternion.identity, 0);

        _playerWait = Player.GetComponent<Player_Wait>();
        _playerWait.OnReadyChanged += CheckIn;
        _playerWait.SetID(PhotonNetwork.playerList.Length);
        UpdateMemberList();
    }

    // Update is called once per frame
    void CheckIn(bool state)
    {
        if (!state) return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        int team1 = 0;
        int team2 = 0;

        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            Player_Wait playerWait = players[i].GetComponent<Player_Wait>();
            // エリア外の人がいたら処理を返す
            if (!playerWait.IsReady) return;

            if (playerWait.SelectTeam == 0) team1++;
            else if (playerWait.SelectTeam == 1) team2++;
        }

        if (DebugMode)
            _myPV.RPC(nameof(SendScene), PhotonTargets.All);
        else
            // プレイヤーが2人以上で、Team1とTeam2に均等に割り振られ、全員が準備完了ならシーン遷移
            if (PhotonNetwork.playerList.Length >= 2 && team1 == team2 && team1 + team2 == PhotonNetwork.playerList.Length) _myPV.RPC(nameof(SendScene), PhotonTargets.All);
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
        if(i_scene.name != "Master_Battle") return;
        PhotonNetwork.isMessageQueueRunning = true;
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().SetInfo(_playerWait.SelectTeam, _playerWait.PlayerID, PhotonNetwork.playerList.Length);
    }

    // 名前の取得
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if (!PhotonNetwork.isMasterClient) return;
        Debug.Log(player.NickName + " is joined.");
        _myPV.RPC(nameof(CalleMemberList), PhotonTargets.All);
    }

    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        if (!PhotonNetwork.isMasterClient) return;
        Debug.Log(player.NickName + " is left.");
        _myPV.RPC(nameof(CalleMemberList), PhotonTargets.All);
    }

    [PunRPC]
    public void CalleMemberList()
    {
        UpdateMemberList();
    }

    void UpdateMemberList()
    {
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            _memberList[i] = PhotonNetwork.playerList[i].NickName;
            _waitUIHandler.SetMemberText(_memberList);
        }

        // それぞれの名前を描画
        _playerWait.CallShreName();
    }

}

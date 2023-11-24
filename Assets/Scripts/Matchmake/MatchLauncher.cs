using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
 
public class MatchLauncher : Photon.PunBehaviour
{
    #region Public変数定義
 
    //Public変数の定義はココで
 
    #endregion
 
    #region Private変数
    //Private変数の定義はココで
    [SerializeField]
    private string _sceneName;
    [SerializeField]
    private byte _maxPlayer = 4;

    #endregion
 
 
    #region Photonコールバック
 
    // ランダムマッチ
    public void OnJoinedRondom()        
    {
        Debug.Log("ロビーに入りました。");
        //Randomで部屋を選び、部屋に入る（部屋が無ければOnPhotonRandomJoinFailedが呼ばれる）
        PhotonNetwork.JoinRandomRoom(); 
    }
 
    //JoinRandomRoomが失敗したときに呼ばれる
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("ルームの入室に失敗しました。");
        //TestRoomという名前の部屋を作成して、部屋に入る
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayer;
        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }

    // プライベートルームへの参加する
    public void OnJoinPrivateRoom(string password)
    {
        RoomInfo[] privateRooms = PhotonNetwork.GetRoomList(); // 現在のルームリストを取得

        foreach (RoomInfo room in privateRooms)
        {
            if (room.CustomProperties.ContainsKey("password") && (string)room.CustomProperties["password"] == password)
            {
                PhotonNetwork.JoinRoom(room.Name); // 合言葉が一致するプライベートルームに参加
                return;
            }
        }

        Debug.Log("指定された合言葉のプライベートルームが見つかりませんでした。");
        
        // プライベートルームが見つからない場合は新しいプライベートルームを作成
        CreatePrivateRoom(password);
    }

    // プライベートルームを作成する
    private void CreatePrivateRoom(string password)
    {
        Debug.Log("PrivateRoomを作成しました。");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = _maxPlayer;
        //roomOptions.IsVisible = false; // プライベートルームは一覧に表示しない
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "password", password } };

        PhotonNetwork.CreateRoom(null, roomOptions, null);
    }
 
    //部屋に入った時に呼ばれる
    public override void OnJoinedRoom()
    {
        Debug.Log("ルームに入りました。");
        //battleシーンをロード
        PhotonNetwork.LoadLevel(_sceneName);
    }
 
    #endregion
}
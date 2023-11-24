using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private MatchLauncher _matchLuncher;
    //ゲームのバージョン。仕様が異なるバージョンとなったときはバージョンを変更しないとエラーが発生する。
    string _gameVersion = "test";
    static string playerNamePrefKey = "PlayerName";

    // プレイヤーの名前をセット
    private string _playerName;
    [SerializeField]
    private InputField _nameField;
    // ルームパスワードをセット
    private string _roomPas = "";
    [SerializeField]
    private InputField _roomPasField;

    // Start is called before the first frame update
    public void Awake()   
    {
        if (!PhotonNetwork.connected) {                         //Photonに接続できていなければ
            PhotonNetwork.ConnectUsingSettings(_gameVersion);   //Photonに接続する
            Debug.Log("Photonに接続しました。");
        }
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            _playerName = PlayerPrefs.GetString(playerNamePrefKey);
            _nameField.text = _playerName;
        }
    }
    
    public void InputPlayerName()
    {
        //テキストにinputFieldの内容を反映
        _playerName = _nameField.text;
        PlayerPrefs.SetString(playerNamePrefKey, _playerName);
        PhotonNetwork.playerName = _playerName;
    }

    public void InputRoomPas()
    {
        //テキストにinputFieldの内容を反映
        _roomPas = _roomPasField.text;
    }

    // プレイべートルームへ参加
    public void joinPrivateRoom()
    {
        _matchLuncher.OnJoinPrivateRoom(_roomPas);
    }

}

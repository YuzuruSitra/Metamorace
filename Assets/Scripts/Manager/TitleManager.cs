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
    private SoundHandler _soundHandler;
    [SerializeField]
    private AudioClip _titleBGM;
    [SerializeField]
    private AudioClip _buttonSE;
    [SerializeField]
    private AudioClip _inputSE;
    [SerializeField]
    private TitleUIHandler _titleUIHandler;

    [SerializeField]
    private Slider _sliderBGM;
    [SerializeField]
    private Slider _sliderSE;
    
    // Start is called before the first frame update
    public void Awake()   
    {
        if (!PhotonNetwork.connected) {                         //Photonに接続できていなければ
            PhotonNetwork.ConnectUsingSettings(_gameVersion);   //Photonに接続する
            Debug.Log("Photonに接続しました。");
        }
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            _playerName = PlayerPrefs.GetString(playerNamePrefKey);
            _nameField.text = _playerName;
        }
        _soundHandler = SoundHandler.InstanceSoundHandler;
        _soundHandler.PlayBGM(_titleBGM);
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

    public void joinRoom()
    {
        switch (_titleUIHandler.ModeState)
        {
            case 0:
                break;
            case 1:
                _matchLuncher.OnJoinedRondom();
                break;
            case 2:
                _matchLuncher.OnJoinPrivateRoom(_roomPas);
                break;
        }
    }

    public void PushSE()
    {
        _soundHandler.PlaySE(_buttonSE);
    }

    public void InputSE()
    {
        _soundHandler.PlaySE(_inputSE);
    }

    public void ChangeBGMValue()
    {
        _soundHandler.SetNewValueBGM(_sliderBGM.value);
    }
    public void ChangeSEValue()
    {
        _soundHandler.SetNewValueSE(_sliderSE.value);
    }

}

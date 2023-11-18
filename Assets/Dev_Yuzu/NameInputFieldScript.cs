using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class NameInputFieldScript : MonoBehaviour
{

#region Private変数定義
static string playerNamePrefKey = "PlayerName";
#endregion
#region MonoBehaviourコールバック
TMP_InputField _inputField;
void Start()
{
    string defaultName = "";   
    _inputField = this.GetComponent<TMP_InputField>();
    //前回プレイ開始時に入力した名前をロードして表示
    if (_inputField != null)
    {
        if (PlayerPrefs.HasKey(playerNamePrefKey))
        {
            defaultName = PlayerPrefs.GetString(playerNamePrefKey);
            _inputField.text = defaultName;
        }
    }
}
#endregion
#region Public Method
public void SetPlayerName(TMP_InputField inputField)
{
    string value = inputField.text; // InputFieldから値を取得
    PhotonNetwork.playerName = value + " "; // プレイヤー名を設定
    PlayerPrefs.SetString(playerNamePrefKey, value); // 名前をセーブ
    Debug.Log(PhotonNetwork.player.NickName);
}
#endregion
}
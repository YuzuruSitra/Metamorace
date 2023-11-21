using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitManagernager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab; 

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.connected)   //Phootnに接続されていなければ
        {
            SceneManager.LoadScene("Launcher"); //ログイン画面に戻る
            return; 
        }
        //Photonに接続していれば自プレイヤーを生成
        GameObject Player = PhotonNetwork.Instantiate(this._playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

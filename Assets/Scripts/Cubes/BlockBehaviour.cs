using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private int _objID;
    [SerializeField]
    private float _objHealth;

    [SerializeField]
    float _blocklength;
    //Vector3 back;
    void Start()
    {
        //Vector3 back = new Vector3(0, 0, -1);
    }
    //Playerによるお邪魔ブロック破壊処理
    public int DestroyBlock(float power, bool developMode)
    {
        _objHealth -= power * Time.deltaTime;
        // 同期処理
        if (!developMode) _myPV.RPC(nameof(SyncHealth), PhotonTargets.All, _objHealth);

        if (_objHealth >= 0) return -1;
        Destroy(this.gameObject);
        return _objID;
    }

    void Update()
    {
        MoveBlock();
    }

    public void MoveBlock()
    {
        Ray _move = new Ray(transform.position, new Vector3(0, 0, -1));
        //Ray _back = new Ray(transform.position, new Vector3(0, 0, -1));
        RaycastHit _hitmove;
        Debug.DrawRay(transform.position, new Vector3(0, 0, -1), Color.red, _blocklength);

        if (Physics.Raycast(_move, out _hitmove, _blocklength))
        {
            if (_hitmove.collider.CompareTag("Ambras") ||
            _hitmove.collider.CompareTag("Heros") || _hitmove.collider.CompareTag("ItemCBlock"))
            {
                Debug.Log("hit");
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
    }



    [PunRPC]
    private void SyncHealth(float currentHealth)
    {
        _objHealth = currentHealth;
        if (_objHealth <= 0) Destroy(this.gameObject);
    }
}

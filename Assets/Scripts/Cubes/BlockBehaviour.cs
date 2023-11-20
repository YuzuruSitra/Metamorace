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
    
    //Playerによるお邪魔ブロック破壊処理
    public int DestroyBlock(float power, bool developMode)
    {
        _objHealth -= power * Time.deltaTime;
        // 同期処理
        if (!developMode) _myPV.RPC(nameof(SynchroHealth), PhotonTargets.All, _objHealth);
        
        if(_objHealth >= 0) return -1;
        Destroy(this.gameObject);
        return _objID;         
    }

    [PunRPC]
    private void SynchroHealth(float currentHealth) 
    {
        _objHealth = currentHealth;
    }
}

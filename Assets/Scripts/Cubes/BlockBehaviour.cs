using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField] 
    private int _objID;
    [SerializeField] 
    private float _objHealth;
    
    //Playerによるお邪魔ブロック破壊処理
    public int DestroyBlock(float power)
    {
        _objHealth -= power * Time.deltaTime;
        if(_objHealth >= 0) return -1;
        return _objID;         
    }  
}

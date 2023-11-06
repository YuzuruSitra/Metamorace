using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectreference : MonoBehaviour
{
    //0自分のブロック、1敵のブロック
    [SerializeField] private int _objID;
    public int _ObjID => _objID;

    [SerializeField] private float _btPlayer,_btEnemy;

    //お邪魔ブロック破壊処理
    public void DestroyBlock()
    {
        if(_objID == 0)
        {
            _btPlayer -= Time.deltaTime;
            if(_btPlayer <= 0)
            {
                Destroy(gameObject);;
            }
        }
        else
        {
            _btEnemy -= Time.deltaTime;
             if(_btEnemy <= 0)
            {
                Destroy(gameObject);;
            }
        }
        
    }

    //お邪魔ブロック生成処理
    public void CreateBlock()
    {

    }
}

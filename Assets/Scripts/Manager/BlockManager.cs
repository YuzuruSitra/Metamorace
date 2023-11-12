using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] GameObject _Block;
    
     //ステージ上のオブジェクトの総数計算
    private int _BlockNum;
   
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SpawnBlock();
        }
    }
    public void CalcBlock()
    {

    }

    //ランダムに降ってくるお邪魔ブロック
    public void SpawnBlock()
    {
        Instantiate(_Block);
        _BlockNum += 1;
        Debug.Log(_BlockNum);
    }
}

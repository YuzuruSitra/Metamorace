using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] GameObject _block;
    
     //ステージ上のオブジェクトの総数計算
    [SerializeField] private int _blockNum;
    public int _BlockNum => _blockNum;
   
    void Start()
    {
       // InvokeRepeating("SpawnBlock", 0,2);
    }
    public void CalcBlock()
    {

    }

    //ランダムにお邪魔ブロック生成
    public void SpawnBlock()
    {
        Instantiate(_block);
        _blockNum += 1;
        Debug.Log(_blockNum);
    }

    //お邪魔ブロックが降ってくる処理（translate)
    public void FallBlock()
    {
        //transform.
    }
}

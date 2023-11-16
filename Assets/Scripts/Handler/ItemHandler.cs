using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    UIHandler _uiHandler;

    //保持ブロックを管理する配列
    private int[] _stackBlocks = new int[3];
    public int[] _StackBlocks => _stackBlocks;

 //スタックの中の自ブロックの数を格納
    int _myBrockNum;
    int _objID;
    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
    }

    //破壊したブロックをスタックする関数
    public void StackBlock(int _objID)
    {
        //0または1でない値を弾く（BlockBehaviourの DestroyBlock()参照）
        if(_objID == -1) return;

        for(int i = 0; i < _stackBlocks.Length; i++) 
        {
            //配列のi番目が空（０）だったら処理実行
            if(_stackBlocks[i] == 0)
            {
                _stackBlocks[i] = _objID;
                Debug.Log(string.Join(", ", _stackBlocks));
                 break;
            }
        }  
    }

    //所持しているブロックによって生成されるアイテム変化
    public void SelectItem()
    {       
        if(_stackBlocks[2] == 1 || _stackBlocks[2] == 2)
        {
            _myBrockNum = CheckMyBrock();
            if(_myBrockNum == 0)
            {
                //アイテムA
                Debug.Log("アイテムA生成");
            }
            else if(_myBrockNum == 3)
            {
                //アイテムC
                Debug.Log("アイテムC生成");
            }
            else
            {
                //アイテムB
                Debug.Log("アイテムB生成");
            }    
        }   
    }

    //自ブロックの数をカウント
    public int CheckMyBrock()
    {
        for(int i = 0; i < _stackBlocks.Length; i++) 
        {
            //配列のi番目が空（０）だったら処理実行
            if(_stackBlocks[i] == 1)
            {
               _myBrockNum += 1;
            }
        }  
        return _myBrockNum;
    }
}

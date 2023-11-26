using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    UIHandler _uiHandler;
    //保持ブロックを管理する配列
    private int[] _stackBlocks = {0,0,0};
    public int[] _StackBlocks => _stackBlocks;
   
    
    //[SerializeField] ItemC _itemc;
    //スタックの中の自ブロックの数を格納
    int _myBrockNum;
    int _objID;
    //float _itemAEffectTime = 6.0f;

    bool _hasItemA,_hasItemB,_hasItemC = false;
    public bool _HasItemA => _hasItemA;
    public bool _HasItemB => _hasItemB;
    public bool _HasItemC => _hasItemC;
    private float _itemAEffectTime = 6.0f;
    public float _ItemAEffectTime => _itemAEffectTime;

    private float _itemCEffectTime = 2.0f;
    public float _ItemCEffectTime => _itemCEffectTime;

    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        //デバッグ用
        //_hasItemB = true;
    }

    //破壊したブロックをスタックする関数
    public void StackBlock(int _objID)
    {        
        //1または2でない値を弾く（BlockBehaviourの DestroyBlock()参照）
        if(_objID == -1) return;
       
        for(int i = 0; i < _stackBlocks.Length; i++) 
        {          
            //配列のi番目が空（０）だったら処理実行
            if(_stackBlocks[i] == 0)
            {
                _stackBlocks[i] = _objID;
                _uiHandler.SetStackImage(_objID);
                 break;
            }
        }  
    }

    //スタックをリセットする関数（item使用時によびだし）
    public void ResetBlock()
    {
         for(int i = 0; i < _stackBlocks.Length; i++) 
        {
            //配列のi番目が空（０）だったら処理実行
           
                _stackBlocks[i] = 0;      
        }   
    }

    //所持しているブロックによって生成されるアイテム変化
    public void CreateItem()
    {   
        //スタックイメージが満杯だったら    
        if(_stackBlocks[2] == 1 || _stackBlocks[2] == 2)
        {
            _myBrockNum = CheckMyBrock();
            if(_myBrockNum == 0)
            {
                //アイテムA
                 _uiHandler.SetItemImage(0);
                _uiHandler.ResetStackImage();
                ResetBlock();
                _hasItemA = true;
            }
            else if(_myBrockNum == 3)
            {
                //アイテムB
                 _uiHandler.SetItemImage(2);
                 _uiHandler.ResetStackImage();
                ResetBlock();
                _hasItemB = true;
            }
            else
            {
                 //アイテムC
                 _uiHandler.SetItemImage(1);
                 _uiHandler.ResetStackImage();
                ResetBlock();
                _hasItemC = true;
                
            }    
            //カウントリセット
            _myBrockNum = 0;
           
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
    public void ItemEffectA(ref float _destroyPower, ref float _playerSpeed)
     {
        //if (!_hasItemC) return;        
        _uiHandler.ResetItemImage();   
        _destroyPower = _destroyPower*2;
        _playerSpeed = _playerSpeed*2;
       
        _hasItemA = false;        
        //return _destroyPower;   
     }
     public void ItemEffectB()
     { 
            _uiHandler.ResetItemImage();         
            _hasItemB = false;
     }
     public void ItemEffectC()
     {
         _uiHandler.ResetItemImage();
        _hasItemC = false;
     }

    //アイテムCの効果抽選 Playerクラスで呼び出す
     public int ChoseEffectC()
     {
        //Cのエフェクト選定
        int _effectid = Random.Range(1,2);
        //ここ修正
       
         return _effectid; 
     }
    // public void ItemEffectC(ref float _playerSpeed)
    // {
      
            
    //        _playerSpeed = _playerSpeed*0;       
          
    // }
          
}

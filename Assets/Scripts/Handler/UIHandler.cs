using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField] private Text _LimitTimeText,_BlockRate;
    [SerializeField] Image[] _StackImage;
    //アンブラスとヘイロスのブロック画像、_objID1がヘイロス2がアンブラス
    [SerializeField] Sprite _herosSprite,_ambrasSprite;

     [SerializeField] Image _itemImage;
     [SerializeField] Sprite[] _itemSprite;
    [SerializeField] GameObject _button;

    public void ShowLimitTime(float _TimeLimit)
    {
        _LimitTimeText.text = _TimeLimit.ToString("f1") + "秒";
    }

    //アンブラスとヘイロスのスプライトを格納
    public void SetStackImage(int _objID)
    {
        for(int i = 0; i < _StackImage.Length; i++)
        {
            if(IsEmpty(i))
            {
                if(_objID == 1)
                {
                    _StackImage[i].sprite = _herosSprite;
                }
                else if(_objID == 2)
                {
                     _StackImage[i].sprite = _ambrasSprite;
                }
                else Debug.Log("Null _objID");
                
                break;
            }
        }       
    }   
    //アンブラスとヘイロスのスプライトをリセット
     public void ResetStackImage()
    {  
        for(int i = 0; i < _StackImage.Length; i++)
        {
            _StackImage[i].sprite = null;
        }   
        
    }
    //アイテムの画像を格納
    public void SetItemImage(int _itemnum)
    {
        _itemImage.sprite = _itemSprite[_itemnum];
    }   
    //アイテムの画像をリセット
    public void ResetItemImage()
    {
        _itemImage.sprite = null;
    }   
    //開いている枠を調べる
    public bool IsEmpty(int _StackNum)
    {
        if(_StackImage[_StackNum].sprite == null)
        {
            return true;
        }
        return false;
    }
    //カラーボタンを押した際の処理
    public void ActiveFalseButton()
   {
        _button.SetActive(false);
   }
}

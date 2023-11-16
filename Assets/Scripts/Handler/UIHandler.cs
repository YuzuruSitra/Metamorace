using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField] private Text _LimitTimeText,_BlockRate;
    [SerializeField] Image[] _StackImage;
    [SerializeField] GameObject _button;

    

    public void ShowLimitTime(float _TimeLimit)
    {
        _LimitTimeText.text = _TimeLimit.ToString("f1") + "秒";
    }

    //スプライトを格納
    public void SetStackImage(Sprite sprite)
    {
        for(int i = 0; i < _StackImage.Length; i++)
        {
            if(IsEmpty(i))
            {
                _StackImage[i].sprite = sprite;
                break;
            }
        }       
    }   

    //アイテムの画像を格納
    public void SetItemImage(Sprite sprite)
    {
        
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

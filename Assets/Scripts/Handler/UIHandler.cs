using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIHandler : MonoBehaviour
{
    [SerializeField] private Text _LimitTimeText;
    [SerializeField] private Text _BlockRateTeam1, _BlockRateTeam2;
    [SerializeField] Image[] _StackImage;
    //アンブラスとヘイロスのブロック画像、_objID1がヘイロス2がアンブラス
    [SerializeField] Sprite _herosSprite, _ambrasSprite;

    [SerializeField] Text _nametext;
    [SerializeField] string name;
    [SerializeField] Image _itemImage;
    [SerializeField] Sprite[] _itemSprite;
    [SerializeField] GameObject _button;
    //下記リザルト用
    [SerializeField] GameObject _resultPanel;
    [SerializeField] GameObject _winnerPanel;
    [SerializeField] Text _winBlockRate, _loseBlockRate;
    [SerializeField] GameObject itemeffect;
    void Start() 
    {
        
        _resultPanel.SetActive(false);
        //DecreceGage();
    }

    public void GetItemEffect()
    {
       // itemeffect.Animator()
    }
    public void ShowLimitTime(float _TimeLimit)
    {
        _LimitTimeText.text = _TimeLimit.ToString("f1") + "秒";
    }

    //アンブラスとヘイロスのスプライトを格納
    public void SetStackImage(int _objID)
    {
        for (int i = 0; i < _StackImage.Length; i++)
        {
            if (IsEmpty(i))
            {
                if (_objID == 1)
                {
                    _StackImage[i].sprite = _herosSprite;
                }
                else if (_objID == 2)
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
        for (int i = 0; i < _StackImage.Length; i++)
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
        if (_StackImage[_StackNum].sprite == null)
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
    //名前表示
    public void ChangeName()
    {
        _nametext.text = name;
    }
    //占有率表示
    public void ShowCalc(int shareTeam1, int shareTeam2)
    {
        string ShareTeam1 = shareTeam1.ToString();
        string ShareTeam2 = shareTeam2.ToString();
        _BlockRateTeam1.text = ShareTeam1;
        _BlockRateTeam2.text = ShareTeam2;
    }

    public void ShowResult(int shareTeam1, int shareTeam2, bool isDead, int winteam)
    {
        // Debug.Log(shareTeam1);
        // Debug.Log(shareTeam2);
        float WinnerSize = 1.2f;
        //時間制限が来た時
        if (!isDead)
        {
            //占有率でTeam1が勝ったとき
            if (shareTeam2 > shareTeam1)
            {
                _winnerPanel.transform.localScale = _winnerPanel.transform.localScale * WinnerSize;
                _winBlockRate.text = shareTeam1.ToString();
                _loseBlockRate.text = shareTeam2.ToString();
                Debug.Log("Team1Rate");
            }
            //占有率でTeam2が勝ったとき
            else if (shareTeam1 > shareTeam2)
            {
                _winnerPanel.transform.localScale = _winnerPanel.transform.localScale * WinnerSize;
                _winBlockRate.text = shareTeam2.ToString();
                _loseBlockRate.text = shareTeam1.ToString();
                Debug.Log("Team2Rate");
            }
            //引き分けの時
            else Debug.Log("Draw");
        }
        //どちらかのチームで死者が出た時
        else
        {
            //Team1が勝ったとき
            if (winteam == 1)
            {
                _winnerPanel.transform.localScale = _winnerPanel.transform.localScale * WinnerSize;
                _winBlockRate.text = shareTeam1.ToString();
                _loseBlockRate.text = "Dead";
                //_loseBlockRate.text = shareTeam2.ToString();
            }
            //Team2が勝ったとき
            else
            {
                _winnerPanel.transform.localScale = _winnerPanel.transform.localScale * WinnerSize;
                _winBlockRate.text = shareTeam2.ToString();
                _loseBlockRate.text = "Dead";
                //_loseBlockRate.text = shareTeam1.ToString();
            }
        }
        _resultPanel.SetActive(true);
    }
}

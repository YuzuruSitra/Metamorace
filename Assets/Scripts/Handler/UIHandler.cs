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
    [SerializeField] Image _BlockImage;
    [SerializeField] Sprite[] _itemSprite;
    //下記リザルト用
    [SerializeField] GameObject _resultPanel;
    [SerializeField] GameObject _winandlose, _draw;
    [SerializeField] Text _winBlockRate, _loseBlockRate, _drawBlockRate1, _drawBlockRate2;
    [SerializeField] GameObject itemeffect;
    Animator itemEffectAnimator;
    private Color toumei = new Color(1f, 1f, 1f, 0f);
    void Start()
    {
        itemeffect.SetActive(false);
        itemEffectAnimator = itemeffect.GetComponent<Animator>();
        _resultPanel.SetActive(false);
        _winandlose.SetActive(false);
        _draw.SetActive(false);
        //GetItemEffect();
        //GetItemEffect();
        //DecreceGage();
        _BlockImage.color = toumei;
    }

    //保持しているブロック画像表示
    public void BlockImage(int _objid)
    {
        if(_objid == 1)
        {
            _BlockImage.sprite = _ambrasSprite;
            _BlockImage.color = Color.white;
        }
        else
        {
            _BlockImage.sprite = _herosSprite;
            _BlockImage.color = Color.white;
        }
    }
    //保持しているブロック画像null
    public void ResetBlockImage()
    {
        _BlockImage.sprite = null;
        _BlockImage.color = toumei;
    }

    public void GetItemEffect()
    {
        // itemEffectAnimator.SetBool("G",true);
        // itemEffectAnimator.SetBool("G",false);
        itemeffect.SetActive(true);
        Invoke("Activefalse", 0.40f);
        Debug.Log("a");
    }
    public void Activefalse()
    {
        itemeffect.SetActive(false);
    }
    public void ShowLimitTime(float _TimeLimit)
    {
        _LimitTimeText.text = _TimeLimit.ToString("f0") + "秒";
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
                    _StackImage[i].color = Color.white;
                }
                else if (_objID == 2)
                {
                    _StackImage[i].sprite = _ambrasSprite;
                    _StackImage[i].color = Color.white;
                }
                else
                {

                }
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
            _StackImage[i].color = toumei;
        }

    }
    //アイテムの画像を格納
    public void SetItemImage(int _itemnum)
    {
        if (_itemSprite[_itemnum] == null) return;
        _itemImage.sprite = _itemSprite[_itemnum];
        _itemImage.color = Color.white;
    }
    //アイテムの画像をリセット
    public void ResetItemImage()
    {
        _itemImage.sprite = null;
        _itemImage.color = toumei;
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
        _BlockRateTeam1.text = ShareTeam1 + "%";
        _BlockRateTeam2.text = ShareTeam2 + "%";
    }

    public void ShowResult(int shareTeam1, int shareTeam2, bool isDead, int winteam)
    {
        float WinnerSize = 1.2f;
        //時間制限が来た時
        if (!isDead)
        {
            //占有率でTeam1が勝ったとき
            if (shareTeam2 > shareTeam1)
            {
                _winandlose.SetActive(true);
                _winBlockRate.text = shareTeam1.ToString() + "%";
                _loseBlockRate.text = shareTeam2.ToString() + "%";
            }
            //占有率でTeam2が勝ったとき
            else if (shareTeam1 > shareTeam2)
            {
                _winandlose.SetActive(true);
                _winBlockRate.text = shareTeam2.ToString() + "%";
                _loseBlockRate.text = shareTeam1.ToString() + "%";
            }
            //引き分けの時
            else;
            {
                _draw.SetActive(true);
                _drawBlockRate1.text = shareTeam1.ToString() + "%";
                _drawBlockRate2.text = shareTeam2.ToString() + "%";
            }
        }
        //どちらかのチームで死者が出た時
        else
        {
            //Team1が勝ったとき
            if (winteam == 1)
            {
                _winandlose.SetActive(true);
                _winBlockRate.text = shareTeam1.ToString();
                _loseBlockRate.text = "Dead";
                //_loseBlockRate.text = shareTeam2.ToString();
            }
            //Team2が勝ったとき
            else
            {
                _winandlose.SetActive(true);
                _winBlockRate.text = shareTeam2.ToString();
                _loseBlockRate.text = "Dead";
                //_loseBlockRate.text = shareTeam1.ToString();
            }
        }
        _resultPanel.SetActive(true);
    }
}

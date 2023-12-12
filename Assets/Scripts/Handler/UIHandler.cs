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
    [SerializeField] Sprite _herosSprite, _ambrasSprite, _itemCSprite;

    [SerializeField]
    private Text[] _nameTexts = new Text[4];
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
    //ItemB用のスケール変更
    Vector3 currentScale;
    float scaleFactor = 1.0f;

    //  アンブラスのID
    public const int _ambrassID = 1;
    //  ヒーロスのID
    public const int _herosID = 2;
    //それぞれのアイテムのID
    public const int _itemAID = 1;
    public const int _itemBID = 2;
    public const int _itemCID = 3;
    void Start()
    {
        currentScale = _BlockImage.transform.localScale;
        _BlockImage.transform.localScale = new Vector3(currentScale.x * scaleFactor, currentScale.y * scaleFactor, currentScale.z);
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
        if (_objid == _ambrassID)
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
    }
    public void Activefalse()
    {
        itemeffect.SetActive(false);
    }
    public void ShowLimitTime(float _TimeLimit)
    {
        _LimitTimeText.text = _TimeLimit.ToString("f0") + "秒";
    }
    //アイテムC使用時のUI変化
    public void ItemUI(int ItemNum)
    {
        //  アイテムUI削除、所持ブロックをCに変更
        ResetItemImage();
        if (ItemNum == _itemBID)
        {
            Debug.Log("2");
            scaleFactor = 1.5f; // ブロックイメージ大きくする 
        }
        //Imageをでかくする処理
        else if (ItemNum == _itemCID)
        {
            _BlockImage.sprite = _itemCSprite;

        }
    }
    //アンブラスとヘイロスのスプライトを格納
    public void SetStackImage(int _objID)
    {
        for (int i = 0; i < _StackImage.Length; i++)
        {
            if (IsEmpty(i))
            {
                if (_objID == _ambrassID)
                {
                    _StackImage[i].sprite = _ambrasSprite;
                    _StackImage[i].color = Color.white;
                }
                else if (_objID == _herosID)
                {
                    _StackImage[i].sprite = _herosSprite;
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
        //配列に辻褄数合わせのー１
        _itemnum = _itemnum - 1;
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

    // UIに名前とチームを描画する処理
    public void SetNames(string[] names, string[] IDs)
    {
        int team0 = 0;
        int team1 = 2;
        for (int i = 0; i < names.Length; i++)
        {
            switch (IDs[i])
            {
                case "Team0":
                    _nameTexts[team0].text = names[i];
                    team0++;
                    break;
                case "Team1":
                    _nameTexts[team1].text = names[i];
                    team1++;
                    break;
                default:
                    break;
            }
        }
    }

}

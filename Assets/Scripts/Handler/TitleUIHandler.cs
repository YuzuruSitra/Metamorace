using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUIHandler : MonoBehaviour
{
    private int _modeState;
    public int ModeState => _modeState;
    [SerializeField]
    private Image[] _buttonImage = new Image[2];
    [SerializeField]
    private Sprite[] _randomSprite = new Sprite[2];
    [SerializeField]
    private Sprite[] _privateSprite = new Sprite[2];
    Color _notSelectColor = new Color(0.8f, 0.8f, 0.8f, 1.0f);
    [SerializeField]
    private GameObject _defaultPanel;
    [SerializeField]
    private GameObject _pasPanel;
    [SerializeField]
    private GameObject _settingPanel;
    [SerializeField]
    private GameObject _infoPanel;

    // Start is called before the first frame update
    void Start()
    {
        _modeState = 1;
        _buttonImage[0].sprite = _randomSprite[1];
        _buttonImage[1].sprite = _privateSprite[0];
    }

    public void SetRandomRoom()
    {
        _modeState = 1;
        _buttonImage[0].sprite = _randomSprite[1];
        _buttonImage[1].sprite = _privateSprite[0];
    }
    public void SetPrivateRoom()
    {
        _modeState = 2;
        _buttonImage[0].sprite = _randomSprite[0];
        _buttonImage[1].sprite = _privateSprite[1];
        _defaultPanel.SetActive(false);
        _pasPanel.SetActive(true);
    }

    public void ClosePasPannel()
    {
        _defaultPanel.SetActive(true);
        _pasPanel.SetActive(false);
    }

    public void OpenSetting()
    {
        _defaultPanel.SetActive(false);
        _settingPanel.SetActive(true);
    }
    public void CloseSetting()
    {
        _defaultPanel.SetActive(true);
        _settingPanel.SetActive(false);
    }
    public void OpenInfo()
    {
        _defaultPanel.SetActive(false);
        _infoPanel.SetActive(true);
    }
    public void CloseInfo()
    {
        _defaultPanel.SetActive(true);
        _infoPanel.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitUIHandler : MonoBehaviour
{
    private bool _isOpenPanel = false;
    public bool IsOpenPanel => _isOpenPanel;

    [SerializeField]
    private Text[] _memberText = new Text[4];

    [SerializeField]
    private GameObject _exitPanel;
    private bool _isOpenExitPanel = false;
    [SerializeField]
    private GameObject _settingPanel;
    private bool _isOpenSettingPanel = false;
    [SerializeField]
    private GameObject _infoPanel;
    private bool _isOpenInfoPanel = false;
    [SerializeField]
    private Dropdown _dropdownNPC;
    private bool _addNPC = false;
    public bool _AddNPC => _addNPC;

    void Update()
    {
         AddNPCDropList();
        //
         //Debug.Log(_addNPC);
    }
    public void SetMemberText(string[] member)
    {
        if (member.Length > 4) return;
        for (int i = 0; i < member.Length; i++)
        {
            _memberText[i].text = member[i];
        }
    }

    public void ChangeStateExitPanel()
    {
        _isOpenExitPanel = !_isOpenExitPanel;
        _isOpenPanel = _isOpenExitPanel;
        _exitPanel.SetActive(_isOpenExitPanel);
    }

    public void ChangeStateSettingPanel()
    {
        _isOpenSettingPanel = !_isOpenSettingPanel;
        _isOpenPanel = _isOpenSettingPanel;
        _settingPanel.SetActive(_isOpenSettingPanel);
    }

    public void ChangeStateInfoPanel()
    {
        _isOpenInfoPanel = !_isOpenInfoPanel;
        _isOpenPanel = _isOpenInfoPanel;
        _infoPanel.SetActive(_isOpenInfoPanel);
    }

    //WaitSceneManageに書いた方がいいかも
    public void AddNPCDropList()
    {
        //NPCなし
        if(_dropdownNPC.value == 0)
        {
            _addNPC = false;
        }
        //あり
        else
        {
            _addNPC = true;
        }
    }

}

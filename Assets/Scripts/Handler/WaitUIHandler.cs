using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitUIHandler : MonoBehaviour
{
    [SerializeField]
    private Text[] _memberText = new Text[4];


    public void SetMemberText(string[] member)
    {
        if (member.Length > 4) return;
        for (int i = 0; i < member.Length; i++)
        {
            _memberText[i].text = member[i];
        }
    }
}

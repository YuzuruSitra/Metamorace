using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHangler : MonoBehaviour
{
    [SerializeField]
    private PlayerDataReceiver _playerDataReceiver;
    [SerializeField] 
    private GameObject _staneffect;
    [SerializeField]
    private Animator _stanEffectAnim;

    [SerializeField] 
    private GameObject _saiyaeffect;
    private const float EFFECT_POS_Z = 0.4f;
    private Vector3 staneOffSetTeam0 = new Vector3(0, 0.8f, -EFFECT_POS_Z);
    private Vector3 staneOffSetTeam1 = new Vector3(0, 0.8f, EFFECT_POS_Z);
    private Vector3 saiyaOffSetTeam0 = new Vector3(0, 1.2f, -EFFECT_POS_Z);
    private Vector3 saiyaOffSetTeam1 = new Vector3(0, 1.2f, EFFECT_POS_Z);
    private bool _animStan = false;
    public bool AnimStan => _animStan;

    void Update()
    {
        // エフェクト移動
        if(_staneffect.activeSelf)
        {
            if (_playerDataReceiver.MineTeamID == 0) _staneffect.transform.position = transform.position + staneOffSetTeam0;
            else _staneffect.transform.position = transform.position + staneOffSetTeam1;
        }
        
        if (_saiyaeffect.activeSelf)
        {
            if (_playerDataReceiver.MineTeamID == 0) _saiyaeffect.transform.position = transform.position + saiyaOffSetTeam0;
            else _saiyaeffect.transform.position = transform.position + saiyaOffSetTeam1;
        }
    }

    public void ChangeStan(bool state)
    {
        _stanEffectAnim.SetBool("Stan", state);
        _animStan = state;
    }

    public void ChangeSaiya(bool state)
    {
        _saiyaeffect.SetActive(state);
    }
}

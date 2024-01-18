using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHangler : MonoBehaviour
{
    [SerializeField]
    private PlayerDataReceiver _playerDataReceiver;
    [SerializeField] 
    private GameObject _stanEffect;
    [SerializeField]
    private Animator _stanEffectAnim;

    [SerializeField] 
    private GameObject _saiyaEffect;
    [SerializeField] 
    private GameObject _dieEffect;

    private const float EFFECT_POS_Z = 0.4f;
    private Vector3 staneOffSetTeam0 = new Vector3(0, 0.8f, -EFFECT_POS_Z);
    private Vector3 staneOffSetTeam1 = new Vector3(0, 0.8f, EFFECT_POS_Z);
    private Vector3 saiyaOffSetTeam0 = new Vector3(0, 1.2f, -EFFECT_POS_Z);
    private Vector3 saiyaOffSetTeam1 = new Vector3(0, 1.2f, EFFECT_POS_Z);
    private Vector3 dieOffSetTeam0 = new Vector3(0, 1.2f, -EFFECT_POS_Z);
    private Vector3 dieOffSetTeam1 = new Vector3(0, 1.2f, EFFECT_POS_Z);
    private bool _animStan = false;
    public bool AnimStan => _animStan;

    void Update()
    {
        // エフェクト移動
        if (_stanEffect.activeSelf)
        {
            if (_playerDataReceiver.MineTeamID == 0) _stanEffect.transform.position = transform.position + staneOffSetTeam0;
            else _stanEffect.transform.position = transform.position + staneOffSetTeam1;
        }
        
        if (_saiyaEffect.activeSelf)
        {
            if (_playerDataReceiver.MineTeamID == 0) _saiyaEffect.transform.position = transform.position + saiyaOffSetTeam0;
            else _saiyaEffect.transform.position = transform.position + saiyaOffSetTeam1;
        }

        if (_dieEffect.activeSelf)
        {
            if (_playerDataReceiver.MineTeamID == 0) _dieEffect.transform.position = transform.position + dieOffSetTeam0;
            else _dieEffect.transform.position = transform.position + dieOffSetTeam1;
        }
    }

    [PunRPC]
    public void ChangeStan(bool state)
    {
        _stanEffect.SetActive(state);
        _stanEffectAnim.SetBool("Stan", state);
        _animStan = state;
    }

    void ChangeStanAnimState(bool state)
    {
        _stanEffect.SetActive(state);
        _stanEffectAnim.SetBool("Stan", state);
    }

    [PunRPC]
    public void ChangeSaiya(bool state)
    {
        _saiyaEffect.SetActive(state);
    }

    [PunRPC]
    public void ChangeDie(bool state)
    {
        _dieEffect.SetActive(state);
    }
}

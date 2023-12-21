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
    private GameObject _saiyaeffect;
    [SerializeField]
    private Animator _stanEffect;
    [SerializeField]
    private float _stanpos = 0.3f;
    private bool _animStan = false;
    public bool AnimStan => _animStan;


    // Start is called before the first frame update
    void Start()
    {
        if (_playerDataReceiver.MineTeamID == 0) _staneffect.transform.position = transform.position + new Vector3(0, 0, -_stanpos);
        else _staneffect.transform.position = transform.position + new Vector3(0, 0, _stanpos);
    }

    public void ChangeStan(bool state)
    {
        _stanEffect.SetBool("Stan", state);
        _animStan = state;
    }

    public void ChangeSaiya(bool state)
    {
        _saiyaeffect.SetActive(state);
    }
}

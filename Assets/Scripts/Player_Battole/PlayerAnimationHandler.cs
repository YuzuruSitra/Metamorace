using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    
    [SerializeField]
    private Animator _playerAnim;
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private PlayerMover _playerMover;
    [SerializeField]
    private PlayerObjectManipulator _playerObjectManipulator;
    [SerializeField]
    private PlayerEffectHangler _playerEffectHangler;
    [SerializeField]
    private PlayerDataReceiver _playerDataReceiver;
    [SerializeField]
    private PlayerDeathDetector _playerDeathDetector;

    void Update()
    {
        if (!_myPV.isMine) return;
        AnimSelecter();
    }

    private void AnimSelecter()
    {
        int animCase = 0;
        if (_playerDeathDetector.VerticalDeath)
        {
            animCase = 0;
        }
        else if(_playerDeathDetector.HorizontalDeath)
        {
            animCase = 1;
        }
        else if(_playerEffectHangler.AnimStan)
        {
            animCase = 2;
        }
        else if(_playerObjectManipulator.AnimSwing)
        {
            animCase = 3;
        }
        else if(!_playerMover.OnGround)
        {
            animCase = 4;
        }
        else if(_playerObjectManipulator.AnimBreak)
        {
            animCase = 5;
        }
        else if(!_playerMover.IsMoving)
        {
            animCase = 6;
        }
        else
        {
            animCase = 7;
        }

        _myPV.RPC(nameof(ShareAnimBool), PhotonTargets.All, animCase);
    }

    [PunRPC]
    private void ShareAnimBool(int animCase)
    {
        switch (animCase)
        {
            case 0:
                _playerAnim.SetBool("_isVDeath", true);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", false);
                break;
            case 1:
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", true);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", false);
                break;
            case 2:
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", true);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", false);
                break;
            case 3:
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", true);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", false);
                break;
            case 4:
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", true);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", false);
                break;
            case 5:
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", true);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", false);
                _playerAnim.SetFloat("MoveSpeed", 0.0f);
                break;
            case 6:
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", true);
                _playerAnim.SetBool("_isWalk", false);
                _playerAnim.SetFloat("MoveSpeed", 0.0f);
                break;
            case 7:
                _playerAnim.SetBool("_isVDeath", false);
                _playerAnim.SetBool("_isHDeath", false);
                _playerAnim.SetBool("_isStan", false);
                _playerAnim.SetBool("_isSwing", false);
                _playerAnim.SetBool("_isJump", false);
                _playerAnim.SetBool("_isBreak", false);
                _playerAnim.SetBool("_isIdole", false);
                _playerAnim.SetBool("_isWalk", true);
                _playerAnim.SetFloat("MoveSpeed", 1.0f);
                break;
        }

    }
}

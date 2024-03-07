using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationHandler : MonoBehaviour
{
     [SerializeField]
    private Animator _playerAnim;
    // [SerializeField]
    // private PhotonView _myPV;
    [SerializeField]
    private NPCMover _npcMover;
    [SerializeField]
    private NPCObjectManipulator _npcObjectManipulator;
    [SerializeField]
    private NPCEffectHandler _npcEffectHandler;
    [SerializeField]
    private NPCDataReceiver _playerDataReceiver;
    [SerializeField]
    private NPCDeathDetector _npcDeathDetector;

    void Update()
    {
       // if (!_myPV.isMine) return;
        //if(!_playerDataReceiver.IsActiveGame) return;
        AnimSelecter();
    }

    // 時短につき良くない実装
    private void AnimSelecter()
    {
        int animCase = 0;
        if (_npcDeathDetector.VerticalDeath)
        {
            animCase = 0;
        }
        else if(_npcDeathDetector.HorizontalDeath)
        {
            animCase = 1;
        }
        else if(_npcEffectHandler.AnimStan)
        {
            animCase = 2;
        }
        else if(_npcObjectManipulator.AnimSwing)
        {
            animCase = 3;
        }
        else if(!_npcMover.OnGround)
        {
            animCase = 4;
        }
        else if(_npcObjectManipulator.AnimBreak)
        {
            animCase = 5;
        }
        else if(!_npcMover.IsMoving)
        {
            animCase = 6;
        }
        else
        {
            animCase = 7;
        }

        //_myPV.RPC(nameof(ShareAnimBool), PhotonTargets.All, animCase);
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

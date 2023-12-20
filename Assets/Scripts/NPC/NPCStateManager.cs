using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateManager : MonoBehaviour
{
    float _interval = 0;
    bool _isInterval = false;
    [SerializeField] NPCPlayer _NPClayer;
     public enum NPCState
    {
        WAIT,	//行動を一旦停止
        MOVE,	//移動
        CREATE,	//生成
        BREAK,	//破壊
        IDLE,	//待機
        AVOID,	//回避
        JUMP,//ジャンプ
    }
    public NPCState npcState;

    void Start()
    {
         npcState = NPCState.JUMP;
    }
    void Update()
    {
         switch (npcState)
        {
            case NPCState.MOVE:
                _NPClayer.PlayerCtrl();
                Debug.Log("1");
                // if(_interval <= 0){
                //     npcState =  NPCState.JUMP;
                // }
                if(_NPClayer._hitBlock){
                  npcState =  NPCState.BREAK;
                }
                break;
            case NPCState.BREAK:
                _NPClayer.BreakBlock();
                //Debug.Log("2");
                if(_NPClayer._hasBlock){
                  npcState =  NPCState.CREATE;
                }
                break;
            case NPCState.CREATE:
                _NPClayer.CreateBlock();
                if(!_NPClayer._hasBlock){
                     Debug.Log("3");
                    npcState = NPCState.MOVE;
                }
                break;
            case NPCState.JUMP:
                if(!_NPClayer._isJump)
                {
                    Debug.Log("4");
                    _NPClayer.Jump();
                    npcState = NPCState.MOVE;
                }
                break;
        }
    }

    void IntervalTime()
    {
        if(_isInterval) return;
        _interval = Random.Range(1.0f,3.0f);
        _interval -= Time.deltaTime;
        if(_interval<=0)
        {
            _isInterval = false;
        }
    }

}

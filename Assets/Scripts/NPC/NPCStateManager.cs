using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCStateManager : MonoBehaviour
{
    float _interval = 0;
    bool isNPCStateRunning = false;
    [SerializeField] NPCMover _npcMover;
    [SerializeField] NPCObjectManipulator _npcObjectManipulator;
    [SerializeField]
    private NPCCheckAround _npcCheckAround;
    public enum NPCState
    {
        WAIT,	//行動を一旦停止
        MOVE,	//ランダム移動
        CREATE,	//生成
        BREAK,	//破壊
        IDLE,	//待機
        AVOID,	//回避
        JUMP,//ジャンプ
        Sight,//ブロックを発見して向かう

    }
    public NPCState npcState;

    void Start()
    {
        npcState = NPCState.MOVE;
    }
    void Update()
    {
        NPCRoutine();
        switch (npcState)
        {
            case NPCState.MOVE:
                _npcMover.NPCCtrl();
                //Debug.Log("1");
                break;
            case NPCState.BREAK:
                _npcObjectManipulator.BreakBlock();
                //Debug.Log("2");
                break;
            case NPCState.CREATE:
                _npcObjectManipulator.CreateBlock();
                break;
            case NPCState.JUMP:
                _npcMover.Jump();
                //Debug.Log("3");
                break;
            case NPCState.AVOID:
                _npcMover.AvoidBlock();
                //Debug.Log("4");
                break;
            case NPCState.IDLE:
                _npcMover.NPCIdle();
                //Debug.Log("5");
                break;
        }
    }

    void NPCRoutine()
    {
         Debug.Log(npcState);
        // if(_npcMover._OverBlock)
        // {
        //     npcState = NPCState.AVOID;
        // }
        //思考止める
        if (isNPCStateRunning) return;

       bool _overBlock = _npcCheckAround.CheckVerticalDeathBlock();
       if(_overBlock)
       {
         npcState = NPCState.JUMP;
       }
        if (_npcObjectManipulator._hitBlock)
        {
            //確率でどれか
            int random = Random.Range(1, 3);
            switch (random)
            {
                case 1:
                    npcState = NPCState.BREAK;
                    break;
                case 2:
                    npcState = NPCState.BREAK;
                    //npcState = NPCState.JUMP;
                    break;
            }
        }
        else
        {
            //確率でどれか
            int random = Random.Range(1, 4);
            switch (random)
            {
                case 1:
                    npcState = NPCState.MOVE;
                    break;
                case 2:
                    npcState = NPCState.CREATE;
                    break;
                case 3:
                    npcState = NPCState.JUMP;
                    break;
            }


        }
        //StartCoroutine("NPCInterval");

        // if (_npcObjectManipulator.HasBlock)
        // {
        //     npcState = NPCState.CREATE;
        // }
        // if (!_npcObjectManipulator.HasBlock)
        // {
        //     Debug.Log("3");
        //     npcState = NPCState.MOVE;
        // }
        // if (!_npcMover.OnGround)
        // {
        //     Debug.Log("4");

        //     npcState = NPCState.MOVE;
        // }
    }
}

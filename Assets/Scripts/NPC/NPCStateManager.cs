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
                Debug.Log("1");
                break;
            case NPCState.BREAK:
                _npcObjectManipulator.BreakBlock();
                Debug.Log("2");
                break;
            case NPCState.CREATE:
                _npcObjectManipulator.CreateBlock();
                break;
            case NPCState.JUMP:
                _npcMover.Jump();
                Debug.Log("3");
                break;
            case NPCState.AVOID:
                _npcMover.AvoidBlock();
                Debug.Log("4");
                break;
            case NPCState.IDLE:
                _npcMover.NPCIdle();
                Debug.Log("5");
                break;
        }
    }

    void NPCRoutine()
    {
        // Debug.Log(npcState);
        // if(_npcMover._OverBlock)
        // {
        //     npcState = NPCState.AVOID;
        // }
        //思考止める
        if (isNPCStateRunning) return;

        bool _overBlock = _npcCheckAround.CheckVerticalDeathBlock();
        bool _depthBlock = _npcCheckAround.CheckHorizontalDeathBlock();
        bool _checkPlayer = _npcCheckAround.CheckPlayer();
        bool _hitBlock = _npcCheckAround.CheckBreakeBlock();
        bool _behindBlock = _npcCheckAround.CheckBehindBlock();
       
        //ブロックにつぶされそうなとき　回避行動　優先度1
        if (_overBlock || _depthBlock)
        {
            //確率でどれか
            int random = Random.Range(1, 3);
            switch (random)
            {
                case 1:
                    npcState = NPCState.JUMP;
                    break;
                case 2:
                    npcState = NPCState.MOVE;
                    //npcState = NPCState.JUMP;
                    break;
            }
        }
        //目の前にブロックがあるとき　ブロック破壊　優先度2
        else if (_hitBlock)
        {
            npcState = NPCState.BREAK;
        }
        //敵Playerとx軸がかさなった時　ブロック生成（攻撃）
        else if (_checkPlayer && _npcObjectManipulator.HasBlock)
        {
            npcState = NPCState.CREATE;
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

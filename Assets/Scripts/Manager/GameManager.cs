using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] 
    private ColorManager _colorManager;
    [SerializeField] 
    private BlockManager _blockManager;
    //ゲームの制限時間
    [SerializeField] 
    private float _TimeLimit;
    public float _timeLimit => _TimeLimit;
    UIHandler _uiHandler;
    private tmpPlayer _player;
    [SerializeField]
    private int _teamID;
    private const float TEAM1_POS_Z = -2.5f;
    private const float TEAM2_POS_Z = 1.5f;

    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        //制限時間を減らす
        InvokeRepeating("ReduceTimeLimit", 0,1);
        _player = GameObject.FindWithTag("Player").GetComponent<tmpPlayer>();
        if(_player == null) return;
        if(_teamID == 0) 
        {
            _blockManager.SetParam(TEAM1_POS_Z);
            _player.SetEnemyPosZ(TEAM2_POS_Z);
        }
        else 
        {
            _blockManager.SetParam(TEAM2_POS_Z);
            _player.SetEnemyPosZ(TEAM1_POS_Z);
        }
    }
    void Update()
    {
        //ReduceTimeLimit();
    }

    void ReduceTimeLimit()
    {
        _TimeLimit -= 1;
        _uiHandler.ShowLimitTime(_TimeLimit);
    }
}

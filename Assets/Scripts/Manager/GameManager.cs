using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] ColorManager _colorManager;
    [SerializeField] BlockManager _blockManager;
    //ゲームの制限時間
    [SerializeField] private float _TimeLimit;
    public float _timeLimit => _TimeLimit;
    UIHandler _uiHandler;

    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        //制限時間を減らす
        InvokeRepeating("ReduceTimeLimit", 0,1);
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

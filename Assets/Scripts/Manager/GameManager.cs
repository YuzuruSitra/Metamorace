using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //ゲームの制限時間
    [SerializeField] private float _TimeLimit;
    public float _timeLimit => _TimeLimit;
    UIHandler _uiHandler;

    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
    }
    void Update()
    {
        ReduceTimeLimit();
    }

    void ReduceTimeLimit()
    {
        _TimeLimit -= Time.deltaTime;
        _uiHandler.ShowLimitTime(_TimeLimit);
    }
}

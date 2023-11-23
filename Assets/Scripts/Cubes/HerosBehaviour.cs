using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerosBehaviour : MonoBehaviour
{
    private bool _oneTime = true;
    private float _targetPosZ = 0.0f;
    private float _speed = 20.0f;
    [SerializeField]
    private int _teamID;
    [SerializeField]
    private Rigidbody _rb;

    void Start()
    {
        if (_teamID == 1) _targetPosZ = GameManager.TEAM2_POS_Z;
        else _targetPosZ = GameManager.TEAM1_POS_Z;
    }

    public void SetTargetPos(float posZ)
    {
        _targetPosZ = posZ;
        Debug.Log(_targetPosZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_oneTime) return;
        
        if(_targetPosZ >= 0) AddPosZ();
        else DecreasePosZ();
    }

    // 奥へ
    void AddPosZ()
    {
        Vector3 direction = new Vector3(0.0f, 0.0f, 1.0f);
        _rb.MovePosition(transform.position + direction * _speed * Time.deltaTime);
        
        if (_targetPosZ > transform.position.z) return;
        Vector3 tmp = transform.position;
        tmp.z = _targetPosZ;
        transform.position = tmp;
        _rb.useGravity = true;
        _oneTime = false;
    }

    // 手前へ
    void DecreasePosZ()
    {
        Vector3 direction = new Vector3(0.0f, 0.0f, -1.0f);
        _rb.MovePosition(transform.position + direction * _speed * Time.deltaTime);
        
        if (_targetPosZ < transform.position.z) return;
        Vector3 tmp = transform.position;
        tmp.z = _targetPosZ;
        transform.position = tmp;
        _rb.useGravity = true;
        _oneTime = false;
    }
}

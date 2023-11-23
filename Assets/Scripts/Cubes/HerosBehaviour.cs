using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerosBehaviour : MonoBehaviour
{
    private bool _oneTime = true;
    private float _targetPosZ = 0.0f;
    private float _speed = 20.0f;
    [SerializeField]
    private bool _isBigBlock;
    [SerializeField]
    private int _teamID;
    
    [SerializeField]
    private Rigidbody _rb;

    void Start()
    {
        if (_isBigBlock)
        {
            if (_teamID == 0) _targetPosZ = GameManager.TEAM2_BIG_POS_Z;
            else _targetPosZ = GameManager.TEAM1_BIG_POS_Z;
        }
        else
        {
            if (_teamID == 0) _targetPosZ = GameManager.TEAM2_POS_Z;
            else _targetPosZ = GameManager.TEAM1_POS_Z;
        }
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
        _rb.constraints = RigidbodyConstraints.FreezePositionZ;
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
        _rb.constraints = RigidbodyConstraints.FreezePositionZ;
    }

    public void SetID(int id)
    {
        _teamID = id;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerosBehaviour : MonoBehaviour
{
    private bool _oneTime = true;
    private bool _isBack = true;
    private float _targetPosZ;
    float _calcDirection;
    private float _speed = 20.0f;
    [SerializeField]
    private Rigidbody _rb;

    public void SetTargetPos(float posZ)
    {
        _targetPosZ = posZ;
        _calcDirection = _targetPosZ - transform.position.z;
    }


    // Update is called once per frame
    void Update()
    {
        if (!_oneTime) return;
        
        if(_calcDirection >= 0) AddPosZ();
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

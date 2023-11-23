using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    public int _objID; 
    
    [SerializeField]
    private float _objHealth;
    [SerializeField]
    private float _blocklength;
    [SerializeField]
    private bool _isBigBlock;
    private float _speed = 20.0f;
    [SerializeField]
    private Rigidbody _rb;

    //Playerによるお邪魔ブロック破壊処理
    public int DestroyBlock(float power, bool developMode)
    {
        _objHealth -= power * Time.deltaTime;
        // 同期処理
        if (!developMode) _myPV.RPC(nameof(SyncHealth), PhotonTargets.All, _objHealth);

        if (_objHealth >= 0) return -1;
        Destroy(this.gameObject);
        return _objID;
    }

    [PunRPC]
    private void SyncHealth(float currentHealth)
    {
        _objHealth = currentHealth;
        if (_objHealth <= 0) Destroy(this.gameObject);
    }

    void Update()
    {
        if(_isBigBlock) BigBlockMove();
        else MoveBlock();
    }

    public void MoveBlock()
    {
        Ray _move = new Ray(transform.position, -transform.forward);
        RaycastHit _hitmove;
        Debug.DrawRay(transform.position, _move.direction * _blocklength, Color.red, _blocklength);

        if (Physics.Raycast(_move, out _hitmove, _blocklength))
        {
            if (_hitmove.collider.CompareTag("Ambras") ||
            _hitmove.collider.CompareTag("Heros") || _hitmove.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                _rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
            }
        }
    }

    public void BigBlockMove()
    {
        CheckAndMove(new Ray(transform.position, -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(0, 1, 0), -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(0, -1, 0), -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(1, 0, 0), -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(-1, 0, 0), -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(1, 1, 0), -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(-1, 1, 0), -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(1, -1, 0), -transform.forward));
        CheckAndMove(new Ray(transform.position + new Vector3(-1, -1, 0), -transform.forward));
    }

    private void CheckAndMove(Ray ray)
    {
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * _blocklength, Color.red, _blocklength);

        if (Physics.Raycast(ray, out hit, _blocklength))
        {
            if (IsBlock(hit.collider)) 
            {
                //当たったら移動
                _rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
            }
        }
    }

    private bool IsBlock(Collider collider)
    {
        return collider.CompareTag("Ambras") || collider.CompareTag("Heros") || collider.CompareTag("ItemCBlock");
    }
}

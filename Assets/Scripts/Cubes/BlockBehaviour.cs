using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BlockBehaviour : MonoBehaviour
{
    [SerializeField]
    private Animator _anim;
    private bool _isAnim = false;
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    public int _objID; 
    
    [SerializeField]
    private float _objHealth;
    private float _maxobjHealth;
    public float _MaxobjHealth => _maxobjHealth;
    [SerializeField]
    private float _blocklength;
    [SerializeField]
    private bool _isBigBlock;
    private float _speed = 20.0f;
    [SerializeField]
    private Rigidbody _rb;
    [SerializeField]
    bool _developMode = false;

    [SerializeField] Image _gage;
    float setTime = 1.0f;
    float currentTime;
    [SerializeField] GameObject _parentBlock;
    [SerializeField] GameObject _cloudeffect;
    void Start()
    {
        _maxobjHealth = _objHealth;
        _cloudeffect.SetActive(false);
        Debug.Log(_maxobjHealth);
    }
    void Update()
    {
        
        if(_isBigBlock) BigBlockMove();
        else MoveBlock();      
        if(currentTime >= 0) 
        {
            _gage.gameObject.SetActive(true);
            currentTime -= Time.deltaTime;
            _anim.SetBool("IsTouch", true);
            _isAnim = true;
        }
        else
        {
            _gage.gameObject.SetActive(false);
            
            _anim.SetBool("IsTouch", false);
            _isAnim = false;
        }    

        if (_objHealth <= 0)
        {
            Debug.Log("aaaa");
            this.gameObject.SetActive(false);
            _cloudeffect.transform.position = transform.position;
            _cloudeffect.SetActive(true);
            Destroy(_parentBlock,2.0f);
        }

    }

    public void DevModeSet(bool developMode)
    {
        _developMode = developMode;
    }

    //Playerによるお邪魔ブロック破壊処理
    public int DestroyBlock(float power)
    {
        _objHealth -= power * Time.deltaTime;
        if (_objHealth <= 0) _objHealth = 0;
        // 同期処理
        _myPV.RPC(nameof(SyncHealth), PhotonTargets.Others, _objHealth);
        
        if (_objHealth >= 0) return -1;
        
        return _objID;
    }

    [PunRPC]
    private void SyncHealth(float value)
    {
        _objHealth = value;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BreakCol"))
        {
            if (!_developMode) _myPV.RPC(nameof(SyncHealth), PhotonTargets.Others, 0);
            else Destroy(this.gameObject);
        }
    }

    public void MoveBlock()
    {
        Ray _move = new Ray(transform.position, -transform.forward);
        RaycastHit _hitmove;
        Debug.DrawRay(transform.position, _move.direction * _blocklength, Color.red, 0.1f);

        if (Physics.Raycast(_move, out _hitmove, _blocklength))
        {
            if (_hitmove.collider.CompareTag("Ambras") ||
            _hitmove.collider.CompareTag("Heros") || _hitmove.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                _rb.isKinematic = true;
                _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                //_rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
                Vector3 movePos = transform.position + transform.forward * _speed * Time.deltaTime;
                transform.position = movePos;
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
                _rb.isKinematic = true;
                _rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
                //当たったら移動
                //_rb.MovePosition(transform.position + transform.forward * _speed * Time.deltaTime);
                Vector3 movePos = transform.position + transform.forward * _speed * Time.deltaTime;
                transform.position = movePos;
            }
        }
    }

    private bool IsBlock(Collider collider)
    {
        return collider.CompareTag("Ambras") || collider.CompareTag("Heros") || collider.CompareTag("ItemCBlock");
    }

    public void DecreceGage()
   {
        currentTime = setTime;
        float _nowhealth =  _objHealth/_maxobjHealth;
        _gage.fillAmount = _nowhealth;
   }
}

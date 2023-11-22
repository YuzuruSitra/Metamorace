using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBlock : MonoBehaviour
{
    [SerializeField]
    float _bigBlocklength;

    void Update()
    {
        BigBlockMove();
    }
   public void BigBlockMove()
    {
        Debug.Log("big");
        Ray _middle = new Ray(transform.position , new Vector3(0, 0, -1));
        Ray _up = new Ray(transform.position + new Vector3(0, 1, 0), new Vector3(0, 0, -1));
        Ray _down = new Ray(transform.position + new Vector3(0, -1, 0), new Vector3(0, 0, -1));
        Ray _right = new Ray(transform.position + new Vector3(1, 0, 0), new Vector3(0, 0, -1));
        Ray _left = new Ray(transform.position + new Vector3(-1, 0, 0), new Vector3(0, 0, -1));
        Ray _upright = new Ray(transform.position + new Vector3(1, 1, 0), new Vector3(0, 0, -1));
        Ray _upleft = new Ray(transform.position + new Vector3(-1, 1, 0), new Vector3(0, 0, -1));
        Ray _downright = new Ray(transform.position + new Vector3(1, -1, 0), new Vector3(0, 0, -1));
        Ray _downleft = new Ray(transform.position + new Vector3(-1, -1, 0), new Vector3(0, 0, -1));
        RaycastHit _middlehit, _hitup, _hitdown, _hitright, _hitleft, _hitupright, _hitupleft, _hitdownright, _hitdownleft;
         Debug.DrawRay(transform.position, new Vector3(0, 0, -1), Color.red, _bigBlocklength);
        if (Physics.Raycast(_middle, out _middlehit,_bigBlocklength))
        {
            if (_middlehit.collider.CompareTag("Ambras") ||
            _middlehit.collider.CompareTag("Heros") || _middlehit.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_up, out _hitup,_bigBlocklength))
        {
            if (_hitup.collider.CompareTag("Ambras") ||
            _hitup.collider.CompareTag("Heros") || _hitup.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_down, out _hitdown, _bigBlocklength))
        {
            if (_hitdown.collider.CompareTag("Ambras") ||
            _hitdown.collider.CompareTag("Heros") || _hitdown.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_right, out _hitright, _bigBlocklength))
        {
            if (_hitright.collider.CompareTag("Ambras") ||
            _hitright.collider.CompareTag("Heros") || _hitright.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_left, out _hitleft,_bigBlocklength))
        {
            if (_hitleft.collider.CompareTag("Ambras") ||
            _hitleft.collider.CompareTag("Heros") || _hitleft.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_upright, out _hitupright,_bigBlocklength))
        {
            if (_hitupright.collider.CompareTag("Ambras") ||
            _hitupright.collider.CompareTag("Heros") || _hitupright.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_upleft, out _hitupleft,_bigBlocklength))
        {
            if (_hitupleft.collider.CompareTag("Ambras") ||
            _hitupleft.collider.CompareTag("Heros") || _hitupleft.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_downright, out _hitdownright,_bigBlocklength))
        {
            if (_hitdownright.collider.CompareTag("Ambras") ||
            _hitdownright.collider.CompareTag("Heros") || _hitdownright.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }
        if (Physics.Raycast(_downleft, out _hitdownleft,_bigBlocklength))
        {
            if (_hitdownleft.collider.CompareTag("Ambras") ||
            _hitdownleft.collider.CompareTag("Heros") || _hitdownleft.collider.CompareTag("ItemCBlock"))
            {
                //当たったら移動
                transform.Translate(0, 0, 0.1f);
            }
        }



    }
}

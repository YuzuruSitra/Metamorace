using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemC : MonoBehaviour
{
   [SerializeField] int _itemCId;
   public int _ItemCId => _itemCId;
   [SerializeField] private bool _useCombo;
    
   ItemC _itemC;
   private bool _used = false;
   public bool _Used => _used;
   BlockBehaviour _currentBlock;
   //スタン　ID Number1
   public void EffectStan(ref float _usePlayerSpeed, ref float _useJumpPower)
   {
      _usePlayerSpeed = 0;
      _useJumpPower = 0;
      //Debug.Log("すたん");
      // _uiHandler.ResetItemImage();
      // Debug.Log("スタン");
      // _playerSpeed = _playerSpeed * 0;
      // _hasItemC = false;
   }
   //周囲4マスのブロックを破壊( 相手への加担 )　 ID Number2
   //CBlockにレイが当たってかつそのブロックがID Number2のとき連鎖的にブロックが壊れる処理(GetComponent)使ってるよ！
   public void Break4()
   {
      //この変数をBreak4が連鎖的に起きる抽選で仕様
      int ItemCEffectNum = 2;
      //後でオフセット変更必要
      Ray _up = new Ray(transform.position, new Vector3(0, 1, 0));
      Ray _down = new Ray(transform.position, new Vector3(0, -1, 0));
      Ray _right = new Ray(transform.position, new Vector3(1, 0, 0));
      Ray _left = new Ray(transform.position, new Vector3(-1, 0, 0));
      float _raylength = 1.0f;
      RaycastHit _hitup, _hitdown, _hitright, _hitleft;
      //ItemCブロックは破壊できない
      //Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), new Vector3(0, 1, 0), Color.red, 3.0f);
      //上側のブロック破壊
      if (Physics.Raycast(_up, out _hitup, _raylength))
      {
         if (_hitup.collider.CompareTag("Ambras") ||
            _hitup.collider.CompareTag("Heros") || _hitup.collider.CompareTag("ItemCBlock")
            )
         {
             Destroy(_hitup.collider.gameObject);
            if (_hitup.collider.CompareTag("ItemCBlock"))
            {
               if(!_useCombo ) return;
               _itemC = _hitup.collider.GetComponent<ItemC>();
               Debug.Log("1");
               if(!_itemC._Used)
               {
                   Debug.Log("2");
                  _used = true;
                  _itemC.Break4();
               } 
            }
           
         }
      }
      //下側のブロック破壊
      if (Physics.Raycast(_down, out _hitdown, _raylength))
      {
         if (_hitdown.collider.CompareTag("Ambras") ||
            _hitdown.collider.CompareTag("Heros") || _hitdown.collider.CompareTag("ItemCBlock"))
         {
             Debug.Log("3");
            Destroy(_hitdown.collider.gameObject);
            if (_hitdown.collider.CompareTag("ItemCBlock"))
            {
               if(!_useCombo ) return;
               _itemC = _hitdown.collider.GetComponent<ItemC>();
               Debug.Log("1");
               if(!_itemC._Used)
               {
                   Debug.Log("2");
                  _used = true;
                  _itemC.Break4();
               }
            }
         }
      }

      //右側のブロック破壊
      if (Physics.Raycast(_right, out _hitright, _raylength))
      {
         if (_hitright.collider.CompareTag("Ambras") ||
            _hitright.collider.CompareTag("Heros") || _hitright.collider.CompareTag("ItemCBlock"))
         {
            Destroy(_hitright.collider.gameObject);
            if (_hitright.collider.CompareTag("ItemCBlock"))
            {
               if(!_useCombo ) return;
               _itemC = _hitright.collider.GetComponent<ItemC>();
               Debug.Log("1");
               if(!_itemC._Used)
               {
                   Debug.Log("2");
                  _used = true;
                  _itemC.Break4();
               } 
            }
         }
      }

      //左側のブロック破壊
      if (Physics.Raycast(_left, out _hitleft, _raylength))
      {
         if (_hitleft.collider.CompareTag("Ambras") ||
            _hitleft.collider.CompareTag("Heros") || _hitleft.collider.CompareTag("ItemCBlock"))
         {
            Destroy(_hitleft.collider.gameObject);
            if (_hitleft.collider.CompareTag("ItemCBlock"))
            {
               if(!_useCombo ) return;
               _itemC = _hitleft.collider.GetComponent<ItemC>();
               Debug.Log("1");
               if(!_itemC._Used)
               {
                   Debug.Log("2");
                  _used = true;
                  _itemC.Break4();
               } 
            }
         }
      }
   }

   void Update()
   {
      //Break4();
   }




}


// RaycastHit hitDown;
// RaycastHit hitUp;
// RaycastHit hitLeft;
// RaycastHit hitRight;

// Ray rayDown = new Ray(transform.position, new Vector3(0, -1, 0));
// Ray rayUp = new Ray(transform.position, new Vector3(0, 1, 0));
// Ray rayLeft = new Ray(transform.position, new Vector3(1, 0, 0));
// Ray rayRight = new Ray(transform.position, new Vector3(-1, 0, 0));

// Debug.Log("break4");

// if (Physics.Raycast(rayDown, out hitDown, -1) || Physics.Raycast(rayUp, out hitUp, -1) || Physics.Raycast(rayLeft, out hitLeft, -1) || Physics.Raycast(rayRight, out hitRight, -1))
// {
//    Debug.Log("rea");
// }

// if (hitDown.collider.CompareTag("Ambras") ||
//     hitDown.collider.CompareTag("Heros") ||
//     hitUp.collider.CompareTag("Ambras") ||
//     hitUp.collider.CompareTag("Heros") ||
//     hitLeft.collider.CompareTag("Ambras") ||
//     hitLeft.collider.CompareTag("Heros") ||
//     hitRight.collider.CompareTag("Ambras") ||
//     hitRight.collider.CompareTag("Heros"))
// {
//    Debug.Log("rea2");
// }

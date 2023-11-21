using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemC : MonoBehaviour
{
   [SerializeField] int _itemCId;
   public int _ItemCId => _itemCId;

   BlockBehaviour _currentBlock;
   //スタン　ID Number1
   public void EffectStan(ref float _usePlayerSpeed)
   {
      _usePlayerSpeed = 0;
      Debug.Log("すたん");
      // _uiHandler.ResetItemImage();
      // Debug.Log("スタン");
      // _playerSpeed = _playerSpeed * 0;
      // _hasItemC = false;
   }
   //周囲4マスのブロックを破壊( 相手への加担 )　 ID Number2
   public void Break4()
   {
      //後でオフセット変更必要
      Debug.Log("a");
      Ray _up = new Ray(transform.position, new Vector3(0, 1, 0));
      Ray _down = new Ray(transform.position, new Vector3(0, -1, 0));
      Ray _right = new Ray(transform.position, new Vector3(1, 0, 0));
      Ray _left = new Ray(transform.position, new Vector3(-1, 0, 0));

      RaycastHit _hitup, _hitdown, _hitright, _hitleft;
      //ItemCブロックは破壊できない
      //Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), new Vector3(0, 1, 0), Color.red, 3.0f);
      //上側のブロック破壊
      if (Physics.Raycast(_up, out _hitup))
      {

         if (_hitup.collider.CompareTag("Ambras") ||
            _hitup.collider.CompareTag("Heros"))
         {
            _currentBlock = _hitup.collider.GetComponent<BlockBehaviour>();
            _currentBlock.DestroyThis();
            Debug.Log("hit");
         }
      }
      //下側のブロック破壊
      if (Physics.Raycast(_down, out _hitdown))
         if (_hitdown.collider.CompareTag("Ambras") ||
            _hitdown.collider.CompareTag("Heros"))
         {
            _currentBlock = _hitdown.collider.GetComponent<BlockBehaviour>();
            _currentBlock.DestroyThis();
            Debug.Log("hit");
         }
      //右側のブロック破壊
      if (Physics.Raycast(_right, out _hitright))
         if (_hitright.collider.CompareTag("Ambras") ||
            _hitright.collider.CompareTag("Heros"))
         {
            _currentBlock = _hitright.collider.GetComponent<BlockBehaviour>();
            _currentBlock.DestroyThis();
            Debug.Log("hit");
         }
      //左側のブロック破壊
      if (Physics.Raycast(_left, out _hitleft))
         if (_hitleft.collider.CompareTag("Ambras") ||
            _hitleft.collider.CompareTag("Heros"))
         {
            _currentBlock = _hitleft.collider.GetComponent<BlockBehaviour>();
            _currentBlock.DestroyThis();
            Debug.Log("hit");
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

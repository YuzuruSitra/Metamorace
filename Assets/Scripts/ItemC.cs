using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemC : MonoBehaviour
{
   [SerializeField] int _itemCId;
   public int _ItemCId => _itemCId;
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
      Ray rayDown = new Ray(transform.position,new Vector3(0,-1,0));
      Ray rayUp = new Ray(transform.position,new Vector3(0,1,0));
      Ray rayLeft = new Ray(transform.position,new Vector3(1,0,0));
      Ray rayRight = new Ray(transform.position,new Vector3(-1,0,0));
      RaycastHit hitDown;
      RaycastHit hitUp;
      RaycastHit hitleft;
      RaycastHit hitRight;
      Debug.Log("break4");
      if (Physics.Raycast(rayDown, out hitDown,-1) || Physics.Raycast(rayUp, out hitUp,-1) || Physics.Raycast(rayLeft, out hitleft,-1) || Physics.Raycast(rayRight, out hitRight,-1))
      {
         Debug.Log("rea");
         //if (hit.collider.CompareTag("Ambras") || hit.collider.CompareTag("Heros"))
      }
//       if (hitDown.collider.CompareTag("Ambras") || 
//     hitDown.collider.CompareTag("Heros") || 
//     hitUp.collider.CompareTag("Ambras") || 
//     hitUp.collider.CompareTag("Heros") || 
//     hitleft.collider.CompareTag("Ambras") || 
//     hitleft.collider.CompareTag("Heros") || 
//     hitRight.collider.CompareTag("Ambras") || 
//     hitRight.collider.CompareTag("Heros"))
// {
//     Debug.Log("rea2");
// }

   }

   

}

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
      Debug.Log("break4");
   }

   

}

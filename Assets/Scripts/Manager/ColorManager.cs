using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    UIHandler _uiHandler;
    //色を変える対象のオブジェクト
    [SerializeField] GameObject _blockPrefab;
  

    GameObject _blockInstance;
    void Start()
    {
         _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();

         //プレハブからインスタンスを生成
          _blockInstance = Instantiate(_blockPrefab);
       
        
       // CreateBlock();
    }

   public void SetBrockMaterial()
   {
       //インスタンスのRendererから共有マテリアルにアクセス
         _blockInstance.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color",Color.green);
       
   }
}

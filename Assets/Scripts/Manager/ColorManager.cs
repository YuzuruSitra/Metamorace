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

   //ブロックに色を割り当てる（ボタンを押したとき）
   //ここを後でオブジェクトから参照したマテリアルにする処理に変更
   public void SetRedBrockMaterial()
   {
       //インスタンスのRendererから共有マテリアルにアクセス
         _blockInstance.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color",Color.red);
         _uiHandler.ActiveFalseButton();
   }
   public void SetBlueBrockMaterial()
   {
       //インスタンスのRendererから共有マテリアルにアクセス
         _blockInstance.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color",Color.blue);
         _uiHandler.ActiveFalseButton();
   }
   public void SetGreenBrockMaterial()
   {
       //インスタンスのRendererから共有マテリアルにアクセス
         _blockInstance.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color",Color.green);
          _uiHandler.ActiveFalseButton();
   }
   public void SetYellowBrockMaterial()
   {
       //インスタンスのRendererから共有マテリアルにアクセス
         _blockInstance.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color",Color.yellow);
         _uiHandler.ActiveFalseButton();
   }  

   
}

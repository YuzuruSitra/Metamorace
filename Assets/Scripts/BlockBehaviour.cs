using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviour : MonoBehaviour
{
    //0自分のブロック、1敵のブロック
    [SerializeField] private int _objID;
    public int _ObjID => _objID;
    [SerializeField] Sprite _playerSprite,_enemySprite;
    [SerializeField] Material _redMaterial,_blueMaterial,_greenMaterial,_yellowMaterial;
    [SerializeField] private float _btPlayer,_btEnemy;

     UIHandler _uiHandler;

    void Start()
    {
         _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
       // CreateBlock();
    }
    //Playerによるお邪魔ブロック破壊処理
    public void DestroyBlock()
    {
        if(_objID == 0)
        {
            _btPlayer -= Time.deltaTime;
            if(_btPlayer <= 0)
            {
                _uiHandler.SetStackImage(_playerSprite);
                Destroy(gameObject);
            }
        }
        else
        {
            _btEnemy -= Time.deltaTime;
             if(_btEnemy <= 0)
            {
                Destroy(gameObject);
            }
        }
        
    }

    //Playerによるお邪魔ブロック生成処理
    public void CreateBlock(Transform playerpos)
    {
        Instantiate(gameObject, playerpos.position, Quaternion.identity);
    }

    
  
}

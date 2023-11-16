using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockBehaviour : MonoBehaviour
{
    //1自分のブロック、2敵のブロック
    [SerializeField] private int _objID;
    public int _ObjID => _objID;
    [SerializeField] Sprite _playerSprite,_enemySprite;
    [SerializeField] Material _redMaterial,_blueMaterial,_greenMaterial,_yellowMaterial;
    [SerializeField] private float _btPlayer,_btEnemy;
    [SerializeField] private float _speed = 1.0f;
    
     UIHandler _uiHandler;

    void Start()
    {
         _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
       // CreateBlock();
    }

    void FixedUpdate()
    {
         //Debug.Log(string.Join(", ", _stackBlocks));
    }
    //Playerによるお邪魔ブロック破壊処理
    public int DestroyBlock()
    {
        if(_objID == 1)
        {
            _btPlayer -= Time.deltaTime * _speed;//ここにスピードをかける(アイテムAの処理)
            if(_btPlayer <= 0)
            {
                _uiHandler.SetStackImage(_playerSprite);
                //スタックに破壊したオブジェクトを格納
              
                Destroy(gameObject);
                 return _objID; 
            }
        }
        else if(_objID == 2)
        {
            _btEnemy -= Time.deltaTime;
             if(_btEnemy <= 0)
            {
                
                _uiHandler.SetStackImage(_enemySprite);
                Destroy(gameObject);
                 return _objID; 
                //スタン処理
            }
        }
        else
        {
            Debug.Log("未登録のブロック");
            
        }
        // オブジェクトが破壊されなかった場合、ここに到達する
        return -1; 
         
    }

    //Playerによるお邪魔ブロック生成処理
    public void CreateBlock(Transform playerpos)
    {
        Instantiate(gameObject, playerpos.position, Quaternion.identity);
    }
  
}

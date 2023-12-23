using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCItemHandler : MonoBehaviour
{
//    [SerializeField]
//     private PhotonView _myPV;
    [SerializeField]
    private NPCMover _npcMover;
    private UIHandler _uiHandler;
    [SerializeField]
    private NPCObjectManipulator _npcObjectManipulator;
    [SerializeField]
    private NPCEffectHandler _npcEffectHandler;
    [SerializeField]
    private ItemHandler _itemHandler;
    private WaitForSeconds _waitTime;
    private int _useItemState = -1;
    public int UseItemState => _useItemState;
    [SerializeField]
    private NPCDataReceiver _npcDataReceiver;

    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        _waitTime = new WaitForSeconds(_itemHandler._ItemAEffectTime);
    }

    void Update()
    {
        //if (!_myPV.isMine) return;
        if(!_npcDataReceiver.IsActiveGame) return;
        UseItem();
        Debug.Log(_itemHandler._HasItemB);
    }


    //アイテムを使う
    public void UseItem()
    {
        if (_itemHandler._HasItemA)
        {
            //さいやエフェクト再生
            _npcEffectHandler.ChangeSaiya(true);
            _itemHandler.ItemEffectA();
            _npcMover.ChangeMoveSpeed(_npcMover.InitialSpeed * 2);
            _npcObjectManipulator.ChangeDestroyPower(_npcObjectManipulator.InitialDestroyPower * 2);
            //ブロック画像nullにする
            _uiHandler.ResetStackImage();
            StartCoroutine(FinishItemA());
        }
        else if(_itemHandler._HasItemC && _npcObjectManipulator.HasBlock)
        {
            //  アイテムUI削除、所持ブロックをCに変更
            _uiHandler.ItemUI(UIHandler._itemCID);
            _uiHandler.ResetItemImage();    
            //ブロック画像nullにする
            _uiHandler.ResetStackImage();
            _useItemState = 1;
        }
        else if(_itemHandler._HasItemB && _npcObjectManipulator.HasBlock)
        {
            //  アイテムUI削除、所持ブロックをBに変更
            _uiHandler.ItemUI(UIHandler._itemBID);
            _uiHandler.ResetItemImage();    
            //ブロック画像nullにする
            _uiHandler.ResetStackImage();
            _useItemState = 0;       
        }
    }

    IEnumerator FinishItemA()
    {
        yield return _waitTime;
        _npcMover.ChangeMoveSpeed(_npcMover.InitialSpeed);
        _npcObjectManipulator.ChangeDestroyPower(_npcObjectManipulator.InitialDestroyPower);
        _npcEffectHandler.ChangeSaiya(false);
    }

    public void UsedItem()
    {
        _useItemState = -1;
    }
}

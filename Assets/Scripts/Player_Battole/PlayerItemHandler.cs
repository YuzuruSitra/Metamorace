using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemHandler : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private PlayerMover _playerMover;
    private UIHandler _uiHandler;
    [SerializeField]
    private PlayerObjectManipulator _playerObjectManipulator;
    [SerializeField]
    private PlayerEffectHangler _playerEffectHangler;
    [SerializeField]
    private ItemHandler _itemHandler;
    private WaitForSeconds _waitTime;
    private int _useItemState = -1;
    public int UseItemState => _useItemState;
    [SerializeField]
    private PlayerDataReceiver _playerDataReceiver;

    void Start()
    {
        _uiHandler = GameObject.FindWithTag("UIHandler").GetComponent<UIHandler>();
        _waitTime = new WaitForSeconds(_itemHandler._ItemAEffectTime);
    }

    void Update()
    {
        if (!_myPV.isMine) return;
        if(!_playerDataReceiver.IsActiveGame) return;
        UseItem();
    }


    //アイテムを使う
    public void UseItem()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (_itemHandler._HasItemA)
        {
            //さいやエフェクト再生
            _myPV.RPC(nameof( _playerEffectHangler.ChangeSaiya), PhotonTargets.All, true);
            _itemHandler.ItemEffectA();
            _playerMover.ChangeMoveSpeed(_playerMover.InitialSpeed * 2);
            _playerObjectManipulator.ChangeDestroyPower(_playerObjectManipulator.InitialDestroyPower * 2);
            //ブロック画像nullにする
            _uiHandler.ResetStackImage();
            StartCoroutine(FinishItemA());
        }
        else if(_itemHandler._HasItemC && _playerObjectManipulator.HasBlock)
        {
            //  アイテムUI削除、所持ブロックをCに変更
            _uiHandler.ItemUI(UIHandler._itemCID);
            _uiHandler.ResetItemImage();    
            //ブロック画像nullにする
            _uiHandler.ResetStackImage();
            _useItemState = 1;
        }
        else if(_itemHandler._HasItemB && _playerObjectManipulator.HasBlock)
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
        _playerMover.ChangeMoveSpeed(_playerMover.InitialSpeed);
        _playerObjectManipulator.ChangeDestroyPower(_playerObjectManipulator.InitialDestroyPower);
        _myPV.RPC(nameof( _playerEffectHangler.ChangeSaiya), PhotonTargets.All, false);
    }

    public void UsedItem()
    {
        _useItemState = -1;
    }

}

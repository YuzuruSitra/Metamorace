using UnityEngine;
using UnityEngine.Pool;

public class PoolHandler : MonoBehaviour
{
    [SerializeField]  GameObject _block;  // プールするゲームオブジェクトのプレハブ

    public int poolSize = 10;  // プールするオブジェクトの初期サイズ

    private ObjectPool<GameObject> _objectPool;  // ゲームオブジェクトのプール
    public ObjectPool<GameObject> _ObjectPool => _objectPool;

    private void Start()
    {
        // オブジェクトプールの作成
        _objectPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_block),          // プールが空のときに新しいインスタンスを生成する処理
            actionOnGet: obj => obj.SetActive(true),        // インスタンスがプールから取り出されたときに呼び出される処理
            actionOnRelease: obj => obj.SetActive(false),   // インスタンスがプールに戻されるときに呼び出される処理
            actionOnDestroy: null,                          // プールがmaxSizeに達した際、要素をプールに戻せなかったときに呼び出される処理
            collectionCheck: false,                         // プールに戻す際に既に同一インスタンスが登録されているか調べ、あれば例外を投げる。エディタでのみ実行されることに注意
            defaultCapacity: poolSize,                      // デフォルトの容量
            maxSize: 100);                                  // プールの最大サイズ
    }

    // private void Update()
    // {
    //     // スペースキーが押されたらオブジェクトをプールから取得してアクティブにする
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         GameObject obj = objectPool.Get();
    //         // 半径５m以内のエリアにランダムに３Dベクトルの座標を取得
    //         obj.transform.position = Random.insideUnitSphere * 5f;
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     // 衝突したオブジェクトをプールに返す
    //     if (other.CompareTag("Poolable"))
    //     {
    //         objectPool.Release(other.gameObject);
    //     }
    // }
}
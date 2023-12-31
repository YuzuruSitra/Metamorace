using UnityEngine;
using UnityEngine.Pool;

public class PoolHandler : MonoBehaviour
{
    // ヒーロス
    private ObjectPool<GameObject> _herosPool;
    public ObjectPool<GameObject> HerosPool => _herosPool;

    [SerializeField]
    private GameObject _heros;
    // アンブラス
    private ObjectPool<GameObject> _ambrasPool;
    public ObjectPool<GameObject> AmbrasPool => _ambrasPool;

    [SerializeField]
    private GameObject _ambras;
    

    void Awake()
    {
        _herosPool = new ObjectPool<GameObject>(OnCreatePooledObject, OnGetFromPool, OnReleaseToPool, OnDestroyPooledObject);
    }

    GameObject OnCreatePooledObject()
    {
        return Instantiate(_heros);
    }

    void OnGetFromPool(GameObject obj)
    {
        obj.SetActive(true);
    }

    void OnReleaseToPool(GameObject obj)
    {
        obj.SetActive(false);
    }

    void OnDestroyPooledObject(GameObject obj)
    {
        Destroy(obj);
    }

    public GameObject GetGameObject(Vector3 position, Quaternion rotation)
    {
        GameObject obj = _herosPool.Get();
        if (obj == null) obj = OnCreatePooledObject();
        Transform tf = obj.transform;
        tf.position = position;
        tf.rotation = rotation;

        return obj;
    }

    public void ReleaseGameObject(GameObject obj)
    {
        _herosPool.Release(obj);
    }
}
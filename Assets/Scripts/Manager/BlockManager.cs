using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField] 
    private Transform _insParent;
    [SerializeField] 
    private GameObject _blockAmbras;
    private float _insInterval = 3.0f;
    private Coroutine _insCoroutine;
    private Vector3 _blockInsPos;
    private Vector3 _rayDirection = Vector3.down;
    const float MAX_POS_Y = 8.0f;
    // ステージ上のオブジェクトの総数計算
    private int _blockCount = 0;
    public int BlockCount => _blockCount;
   
    void Start()
    {
        _insCoroutine = StartCoroutine(GenerateSetParam());
        _blockInsPos.y = MAX_POS_Y;
        _blockInsPos.z = GameObject.FindWithTag("Player").transform.position.z;
    }

    void Update()
    {
        ObjectExistsInRaycast(3);
    }

    //ランダムにお邪魔ブロック生成
    void GenerateBlock(Vector3 insPos)
    {
        Instantiate(_blockAmbras, insPos, Quaternion.identity, _insParent);
        _blockCount += 1;
    }

    IEnumerator GenerateSetParam()
    {
        yield return new WaitForSeconds(_insInterval);

        int insCount = Random.Range(1, 3);

        int insPosX;
        for (int i = 0; i < insCount; i++)
        {
            // insPosXの被りケア
            do
            {
                // 暫定
                insPosX = Random.Range(-7, 8);
                
            } while (insPosX == _blockInsPos.x || ObjectExistsInRaycast(insPosX));

            _blockInsPos.x = insPosX;
            Instantiate(_blockAmbras, _blockInsPos, Quaternion.identity);
        }
        _insCoroutine = StartCoroutine(GenerateSetParam());
    }

    bool ObjectExistsInRaycast(int targetPosX)
    {
        Vector3 startPos = _blockInsPos;
        startPos.x = targetPosX;
        // Raycastでオブジェクトが存在するかどうかを判定
        if (Physics.Raycast(startPos, _rayDirection, out RaycastHit hit, 10.0f))
        {
            Debug.DrawRay(startPos, _rayDirection, Color.red, 1f);
            // Rayがオブジェクトに当たった場合
            return true;
        }

        // Rayが何にも当たらなかった場合
        return false;
    }
}

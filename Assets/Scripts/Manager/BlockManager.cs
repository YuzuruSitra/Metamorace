using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Transform _cubeParent;
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

    public void SetParam(float insPosZ)
    {
        _cubeParent = GameObject.FindWithTag("CubeParent").transform;
        
        _insCoroutine = StartCoroutine(GenerateSetParam());
        _blockInsPos.y = MAX_POS_Y;
        _blockInsPos.z = insPosZ;
    }

    //ランダムにお邪魔ブロック生成
    void GenerateBlock(Vector3 insPos)
    {
        Instantiate(_blockAmbras, insPos, Quaternion.identity, _cubeParent);
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
            GenerateBlock(_blockInsPos);
        }
        _insCoroutine = StartCoroutine(GenerateSetParam());
    }

    bool ObjectExistsInRaycast(int targetPosX)
    {
        Vector3 startPos = _blockInsPos;
        startPos.x = targetPosX;
        // Raycastでオブジェクトが存在するかどうかを判定
        Debug.DrawRay(startPos, _rayDirection * 1.0f, Color.red, 1f);
        if (Physics.Raycast(startPos, _rayDirection, out RaycastHit hit, 1.0f))
        {
            // Rayが指定オブジェクトに当たった場合
            if (hit.collider.CompareTag("Ambras") || hit.collider.CompareTag("Heros")) return true;
        }

        // Rayが何にも当たらなかった場合
        return false;
    }
}

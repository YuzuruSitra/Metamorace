using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    private Transform _cubeParent;
    [SerializeField] 
    private GameObject _blockAmbras;
    private float _insInterval = 3.0f;
    private Coroutine _coroutineTeam1;
    private Coroutine _coroutineTeam2;
    private Vector3 _insPosTeam1;
    private Vector3 _insPosTeam2;
    private Vector3 _rayDirection = Vector3.down;
    const float MAX_POS_Y = 8.0f;
    // ステージ上のオブジェクトの総数計算
    private int _blockCount = 0;
    public int BlockCount => _blockCount;
    private bool _developMode = false;

    void Awake()
    {
        GameObject existingBlockManager = GameObject.FindWithTag("BlockManager");
        // 既にBlockManager が存在する場合破棄する
        if (existingBlockManager != null && existingBlockManager != gameObject) Destroy(gameObject);
    }

    void Start()
    {
        _cubeParent = GameObject.FindWithTag("CubeParent").transform;
        _coroutineTeam1 = StartCoroutine(SetParamTeam1());
         _coroutineTeam2 = StartCoroutine(SetParamTeam2());
        _insPosTeam1.y = MAX_POS_Y;
        _insPosTeam2.y = MAX_POS_Y;
        _insPosTeam1.z = GameManager.TEAM1_POS_Z;
        _insPosTeam2.z = GameManager.TEAM2_POS_Z;
    }

    public void SetParam(bool isDevelop)
    {
        _developMode = isDevelop;
    }

    //ランダムにお邪魔ブロック生成
    void GenerateBlock(Vector3 insPos)
    {
        if(_developMode) Instantiate(_blockAmbras, insPos, Quaternion.identity, _cubeParent);
        else PhotonNetwork.Instantiate(_blockAmbras.name, insPos, Quaternion.identity, 0);
        _blockCount += 1;
    }

    
    IEnumerator SetParamTeam1()
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
            } while (insPosX == _insPosTeam1.x || ObjectExistsInRaycast(_insPosTeam1, insPosX));
            
            _insPosTeam1.x = insPosX;
            GenerateBlock(_insPosTeam1);
        }
        _coroutineTeam1 = StartCoroutine(SetParamTeam1());
    }

    IEnumerator SetParamTeam2()
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
            } while (insPosX == _insPosTeam2.x || ObjectExistsInRaycast(_insPosTeam2, insPosX));
            
            _insPosTeam2.x = insPosX;
            GenerateBlock(_insPosTeam2);
        }
        _coroutineTeam2 = StartCoroutine(SetParamTeam2());
    }

    bool ObjectExistsInRaycast(Vector3 startPos , int targetPosX)
    {
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

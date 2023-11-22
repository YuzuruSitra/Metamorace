using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    // キューブの親
    private Transform _cubeParentTeam1, _cubeParentTeam2;
    [SerializeField] 
    private GameObject _blockAmbras;
    [Header("生成間隔")]
    [SerializeField] 
    private float _insInterval = 3.0f;
    private WaitForSeconds _waitTime;
    private Coroutine _coroutineTeam1, _coroutineTeam2;
    private Vector3 _insPosTeam1, _insPosTeam2;
    private bool _developMode = false;
    private const float RAY_DISTANCE = 1.0f;

    void Awake()
    {
        GameObject existingBlockManager = GameObject.FindWithTag("BlockManager");
        if (existingBlockManager != null && existingBlockManager != gameObject) Destroy(gameObject);
    }

    public void SetParam(bool isDevelop, Transform parent1, Transform parent2)
    {
        _developMode = isDevelop;
        _cubeParentTeam1 = parent1;
        _cubeParentTeam2 = parent2;
    }

    void Start()
    {
        if (!PhotonNetwork.isMasterClient && !_developMode) return;
        
        _insPosTeam1.y = GameManager.MAX_POS_Y;
        _insPosTeam2.y = GameManager.MAX_POS_Y;
        _insPosTeam1.z = GameManager.TEAM1_POS_Z;
        _insPosTeam2.z = GameManager.TEAM2_POS_Z;
        _waitTime = new WaitForSeconds(_insInterval);

        _coroutineTeam1 = StartCoroutine(SetParamForTeam(_cubeParentTeam1, _insPosTeam1));
        _coroutineTeam2 = StartCoroutine(SetParamForTeam(_cubeParentTeam2, _insPosTeam2));
    }

    IEnumerator SetParamForTeam(Transform cubeParent, Vector3 insPos)
    {
        while (true)
        {
            yield return _waitTime;
            int insCount = Random.Range(1, 3);

            int insPosX;
            for (int i = 0; i < insCount; i++)
            {
                do
                {
                    insPosX = Random.Range(GameManager.MinPosX, GameManager.MaxPosX);
                } while (insPosX == insPos.x || ObjectExistsInRaycast(insPos, insPosX));

                insPos.x = insPosX;
                GenerateBlock(insPos, cubeParent);
            }
        }
    }

    private void GenerateBlock(Vector3 insPos, Transform parent)
    {
        GameObject insObj;
        if (_developMode) 
            insObj = Instantiate(_blockAmbras, insPos, Quaternion.identity);
        else 
            insObj = PhotonNetwork.Instantiate(_blockAmbras.name, insPos, Quaternion.identity, 0);

        insObj.transform.parent = parent;
    }

    public int CalcCubeShare1(int fieldSize)
    {
        return CalcCubeShare(_cubeParentTeam1, fieldSize);
    }

    public int CalcCubeShare2(int fieldSize)
    {
        return CalcCubeShare(_cubeParentTeam2, fieldSize);
    }

    private int CalcCubeShare(Transform cubeParent, int fieldSize)
    {
        int share = (cubeParent.childCount * 100) / fieldSize;
        return share;
    }

    private bool ObjectExistsInRaycast(Vector3 startPos, int targetPosX)
    {
        startPos.x = targetPosX;
        Debug.DrawRay(startPos, Vector3.down * RAY_DISTANCE, Color.red, 1f);
        
        if (Physics.Raycast(startPos, Vector3.down, out RaycastHit hit, RAY_DISTANCE))
        {
            if (hit.collider.CompareTag("Ambras") || hit.collider.CompareTag("Heros")) 
            {
                return true;
            }
        }
        
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;
    private AmbrasPoolHandler _ambrasPoolHandler;
    // キューブの親
    private Transform[] _cubeParentTeams = new Transform[2];
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
    private bool _isGame = false;
    private const int MAX_GENERATE = 3;
    [SerializeField]
    private GameObject predictPrefab;
    private GameObject[] predictObjs = new GameObject[MAX_GENERATE * 2];
    private const float PREDICT_POS_Y = 5.8f;

    void Awake()
    {
        GameObject existingBlockManager = GameObject.FindWithTag("BlockManager");
        if (existingBlockManager != null && existingBlockManager != gameObject) Destroy(gameObject);
    }

    public void SetParam(bool isDevelop, Transform parent1, Transform parent2)
    {
        _developMode = isDevelop;
        _cubeParentTeams[0] = parent1;
        _cubeParentTeams[1] = parent2;
    }

    public void SetGameState(bool isGame)
    {
        _isGame = isGame;
    }

    void Start()
    {
        for (int i = 0; i < MAX_GENERATE * 2; i++)
        {
            predictObjs[i] = Instantiate(predictPrefab, Vector3.zero, Quaternion.identity);
        }
        
        if (!PhotonNetwork.isMasterClient) return;
        
        _insPosTeam1.y = GameManager.INS_POS_Y;
        _insPosTeam2.y = GameManager.INS_POS_Y;
        _insPosTeam1.z = GameManager.TEAM1_POS_Z;
        _insPosTeam2.z = GameManager.TEAM2_POS_Z;
        _waitTime = new WaitForSeconds(_insInterval);

        _coroutineTeam1 = StartCoroutine(SetParamForTeam1(0, _insPosTeam1, Quaternion.Euler(0, 180, 0)));
        _coroutineTeam2 = StartCoroutine(SetParamForTeam2(1, _insPosTeam2, Quaternion.Euler(0, 0, 0)));
    }

    IEnumerator SetParamForTeam1(int cubeParentNum, Vector3 insPos, Quaternion rot)
    {
        Debug.Log("aaa");
        while (true)
        {
            while (!_isGame) yield return null;

            yield return _waitTime;
            int insCount = Random.Range(1, MAX_GENERATE + 1);

            int insPosX;
            for (int i = 0; i < insCount; i++)
            {
                do
                {
                    insPosX = Random.Range(GameManager.MinPosX, GameManager.MaxPosX);
                } while (insPosX == insPos.x || ObjectExistsInRaycast(insPos, insPosX));

                insPos.x = insPosX;
                _myPV.RPC(nameof(CallInsBlock), PhotonTargets.All, insPos, cubeParentNum, rot, i);
            }
        }
    }

    IEnumerator SetParamForTeam2(int cubeParentNum, Vector3 insPos, Quaternion rot)
    {
        while (true)
        {
            while (!_isGame) yield return null;

            yield return _waitTime;
            int insCount = Random.Range(1, MAX_GENERATE + 1);

            int insPosX;
            for (int i = 0; i < insCount; i++)
            {
                do
                {
                    insPosX = Random.Range(GameManager.MinPosX, GameManager.MaxPosX);
                } while (insPosX == insPos.x || ObjectExistsInRaycast(insPos, insPosX));

                insPos.x = insPosX;
                _myPV.RPC(nameof(CallInsBlock), PhotonTargets.All, insPos, cubeParentNum, rot, i + MAX_GENERATE / 2);
            }
        }
    }

    [PunRPC]
    private void CallInsBlock(Vector3 insPos, int parentNum, Quaternion insRot, int predictNum)
    {
        StartCoroutine(InsAmbras(insPos, parentNum, insRot, predictNum));
    }

    private IEnumerator InsAmbras(Vector3 insPos, int parentNum, Quaternion insRot, int predictNum)
    {
        predictObjs[predictNum].transform.position = new Vector3(insPos.x , PREDICT_POS_Y, insPos.z);
        for (int i = 0; i < 4; i++)
        {
            predictObjs[predictNum].SetActive(true);
            yield return new WaitForSeconds(0.2f);
            predictObjs[predictNum].SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }

        if (PhotonNetwork.isMasterClient)
        {
            GameObject insObj = PhotonNetwork.Instantiate(_blockAmbras.name, insPos, insRot, 0);
            insObj.transform.SetParent(_cubeParentTeams[parentNum].transform);
        }
    }

    public int CalcCubeShare1(int fieldSize)
    {
        return CalcCubeShare(_cubeParentTeams[0], fieldSize);
    }

    public int CalcCubeShare2(int fieldSize)
    {
        return CalcCubeShare(_cubeParentTeams[1], fieldSize);
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

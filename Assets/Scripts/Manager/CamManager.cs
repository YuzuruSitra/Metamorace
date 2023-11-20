using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCameraBase[] _rightCams = new CinemachineVirtualCameraBase[2];
    private CinemachineVirtualCameraBase _vcamRight;
    [SerializeField]
    private CinemachineVirtualCameraBase[] _centerCams = new CinemachineVirtualCameraBase[2];
    private CinemachineVirtualCameraBase _vcamCenter;
    [SerializeField]
    private CinemachineVirtualCameraBase[] _leftCams = new CinemachineVirtualCameraBase[2];
    private CinemachineVirtualCameraBase _vcamLeft;
    private GameObject _targetPlayer;

    // Start is called before the first frame update
    public void SetPlayer(GameObject player, int teamID)
    {
        _targetPlayer = player;
        _vcamRight = _rightCams[teamID];
        _vcamCenter = _centerCams[teamID];
        _vcamLeft = _leftCams[teamID];
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetPlayer.transform.position.x <= -3.5f)
        {
            _vcamLeft.Priority = 2;
            _vcamRight.Priority = 1;
            _vcamCenter.Priority = 1;
        }
        else if(_targetPlayer.transform.position.x >= 3.5f)
        {
            _vcamLeft.Priority = 1;
            _vcamRight.Priority = 2;
            _vcamCenter.Priority = 1;
        }
        else
        {
            _vcamLeft.Priority = 1;
            _vcamRight.Priority = 1;
            _vcamCenter.Priority = 2;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    private int _teamID;
    [SerializeField]
    private CinemachineVirtualCameraBase[] _bottomRightCams = new CinemachineVirtualCameraBase[2];
    private CinemachineVirtualCameraBase _vcamRight;
    [SerializeField]
    private CinemachineVirtualCameraBase[] _bottomCenterCams = new CinemachineVirtualCameraBase[2];
    private CinemachineVirtualCameraBase _vcamCenter;
    [SerializeField]
    private CinemachineVirtualCameraBase[] _bottomLeftCams = new CinemachineVirtualCameraBase[2];
    private CinemachineVirtualCameraBase _vcamLeft;
    [SerializeField]
    private CinemachineVirtualCameraBase[] _topRightCams = new CinemachineVirtualCameraBase[2];
    [SerializeField]
    private CinemachineVirtualCameraBase[] _topCenterCams = new CinemachineVirtualCameraBase[2];
    [SerializeField]
    private CinemachineVirtualCameraBase[] _topLeftCams = new CinemachineVirtualCameraBase[2];
    private GameObject _targetPlayer;

    // Start is called before the first frame update
    public void SetPlayer(GameObject player, int teamID)
    {
        _targetPlayer = player.transform.GetChild(0).gameObject;
        _teamID = teamID;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_targetPlayer.transform.position.y);
        if(_targetPlayer.transform.position.y <= 4.0f)
        {
            if (_targetPlayer.transform.position.x <= -3.5f)
                ActivateCam(_bottomLeftCams[_teamID]);
            else if(_targetPlayer.transform.position.x >= 3.5f)
                ActivateCam(_bottomRightCams[_teamID]);
            else
                ActivateCam(_bottomCenterCams[_teamID]);
        }
        else
        {
            if (_targetPlayer.transform.position.x <= -3.5f)
                ActivateCam(_topLeftCams[_teamID]);
            else if(_targetPlayer.transform.position.x >= 3.5f)
                ActivateCam(_topRightCams[_teamID]);
            else
                ActivateCam(_topCenterCams[_teamID]);
        }
    }
    void ActivateCam(CinemachineVirtualCameraBase vcam)
    {
        if(_topLeftCams[_teamID].Priority != 1) _topLeftCams[_teamID].Priority = 1;
        if(_topRightCams[_teamID].Priority != 1) _topRightCams[_teamID].Priority = 1;
        if(_topCenterCams[_teamID].Priority != 1) _topCenterCams[_teamID].Priority = 1;
        if(_bottomLeftCams[_teamID].Priority != 1) _bottomLeftCams[_teamID].Priority = 1;
        if(_bottomRightCams[_teamID].Priority != 1) _bottomRightCams[_teamID].Priority = 1;
        if(_bottomCenterCams[_teamID].Priority != 1) _bottomCenterCams[_teamID].Priority = 1;
        vcam.Priority = 3;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCameraBase _vcamRight;
    [SerializeField]
    CinemachineVirtualCameraBase _vcamCenter;
    [SerializeField]
    CinemachineVirtualCameraBase _vcamLeft;
    private GameObject _targetPlayer;

    // Start is called before the first frame update
    public void SetPlayer(GameObject player)
    {
        _targetPlayer = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetPlayer.transform.position.x <= -3.5f)
        {
            _vcamLeft.Priority = 1;
            _vcamRight.Priority = 0;
            _vcamCenter.Priority = 0;
        }
        else if(_targetPlayer.transform.position.x >= 3.5f)
        {
            _vcamLeft.Priority = 0;
            _vcamRight.Priority = 1;
            _vcamCenter.Priority = 0;
        }
        else
        {
            _vcamLeft.Priority = 0;
            _vcamRight.Priority = 0;
            _vcamCenter.Priority = 1;
        }
    }
}

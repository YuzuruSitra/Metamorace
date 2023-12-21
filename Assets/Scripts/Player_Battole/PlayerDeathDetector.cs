using System.Collections;
using UnityEngine;

public class PlayerDeathDetector : MonoBehaviour
{
    [SerializeField]
    private PhotonView _myPV;
    [SerializeField]
    private PlayerDataReceiver _playerDataReceiver;
    [SerializeField] 
    private float _deathDecisionRayRange = 0.15f;
    [SerializeField] 
    private float _verticalRayOffset = 2.0f;
    [SerializeField] 
    private float _horizontalRayOffset = 0.5f;
    [SerializeField]
    private PlayerMover _playerMover;
    
    private bool _verticalDeath = false;
    public bool VerticalDeath => _verticalDeath;
    private bool _horizontalDeath = false;
    public bool HorizontalDeath => _horizontalDeath; 
    private bool _isPlayerDeath = false;
    public bool IsPlayerDeath => _isPlayerDeath;

    private void Update()
    {
        if (!_myPV.isMine) return;
        if(!_playerDataReceiver.IsActiveGame) return;
        JudgeVerticalDeath();
        JudgeHorizontalDeath();
    }

    private void JudgeVerticalDeath()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * _verticalRayOffset;
        Ray ray = new Ray(rayOrigin, Vector3.up);
        Debug.DrawRay(ray.origin, ray.direction * _deathDecisionRayRange, Color.green);

        if (Physics.Raycast(ray, out RaycastHit hit, _deathDecisionRayRange) && _playerMover.OnGround) 
        {
            _verticalDeath = true;
            _isPlayerDeath = true;
        }
    }

    private void JudgeHorizontalDeath()
    {
        Vector3 rayDirection = (_playerDataReceiver.MineTeamID == 0) ? Vector3.left : Vector3.right;
        Vector3 rayOrigin = transform.position + new Vector3(0f, 0.5f, 0f);
        Ray ray = new Ray(rayOrigin, rayDirection);

        Debug.DrawRay(rayOrigin, rayDirection * _deathDecisionRayRange, Color.blue, 1.0f);

        if (Physics.Raycast(ray, out RaycastHit hit, _deathDecisionRayRange))
        {
            _horizontalDeath = true;
            _isPlayerDeath = true;
        }
    }

}

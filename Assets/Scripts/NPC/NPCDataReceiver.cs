using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDataReceiver : MonoBehaviour
{
    private int _mineTeamID;
    public int MineTeamID => _mineTeamID;
    private int _enemyTeamID;
    public int EnemyTeamID => _enemyTeamID;
    private bool _isActiveGame;
    public bool IsActiveGame => _isActiveGame;
    private Transform[] _insCubeParent = new Transform[2];
    public Transform[] InsCubeParent => _insCubeParent;

    public void SetTeamID(int thisTeam)
    {
        _mineTeamID = thisTeam;
        _enemyTeamID = 1 - _mineTeamID;
    }

    public void SetGameState(bool isGame)
    {
        _isActiveGame = isGame;
    }

    public void SetInsCubeParent(Transform parent1, Transform parent2)
    {
        _insCubeParent[0] = parent1;
        _insCubeParent[1] = parent2;
    }
}

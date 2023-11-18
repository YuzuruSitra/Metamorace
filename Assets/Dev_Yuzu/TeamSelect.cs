using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelect : MonoBehaviour
{
    TeamHandler _teamHandler;
    // Start is called before the first frame update
    void Start()
    {
        _teamHandler = TeamHandler.InstanceTeamHandler;
    }

    public void SetTeam0()
    {
        _teamHandler.SetTeamID(0);
    }

    public void SetTeam1()
    {
        _teamHandler.SetTeamID(1);
    }
}

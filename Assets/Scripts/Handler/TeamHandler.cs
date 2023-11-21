using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamHandler : MonoBehaviour
{
    public static TeamHandler InstanceTeamHandler;
    [SerializeField]
    private int _teamID = 0;
    public int TeamID => _teamID;

    // Start is called before the first frame update
    void Awake()
    {
        if(InstanceTeamHandler != null)
        {
            Destroy(this);
            return;
        } 

        InstanceTeamHandler = this;
        DontDestroyOnLoad(this);
    }

    public void SetTeamID(int id)
    {
        _teamID = id;
    }
}

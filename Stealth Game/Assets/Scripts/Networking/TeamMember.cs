using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMember : MonoBehaviour
{
    private int teamNumber = 0;

    public void SetTeam(int team)
    {
        teamNumber = team;
    }

    public int GetTeam()
    {
        return teamNumber;
    }
}

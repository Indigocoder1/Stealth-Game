using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamScript : MonoBehaviour
{
    private int teamNumber = 0;

    public void setTeam(int team)
    {
        teamNumber = team;
    }

    public int getTeam()
    {
        return teamNumber;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TeamParagraph : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetTeam(int teamNumber)
    {
        text.text = "Team: " + teamNumber;
    }
}

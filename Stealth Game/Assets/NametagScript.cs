using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NametagScript : MonoBehaviour
{
    [SerializeField] private TMP_Text nametag;

    public void ChangeName(string name)
    {
        nametag.text = name;
    }
}

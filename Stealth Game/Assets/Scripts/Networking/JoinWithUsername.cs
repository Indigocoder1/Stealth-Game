using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Linq;

public class JoinWithUsername : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TMP_Text errorParagraph;
    [SerializeField] private Button button;

    public void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        string username = input.text;
        if (username.Length >= 2 && username.Length <= 14 && !username.Contains(" "))
        {
            if (DoesNotContainExplicatives(username))
            {
                bool nameTaken = false;
                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerListOthers)
                {
                    if (player.NickName == input.text)
                    {
                        nameTaken = true;
                    }
                }

                if (!nameTaken)
                {
                    //All information confirmed
                    PhotonNetwork.NickName = input.text;
                    SceneManager.LoadScene("MultiplayerTesting");
                }
            } else
            {
                errorParagraph.text = "No explicatives allowed.";
            }
        } else
        {
            errorParagraph.text = "Please enter a username with over 2 characters, but under 14 characters that doesn't use any spaces.";
        }
        
    }

    bool DoesNotContainExplicatives(string text)
    {
        return !text.Contains("fuck") && !text.Contains("bitch") && !text.Contains("nigga") && !text.Contains("nigger") && !text.Contains("shit");
    }
}

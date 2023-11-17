using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NewBehaviourScript : MonoBehaviour
{

    [SerializeField] private Button serverBtn;
    [SerializeField] private Button hostBtn;
    [SerializeField] private Button clientBtn;
    [SerializeField] private GameObject NetworkManagerUI;
    [SerializeField] private GameObject HealthUI;

    private void Awake()
    {
        serverBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();
            defaultOnClick();
        });
        hostBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            defaultOnClick();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            defaultOnClick();
        });
    }

    private void defaultOnClick()
    {
        NetworkManagerUI.SetActive(false);
        HealthUI.SetActive(true);
    }
}

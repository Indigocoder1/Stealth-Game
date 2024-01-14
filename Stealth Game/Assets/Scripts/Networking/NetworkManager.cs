using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    SpawnPad[] spawnPads;
    public PhotonView playerPrefab;
    private string roomName = "MR1";

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        spawnPads = GameObject.FindObjectsOfType<SpawnPad>();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        RoomOptions roomOptions = new RoomOptions() { IsVisible = false, MaxPlayers = 5 };
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined a room.");
        int teamNumber = Random.Range(0, 2);
        SpawnPlayer(teamNumber);
    }

    private void SpawnPlayer(int teamNumber)
    {

        Debug.Log(teamNumber);

        if (spawnPads == null)
        {
            Debug.LogError("No Spawn Pads Found");
            return;
        }

        SpawnPad mySpawnPad = spawnPads[ Random.Range(0, spawnPads.Length) ];

        //makes sure that the spawn pad is on the same team as the player.
        while (mySpawnPad.teamID != teamNumber) {
            mySpawnPad = spawnPads[Random.Range(0, spawnPads.Length)];
        }


        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, mySpawnPad.transform.position, mySpawnPad.transform.rotation);//spawns player


        //Enable the controls only for your player. Everything starts as disabled
        player.GetComponent<TeamMember>().SetTeam(teamNumber); //assigns player to random team generated above
        player.GetComponent<TeamParagraph>().SetTeam(teamNumber); //sets the UI element to show the team name

        player.GetComponent<ZeroGMovement>().enabled = true;
        // player.GetComponent<GroundMovement>().enabled = true;// not set up yet.
        player.transform.Find("CameraPosition").Find("Cameras").Find("Main Camera").gameObject.SetActive(true);
        player.transform.Find("UI").gameObject.SetActive(true);
        player.transform.Find("CameraPosition").Find("Guns").Find("Taser").GetComponent<Taser>().enabled = true;
    }

}
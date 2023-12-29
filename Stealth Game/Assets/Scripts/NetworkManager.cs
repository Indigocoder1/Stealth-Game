using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.Cockpit;

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
        player.GetComponent<TeamScript>().teamNumber = teamNumber; //assigns player to random team generated above
    }

}
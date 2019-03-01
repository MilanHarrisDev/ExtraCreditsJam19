using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    private Text statusText;

    private void Awake()
    {
        lobby = this;
        cancelButton.gameObject.SetActive(false);
        playButton.interactable = false;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings(); //Connects to photon master server
        statusText.text = "Trying to connect to matchmaking server.";
    }

    public void SetStatusText(string status)
    {
        statusText.text = status;
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Client connected to photon master server");
        PhotonNetwork.AutomaticallySyncScene = true;
        playButton.interactable = true;
        statusText.text = "Connected to matchmaking server";
    }

    public void OnPlayButtonClicked()
    {
        PhotonNetwork.JoinRandomRoom();
        playButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(true);
        statusText.text = "Trying to join a random room";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.Log("Failed to join random room, creating new room.");
        statusText.text = "No rooms available. Creating new room.";
        CreateRoom();
    }


    private void CreateRoom()
    {
        int randomRoomName = Random.Range(0, 1000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)MultiplayerSettings.mpSettings.maxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.Log("Tried to create a new room but failed.");
        CreateRoom();
    }

    public void OnCancelButtonClicked()
    {
        cancelButton.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}

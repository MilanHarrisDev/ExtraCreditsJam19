using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    //Room info
    public static PhotonRoom room;
    private PhotonView PV;

    public Text playerText;

    public GameObject myPlayer;

    public bool isGameLoaded;
    public int currentScene;

    //Player Info
    Player[] photonPlayers;
    public int playersInRoom;
    public int myNumberInRoom;

    public Dictionary<string, float> playerTimes;

    public int playersInGame;

    //Delayed start
    private bool readyToCount;
    private bool readyToStart;
    public float startingTime;
    private float lessThanMaxPlayers;
    private float atMaxPlayers;
    private float timeToStart;

    private void Awake()
    {
        if (PhotonRoom.room == null)
            PhotonRoom.room = this;
        else
        {
            if (PhotonRoom.room != this)
            {
                Destroy(PhotonRoom.room.gameObject);
                PhotonRoom.room = this;
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (!MultiplayerSettings.mpSettings)
            return;
        if (MultiplayerSettings.mpSettings.delayStart)
        {
            if (playersInRoom == 1)
                RestartTimer();

            if (!isGameLoaded)
            {
                if (readyToStart)
                {
                    atMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers = atMaxPlayers;
                }
                else if (readyToCount)
                {
                    lessThanMaxPlayers -= Time.deltaTime;
                    timeToStart = lessThanMaxPlayers;
                }

                if(readyToStart)
                    PhotonLobby.lobby.SetStatusText("Game starting in " + (int)timeToStart);

                if (timeToStart <= 0)
                    StartGame();
            }
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
    }

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        readyToCount = false;
        readyToStart = false;
        lessThanMaxPlayers = startingTime;
        atMaxPlayers = 15f;
        timeToStart = startingTime;
        playerText.text = "";
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonLobby.lobby.SetStatusText("Joined room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;

        PhotonNetwork.NickName = "Player" + myNumberInRoom.ToString();
        playerText.text = "you are: " + PhotonNetwork.LocalPlayer.NickName;

        if (MultiplayerSettings.mpSettings.delayStart)
        {
            PhotonLobby.lobby.SetStatusText(playersInRoom + "/" + MultiplayerSettings.mpSettings.maxPlayers + " players");
            if (playersInRoom > 1)
                readyToCount = true;

            if (playersInRoom == MultiplayerSettings.mpSettings.maxPlayers)
            {


                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient) //check if host
                    return;

                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
        else
        {
            StartGame();
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log("New player joined the room");
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;

        if (MultiplayerSettings.mpSettings.delayStart)
        {
            PhotonLobby.lobby.SetStatusText(playersInRoom + "/" + MultiplayerSettings.mpSettings.maxPlayers + " players");

            if (playersInRoom > 1)
                readyToCount = true;
            if (playersInRoom == MultiplayerSettings.mpSettings.maxPlayers)
            {
                readyToStart = true;
                if (!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }
    }

    private void StartGame()
    {
        playerTimes = new Dictionary<string, float>();
        PV.RPC("RPC_UpdatePlayerTime", RpcTarget.All, PhotonNetwork.LocalPlayer.NickName, 0f);

        isGameLoaded = true;
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (MultiplayerSettings.mpSettings.delayStart)
            PhotonNetwork.CurrentRoom.IsOpen = false;

        PhotonNetwork.LoadLevel(MultiplayerSettings.mpSettings.gameScene);
    }

    public void UpdatePlayerTime(string name, float time)
    {
        PV.RPC("RPC_UpdatePlayerTime", RpcTarget.All, name, time);
    }

    [PunRPC]
    private void RPC_UpdatePlayerTime(string name, float time)
    {
        if (playerTimes.ContainsKey(name))
        {
            playerTimes[name] = time;
        }
        else
            playerTimes.Add(name, time);

        Debug.Log("Added time for " + name + ": " + time);


    }

    private void SelectStartingRoles()
    {
        int racer = Random.Range(0, photonPlayers.Length);
        int sharkColor = 1;

        for(int i = 0; i < photonPlayers.Length; i++)
        {
            if (i == racer)
                PV.RPC("RPC_SetRole", photonPlayers[i], 0);
            else
            {
                PV.RPC("RPC_SetRole", photonPlayers[i], sharkColor);
                sharkColor++;
            }
        }
    }

    [PunRPC]
    private void RPC_SetRole(int role)
    {
        PlayerInfo.PI.selectedGraphic = role;
    }

    private void RestartTimer()
    {
        lessThanMaxPlayers = startingTime;
        timeToStart = startingTime;
        atMaxPlayers = 15f;
        readyToCount = false;
        readyToStart = false;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        if (currentScene == MultiplayerSettings.mpSettings.gameScene)
        {
            isGameLoaded = true;

            if (MultiplayerSettings.mpSettings.delayStart)
            {
                PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
            }
            else
                RPC_CreatePlayer();

            SelectStartingRoles();
        }
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length)
            PV.RPC("RPC_CreatePlayer", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    private PhotonView PV;
    public int graphicValue;
    public GameObject myGraphic;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_AddGraphic", RpcTarget.AllBuffered, PlayerInfo.PI.selectedGraphic);
            SetupCamera();

            if (PlayerInfo.PI.selectedGraphic == 1)
                GetComponent<PlayerMovement>().enabled = false;
        }
    }

    private void SetupCamera()
    {
        if(PlayerInfo.PI.selectedGraphic == 0) // If the player is the racer
        {
            CameraFollow camFollow = gameObject.AddComponent<CameraFollow>();
            camFollow.SetTarget(transform.Find("CameraTrack"));
            camFollow.SetCameraType(0);
        }
        else // if the player is a shark
        {
            CameraFollow camFollow = gameObject.AddComponent<CameraFollow>();
            Transform cameraTrack = transform.Find("CameraTrack");
            cameraTrack.localPosition = Vector3.zero;
            camFollow.SetTarget(cameraTrack);
            camFollow.SetCameraType(1);
        }
    }

    [PunRPC]
    void RPC_AddGraphic(int whichGraphic)
    {
        graphicValue = whichGraphic;
        myGraphic = Instantiate(PlayerInfo.PI.characterGraphics[whichGraphic], transform.position, transform.rotation, transform);

        transform.position = (whichGraphic == 0) ? GameSetup.GS.racerSpawn.position : GameSetup.GS.sharkSpawnPoints[whichGraphic - 1].position;
    }
}

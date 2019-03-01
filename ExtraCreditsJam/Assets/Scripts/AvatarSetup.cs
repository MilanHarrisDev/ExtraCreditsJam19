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
            PV.RPC("RPC_AddGraphic", RpcTarget.AllBuffered, PlayerInfo.PI.selectedGraphic);
    }

    [PunRPC]
    void RPC_AddGraphic(int whichGraphic)
    {
        graphicValue = whichGraphic;
        myGraphic = Instantiate(PlayerInfo.PI.characterGraphics[whichGraphic], transform.position, transform.rotation, transform);
    }
}

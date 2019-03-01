using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotonPlayer : MonoBehaviour
{
    private PhotonView PV;
    public GameObject myAvatar;
    private int currentRole;

    public void SetCurrentRole(int role)
    {
        PV.RPC("RPC_SetCurrentRole", RpcTarget.All, role);
    }

    [PunRPC]
    private void RPC_SetCurrentRole(int role)
    {
        currentRole = role;
        Debug.Log("Photon player " + PV.ViewID + " role set to " + role);
    }

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        
        if (PV.IsMine)
        {
            myAvatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
                    Vector3.zero, Quaternion.identity, 0);
        }
    }
}

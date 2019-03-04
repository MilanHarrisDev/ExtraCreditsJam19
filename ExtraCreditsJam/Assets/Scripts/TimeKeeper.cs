using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeKeeper : MonoBehaviour
{
    public Text timeText;
    public float time = 0;

    private bool counting = true;

    // Start is called before the first frame update
    void Start()
    {
        timeText.text = time + " seconds";
    }

    // Update is called once per frame
    void Update()
    {
        if(counting)
            time += Time.deltaTime;

        timeText.text = time + " seconds";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PhotonView pv = other.GetComponent<PhotonView>();
            counting = false;
            PhotonRoom.room.UpdatePlayerTime(pv.Owner.NickName, time);
        }
    }
}

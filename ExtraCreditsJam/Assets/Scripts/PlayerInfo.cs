using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo PI;

    public int selectedGraphic;

    public GameObject[] characterGraphics;

    private void OnEnable()
    {
        if (PlayerInfo.PI == null)
            PlayerInfo.PI = this;
        else if (PlayerInfo.PI != this)
        {
            Destroy(PlayerInfo.PI.gameObject);
            PlayerInfo.PI = this;
        }
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

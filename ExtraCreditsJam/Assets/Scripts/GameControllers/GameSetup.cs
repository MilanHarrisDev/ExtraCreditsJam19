using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public static GameSetup GS;

    public LayerMask racerCamMask;
    public LayerMask sharkCamMask;

    public Transform racerSpawn;
    public Transform[] sharkSpawnPoints;

    private void OnEnable()
    {
        if (GameSetup.GS == null)
            GameSetup.GS = this;
    }
}

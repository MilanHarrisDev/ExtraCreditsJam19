using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerSettings : MonoBehaviour
{
    public static MultiplayerSettings mpSettings;

    public bool delayStart;
    public int maxPlayers;

    public int menuScene;
    public int gameScene;

    private void Awake()
    {
        if (MultiplayerSettings.mpSettings == null)
            mpSettings = this;
        else
        {
            if (MultiplayerSettings.mpSettings != this)
                Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

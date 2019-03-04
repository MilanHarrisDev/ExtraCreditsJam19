using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardManager : MonoBehaviour
{
    public ScoreboardPlayer[] playersArray;
    public Dictionary<string, ScoreboardPlayer> players;

    private void Awake()
    {
        players.Add("Player1", playersArray[0]);
        players.Add("Player2", playersArray[1]);
        players.Add("Player3", playersArray[2]);
        players.Add("Player4", playersArray[3]);
    }

    public void InitScoreboard(string[] playerNames)
    {
        foreach(string str in playerNames)
        {
            players[str].owningPlayer = str;
            players[str].UpdateName(str);
            players[str].UpdateTime(0);
        }
    }
}

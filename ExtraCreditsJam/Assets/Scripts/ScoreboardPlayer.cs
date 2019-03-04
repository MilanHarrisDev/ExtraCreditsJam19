using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardPlayer : MonoBehaviour
{
    public string owningPlayer;
    public Text playerName;
    public Text playerTime;

    public void UpdateTime(float time)
    {
        playerTime.text = time.ToString();
    }

    public void UpdateName(string name)
    {
        playerName.text = name;

    }
}

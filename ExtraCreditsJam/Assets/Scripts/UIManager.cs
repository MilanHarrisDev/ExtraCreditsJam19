using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject main;
    public GameObject howTo;

    public GameObject howToRacer;
    public GameObject howToObstacle;

    public void SwitchToMain()
    {
        main.SetActive(true);
        howTo.SetActive(false);
    }

    public void SwitchToHowTo()
    {
        main.SetActive(false);
        howTo.SetActive(true);
    }

    public void SwitchToHowToRacer()
    {
        howToRacer.SetActive(true);
        howToObstacle.SetActive(false);
    }

    public void SwitchToHowToObstacle()
    {
        howToRacer.SetActive(false);
        howToObstacle.SetActive(true);
    }
}

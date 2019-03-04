﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum ObstacleType
{
    ICE,
    MINE,
    GUN
}

public class ObstaclePlacer : MonoBehaviour
{
    private Ray myRay;
    private RaycastHit hit;
    private GameObject crosshairObj;

    private bool init = false;
    private ObstacleType type;

    private GameObject crosshair;

    public void Init(ObstacleType type)
    {
        crosshairObj = Resources.Load("CrossHair") as GameObject;

        Debug.Log("Obstacle init");

        this.type = type;
        init = true;

        switch(type)
        {
            case ObstacleType.GUN:

                break;
            case ObstacleType.ICE:

                break;
            case ObstacleType.MINE:

                break;
        }

        crosshair = Instantiate(crosshairObj, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        if (!init)
            return;

        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(myRay, out hit, 10000000f))
        {
            if (crosshair)
                crosshair.transform.position = hit.point + new Vector3(0, .1f, 0);

            if (Input.GetMouseButtonDown(0))
            {
                switch (type)
                {
                    case ObstacleType.GUN:
                        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Obstacle_Ice"), GetYZero(hit.point), Quaternion.identity, 0);
                        break;
                    case ObstacleType.ICE:
                        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Obstacle_Ice"), GetYZero(hit.point), Quaternion.identity, 0);
                        break;
                    case ObstacleType.MINE:
                        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Obstacle_Ice"), GetYZero(hit.point), Quaternion.identity, 0);
                        break;
                }
            }
        }
    }

    private Vector3 GetYZero(Vector3 point)
    {
        return new Vector3(point.x, 0, point.z);
    }
}

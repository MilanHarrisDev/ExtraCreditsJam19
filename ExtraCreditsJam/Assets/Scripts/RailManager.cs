using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailManager : MonoBehaviour
{
    public static RailManager RM;

    [SerializeField]
    private Transform[] railPoints;

    private float[] distances;
    private float totalDist = 0;

    private int direction = 0;
    private float speed = .03f;

    [SerializeField]
    private int currentPoint;
    [SerializeField]
    private int currentTarget;
    private float currentDistance = 0;

    private float currentPos = 0; //current position between 2 points

    [SerializeField]
    private Transform railObject;

    public bool controlling = false;


    private void Awake()
    {
        speed = .05f;
    }

    private void Start()
    {
        RM = this;

        if (railPoints.Length == 0)
        {
            railPoints = new Transform[transform.childCount];

            for (int j = 0; j < transform.childCount; j++)
            {
                railPoints[j] = transform.GetChild(j);
            }
        }

        distances = new float[railPoints.Length - 1];

        for (int i = 0; i < railPoints.Length - 1; i++)
        {
            distances[i] = Vector3.Distance(railPoints[i].position, railPoints[i + 1].position);
            totalDist += distances[i];
        }

        currentDistance = distances[0];
        currentPoint = 0;
        currentTarget = 1;
    }

    private void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            direction = (int)Mathf.Ceil(Input.GetAxis("Horizontal"));
        else
            direction = 0;

        if(direction != 0)
        {
            currentPos += direction * ((Time.deltaTime * speed) / (currentDistance / totalDist));

            if (currentPos >= 1)
            {
                if (currentPoint == railPoints.Length - 1)
                {
                    currentPoint = 0;
                    currentTarget = currentPoint + 1;
                }
                else if (currentPoint == railPoints.Length - 2)
                {
                    currentPoint++;
                    currentTarget = 0;
                }
                else
                {
                    currentPoint++;
                    currentTarget = currentPoint + 1;
                }

                currentPos -= 1;
            }
            else if (currentPos <= 0)
            {
                if (currentPoint == 0)
                    currentPoint = railPoints.Length - 1;
                else
                    currentPoint--;

                if(currentTarget == 0)
                    currentTarget = railPoints.Length - 1;
                else
                    currentTarget--;

                currentPos += 1;
            }
            if(railObject)
                railObject.position = Vector3.Lerp(railPoints[currentPoint].position, railPoints[currentTarget].position, currentPos);
        }
    }

    public void SetRailObject(Transform obj)
    {
        railObject = obj;
    }
}

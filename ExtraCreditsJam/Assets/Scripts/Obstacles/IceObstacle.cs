using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceObstacle : MonoBehaviour
{
    [SerializeField]
    private Transform graphic;
    [SerializeField]
    private Transform startPos;
    [SerializeField]
    private Transform endPos;

    private bool starting = true;


    // Start is called before the first frame update
    void Start()
    {
        graphic.position = startPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(graphic.position, endPos.position) > .05f && starting)
            graphic.position = Vector3.Lerp(graphic.position, endPos.position, Time.deltaTime);
        else
        {
            starting = false;
            GetComponent<Collider>().enabled = true;
            //TURN ON OBSTACLE ABILITYS
            graphic.position = endPos.position + new Vector3(0, Mathf.Sin(Time.time) * .15f, 0);
        }
    }
}

using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PhotonView PV;

    public float accel = 1f;
    public float deccel = 4f;
    public float moveSpeed = 0f;
    public float maxSpeed = 5f;
    public float turnSpeed = 3f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (PV.IsMine)
        {
            if (Input.GetAxis("Vertical") > 0)
                moveSpeed += accel * Time.deltaTime;
            else
                moveSpeed -= deccel * Time.deltaTime;

            moveSpeed = Mathf.Clamp(moveSpeed, 0, maxSpeed);

            if (Input.GetAxis("Horizontal") > 0)
                Turn(1);
            else if (Input.GetAxis("Horizontal") < 0)
                Turn(-1);
            else
                Turn(0);

            rb.velocity = transform.forward * moveSpeed;
        }
    }

    private void Turn(int dir)
    {
        rb.rotation = Quaternion.Lerp(rb.rotation, Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0, 5f * dir, 0)), turnSpeed * Time.deltaTime);
        rb.rotation = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
    }
}

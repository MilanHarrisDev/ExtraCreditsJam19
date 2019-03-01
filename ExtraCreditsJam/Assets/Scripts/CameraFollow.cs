﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private PhotonView PV;

    [SerializeField]
    private float moveSpeed = 4f;
    [SerializeField]
    private float rotateSpeed = 1.5f;

    private Transform camera;

    [SerializeField]
    private Transform target;

    private bool follow = true;
    [SerializeField]
    private int followType = 0;

    private float sensitivity = 2.5f;
    private float minimumX = -360F;
    private float maximumX = 360F;
    private float minimumY = -20F;
    private float maximumY = 20F;
    private float rotationX = 0F;
    private float rotationY = 0F;
    Quaternion originalRotation;

    public float frameCounter = 20;

    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0F;

    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0F;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
            rb.freezeRotation = true;

        originalRotation = transform.localRotation;

        PV = GetComponent<PhotonView>();

        if (camera == null)
            camera = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (target && follow)
        {
            if (followType == 0)
            {
                camera.position = Vector3.Lerp(camera.position, target.position, moveSpeed * Time.fixedDeltaTime);
                camera.rotation = Quaternion.Lerp(camera.rotation, target.rotation, rotateSpeed * Time.fixedDeltaTime);
            }
            else if(followType == 1)
            {
                if (Input.GetMouseButton(1))
                {
                    rotAverageY = 0f;
                    rotAverageX = 0f;

                    rotationY += Input.GetAxis("Mouse Y") * sensitivity;
                    rotationX += Input.GetAxis("Mouse X") * sensitivity;

                    rotArrayY.Add(rotationY);
                    rotArrayX.Add(rotationX);

                    if (rotArrayY.Count >= frameCounter)
                        rotArrayY.RemoveAt(0);
                    if (rotArrayX.Count >= frameCounter)
                        rotArrayX.RemoveAt(0);

                    for (int j = 0; j < rotArrayY.Count; j++)
                        rotAverageY += rotArrayY[j];
                    for (int i = 0; i < rotArrayX.Count; i++)
                        rotAverageX += rotArrayX[i];

                    rotAverageY /= rotArrayY.Count;
                    rotAverageX /= rotArrayX.Count;

                    rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
                    rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                    Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                    Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);

                    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                }

                camera.position = target.position;
                camera.rotation = target.rotation;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
         angle += 360F;
        if (angle > 360F)
         angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void SetCameraType(int type)
    {
        followType = type;

        if (camera == null)
            camera = Camera.main.transform;

        if (type != 0)
        {
            Renderer[] renderers = transform.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
                r.enabled = false;
        }
        //camera.GetComponent<Camera>().cullingMask = (type == 1) ? GameSetup.GS.sharkCamMask : GameSetup.GS.racerCamMask;
    }
}

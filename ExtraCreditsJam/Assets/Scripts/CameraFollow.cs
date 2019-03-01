using Photon.Pun;
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

    private void Start()
    {
        PV = GetComponent<PhotonView>();

        if (camera == null)
            camera = Camera.main.transform;
    }

    void Update()
    {
        if (target && follow)
        {
            camera.position = Vector3.Lerp(camera.position, target.position, moveSpeed * Time.deltaTime);
            camera.rotation = Quaternion.Lerp(camera.rotation, target.rotation, rotateSpeed * Time.deltaTime);
        }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairRotate : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.up, 40 * Time.deltaTime);
    }
}

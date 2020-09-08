using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
            transform.GetChild(0).LookAt(Vector3.zero);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, -speed * Time.deltaTime);
            transform.GetChild(0).LookAt(Vector3.zero);
        }
    }
}

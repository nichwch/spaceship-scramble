using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    public float defaultZoom = -35;
    public GameObject target;
    float zoomLevel;
    public float smoothSpeed = 0.1f;
    private Vector3 velocity = Vector3.zero;

    Vector3 desiredPosition;
    private void Start()
    {
        zoomLevel = defaultZoom;
    }

    public void FixedUpdate()
    {
        desiredPosition = target.GetComponent<Transform>().position + new Vector3(0, 0, zoomLevel);



        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }
    public void SetZoom(int zoom)
    {
        zoomLevel = zoom;
    }



}



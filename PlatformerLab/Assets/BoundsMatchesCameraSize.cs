using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsMatchesCameraSize : MonoBehaviour
{

    public Camera camera
    {
        get
        {
            return GetComponent<Camera>();
        }
    }


    // Update is called once per frame
    void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        Bounds CameraBounds = new Bounds(camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.nearClipPlane)), new Vector3(camera.ViewportToWorldPoint(new Vector3(1, 0.5f, camera.nearClipPlane)).x - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.nearClipPlane)).x, camera.ViewportToWorldPoint(new Vector3(0.5f, 1f, camera.nearClipPlane)).y - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, camera.nearClipPlane)).y, 1000) * 2f);

        Debug.Log(CameraBounds);

        if (collider.size != new Vector2(CameraBounds.size.x, CameraBounds.size.y))
        {
            collider.size = CameraBounds.size;
        }

       
	}
}

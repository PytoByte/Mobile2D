using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamEvents : MonoBehaviour
{

    Vector3 touch;
    public float sizeWidthTemp = 0;
    public float sizeHeightTemp = 0;

    public float zoomMin = 1;
    public float zoomMax = 8;
    public bool pause = false;
    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroLastPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOneLastPos = touchOne.position - touchOne.deltaPosition;

                float distTouch = (touchZeroLastPos - touchOneLastPos).magnitude;
                float currentDistTouch = (touchZero.position - touchOne.position).magnitude;

                float difference = currentDistTouch - distTouch;
                zoom(difference * 0.01f);
            }

            else if (Input.GetMouseButton(0))
            {
                Vector3 direction = touch - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (((Camera.main.transform.position.y + direction.y <= sizeHeightTemp) & (Camera.main.transform.position.x + direction.x <= sizeWidthTemp)) & ((Camera.main.transform.position.y + direction.y >= 0) & (Camera.main.transform.position.x + direction.x >= 0)))
                {
                    Camera.main.transform.position += direction;
                }
            }

            zoom(Input.GetAxis("Mouse ScrollWheel"));
        }
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomMin, zoomMax);
    }
}

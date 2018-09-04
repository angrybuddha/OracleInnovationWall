using UnityEngine;
using System.Collections;

public class CameraOrbit : MonoBehaviour
{
    public Transform Target;
    public float Distance = 5.0f;
    public float xSpeed = 250.0f;
    public float ySpeed = 120.0f;
    public float yMinLimit = -20.0f;
    public float yMaxLimit = 80.0f;

    private float x;
    private float y;
    private float prev_x;
    private float prev_y;
    private bool mouse_down;


    void Awake()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        updateCamera();
    }


    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //Debug.Log("Pressed left click.");
            mouse_down = true;
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            mouse_down = false;
        }

        if (Target != null && mouse_down)
        {
            updateCamera();
        }
    }

    private void updateCamera()
    {
        x += (float)(Input.GetAxis("Mouse X") * xSpeed * 0.02f);
        y -= (float)(Input.GetAxis("Mouse Y") * ySpeed * 0.02f);

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * (new Vector3(0.0f, 1.0f, -Distance)) + Target.position;

        transform.rotation = rotation;
        transform.position = position;
    }

    public static float Wrap(float value, float max, float min)
    {
        value -= min;
        max -= min;
        if (max == 0)
            return min;

        value = value % max;
        value += min;
        while (value < min)
        {
            value += max;
        }

        return value;

    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}

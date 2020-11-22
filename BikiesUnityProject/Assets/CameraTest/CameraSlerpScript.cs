using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSlerpScript : MonoBehaviour
{
    // Public Interpolation Variables
    public GameObject Destination;
    public float InterpolationSpeed;
    public float InterpolationAcceleration;

    // Private Interpolation Triggers
    float m_StartedAt = 0.0f;
    bool m_Interpolate = false;

    // Private Interpolation Variables
    Vector3 m_Position;
    Quaternion m_Orientation;
    Transform m_dTransform;

    // Variable to enable user camera movement
    Camera_Movement m_CamScript;

    // Start is called before the first frame update
    void Start()
    {
        m_CamScript = GetComponent<Camera_Movement>();

        m_dTransform = Destination.transform;
        m_Orientation = transform.rotation;
        m_Position = transform.position;

        m_Interpolate = true;
        m_StartedAt = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Interpolate)
        {
            float time = (Time.realtimeSinceStartup - m_StartedAt);
            float interpIndex = InterpolationSpeed*time + 1/2*InterpolationAcceleration*Mathf.Pow(time, 2);
            
            transform.rotation = Quaternion.Slerp(m_Orientation, m_dTransform.rotation, interpIndex);
            transform.position = Vector3.Slerp(m_Position, m_dTransform.position, interpIndex);

            if (transform.position == m_dTransform.position && transform.rotation == m_dTransform.rotation)
            {
                m_CamScript.AllowMovement = true;
                m_Interpolate = false;
            }


           // --- ---
           // while (m_TimePassed <= InterpolationTime)
           // {
           //     m_TimePassed += Time.deltaTime;
           //     Transform dTransform = Destination.transform;
           //
           //     transform.rotation = Quaternion.Slerp(m_Orientation, dTransform.rotation, m_TimePassed*Time.deltaTime);
           //     transform.position = Vector3.Slerp(m_Position, dTransform.position, m_TimePassed * Time.deltaTime);
           //
           //     if (m_Position == dTransform.position && m_Orientation == dTransform.rotation)
           //     {
           //         m_TimePassed = 0.0f;
           //         m_Interpolate = false;
           //     }
           // }
        }
    }
}

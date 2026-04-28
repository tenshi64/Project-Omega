using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidSimulation : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    Transform liquidBone;

    [SerializeField]
    float shakeForce;

    Vector3 oldPosition;
    [SerializeField]
    float shakeThreshold;

    float t;
    Vector3 currentRotation;

    private void Start()
    {
        currentRotation = transform.eulerAngles;
    }

    void LateUpdate()
    {
        float speedPerSec = Vector3.Distance(transform.position, oldPosition) / Time.deltaTime;
        liquidBone.eulerAngles = new Vector3(transform.eulerAngles.x + ((speedPerSec + 1) * shakeForce * shakeThreshold * Mathf.Sin(t)), transform.eulerAngles.y, transform.eulerAngles.z + ((speedPerSec + 1) * shakeForce * shakeThreshold * Mathf.Sin(t)));

        if(transform.position != oldPosition)
        {
            shakeThreshold = 1;
        }

        shakeThreshold = Mathf.Lerp(shakeThreshold, 0, 0.5f * Time.deltaTime);

        if(shakeThreshold < 0.02f)
        {
            shakeThreshold = 0;
            t = 0;
        }

        t += 50 * Time.deltaTime;

        currentRotation = transform.eulerAngles;
        oldPosition = transform.position;
    }
}

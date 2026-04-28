using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Dependencies")]
    float currentY;
    float currentX;

    [SerializeField]
    Transform cameraTarget;

    [Header("Configuration")]
    [SerializeField]
    Vector2 minMaxRotation;

    [SerializeField]
    Vector2 minMaxDistance;

    [SerializeField]
    [Range(0.01f, 10f)]
    float sensitivity;

    float distance;

    [SerializeField]
    LayerMask cameraCollisionExcludeMask;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        currentY += Input.GetAxis("Mouse X") * sensitivity;
        currentX -= Input.GetAxis("Mouse Y") * sensitivity;
        currentX = Mathf.Clamp(currentX, minMaxRotation.x, minMaxRotation.y);
    }

    void LateUpdate()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UnityEditor.EditorWindow.focusedWindow.maximized = !UnityEditor.EditorWindow.focusedWindow.maximized;
        }
        #endif

        Vector3 dir = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX, currentY, 0);
        transform.position = cameraTarget.position + rotation * dir;
        transform.LookAt(cameraTarget.position);

        if (Physics.Raycast(cameraTarget.position, rotation * dir, out RaycastHit hit, minMaxDistance.y + 0.1f, cameraCollisionExcludeMask))
        {
            distance = Mathf.Clamp(hit.distance - 0.1f, minMaxDistance.x, minMaxDistance.y);
        }
        else
        {
            distance = Mathf.Lerp(distance, minMaxDistance.y, 5 * Time.deltaTime);
        }
    }
}

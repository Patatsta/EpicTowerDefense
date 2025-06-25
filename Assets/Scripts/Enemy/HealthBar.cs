using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera == null) return;

        transform.rotation = mainCamera.transform.rotation;
    }
}

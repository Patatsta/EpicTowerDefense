using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform cam;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private Vector2 limitX = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 limitZ = new Vector2(-10f, 10f);

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float zoomSmoothness = 10f;

    private float targetZoom;
    private float currentZoom;

    private void Start()
    {
        currentZoom = cam.localPosition.magnitude;
        targetZoom = currentZoom;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Vector3 right = new Vector3(cam.right.x, 0, cam.right.z).normalized;
        Vector3 forward = new Vector3(cam.forward.x, 0, cam.forward.z).normalized;
        Vector3 direction = (right * hor + forward * vert).normalized;

        transform.position += direction * moveSpeed * Time.unscaledDeltaTime;

        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, limitX.x, limitX.y);
        clamped.z = Mathf.Clamp(clamped.z, limitZ.x, limitZ.y);
        transform.position = clamped;
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            targetZoom -= scroll * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        currentZoom = Mathf.Lerp(currentZoom, targetZoom, Time.unscaledDeltaTime * zoomSmoothness);
        cam.localPosition = -cam.forward * currentZoom;
    }
}




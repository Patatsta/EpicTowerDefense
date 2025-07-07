using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _cam;

    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private Vector2 _limitX = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 _limitZ = new Vector2(-10f, 10f);

    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 20f;
    [SerializeField] private float _zoomSmoothness = 10f;

    private float _targetZoom;
    private float _currentZoom;

    private void Start()
    {
        _currentZoom = _cam.localPosition.magnitude;
        _targetZoom = _currentZoom;
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

        Vector3 right = new Vector3(_cam.right.x, 0, _cam.right.z).normalized;
        Vector3 forward = new Vector3(_cam.forward.x, 0, _cam.forward.z).normalized;
        Vector3 direction = (right * hor + forward * vert).normalized;

        transform.position += direction * _moveSpeed * Time.unscaledDeltaTime;

        Vector3 clamped = transform.position;
        clamped.x = Mathf.Clamp(clamped.x, _limitX.x, _limitX.y);
        clamped.z = Mathf.Clamp(clamped.z, _limitZ.x, _limitZ.y);
        transform.position = clamped;
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) > 0.01f)
        {
            _targetZoom -= scroll * _zoomSpeed;
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        }

        _currentZoom = Mathf.Lerp(_currentZoom, _targetZoom, Time.unscaledDeltaTime * _zoomSmoothness);
        _cam.localPosition = -_cam.forward * _currentZoom;
    }
}




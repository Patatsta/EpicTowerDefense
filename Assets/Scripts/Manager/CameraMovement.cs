using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _cam;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector2 limitX = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 limitZ = new Vector2(-10f, 10f);

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;

    private void Update()
    {
       
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        
        Vector3 right = new Vector3(_cam.right.x, 0, _cam.right.z).normalized;
        Vector3 forward = new Vector3(_cam.forward.x, 0, _cam.forward.z).normalized;
        Vector3 moveDir = (right * hor + forward * vert).normalized;

       
        Vector3 newPos = _cam.position + moveDir * moveSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, limitX.x, limitX.y);
        newPos.z = Mathf.Clamp(newPos.z, limitZ.x, limitZ.y);

        _cam.position = newPos;

        //// Zoom (ohne Einschränkungen)
        //float scroll = Input.GetAxis("Mouse ScrollWheel");
        //Vector3 zoomDir = _cam.forward * scroll * zoomSpeed;
        //_cam.position += zoomDir;
    }
}

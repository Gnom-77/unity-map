using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private MapController mapController;
    private Vector2 _direction;

    private void Update()
    {
        PlayerInput();
        Movement();
        HandleZoomInput();
    }


    private void PlayerInput()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _direction.y = Input.GetAxisRaw("Vertical");
    }

    private void Movement()
    {
        _cameraTransform.position = new Vector3(_cameraTransform.position.x + _direction.x * _speed * Time.deltaTime, _cameraTransform.position.y + _direction.y * _speed * Time.deltaTime, _cameraTransform.position.z);
    }

    private void HandleZoomInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            int newZoom = mapController.markerManager.currentZoom + (scroll > 0 ? 1 : -1);
            newZoom = Mathf.Clamp(newZoom, 0, 19);
            mapController.SetZoomLevel(newZoom);
        }
    }
}

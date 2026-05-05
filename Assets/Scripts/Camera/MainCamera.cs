using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [SerializeField] Transform targetTransform;

    [Space(10)]
    [SerializeField] float sensitive = 0.1f;
    [SerializeField] float smoothSpeed = 0.5f;

    [Space(10)]
    [SerializeField] Vector3 offset;
    [SerializeField] public Vector3 mainCameraoffset;

    [Header("Camera Rotation Limits")]
    [SerializeField] float minVerticalAngle = -80f; 
    [SerializeField] float maxVerticalAngle = 80f;

    [Header("Camera Zoom Limits")]
    [SerializeField] float minZoom = -2.55f;
    [SerializeField] float maxZoom = 5;

    private float currentXRotation = 0f;

    private Transform CameraTransform;

    InputAction lookAction;
    InputAction zoomAction;

    void Start()
    {
        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();

        Cursor.lockState = CursorLockMode.Locked;

        lookAction = InputSystem.actions.FindAction("Look");
        zoomAction = InputSystem.actions.FindAction("MouseScroll");

        CameraTransform.localPosition = mainCameraoffset;

        currentXRotation = transform.eulerAngles.x;
        if (currentXRotation > 180f)
            currentXRotation -= 360f; 
    }

    void Update()
    {
        Zoom();
        Move();
        Rotation();
    }

    private void Zoom()
    {
        Vector2 zoomVector = zoomAction.ReadValue<Vector2>();
        CameraTransform.localPosition = new Vector3(CameraTransform.localPosition.x, CameraTransform.localPosition.y, Mathf.Clamp(CameraTransform.localPosition.z + zoomVector.y, 
            minZoom, maxZoom));
    }

    private void Move()
    {
        Vector3 targetPos = targetTransform.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        transform.position = smoothFollow;
    }

    private void Rotation()
    {
        Vector2 moveValue = lookAction.ReadValue<Vector2>();

        currentXRotation -= moveValue.y * sensitive * Time.deltaTime;
        currentXRotation = Mathf.Clamp(currentXRotation, minVerticalAngle, maxVerticalAngle);

        float yRotation = moveValue.x * sensitive * Time.deltaTime;

        transform.rotation = Quaternion.Euler(currentXRotation, transform.eulerAngles.y + yRotation, 0f);
    }
}
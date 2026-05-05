using System.Collections;
using Unity.Android.Gradle.Manifest;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraEffects : MonoBehaviour
{
    [SerializeField] float frequence = 1.0f;
    [SerializeField] float amplitude = 3.0f;
    [SerializeField] float smoothSpeed = 5f; 

    private float intensity = 0f;
    private float multiply = 0f;

    InputAction moveAction;
    InputAction Sprint;

    private MainCamera mainCamera;
    private Transform CameraTransform;
    private float timePassed = 0;

    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        Sprint = InputSystem.actions.FindAction("Sprint");

        mainCamera = GetComponent<MainCamera>();

        CameraTransform = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveShake();
    }

    IEnumerator Vibration(float amountTime, float frq, float ampl)
    {
        float timeCont = 0;
        while (timeCont < amountTime)
        {
            timeCont += Time.deltaTime;

            float offsetX = (Mathf.Sin(timeCont * frq) * ampl);

            CameraTransform.localRotation = Quaternion.Euler(CameraTransform.rotation.x,
                 CameraTransform.rotation.y, offsetX);

            yield return null;
        }
    }


    private void MoveShake()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        bool isMoving = moveValue != Vector2.zero;

        multiply = Sprint.WasPerformedThisFrame() && isMoving ? 1 :
           Sprint.WasReleasedThisFrame() ? 0 : multiply;

        float target = isMoving ? 1f : 0f;
        intensity = Mathf.MoveTowards(intensity, target + multiply, smoothSpeed * Time.deltaTime);

        if (intensity <= 0f)
            timePassed = 0f;

        timePassed += Time.deltaTime;

        float offsetX = (Mathf.Sin(timePassed * frequence) * amplitude) * intensity;
        float offsetY = (Mathf.Cos(timePassed * frequence) * amplitude) * intensity;

        CameraTransform.localPosition = new Vector3(
            mainCamera.mainCameraoffset.x + offsetX, 
            mainCamera.mainCameraoffset.y + offsetY, 
            CameraTransform.localPosition.z
        );
    }
}

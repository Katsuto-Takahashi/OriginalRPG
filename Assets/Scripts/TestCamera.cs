using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TestCamera : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    bool enableInput = true;

    enum SimulateType
    {
        Update,
        FixedUpdate
    }
    [SerializeField]
    SimulateType m_type = SimulateType.Update;

    [SerializeField]
    bool enableDollyZoom = true;

    [SerializeField]
    bool enableWallDetection = true;

    [SerializeField]
    bool enableFixedPoint = false;

    [SerializeField]
    float inputSpeed = 4.0f;

    [SerializeField]
    Vector3 freeLookRotation;

    [SerializeField]
    float height;

    [SerializeField]
    float distance = 8.0f;

    [SerializeField]
    Vector3 rotation;

    [SerializeField]
    [Range(0.01f, 100.0f)]
    float positionDamping = 16.0f;

    [SerializeField]
    [Range(0.01f, 100.0f)]
    float rotationDamping = 16.0f;

    [SerializeField]
    [Range(0.1f, 0.99f)]
    float dolly = 0.34f;

    [SerializeField]
    float noise = 0.0f;

    [SerializeField]
    float noiseZ = 0.0f;

    [SerializeField]
    float noiseSpeed = 1.0f;

    [SerializeField]
    Vector3 vibration = Vector3.zero;

    [SerializeField]
    float wallDetectionDistance = 0.3f;

    [SerializeField]
    LayerMask wallDetectionMask = 1;

    Camera cam;
    float targetDistance;
    Vector3 targetPosition;
    Vector3 targetRotation;
    Vector3 targetFree;
    float targetHeight;
    float targetDolly;

    void Start()
    {
        cam = GetComponent<Camera>();

        targetDistance = distance;
        targetRotation = rotation;
        targetFree = freeLookRotation;
        targetHeight = height;
        targetDolly = dolly;

        var dollyDist = targetDistance;

        if (enableDollyZoom)
        {
            var dollyFoV = GetDollyFoV(Mathf.Pow(1.0f / targetDolly, 2.0f), targetDistance);
            dollyDist = GetDollyDistance(dollyFoV, targetDistance);
            cam.fieldOfView = dollyFoV;
        }
        if (target == null) return;

        var pos = target.position + Vector3.up * targetHeight;
        var offset = Vector3.zero;

        offset.x += Mathf.Sin(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.z += -Mathf.Cos(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.y += Mathf.Sin(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        targetPosition = pos + offset;

        if (m_type == SimulateType.Update) Observable.EveryUpdate().Subscribe(_ => OnUpdate()).AddTo(this);
        else Observable.EveryFixedUpdate().Subscribe(_ => OnFixedUpdate()).AddTo(this);
    }

    void OnUpdate()
    {
        Simulate(Time.deltaTime);
    }

    void OnFixedUpdate()
    {
        Simulate(Time.fixedDeltaTime);
    }

    void Simulate(float deltaTime)
    {
        ApplyInput();

        var posDampRate = Mathf.Clamp01(deltaTime * 100.0f / positionDamping);
        var rotDampRate = Mathf.Clamp01(deltaTime * 100.0f / rotationDamping);

        targetDistance = Mathf.Lerp(targetDistance, distance, posDampRate);
        targetRotation = Vector3.Lerp(targetRotation, rotation, rotDampRate);
        targetFree = Vector3.Lerp(targetFree, freeLookRotation, rotDampRate);
        targetHeight = Mathf.Lerp(targetHeight, height, posDampRate);
        targetDolly = Mathf.Lerp(targetDolly, dolly, posDampRate);

        if (Mathf.Abs(targetDolly - dolly) > 0.005f)
        {
            targetDistance = distance;
        }

        var dollyDist = targetDistance;
        if (enableDollyZoom)
        {
            var dollyFoV = GetDollyFoV(Mathf.Pow(1.0f / targetDolly, 2.0f), targetDistance);
            dollyDist = GetDollyDistance(dollyFoV, targetDistance);
            cam.fieldOfView = dollyFoV;
        }

        //if (target == null) return;

        var pos = target.position + Vector3.up * targetHeight;

        if (enableWallDetection)
        {
            RaycastHit hit;
            var dir = (targetPosition - pos).normalized;
            if (Physics.SphereCast(pos, wallDetectionDistance, dir, out hit, dollyDist, wallDetectionMask))
            {
                dollyDist = hit.distance;
            }
        }

        var offset = Vector3.zero;
        offset.x += Mathf.Sin(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.z += -Mathf.Cos(targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.y += Mathf.Sin(targetRotation.x * Mathf.Deg2Rad) * dollyDist;

        if (Mathf.Abs(targetDolly - dolly) > 0.005f)
        {
            targetPosition = offset + pos;
        }
        else
        {
            targetPosition = Vector3.Lerp(targetPosition, offset + pos, posDampRate);
        }

        if (!enableFixedPoint) cam.transform.position = targetPosition;
        cam.transform.LookAt(pos, Quaternion.Euler(0.0f, 0.0f, targetRotation.z) * Vector3.up);
        cam.transform.Rotate(targetFree);

        if (noise > 0.0f || noiseZ > 0.0f)
        {
            var rotNoise = Vector3.zero;
            rotNoise.x = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0.0f) - 0.5f) * noise;
            rotNoise.y = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0.4f) - 0.5f) * noise;
            rotNoise.z = (Mathf.PerlinNoise(Time.time * noiseSpeed, 0.8f) - 0.5f) * noiseZ;
            cam.transform.Rotate(rotNoise);
        }

        if (vibration.sqrMagnitude > 0.0f)
        {
            cam.transform.Rotate(new Vector3(Random.Range(-1.0f, 1.0f) * vibration.x, Random.Range(-1.0f, 1.0f) * vibration.y, Random.Range(-1.0f, 1.0f) * vibration.z));
        }
    }

    void ApplyInput()
    {
        if (enableInput)
        {

            //if (InputController.Instance.Option())
            //{
            //    dolly += InputController.Instance.Zoom() * 0.1f;
            //    //dolly += Input.GetAxis("Mouse ScrollWheel") * 0.2f;
            //    //dolly += (Input.GetAxis("L2trigger") - Input.GetAxis("R2trigger")) * 0.1f;
            //    Debug.Log(InputController.Instance.Zoom() * 0.1f);
            //    //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            //    //Debug.Log(Input.GetAxis("L2trigger") - Input.GetAxis("R2trigger"));
            //    dolly = Mathf.Clamp(dolly, 0.1f, 0.99f);
            //}
            //else
            if (InputController.Instance.Option())
            {
                distance *= 1.0f - InputController.Instance.Zoom();
                //distance *= 1.0f - Input.GetAxis("Mouse ScrollWheel");
                //distance *= 1.0f + Input.GetAxis("L2trigger") - Input.GetAxis("R2trigger");
                distance = Mathf.Clamp(distance, 0.01f, 1000.0f);
            }

            //if (Input.GetMouseButton(0))
            {
                Vector2 inputValue = InputController.Instance.CameraMove();
                inputValue.Normalize();
                rotation.x -= inputValue.y * inputSpeed;
                rotation.x = Mathf.Clamp(rotation.x, -89.9f, 89.9f);
                rotation.y -= inputValue.x * inputSpeed;
            }
            //if (Input.GetMouseButton(1))
            //{
            //    freeLookRotation.x -= Input.GetAxis("Mouse Y") * inputSpeed * 0.2f;
            //    freeLookRotation.y += Input.GetAxis("Mouse X") * inputSpeed * 0.2f;
            //}
            //if (Input.GetMouseButtonDown(2))
            //{
            //    freeLookRotation = Vector3.zero;
            //}
        }
    }

    void WallCheck(Vector3 pos, float dollyDist)
    {

    }

    float GetDollyDistance(float fov, float distance)
    {
        return distance / (2.0f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad));
    }

    float GetFrustumHeight(float distance, float fov)
    {
        return 2.0f * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
    }

    float GetDollyFoV(float dolly, float distance)
    {
        return 2.0f * Mathf.Atan(distance * 0.5f / dolly) * Mathf.Rad2Deg;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TestCamera : MonoBehaviour
{
    [SerializeField]
    Transform m_target;

    [SerializeField]
    bool m_enableInput = true;

    enum SimulateType
    {
        Update,
        FixedUpdate
    }
    [SerializeField]
    SimulateType m_type = SimulateType.Update;

    [SerializeField]
    bool m_enableDollyZoom = true;

    [SerializeField]
    bool m_enableWallDetection = true;

    [SerializeField]
    bool m_enableFixedPoint = false;

    [SerializeField]
    float m_inputSpeed = 1.0f;

    [SerializeField]
    Vector3 m_freeLookRotation;

    [SerializeField]
    float m_height;

    [SerializeField]
    float m_distance = 8.0f;

    [SerializeField]
    Vector3 m_rotation = new Vector3(15.0f, 0.0f, 0.0f);

    [SerializeField]
    [Range(0.01f, 100.0f)]
    float m_positionDamping = 16.0f;

    [SerializeField]
    [Range(0.01f, 100.0f)]
    float m_rotationDamping = 16.0f;

    [SerializeField]
    [Range(0.1f, 0.99f)]
    float m_dolly = 0.34f;

    [SerializeField]
    float m_noise = 0.0f;

    [SerializeField]
    float m_noiseZ = 0.0f;

    [SerializeField]
    float m_noiseSpeed = 1.0f;

    [SerializeField]
    Vector3 m_vibration = Vector3.zero;

    [SerializeField]
    float m_wallDetectionDistance = 0.3f;

    [SerializeField]
    LayerMask m_wallDetectionMask = default;

    Camera m_cam;
    float m_targetDistance;
    Vector3 m_targetPosition;
    Vector3 m_targetRotation;
    Vector3 m_targetFree;
    float m_targetHeight;
    float m_targetDolly;

    void Start()
    {
        m_cam = GetComponent<Camera>();

        m_targetDistance = m_distance;
        m_targetRotation = m_rotation;
        m_targetFree = m_freeLookRotation;
        m_targetHeight = m_height;
        m_targetDolly = m_dolly;

        var dollyDist = m_targetDistance;

        if (m_enableDollyZoom)
        {
            var dollyFoV = DollyFoV(Mathf.Pow(1.0f / m_targetDolly, 2.0f), m_targetDistance);
            dollyDist = DollyDistance(dollyFoV, m_targetDistance);
            m_cam.fieldOfView = dollyFoV;
        }
        if (m_target == null) return;

        var pos = m_target.position + Vector3.up * m_targetHeight;
        var offset = Vector3.zero;

        offset.x += Mathf.Sin(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.z += -Mathf.Cos(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.y += Mathf.Sin(m_targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        m_targetPosition = pos + offset;

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

        var posDampRate = Mathf.Clamp01(deltaTime * 100.0f / m_positionDamping);
        var rotDampRate = Mathf.Clamp01(deltaTime * 100.0f / m_rotationDamping);

        m_targetDistance = Mathf.Lerp(m_targetDistance, m_distance, posDampRate);
        m_targetRotation = Vector3.Lerp(m_targetRotation, m_rotation, rotDampRate);
        m_targetFree = Vector3.Lerp(m_targetFree, m_freeLookRotation, rotDampRate);
        m_targetHeight = Mathf.Lerp(m_targetHeight, m_height, posDampRate);
        m_targetDolly = Mathf.Lerp(m_targetDolly, m_dolly, posDampRate);

        if (Mathf.Abs(m_targetDolly - m_dolly) > 0.005f)
        {
            m_targetDistance = m_distance;
        }

        var dollyDist = m_targetDistance;
        if (m_enableDollyZoom)
        {
            var dollyFoV = DollyFoV(Mathf.Pow(1.0f / m_targetDolly, 2.0f), m_targetDistance);
            dollyDist = DollyDistance(dollyFoV, m_targetDistance);
            m_cam.fieldOfView = dollyFoV;
        }

        //if (target == null) return;

        var pos = m_target.position + Vector3.up * m_targetHeight;

        if (m_enableWallDetection)
        {
            RaycastHit hit;
            var dir = (m_targetPosition - pos).normalized;
            if (Physics.SphereCast(pos, m_wallDetectionDistance, dir, out hit, dollyDist, m_wallDetectionMask))
            {
                dollyDist = hit.distance;
            }
        }

        var offset = Vector3.zero;
        offset.x += Mathf.Sin(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.z += -Mathf.Cos(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * dollyDist;
        offset.y += Mathf.Sin(m_targetRotation.x * Mathf.Deg2Rad) * dollyDist;

        if (Mathf.Abs(m_targetDolly - m_dolly) > 0.005f)
        {
            m_targetPosition = offset + pos;
        }
        else
        {
            m_targetPosition = Vector3.Lerp(m_targetPosition, offset + pos, posDampRate);
        }

        if (!m_enableFixedPoint) m_cam.transform.position = m_targetPosition;
        m_cam.transform.LookAt(pos, Quaternion.Euler(0.0f, 0.0f, m_targetRotation.z) * Vector3.up);
        m_cam.transform.Rotate(m_targetFree);

        if (m_noise > 0.0f || m_noiseZ > 0.0f)
        {
            var rotNoise = Vector3.zero;
            rotNoise.x = (Mathf.PerlinNoise(Time.time * m_noiseSpeed, 0.0f) - 0.5f) * m_noise;
            rotNoise.y = (Mathf.PerlinNoise(Time.time * m_noiseSpeed, 0.4f) - 0.5f) * m_noise;
            rotNoise.z = (Mathf.PerlinNoise(Time.time * m_noiseSpeed, 0.8f) - 0.5f) * m_noiseZ;
            m_cam.transform.Rotate(rotNoise);
        }

        if (m_vibration.sqrMagnitude > 0.0f)
        {
            m_cam.transform.Rotate(new Vector3(Random.Range(-1.0f, 1.0f) * m_vibration.x, Random.Range(-1.0f, 1.0f) * m_vibration.y, Random.Range(-1.0f, 1.0f) * m_vibration.z));
        }
    }

    void ApplyInput()
    {
        if (m_enableInput)
        {
            Vector2 inputValue = InputController.Instance.CameraMove();
            inputValue.Normalize();
            m_rotation.x -= inputValue.y * m_inputSpeed;
            m_rotation.x = Mathf.Clamp(m_rotation.x, -89.9f, 89.9f);
            m_rotation.y -= inputValue.x * m_inputSpeed;

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
                float zoom = 0.0f;
                if (InputController.Instance.Zoom() > 0.0f)
                {
                    zoom = InputController.Instance.Zoom() / InputController.Instance.Zoom();
                }
                else if (InputController.Instance.Zoom() < 0.0f)
                {
                    zoom = -InputController.Instance.Zoom() / InputController.Instance.Zoom();
                }
                m_distance *= 1.0f - zoom;
                //distance *= 1.0f - Input.GetAxis("Mouse ScrollWheel");
                //distance *= 1.0f + Input.GetAxis("L2trigger") - Input.GetAxis("R2trigger");
                m_distance = Mathf.Clamp(m_distance, 0.01f, 1000.0f);
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

    float DollyDistance(float fov, float distance)
    {
        return distance / (2.0f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad));
    }

    float FrustumHeight(float distance, float fov)
    {
        return 2.0f * distance * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
    }

    float DollyFoV(float dolly, float distance)
    {
        return 2.0f * Mathf.Atan(distance * 0.5f / dolly) * Mathf.Rad2Deg;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class CameraController : MonoBehaviour
{
    /// <summary>cameraが見る対象</summary>
    [SerializeField]
    Transform m_target;

    /// <summary>入力を受け付けるか</summary>
    [SerializeField]
    bool m_enableInput = true;

    enum SimulateType
    {
        Update,
        FixedUpdate
    }
    [SerializeField]
    SimulateType m_simulateType = SimulateType.Update;

    /// <summary>ドリーズームをするか</summary>
    [SerializeField]
    bool m_enableDollyZoom = true;

    /// <summary>壁裏にカメラがめり込むのを防ぐか</summary>
    [SerializeField]
    bool m_enableWallDetection = true;

    /// <summary>cameraを止めるか</summary>
    [SerializeField]
    bool m_enableFixedPoint = false;

    /// <summary>パッドでのcameraの縦方向移動速度</summary>
    [SerializeField]
    float m_padCameraVerticalMoveSpeed = 1.0f;

    /// <summary>パッドでのcameraの横方向移動速度</summary>
    [SerializeField]
    float m_padCameraHorizontalMoveSpeed = 1.0f;

    /// <summary>マウスでのcameraの縦方向移動速度</summary>
    [SerializeField]
    float m_mouseCameraVerticalMoveSpeed = 1.0f;

    /// <summary>マウスでのcameraの横方向移動速度</summary>
    [SerializeField]
    float m_mouseCameraHorizontalMoveSpeed = 1.0f;

    /// <summary>targetの周りを見渡す際の回転</summary>
    [SerializeField]
    Vector3 m_freeLookRotation;

    /// <summary>cameraが見る高さ</summary>
    [SerializeField]
    float m_height;

    /// <summary>対象とcameraとの距離</summary>
    [SerializeField]
    float m_distance = 8.0f;

    /// <summary>cameraの回転</summary>
    [SerializeField]
    Vector3 m_rotation = new Vector3(15.0f, 0.0f, 0.0f);

    /// <summary>移動時の遅延</summary>
    [SerializeField]
    [Range(0.01f, 100.0f)]
    float m_positionDamping = 16.0f;

    /// <summary>回転時の遅延</summary>
    [SerializeField]
    [Range(0.01f, 100.0f)]
    float m_rotationDamping = 16.0f;

    /// <summary>ドリーズームの範囲</summary>
    [SerializeField]
    [Range(0.1f, 0.99f)]
    float m_dolly = 0.34f;

    /// <summary>手ブレの大きさ</summary>
    [SerializeField]
    float m_noise = 0.0f;

    /// <summary>z方向の手ブレの大きさ</summary>
    [SerializeField]
    float m_noiseZ = 0.0f;

    /// <summary>手ブレの速さ</summary>
    [SerializeField]
    float m_noiseSpeed = 1.0f;

    /// <summary>振動の大きさ</summary>
    [SerializeField]
    Vector3 m_vibration = Vector3.zero;

    /// <summary>壁を検出する距離</summary>
    [SerializeField]
    float m_wallDetectionDistance = 0.3f;

    /// <summary>壁と認識するLayerMask</summary>
    [SerializeField]
    LayerMask m_wallDetectionMask = default;

    /// <summary>最小の角度</summary>
    [SerializeField]
    float m_minAngle = -80.0f;

    /// <summary>最大の角度</summary>
    [SerializeField]
    float m_maxAngle = 80.0f;

    /// <summary>カメラ</summary>
    Camera m_cam;
    /// <summary>目標の距離</summary>
    float m_targetDistance;
    /// <summary>目標の移動</summary>
    Vector3 m_targetPosition;
    /// <summary>目標の回転</summary>
    Vector3 m_targetRotation;
    /// <summary>目標のフリールック</summary>
    Vector3 m_targetFree;
    /// <summary>目標の高さ</summary>
    float m_targetHeight;
    /// <summary>目標のドリーズーム</summary>
    float m_targetDolly;
    /// <summary>目標のドリーズーム時の距離</summary>
    float m_dollyDistance;
    /// <summary>初期の回転</summary>
    Quaternion m_initialRotation;
    /// <summary>ターゲットとカメラの高さの差</summary>
    float m_heightdifference;
    /// <summary>cameraの縦方向移動速度</summary>
    float m_verticalMoveSpeed;
    /// <summary>cameraの横方向移動速度</summary>
    float m_horizontalMoveSpeed;

    void Start()
    {
        m_cam = GetComponent<Camera>();

        m_targetDistance = m_distance;
        m_targetRotation = m_rotation;
        m_targetFree = m_freeLookRotation;
        m_targetHeight = m_height;
        m_targetDolly = m_dolly;

        m_dollyDistance = m_targetDistance;

        DollyZoom();

        if (m_target == null) return;

        var pos = m_target.position + Vector3.up * m_targetHeight;
        var offset = Vector3.zero;

        offset.x += Mathf.Sin(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        offset.z += -Mathf.Cos(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        offset.y += Mathf.Sin(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        m_targetPosition = pos + offset;

        m_initialRotation = new Quaternion(m_targetRotation.x, m_targetRotation.y, m_targetRotation.z, m_cam.transform.rotation.w);
        m_heightdifference = m_targetPosition.y - pos.y;

        if (m_simulateType == SimulateType.Update) Observable.EveryUpdate().Subscribe(_ => OnUpdate()).AddTo(this);
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

        m_dollyDistance = m_targetDistance;

        DollyZoom();

        var pos = m_target.position + Vector3.up * m_targetHeight;

        WallCheck(pos);

        var offset = Vector3.zero;
        offset.x += Mathf.Sin(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        offset.z += -Mathf.Cos(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        offset.y += Mathf.Sin(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;

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
            switch (InputController.Instance.InputControllerType)
            {
                case InputType.Pad:
                    m_verticalMoveSpeed = m_padCameraVerticalMoveSpeed;
                    m_horizontalMoveSpeed = m_padCameraHorizontalMoveSpeed;
                    break;
                case InputType.Mouse:
                    m_verticalMoveSpeed = m_mouseCameraVerticalMoveSpeed;
                    m_horizontalMoveSpeed = m_mouseCameraHorizontalMoveSpeed;
                    break;
                default:
                    break;
            }

            Vector2 inputValue = InputController.Instance.CameraMove();
            inputValue.Normalize();
            m_rotation.x -= inputValue.y * m_horizontalMoveSpeed;
            m_rotation.x = Mathf.Clamp(m_rotation.x, m_minAngle, m_maxAngle);
            m_rotation.y -= inputValue.x * m_verticalMoveSpeed;

            m_rotation.y = m_rotation.y > 180 ? m_rotation.y - 360 : m_rotation.y;
            m_rotation.y = m_rotation.y < -180 ? m_rotation.y + 360 : m_rotation.y;
            m_targetRotation.y = m_rotation.y;

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
            if (InputController.Instance.ResetCamera())
            {
                ResetCamera();
            }
        }
    }

    void ResetCamera()
    {
        var angle = GetResetAngle();
        var pos = m_target.position + Vector3.up * m_targetHeight;
        m_rotation = new Vector3(m_initialRotation.x, m_rotation .y + angle, m_initialRotation.z);
        m_cam.transform.rotation = Quaternion.Euler(m_rotation);
        m_cam.transform.LookAt(pos, Quaternion.Euler(0.0f, 0.0f, m_rotation.z) * Vector3.up);
        m_targetRotation.y = m_rotation.y;
        m_targetRotation.x = m_initialRotation.x;

        var resetPos = GetResetPosition();
        m_targetPosition = resetPos;
        m_cam.transform.position = m_targetPosition;
    }

    Vector3 GetResetPosition()
    {
        var playerPosition = m_target.position + Vector3.up * m_targetHeight;
        var distance = Mathf.Abs(Mathf.Pow(m_dollyDistance, 2) - Mathf.Pow(m_heightdifference + playerPosition.y, 2));
        var resetPosition = -m_target.transform.forward * Mathf.Sqrt(distance);
        resetPosition = new Vector3(playerPosition.x + resetPosition.x, playerPosition.y + m_heightdifference, playerPosition.z + resetPosition.z);
        return resetPosition;
    }

    float GetResetAngle()
    {
        var playerFoward = Vector3.ProjectOnPlane(m_target.transform.forward, Vector3.up);
        var cameraFoward = Vector3.ProjectOnPlane(m_cam.transform.forward, Vector3.up);
        var angle = Vector3.SignedAngle(playerFoward, cameraFoward, Vector3.up);
        return angle;
    }

    void DollyZoom()
    {
        if (m_enableDollyZoom)
        {
            var dollyFoV = DollyFoV(Mathf.Pow(1.0f / m_targetDolly, 2.0f), m_targetDistance);
            m_dollyDistance = DollyDistance(dollyFoV, m_targetDistance);
            m_cam.fieldOfView = dollyFoV;
        }
    }

    void WallCheck(Vector3 pos)
    {
        if (m_enableWallDetection)
        {
            var dir = (m_targetPosition - pos).normalized;
            if (Physics.SphereCast(pos, m_wallDetectionDistance, dir, out RaycastHit hit, m_dollyDistance, m_wallDetectionMask))
            {
                m_dollyDistance = hit.distance;
            }
        }
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

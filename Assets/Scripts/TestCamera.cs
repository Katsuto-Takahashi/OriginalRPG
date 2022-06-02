﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class TestCamera : MonoBehaviour
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

    /// <summary></summary>
    [SerializeField]
    bool m_enableDollyZoom = true;

    /// <summary></summary>
    [SerializeField]
    bool m_enableWallDetection = true;

    /// <summary></summary>
    [SerializeField]
    bool m_enableFixedPoint = false;

    /// <summary></summary>
    [SerializeField]
    float m_inputSpeed = 1.0f;

    /// <summary></summary>
    [SerializeField]
    Vector3 m_freeLookRotation;

    /// <summary>cameraが見る高さ</summary>
    [SerializeField]
    float m_height;

    /// <summary>対象とcameraとの距離</summary>
    [SerializeField]
    float m_distance = 8.0f;

    /// <summary></summary>
    [SerializeField]
    Vector3 m_rotation = new Vector3(15.0f, 0.0f, 0.0f);

    /// <summary></summary>
    [SerializeField]
    [Range(0.01f, 100.0f)]
    float m_positionDamping = 16.0f;

    /// <summary></summary>
    [SerializeField]
    [Range(0.01f, 100.0f)]
    float m_rotationDamping = 16.0f;

    /// <summary></summary>
    [SerializeField]
    [Range(0.1f, 0.99f)]
    float m_dolly = 0.34f;

    /// <summary></summary>
    [SerializeField]
    float m_noise = 0.0f;

    /// <summary></summary>
    [SerializeField]
    float m_noiseZ = 0.0f;

    /// <summary></summary>
    [SerializeField]
    float m_noiseSpeed = 1.0f;

    /// <summary></summary>
    [SerializeField]
    Vector3 m_vibration = Vector3.zero;

    /// <summary></summary>
    [SerializeField]
    float m_wallDetectionDistance = 0.3f;

    /// <summary></summary>
    [SerializeField]
    LayerMask m_wallDetectionMask = default;

    /// <summary></summary>
    Camera m_cam;
    /// <summary></summary>
    float m_targetDistance;
    /// <summary></summary>
    Vector3 m_targetPosition;
    /// <summary></summary>
    Vector3 m_targetRotation;
    /// <summary></summary>
    Vector3 m_targetFree;
    /// <summary></summary>
    float m_targetHeight;
    /// <summary></summary>
    float m_targetDolly;
    /// <summary></summary>
    float m_dollyDistance;
    /// <summary></summary>
    Quaternion m_initialRotation;
    /// <summary></summary>
    bool m_isReset = false;

    float m_initialHeight;

    void Start()
    {
        m_cam = GetComponent<Camera>();

        m_targetDistance = m_distance;
        m_targetRotation = m_rotation;
        m_targetFree = m_freeLookRotation;
        m_targetHeight = m_height;
        m_targetDolly = m_dolly;

        m_dollyDistance = m_targetDistance;

        //if (m_enableDollyZoom)
        //{
        //    var dollyFoV = DollyFoV(Mathf.Pow(1.0f / m_targetDolly, 2.0f), m_targetDistance);
        //    m_dollyDistance = DollyDistance(dollyFoV, m_targetDistance);
        //    m_cam.fieldOfView = dollyFoV;
        //}
        DollyZoom();

        if (m_target == null) return;

        var pos = m_target.position + Vector3.up * m_targetHeight;
        var offset = Vector3.zero;

        offset.x += Mathf.Sin(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        offset.z += -Mathf.Cos(m_targetRotation.y * Mathf.Deg2Rad) * Mathf.Cos(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        offset.y += Mathf.Sin(m_targetRotation.x * Mathf.Deg2Rad) * m_dollyDistance;
        m_targetPosition = pos + offset;

        m_initialRotation = new Quaternion(m_rotation.x, m_rotation.y, m_rotation.z, m_cam.transform.rotation.w);
        Debug.Log($"ターゲット{pos.y}カメラ{m_targetPosition.y}");
        m_initialHeight = m_targetPosition.y - pos.y;
        Debug.Log($"はじめ{m_initialRotation}");
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
        if (m_isReset)
        {
            if (InputController.Instance.Option())
            {
                m_isReset = false;
            }
        }
        else
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

            //if (m_enableDollyZoom)
            //{
            //    var dollyFoV = DollyFoV(Mathf.Pow(1.0f / m_targetDolly, 2.0f), m_targetDistance);
            //    m_dollyDistance = DollyDistance(dollyFoV, m_targetDistance);
            //    m_cam.fieldOfView = dollyFoV;
            //}

            DollyZoom();

            //if (target == null) return;

            var pos = m_target.position + Vector3.up * m_targetHeight;

            //if (m_enableWallDetection)
            //{
            //    var dir = (m_targetPosition - pos).normalized;
            //    if (Physics.SphereCast(pos, m_wallDetectionDistance, dir, out RaycastHit hit, m_dollyDistance, m_wallDetectionMask))
            //    {
            //        m_dollyDistance = hit.distance;
            //    }
            //}

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
        //Debug.Log($"update{m_initialRotation}");
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
            if (InputController.Instance.Reset())
            {
                Debug.Log("押した");
                ResetCameraDirection();
            }
        }
    }

    void ResetCameraDirection()
    {
        Debug.Log($"targetの正面{m_target.forward}");
        Debug.Log($"見る位置{m_target.position + Vector3.up * m_targetHeight}");
        Debug.Log($"カメラの目標位置{GetResetPosition()}");

        var resetPos = GetResetPosition();
        m_targetPosition = resetPos;
        m_cam.transform.position = m_targetPosition;
        Debug.Log($"ポジション変更後{m_cam.transform.position}");

        var angle = GetResetAngle();
        Debug.Log($"角度{angle}");
        var pos = m_target.position + Vector3.up * m_targetHeight;
        m_rotation = new Vector3(m_initialRotation.x, angle, m_initialRotation.z);
        m_cam.transform.rotation = Quaternion.Euler(m_rotation);
        m_cam.transform.LookAt(pos, Quaternion.Euler(0.0f, 0.0f, m_rotation.z) * Vector3.up);
        m_isReset = true;
        m_targetRotation.y = m_cam.transform.rotation.y;
        m_targetRotation.x = m_initialRotation.x;
        m_rotation = m_targetRotation;
    }

    Vector3 GetResetPosition()
    {
        var pos = m_target.position + Vector3.up * m_targetHeight;
        var xnijou = Mathf.Pow(m_dollyDistance, 2) - Mathf.Pow(m_initialHeight + pos.y, 2);
        var resetPos = -m_target.transform.forward * Mathf.Sqrt(xnijou);
        resetPos = new Vector3(resetPos.x, m_initialHeight + pos.y, resetPos.z);
        return resetPos;
    }

    float GetResetAngle()
    {
        var a = Vector3.ProjectOnPlane(m_target.transform.forward, Vector3.up);
        Debug.Log($"プレイヤーの正面{a}");
        var b = Vector3.ProjectOnPlane(m_cam.transform.forward, Vector3.up);
        Debug.Log($"現在のカメラの正面{b}");
        var angle = -Vector3.SignedAngle(a, b, Vector3.up);
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

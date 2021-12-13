using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    /// <summary>視点をリセットする際のpositionの目安</summary>
    [SerializeField] Transform m_resetLookPosition = null;
    /// <summary>CinemachineFreeLookのコンポーネント</summary>
    CinemachineFreeLook m_freeLookCamera;
    /// <summary>カメラが目標に動いているかどうか</summary>
    bool m_isMoved;
    /// <summary>現在地と目標地のXZ平面ベクトルのなす角</summary>
    float m_nowTheAngle;
    /// <summary>目標までの現在の角度</summary>
    float m_autoLookAtAngleProgress;
    /// <summary>現在のカメラの速度</summary>
    float m_currentCameraVelocity;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Rstickbutton"))
        {
            ResetCameraDirection(m_resetLookPosition.position);
        }

        if (m_isMoved)
        {
            //高さ
            m_freeLookCamera.m_YAxis.Value = 0.5f;
            //向き
            float angle = Mathf.SmoothDamp(m_autoLookAtAngleProgress, m_nowTheAngle, ref m_currentCameraVelocity, 0.2f);

            m_freeLookCamera.m_XAxis.Value = angle - m_autoLookAtAngleProgress;

            // 殆ど目標角度になったら終了
            if (Mathf.Abs(m_autoLookAtAngleProgress - m_nowTheAngle) <= 0.01f)
            {
                m_isMoved = false;
            }

            // 前回との差分を設定
            
            m_autoLookAtAngleProgress = angle;
        }
    }

    void ResetCameraDirection(Vector3 target)
    {
        float cameraHeight = m_freeLookCamera.transform.position.y;//高さ統一
        Vector3 followPosition = new Vector3(m_freeLookCamera.Follow.position.x, cameraHeight, m_freeLookCamera.Follow.position.z);
        Vector3 targetPosition = new Vector3(target.x, cameraHeight, target.z);

        Vector3 followToTarget = targetPosition - followPosition;
        Vector3 followToTargetReverse = Vector3.Scale(followToTarget, new Vector3(-1, 1, -1));
        Vector3 followToCamera = m_freeLookCamera.transform.position - followPosition;

        Vector3 axis = Vector3.Cross(followToCamera, followToTargetReverse);
        float direction = axis.y < 0 ? -1 : 1;
        float angle = Vector3.Angle(followToCamera, followToTargetReverse);

        m_nowTheAngle = angle * direction;
        m_isMoved = true;
        m_currentCameraVelocity = 0;
        m_autoLookAtAngleProgress = 0;
    }
}

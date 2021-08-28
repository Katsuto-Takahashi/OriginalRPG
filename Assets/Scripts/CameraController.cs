using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    /// <summary>視点をリセットする際のpositionの目安</summary>
    public Transform m_resetLookPosition;
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
        m_freeLookCamera = GetComponent<CinemachineFreeLook>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Rstickbutton"))
        {
            ResetCameraDirection(m_resetLookPosition.position);
            Debug.Log("Rstickbutton");
        }

        if (m_isMoved)
        {
            //高さ
            m_freeLookCamera.m_YAxis.Value = 0.5f;
            //向き
            float angle = Mathf.SmoothDamp(m_autoLookAtAngleProgress, m_nowTheAngle, ref m_currentCameraVelocity, 0.2f);
            
            // 前回との差分を設定
            m_freeLookCamera.m_XAxis.Value = angle - m_autoLookAtAngleProgress;
            m_autoLookAtAngleProgress = angle;

            // 殆ど目標角度になったら終了
            if (Mathf.Abs((int)m_autoLookAtAngleProgress) >= Mathf.Abs((int)m_nowTheAngle))
            {
                m_isMoved = false;
            }
        }
    }

    private void ResetCameraDirection(Vector3 target)
    {
        // それぞれのY座標をカメラの高さに合わせる
        float cameraHeight = m_freeLookCamera.transform.position.y;
        Vector3 followPosition = new Vector3(m_freeLookCamera.Follow.position.x, cameraHeight, m_freeLookCamera.Follow.position.z);
        Vector3 targetPosition = new Vector3(target.x, cameraHeight, target.z);

        // それぞれのベクトルを計算
        Vector3 followToTarget = targetPosition - followPosition;
        Vector3 followToTargetReverse = Vector3.Scale(followToTarget, new Vector3(-1, 1, -1));
        Vector3 followToCamera = m_freeLookCamera.transform.position - followPosition;

        // カメラ回転の角度と方向を計算
        Vector3 axis = Vector3.Cross(followToCamera, followToTargetReverse);
        float direction = axis.y < 0 ? -1 : 1;
        float angle = Vector3.Angle(followToCamera, followToTargetReverse);

        m_nowTheAngle = angle * direction;
        m_isMoved = true;
        m_currentCameraVelocity = 0;
        m_autoLookAtAngleProgress = 0;
    }
}

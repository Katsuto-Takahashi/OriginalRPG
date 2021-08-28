using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChacker : MonoBehaviour
{
    private bool m_isDad;
    /// <summary>長押しされていると判定する時間</summary>
    public float downTime = 0.2f;
    /// <summary>長押し中に処理をするときのインターバル</summary>
    public float interval = 0.1f;
    /// <summary>"interval"ごとに保持しておく時間</summary>
    float saveTime = 0f;
    /// <summary>D-padが押されている時間</summary>
    float getTime = 0f;

    float check;

    void Start()
    {
        m_isDad = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Squarebutton"))
        {
            Debug.Log("Squarebutton");
        }
        if (Input.GetButtonDown("×button"))
        {
            Debug.Log("×button");
        }
        if (Input.GetButtonDown("Circlebutton"))
        {
            Debug.Log("Circlebutton");
        }
        if (Input.GetButtonDown("Trianglebutton"))
        {
            Debug.Log("Trianglebutton");
        }
        if (Input.GetButtonDown("L1button"))
        {
            Debug.Log("L1button");
        }
        if (Input.GetButtonDown("R1button"))
        {
            Debug.Log("R1button");
        }
        if (Input.GetButtonDown("L2button"))
        {
            Debug.Log("L2button");
        }
        if (Input.GetButtonDown("R2button"))
        {
            Debug.Log("R2button");
        }
        if (Input.GetButtonDown("Sharebutton"))
        {
            Debug.Log("Sharebutton");
        }
        if (Input.GetButtonDown("Optionsbutton"))
        {
            Debug.Log("Optionsbutton");
        }
        if (Input.GetButtonDown("Lstickbutton"))
        {
            Debug.Log("Lstickbutton");
        }
        if (Input.GetButtonDown("Rstickbutton"))
        {
            Debug.Log("Rstickbutton");
        }
        if (Input.GetButtonDown("PSbutton"))
        {
            Debug.Log("PSbutton");
        }
        if (Input.GetButtonDown("Trackpad"))
        {
            Debug.Log("Trackpad");
        }

        float v = Input.GetAxisRaw("Dpad_v");
        float h = Input.GetAxisRaw("Dpad_h");
        float lv = Input.GetAxis("Lstick_v");
        float lh = Input.GetAxis("Lstick_h");
        float rv = Input.GetAxis("Rstick_v");
        float rh = Input.GetAxis("Rstick_h");
        float l = Input.GetAxis("L2trigger");
        float r = Input.GetAxis("R2trigger");

        PlayerMove(v, h);

        if (lh > 0)
        {
            Debug.Log("Lstick_h　→");
        }
        if (lh < 0)
        {
            Debug.Log("Lstick_h　←");
        }
        if (lv > 0)
        {
            Debug.Log("Lstick_v　↑");
        }
        if (lv < 0)
        {
            Debug.Log("Lstick_v　↓");
        }
        if (rh > 0)
        {
            Debug.Log("Rstick_h　→");
        }
        if (rh < 0)
        {
            Debug.Log("Rstick_h　←");
        }
        if (rv > 0)
        {
            Debug.Log("Rstick_v　↑");
        }
        if (rv < 0)
        {
            Debug.Log("Rstick_v　↓");
        }
        if (l > 0)
        {
            Debug.Log("L2trigger");
        }
        if (r > 0)
        {
            Debug.Log("R2trigger");
        }
    }

    private float PlayerMove(float v , float h)
    {
        if (v < 0)
        {
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_v　↓");
                check = 1f;
            }

            m_isDad = true;
            getTime += Time.deltaTime;

            //長押し
            if (getTime > downTime)
            {
                //Debug.Log(getTime_v);
                if (getTime - saveTime > interval)
                {
                    saveTime = getTime;
                    Debug.Log("Dpad_v　↓ long");
                    check = 2f;
                }
            }
        }
        else if (v > 0)
        {
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_v　↑");
                check = 3f;
            }

            m_isDad = true;
            getTime += Time.deltaTime;

            //長押し
            if (getTime > downTime)
            {
                //Debug.Log(getTime_v);
                if (getTime - saveTime > interval)
                {
                    saveTime = getTime;
                    Debug.Log("Dpad_v　↑ long");
                    check = 4f;
                }
            }
        }
        else if (h < 0)
        {
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_h　←");
                check = 5f;
            }

            m_isDad = true;
            getTime += Time.deltaTime;

            //長押し
        if (getTime > downTime)
            {
                //Debug.Log(getTime_h);
                if (getTime - saveTime > interval)
                {
                    saveTime = getTime;
                    Debug.Log("Dpad_h　← long");
                    check = 6f;
                }
            }
        }
        else if (h > 0)
        {
            //短押し
            if (!m_isDad)
            {
                Debug.Log("Dpad_h　→");
                check = 7f;
            }

            m_isDad = true;
            getTime += Time.deltaTime;

            //長押し
        if (getTime > downTime)
            {
                //Debug.Log(getTime_h);
                if (getTime - saveTime > interval)
                {
                    saveTime = getTime;
                    Debug.Log("Dpad_h　→ long");
                    check = 8f;
                }
            }
        }
        else
        {
            m_isDad = false;

            getTime = 0f;
            saveTime = 0f;
            check = 0f;
        }
        return check;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    [SerializeField] float startDistance = 10;
    [SerializeField] float hiddenDisanta = 2;
    MeshRenderer m_renderer;
    void Start()
    {
        m_renderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        float dis = Vector3.Distance(Camera.main.transform.position, transform.position);

        Color color = m_renderer.material.color;
        if (dis <= hiddenDisanta)
        {
            color.a = 0.0f;
        }
        else if (dis <= startDistance)
        {
            color.a = (dis - hiddenDisanta) / (startDistance - hiddenDisanta);
        }
        else
        {
            color.a = 1.0f;
        }
        m_renderer.material.color = color;
    }
}

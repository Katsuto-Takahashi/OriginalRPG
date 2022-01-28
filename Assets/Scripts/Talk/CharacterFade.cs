using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterFade : MonoBehaviour
{
    [SerializeField] 
    List<Image> m_characterImage = new List<Image>();

    [SerializeField] 
    List<RectTransform> m_characterTrans = new List<RectTransform>();

    [SerializeField] 
    float m_requiredTime = 0f;

    List<Color> m_colors = new List<Color>();

    float m_time;

    [SerializeField]
    List<Color> m_chengedColor = new List<Color>();

    [SerializeField]
    List<RectTransform> m_rectTransform = new List<RectTransform>();

    [SerializeField]
    int m_index = 0;

    void Start()
    {
        if (m_requiredTime > 0f)
        {
            ChangeColor(m_characterImage[m_index], m_requiredTime, m_chengedColor[m_index]);
            ChangePoint(m_characterTrans[m_index], m_rectTransform[m_index].localPosition, m_requiredTime);
        }
        else
        {
            Debug.LogError("所要時間は0より大きくしてください");
        }
    }

    void Update()
    {
    }

    public void ChangeColor(Image image, float requiredTime, Color color)
    {
        StartCoroutine(GotoAllColoer(image, requiredTime, color));
    }

    IEnumerator GotoAllColoer(Image image, float requiredTime, Color color)
    {
        Color imageColor = image.color;
        bool isChange = requiredTime > 0f;

        while (isChange)
        {
            imageColor.r = ChangeColor(imageColor.r, color.r, requiredTime);
            imageColor.g = ChangeColor(imageColor.g, color.g, requiredTime);
            imageColor.b = ChangeColor(imageColor.b, color.b, requiredTime);
            imageColor.a = ChangeColor(imageColor.a, color.a, requiredTime);
            image.color = imageColor;

            if (CheckColor(imageColor.r, color.r) && CheckColor(imageColor.g, color.g) && CheckColor(imageColor.b, color.b) && CheckColor(imageColor.a, color.a))
            {
                isChange = false;
            }
            yield return null;
        }
        Debug.Log("完");
    }

    float ChangeColor(float myColor, float afterColor, float requiredTime)
    {
        if (Mathf.Abs(myColor - afterColor) > 0.01f)
        {
            if (myColor > afterColor)
            {
                myColor -= Time.deltaTime / requiredTime;
            }
            else if (myColor < afterColor)
            {
                myColor += Time.deltaTime / requiredTime;
            }
        }
        return myColor;
    }

    bool CheckColor(float myColor, float afterColor)
    {
        if (Mathf.Abs(myColor - afterColor) > 0.01f) return false;
        else return true;
    }

    public void ChangePoint(RectTransform rectTransform, Vector3 finshPoint, float requiredTime)
    {
        StartCoroutine(MoveCharacter(rectTransform, finshPoint, requiredTime));
    }

    IEnumerator MoveCharacter(RectTransform rectTransform, Vector3 finshPoint, float requiredTime)
    {
        bool move = requiredTime > 0f;
        Vector3 nowPosition = rectTransform.localPosition;
        var distance = new Vector3(Mathf.Abs(nowPosition.x - finshPoint.x), Mathf.Abs(nowPosition.y - finshPoint.y), Mathf.Abs(nowPosition.z - finshPoint.z));

        while (move)
        {
            nowPosition.x = ChangePoint(nowPosition.x, finshPoint.x, distance.x, requiredTime);
            nowPosition.y = ChangePoint(nowPosition.y, finshPoint.y, distance.y, requiredTime);
            nowPosition.z = ChangePoint(nowPosition.z, finshPoint.z, distance.z, requiredTime);
            rectTransform.localPosition = nowPosition;

            if (CheckPoint(nowPosition.x, finshPoint.x) && CheckPoint(nowPosition.y, finshPoint.y) && CheckPoint(nowPosition.z, finshPoint.z))
            {
                move = false;
            }
            yield return null;
        }
        Debug.Log("完");
    }

    float ChangePoint(float myPoint, float afterPoint, float distance, float requiredTime)
    {
        var abs = Mathf.Abs(myPoint - afterPoint);
        var v = Time.deltaTime * distance / requiredTime;

        if (abs > 1f)
        {
            var d = abs > v ? v : abs;

            if (myPoint > afterPoint)
            {
                myPoint -= d;
            }
            else if (myPoint < afterPoint)
            {
                myPoint += d;
            }
        }
        return myPoint;
    }

    bool CheckPoint(float myPoint, float afterPoint)
    {
        if (Mathf.Abs(myPoint - afterPoint) > 0.01f) return false;
        else return true;
    }
}

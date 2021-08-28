using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] DamageCalculator damageCalculator;
    GameManager gameManager;
    private bool isSetting = false;
    [SerializeField] Transform[] spanPoint = new Transform[9];
    Transform[] setSpanPosition;
    void Start()
    {
        SettingCharacterPosition();
        BattleStanby();
    }

    void Update()
    {

    }
    void SettingCharacterPosition()
    {
        //if (partyManager.m_characterParty.Count == 1)
        //{
        //    setSpanPosition[0] = spanPoint[0];
        //    Debug.Log("1");
        //}
        //else if (partyManager.m_characterParty.Count == 2)
        //{
        //    setSpanPosition[0] = spanPoint[1];
        //    setSpanPosition[1] = spanPoint[2];
        //    Debug.Log("2");
        //}
        //else if (partyManager.m_characterParty.Count == 3)
        //{
        //    setSpanPosition[0] = spanPoint[3];
        //    setSpanPosition[1] = spanPoint[0];
        //    setSpanPosition[2] = spanPoint[4];
        //    Debug.Log("3");
        //}
        //else if (partyManager.m_characterParty.Count == 4)
        //{
        //    setSpanPosition[0] = spanPoint[5];
        //    setSpanPosition[1] = spanPoint[1];
        //    setSpanPosition[2] = spanPoint[2];
        //    setSpanPosition[3] = spanPoint[6];
        //    Debug.Log("4");
        //}
        //else if (partyManager.m_characterParty.Count == 5)
        //{
        //    setSpanPosition[0] = spanPoint[7];
        //    setSpanPosition[1] = spanPoint[3];
        //    setSpanPosition[2] = spanPoint[0];
        //    setSpanPosition[3] = spanPoint[4];
        //    setSpanPosition[4] = spanPoint[8];
        //    Debug.Log("5");
        //}
        //else
        //{
        //    Debug.Log("error");
        //}
    }
    void BattleStanby()
    {
        //for (int i = 0; i < partyManager.m_characterParty.Count; i++)
        //{
        //    Instantiate(partyManager.m_characterParty[i], setSpanPosition[i]);
        //}
        isSetting = true;
    }

    void Attack()
    {
        if (true)
        {
            
        }
        else
        {

        }
    }
    void Hit()
    {
        
    }
}

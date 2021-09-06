using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ContactEnemy : MonoBehaviour
{
    [SerializeField] GameObject m_battleFeildPrefab;
    private GameObject m_battleFeild;
    public bool m_isContact = false;
    public bool m_isBattle = false;
    public int m_enemyParty;
    public int m_enemyID;
    public Vector3 m_contactPosition;
    private float distsnce;

    void Update()
    {
        if (m_isContact)
        {
            distsnce = Vector3.Distance(m_contactPosition, transform.position);
            if (distsnce > 15)
            {
                DeleteField();
            }
        }
    }
    public void CreateField(Collider other)
    {
        m_battleFeildPrefab.transform.position = other.transform.position;
        m_battleFeild = Instantiate(m_battleFeildPrefab);
    }
    public void DeleteField()
    {
        Destroy(m_battleFeild);
        m_isContact = false;
        m_contactPosition = Vector3.zero;
        distsnce = 0f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !m_isContact)
        {
            m_contactPosition = other.transform.position;
            CreateField(other);
            m_enemyParty = other.gameObject.GetComponent<EnemyManager>().enemyParameters.EnemyPartyNumber;
            m_enemyID = other.gameObject.GetComponent<EnemyManager>().enemyParameters.EnemyCharacterID;
            m_isContact = true;
            m_isBattle = true;
            Destroy(other.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContactEnemy : MonoBehaviour
{
    [SerializeField] GameObject m_battleFeildPrefab = null;
    private GameObject m_battleFeild;
    private bool m_isContact = false;
    private bool m_isBattle = false;
    private int m_enemyParty;
    private int m_enemyID;
    private Vector3 m_contactPosition;
    private float m_distsnce;

    public Vector3 ContactPosition { get => m_contactPosition; set => m_contactPosition = value; }
    public int EnemyID { get => m_enemyID; set => m_enemyID = value; }
    public int EnemyParty { get => m_enemyParty; set => m_enemyParty = value; }
    public bool IsBattle { get => m_isBattle; set => m_isBattle = value; }
    public bool IsContact { get => m_isContact; set => m_isContact = value; }

    public Transform ptr;

    public event Action Battle;

    void Update()
    {
        if (IsContact)
        {
            m_distsnce = (ContactPosition.x - transform.position.x) * (ContactPosition.x - transform.position.x) + (ContactPosition.z - transform.position.z) * (ContactPosition.z - transform.position.z);
            if (Mathf.Sqrt(m_distsnce) > 15f)
            {
                IsBattle = false;
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
        IsContact = false;
        ContactPosition = Vector3.zero;
        m_distsnce = 0f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !IsContact)
        {
            ptr = transform;
            ContactPosition = other.transform.position;
            CreateField(other);
            EnemyParty = other.gameObject.GetComponent<EnemyManager>().EnemyParameters.EnemyPartyNumber;
            EnemyID = other.gameObject.GetComponent<EnemyManager>().EnemyParameters.EnemyCharacterID;
            IsContact = true;
            IsBattle = true;
            Destroy(other.gameObject);
            Battle.Invoke();
        }
    }
}

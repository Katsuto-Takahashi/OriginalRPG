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
    private Transform m_PlayerTransform;

    public Vector3 ContactPosition { get => m_contactPosition;}
    public int EnemyID { get => m_enemyID;}
    public int EnemyParty { get => m_enemyParty;}
    public bool IsBattle { get => m_isBattle; set => m_isBattle = value; }
    public bool IsContact { get => m_isContact; set => m_isContact = value; }
    public Transform PlayerTransform { get => m_PlayerTransform;}

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
    void CreateField(Collider other)
    {
        m_battleFeildPrefab.transform.position = other.transform.position;
        m_battleFeild = Instantiate(m_battleFeildPrefab);
    }
    public void DeleteField()
    {
        Destroy(m_battleFeild);
        IsContact = false;
        m_contactPosition = Vector3.zero;
        m_distsnce = 0f;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && !IsContact)
        {
            m_PlayerTransform = transform;
            m_contactPosition = other.transform.position;
            CreateField(other);
            m_enemyParty = other.gameObject.GetComponent<EnemyManager>().EnemyParameters.EnemyPartyNumber;
            m_enemyID = other.gameObject.GetComponent<EnemyManager>().EnemyParameters.EnemyCharacterID;
            IsContact = true;
            IsBattle = true;
            Destroy(other.gameObject);
            Battle.Invoke();
        }
    }
}

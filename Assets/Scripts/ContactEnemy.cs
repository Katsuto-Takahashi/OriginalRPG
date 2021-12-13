using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContactEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject m_battleFeildPrefab = null;

    GameObject m_battleFeild;
    bool m_isContact = false;
    bool m_isBattle = false;
    int m_enemyParty;
    int m_enemyID;
    Vector3 m_contactPosition;
    float m_distsnce;
    Transform m_PlayerTransform;

    /// <summary>接敵時のPosition</summary>
    public Vector3 ContactPosition { get => m_contactPosition;}

    /// <summary>接敵した敵のID</summary>
    public int EnemyID { get => m_enemyID;}

    /// <summary>接敵した敵のパーティ</summary>
    public int EnemyParty { get => m_enemyParty;}

    /// <summary>バトルの開始フラグ</summary>
    public bool IsBattle { get => m_isBattle; set => m_isBattle = value; }

    /// <summary>接敵したかどうか</summary>
    public bool IsContact { get => m_isContact; set => m_isContact = value; }

    /// <summary>接敵時のPlayerのTransform</summary>
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

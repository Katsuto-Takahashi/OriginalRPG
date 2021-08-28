using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContactEnemy : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    GameManager gameManager;
    PartyManager partyManager;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            sceneLoader.LoadedBattleScene();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            sceneLoader.LoadedBattleScene();
        }
    }
    private void Stanby(Collision collision)
    {
        gameManager.m_isBattleStanby = true;
        gameManager.m_gameState = GameManager.GameStatus.BattleStanby;
        sceneLoader.LoadScene("BattleScene");
    }
}

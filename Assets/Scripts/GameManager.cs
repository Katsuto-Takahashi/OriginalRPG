using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public bool m_isField = true;
    public bool m_isBattleStanby = false;
    public bool m_isBattle = false;
    public bool m_isBattleResult = false;
    public enum GameStatus
    {
        Field,
        BattleStanby,
        Battle,
        BattleResult
    }
    public GameStatus m_gameState = GameStatus.Field;

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        switch (m_gameState)
        {
            case GameStatus.Field:
                {
                    Field();
                }
                break;
            case GameStatus.BattleStanby:
                {
                    BattleStanby();
                }
                break;
            case GameStatus.Battle:
                {
                    Battle();
                }
                break;
            case GameStatus.BattleResult:
                {
                    BattleResult();
                }
                break;
            default:
                break;
        }
    }

    void Field()
    {
        if (m_isBattleStanby)
        {
            m_isField = false;
            m_gameState = GameStatus.BattleStanby;
        }
    }

    void BattleStanby()
    {
        if (m_isBattle)
        {
            m_isBattleStanby = false;
            m_gameState = GameStatus.Battle;
        }
    }

    void Battle()
    {
        if (m_isBattleResult)
        {
            m_isBattle = false;
            m_gameState = GameStatus.BattleResult;
        }
    }

    void BattleResult()
    {
        if (m_isField)
        {
            m_isBattleResult = false;
            m_gameState = GameStatus.Field;
        }
    }
}

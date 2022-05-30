using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : SingletonMonoBehaviour<CharactersManager>
{
    [SerializeField, Tooltip("Player側")] 
    List<Character> m_characters = new List<Character>();

    [SerializeField, Tooltip("敵側")] 
    List<Enemy> m_enemies = new List<Enemy>();

    /// <summary>全キャラクターList</summary>
    public List<Character> Characters  => m_characters;

    /// <summary>全敵のList</summary>
    public List<Enemy> Enemies => m_enemies;
}

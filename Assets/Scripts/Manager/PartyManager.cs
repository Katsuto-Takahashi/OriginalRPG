using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PartyManager : SingletonMonoBehaviour<PartyManager>
{
    List<Character> m_party = new List<Character>();
    public List<Character> Party => m_party;

    /// <summary>パーティーに加える</summary>
    /// <param name="character">パーティーに加えるCharacter</param>
    public void JoinParty(Character character)
    {
        m_party.Add(character);
        UIManager.Instance.CreateHPAndAPUI(character);
    }

    /// <summary>パーティーを抜ける</summary>
    /// <param name="character">パーティーを抜けるCharacter</param>
    public void LeaveParty(Character character)
    {
        UIManager.Instance.DeleteHPAndAPUI(character);
        m_party.Remove(character);
    }
}

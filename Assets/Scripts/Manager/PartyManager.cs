using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PartyManager : SingletonMonoBehaviour<PartyManager>
{
    List<Character> m_party = new List<Character>();
    public List<Character> Party => m_party;

    /// <summary>�p�[�e�B�[�ɉ�����</summary>
    /// <param name="character">�p�[�e�B�[�ɉ�����Character</param>
    public void JoinParty(Character character)
    {
        m_party.Add(character);
        UIManager.Instance.CreateHPAndAPUI(character);
    }

    /// <summary>�p�[�e�B�[�𔲂���</summary>
    /// <param name="character">�p�[�e�B�[�𔲂���Character</param>
    public void LeaveParty(Character character)
    {
        UIManager.Instance.DeleteHPAndAPUI(character);
        m_party.Remove(character);
    }
}

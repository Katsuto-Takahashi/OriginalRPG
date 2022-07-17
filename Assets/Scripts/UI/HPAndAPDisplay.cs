using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class HPAndAPDisplay
{
    List<CharacterParameterUI> m_parameterUIList;

    GameObject m_hpapPanel;

    public HPAndAPDisplay(GameObject hpapPanel)
    {
        m_hpapPanel = hpapPanel;
        m_parameterUIList = new List<CharacterParameterUI>();
    }

    public void Create(Character character)
    {
        Debug.Log("Create");
        int index = PartyManager.Instance.Party.IndexOf(character);
        var ui = m_hpapPanel.transform.GetChild(index).gameObject;
        ui.SetActive(true);
        m_parameterUIList.Add(ui.GetComponent<CharacterParameterUI>());
        m_parameterUIList[index].CreateName(character.Name.Value);
        character.HP
            .DistinctUntilChanged()
            .Subscribe
            (
                _ =>
                m_parameterUIList[index].CreateHP(
                    character.HP.Value,
                    character.MaxHP.Value
                )
            )
            .AddTo(m_parameterUIList[index]);
        character.MaxHP
            .DistinctUntilChanged()
            .Subscribe
            (
                _ =>
                m_parameterUIList[index].CreateHP(
                    character.HP.Value,
                    character.MaxHP.Value
                )
            )
            .AddTo(m_parameterUIList[index]);
        character.AP
            .DistinctUntilChanged()
            .Subscribe
            (
                _ =>
                m_parameterUIList[index].CreateAP(
                    character.AP.Value,
                    character.MaxAP.Value
                )
            )
            .AddTo(m_parameterUIList[index]);
        character.MaxAP
            .DistinctUntilChanged()
            .Subscribe
            (
                _ =>
                m_parameterUIList[index].CreateAP(
                    character.AP.Value,
                    character.MaxAP.Value
                )
            )
            .AddTo(m_parameterUIList[index]);
        
    }
    
    public void Delete(Character character)
    {
        Debug.Log("Delete");
        int index = PartyManager.Instance.Party.IndexOf(character);
        character.HP.Dispose();
        character.MaxHP.Dispose();
        character.AP.Dispose();
        character.MaxAP.Dispose();
        m_parameterUIList.RemoveAt(index);
        m_hpapPanel.transform.GetChild(index).gameObject.SetActive(false);
    }
}

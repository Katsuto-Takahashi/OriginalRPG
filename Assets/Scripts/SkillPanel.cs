using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillPanel : MonoBehaviour
{
    [SerializeField]
    List<OldSkillData> skillData = new List<OldSkillData>();

    [SerializeField]
    List<SkillData> skills = new List<SkillData>();

    [SerializeField]
    List<HasSkillList> characters = new List<HasSkillList>();

    ReactiveCollection<OldSkillData> skillss = new ReactiveCollection<OldSkillData>();


    void Start()
    {
        skillss.Add(skillData[0]);
        skillss.Remove(skillData[0]);
        int a = skillss.Count;
    }

    void Update()
    {
    }
    public enum CharacterNum
    {
        AAA,
        SSS,
        DDD
    }
    public void SetSkill(CharacterNum num, int index)
    {

    }
}
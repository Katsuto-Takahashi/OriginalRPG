using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SkillPanel : MonoBehaviour
{
    [SerializeField]
    List<SkillData> skillData = new List<SkillData>();

    [SerializeField]
    List<Skill> skills = new List<Skill>();

    [SerializeField]
    List<HasSkillList> characters = new List<HasSkillList>();

    ReactiveCollection<SkillData> skillss = new ReactiveCollection<SkillData>();


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
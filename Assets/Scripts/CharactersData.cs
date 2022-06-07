using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CharactersData
{
    public ParamData[] datas;
}

[System.Serializable]
public class ParamData
{
    public string name;
    public int hp;
    public int maxhp;
    public int ap;
    public int maxap;
    public int strength;
    public int defence;
    public int magicpower;
    public int magicresist;
    public int luck;
    public int speed;
    public int level;
    public int skillpoint;
    public int nowexp;
    public int totalexp;
    public int nextexp;
    public SkillIndex skillindex;
}

[System.Serializable]
public class SkillIndex
{
    [SerializeField]
    private int[] physicals;

    [SerializeField]
    private int[] magicals;

    public int[] Physicals => physicals;
    public int[] Magicals => magicals;
}
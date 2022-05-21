using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class JsonData
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
    public int[] skillindex;
}
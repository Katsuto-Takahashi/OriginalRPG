using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoveryCalculator
{
    int CalculateRecovery(int luck, int param, int standardValue, SkillData skillData)
    {
        float heal = param * standardValue * (Random.Range(0, luck) + 1);
        return (int)(heal * skillData.SkillMagnification);
    }

    public int Recovery(Character character, SkillData skillData, int standardValue)
    {
        int param = skillData.Type == SkillType.Physical ? character.Strength.Value : character.MagicPower.Value;
        return CalculateRecovery(character.Luck.Value, param, standardValue, skillData);
    }

    public int Recovery(Enemy enemy, SkillData skillData, int standardValue)
    {
        int param = skillData.Type == SkillType.Physical ? enemy.Strength.Value : enemy.MagicPower.Value;
        return CalculateRecovery(enemy.Luck.Value, param, standardValue, skillData);
    }
}

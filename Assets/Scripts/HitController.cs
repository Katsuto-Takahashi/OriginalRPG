using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour
{
    SkillEffectController _skillEffectController;

    public void SetUp(SkillEffectController skillEffectController)
    {
        _skillEffectController = skillEffectController;
    }

    private void OnTriggerEnter(Collider other)
    {
        _skillEffectController.HitEffect(other);
    }
}

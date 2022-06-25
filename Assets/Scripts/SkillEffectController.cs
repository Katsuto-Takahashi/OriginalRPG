using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectController : MonoBehaviour
{
    SphereCollider _sphereCollider;
    GameObject _targetObject;
    Skill _skillData;

    public void SetUp(SphereCollider sphereCollider)
    {
        _sphereCollider = sphereCollider;
    }

    public void SetSkill(Skill skill, GameObject user = null, GameObject target = null)
    {
        _skillData = skill;
        _targetObject = target;

        switch (_skillData.SkillParameter.Target)
        {
            case SkillData.SkillTarget.all:
                AllEffect(user.transform.position);
                break;
            case SkillData.SkillTarget.oneEnemy:
                OneEffect(user.transform.position, target.transform.position);
                break;
            case SkillData.SkillTarget.enemyOnly:
                OnlyEffect(target.transform.position);
                break;
            case SkillData.SkillTarget.oneAlly:
                OneEffect(user.transform.position, target.transform.position);
                break;
            case SkillData.SkillTarget.allyOnly:
                OnlyEffect(target.transform.position);
                break;
            case SkillData.SkillTarget.myself:
                OneEffect(user.transform.position, target.transform.position);
                break;
            default:
                break;
        }
    }

    void AllEffect(Vector3 myPosition)
    {
        ChangeColliderEnable(true);
        _sphereCollider.radius = _skillData.SkillParameter.EffectRange;
        StartCoroutine(PlayEffect(myPosition));
    }

    void OneEffect(Vector3 myPosition, Vector3 targetPosition)
    {
        ChangeColliderEnable(true);
        _sphereCollider.radius = _skillData.SkillParameter.EffectRange;
        StartCoroutine(PlayEffect(myPosition, targetPosition));
    }

    void OnlyEffect(Vector3 targetPosition)
    {
        ChangeColliderEnable(true);
        _sphereCollider.radius = _skillData.SkillParameter.EffectRange;
        Vector3 startPosition = new Vector3(targetPosition.x, targetPosition.y + 10f, targetPosition.z);
        StartCoroutine(PlayEffect(startPosition, targetPosition));
    }

    void SkillPlay(Skill skill)
    {

    }

    IEnumerator PlayEffect(Vector3 myPosition)
    {

        yield return null;
    }

    IEnumerator PlayEffect(Vector3 myPosition, Vector3 targetPosition)
    {

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (_skillData.SkillParameter.Target)
        {
            case SkillData.SkillTarget.all:
                ChangeColliderEnable(false);
                break;
            case SkillData.SkillTarget.oneEnemy:
                if (other.gameObject == _targetObject)
                {

                }
                ChangeColliderEnable(false);
                break;
            case SkillData.SkillTarget.enemyOnly:
                if (other.gameObject.CompareTag(_targetObject.tag))
                {

                }
                ChangeColliderEnable(false);
                break;
            case SkillData.SkillTarget.oneAlly:
                if (other.gameObject == _targetObject)
                {

                }
                ChangeColliderEnable(false);
                break;
            case SkillData.SkillTarget.allyOnly:
                if (other.gameObject.CompareTag(_targetObject.tag))
                {

                }
                break;
            case SkillData.SkillTarget.myself:
                if (other.gameObject == _targetObject)
                {

                }
                ChangeColliderEnable(false);
                break;
            default:
                break;
        }
    }

    void ChangeColliderEnable(bool flag)
    {
        _sphereCollider.enabled = flag;
    }
}

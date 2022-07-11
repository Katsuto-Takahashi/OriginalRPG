using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectController : MonoBehaviour
{
    GameObject _user;
    GameObject _targetObject;
    Skill _skillData;
    Vector3 _initializePosition;

    [SerializeField]
    [EnumIndex(typeof(EffectType))]
    List<HitController> _hitControllers = new List<HitController>();
    List<GameObject> _hitObjects = new List<GameObject>();

    public void SetUp()
    {
        for (int i = 0; i < _hitControllers.Count; i++)
        {
            _hitControllers[i].SetUp(this);
            _hitObjects.Add(_hitControllers[i].gameObject);
        }
        _initializePosition = transform.position;
        ChangeColliderEnable(false);
    }

    public void SetSkill(Skill skill, GameObject user = null, GameObject target = null)
    {
        _skillData = skill;
        _user = user;
        _targetObject = target;

        switch (_skillData.SkillParameter.Target)
        {
            case SkillTarget.All:
                AllEffect(user.transform.position);
                break;
            case SkillTarget.OneEnemy:
                OneEffect(user.transform.position, target.transform.position);
                break;
            case SkillTarget.EnemyOnly:
                OnlyEffect(target.transform.position);
                break;
            case SkillTarget.OneAlly:
                OneEffect(user.transform.position, target.transform.position);
                break;
            case SkillTarget.AllyOnly:
                OnlyEffect(target.transform.position);
                break;
            case SkillTarget.Myself:
                OneEffect(user.transform.position, target.transform.position);
                break;
            default:
                break;
        }
    }

    void AllEffect(Vector3 myPosition)
    {
        //ChangeColliderEnable(true);
        //_sphereCollider.radius = _skillData.SkillParameter.EffectRange;
        StartCoroutine(PlayEffect(myPosition));
    }

    void OneEffect(Vector3 myPosition, Vector3 targetPosition)
    {
        ChangeColliderEnable(true);
        //_sphereCollider.radius = _skillData.SkillParameter.EffectRange;
        StartCoroutine(PlayEffect(myPosition, targetPosition));
    }

    void OnlyEffect(Vector3 targetPosition)
    {
        ChangeColliderEnable(true);
        //_sphereCollider.radius = _skillData.SkillParameter.EffectRange;
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

    IEnumerator PlayEffect(Vector3 startPosition, Vector3 targetPosition)
    {
        //while (Vector3.Distance(_sphereCollider.transform.position, _targetObject.transform.position) > 0.01f)
        {
            startPosition = Vector3.MoveTowards(startPosition, targetPosition, 10.0f * Time.deltaTime);
            //_sphereCollider.transform.position = startPosition;
            yield return null;
        }
    }

    public void HitEffect(Collider other)
    {
        if (_skillData != null)
        {
            switch (_skillData.SkillParameter.Target)
            {
                case SkillTarget.All:
                    ChangeColliderEnable(false);
                    break;
                case SkillTarget.OneEnemy:
                    if (other.gameObject == _targetObject)
                    {
                        BattleManager.Instance.PlayAdditionalSkillEffect(_user, _targetObject, _skillData);
                        ChangeColliderEnable(false);
                    }
                    break;
                case SkillTarget.EnemyOnly:
                    if (((1 << other.gameObject.layer) & _targetObject.layer) != 0)
                    {
                        BattleManager.Instance.PlayAdditionalSkillEffect(_user, _targetObject, _skillData);
                        ChangeColliderEnable(false);
                    }
                    break;
                case SkillTarget.OneAlly:
                    if (other.gameObject == _targetObject)
                    {
                        BattleManager.Instance.PlayAdditionalSkillEffect(_user, _targetObject, _skillData);
                        ChangeColliderEnable(false);
                    }
                    break;
                case SkillTarget.AllyOnly:
                    if (((1 << other.gameObject.layer) & _targetObject.layer) != 0)
                    {
                        BattleManager.Instance.PlayAdditionalSkillEffect(_user, _targetObject, _skillData);
                        ChangeColliderEnable(false);
                    }
                    break;
                case SkillTarget.Myself:
                    if (other.gameObject == _targetObject)
                    {
                        BattleManager.Instance.PlayAdditionalSkillEffect(_user, _targetObject, _skillData);
                        ChangeColliderEnable(false);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    void ChangeColliderEnable(bool flag)
    {
        if (flag == false)
        {
            //_sphereCollider.transform.position = _initializePosition;
            
        }
        //_sphereCollider.gameObject.SetActive(flag);
    }

    void EffectObjectSetActive(bool flag, EffectType effectType, Skill skill = null)
    {
        if (flag)
        {
            _hitObjects[(int)effectType].SetActive(flag);
        }
        else
        {
            if (skill != null)
            {
                StartCoroutine(Change(effectType, skill));
            }
            else
            {
                _hitObjects[(int)effectType].SetActive(flag);
            }
        }
    }

    IEnumerator Change(EffectType effectType, Skill skill)
    {
        Instantiate(skill.SkillParameter.SkillFinishEffect, _hitObjects[(int)effectType].transform.position, Quaternion.identity, gameObject.transform);
        yield return null;
        
    }
}

public enum EffectType
{
    Sphere,
    Cylinder,
    Capsule,
    Box
}
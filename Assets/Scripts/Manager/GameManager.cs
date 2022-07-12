using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : SingletonMonoBehaviour<GameManager>, IManagable
{
    bool m_isDisplay = false;
    bool m_canDisplay = true;

    Character m_player;
    public Character Player => m_player;

    ReactiveCollection<Character> m_party = new ReactiveCollection<Character>();

    public ReactiveCollection<Character> Party => m_party;

    protected override void Awake()
    {
        base.Awake();
        Initialize();
        UIManager.Instance.Initialize();
        //DataManager.Instance.Initialize();
        HPAndAPDisplay.Instance.Initialize();
    }

    void Start()
    {
        m_player.BCSM.CanSelect.DistinctUntilChanged().Subscribe(s => UIManager.Instance.DisplayFirstBattleCommandPanel(s)).AddTo(m_player.gameObject);
        //DataManager.Instance.DataRead();
        //DataManager.Instance.DataSave();
        Observable.EveryUpdate().Subscribe(_ => OnUpdate()).AddTo(this);
    }

    void OnUpdate()
    {
        Display();
    }

    void Display()
    {
        if (m_canDisplay)
        {
            if (InputController.Instance.Menu())
            {
                m_isDisplay = !m_isDisplay;
            }
            UIManager.Instance.DisplayMenu(m_isDisplay);
        }
        else
        {
            UIManager.Instance.DisplayMenu(m_canDisplay);
        }
    }

    public void Initialize()
    {
        m_player = CharactersManager.Instance.Characters[0];
        m_party.Add(m_player);
    }
}

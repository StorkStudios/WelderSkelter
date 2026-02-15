using StorkStudios.CoreNest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private enum Phase { Menu, Work, Shop, End }

    [SerializeField]
    private SerializedDictionary<Phase, GameObject> phaseParents;

    [SerializeField]
    private List<DayshiftData> dayshifts;

    [SerializeField]
    [ReadOnly]
    private int currentDay;

    private bool win;

    private void Start()
    {
        MainMenuController.Instance.StartGameEvent += StartGame;
        MainMenuController.Instance.StartTutorialEvent += StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded += OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded += OnShopPhaseEnded;

        SetPhase(Phase.Menu);
    }

    private void StartTutorial()
    {
        TutorialManager.Instance.InitTutorial();
        StartGame();
    }

    private void StartGame()
    {
        currentDay = 0;
        SetPhase(Phase.Work);
    }

    private void OnGamePhaseEnded(bool won)
    {
        if (won && currentDay < dayshifts.Count - 1)
        {
            SetPhase(Phase.Shop);
        }
        else
        {
            win = won;
            SetPhase(Phase.End);
        }
    }

    private void OnShopPhaseEnded()
    {
        currentDay++;
        SetPhase(Phase.Work);
    }

    private void SetPhase(Phase phase)
    {
        foreach (Phase key in phaseParents.Keys)
        {
            phaseParents[key].SetActive(key == phase);
        }

        switch (phase)
        {
            case Phase.Work:
                WorkPhaseManager.Instance.BeginWorkPhase(dayshifts[currentDay]);
                break;
            case Phase.Shop:
                ShopPhaseManager.Instance.BeginShopPhase(dayshifts[currentDay + 1]);
                break;
            case Phase.End:
                EndScreenController.Instance.SetScreen(win ? EndScreenController.Screen.Win : EndScreenController.Screen.Lose);
                break;
        }
    }
}

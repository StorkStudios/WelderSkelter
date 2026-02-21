using StorkStudios.CoreNest;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameManagerHelper gameManagerHelper;

    private void Start()
    {
        MainMenuController.Instance.StartGameEvent += gameManagerHelper.StartGame;
        MainMenuController.Instance.StartTutorialEvent += StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded += OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded += OnShopPhaseEnded;

        this.CallNextFrame(() => gameManagerHelper.SetPhase(GameManagerHelper.Phase.Menu));
    }

    private void StartTutorial()
    {
        enabled = false;
        //Tutorial logic is being handled by TutorialManager
        MainMenuController.Instance.StartGameEvent -= gameManagerHelper.StartGame;
        MainMenuController.Instance.StartTutorialEvent -= StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded -= OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded -= OnShopPhaseEnded;
    }

    private void OnGamePhaseEnded(bool won)
    {
        if (won && !gameManagerHelper.IsLastDay)
        {
            gameManagerHelper.SetPhase(GameManagerHelper.Phase.Shop);
        }
        else
        {
            gameManagerHelper.SetPhase(won ? GameManagerHelper.Phase.Win : GameManagerHelper.Phase.Lose);
        }
    }

    private void OnShopPhaseEnded()
    {
        gameManagerHelper.StartNextDay();
        gameManagerHelper.SetPhase(GameManagerHelper.Phase.Work);
    }
}

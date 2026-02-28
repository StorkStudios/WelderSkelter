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
        MainMenuController.Instance.StartGameEvent += OnStartGame;
        MainMenuController.Instance.StartTutorialEvent += StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded += OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded += OnShopPhaseEnded;
        PauseScreenController.Instance.BackToMenuEvent += OnBackToMenu;

        StartCoroutine(gameManagerHelper.SetPhase(GameManagerHelper.Phase.Menu));
    }

    private void StartTutorial()
    {
        enabled = false;
        //Tutorial logic is being handled by TutorialManager
        MainMenuController.Instance.StartGameEvent -= OnStartGame;
        MainMenuController.Instance.StartTutorialEvent -= StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded -= OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded -= OnShopPhaseEnded;
        PauseScreenController.Instance.BackToMenuEvent -= OnBackToMenu;
    }

    private void OnStartGame()
    {
        StartCoroutine(gameManagerHelper.StartGame());
    }

    private void OnGamePhaseEnded(bool won)
    {
        if (won && !gameManagerHelper.IsLastDay)
        {
            StartCoroutine(gameManagerHelper.SetPhase(GameManagerHelper.Phase.Shop));
        }
        else
        {
            StartCoroutine(gameManagerHelper.SetPhase(won ? GameManagerHelper.Phase.Win : GameManagerHelper.Phase.Lose));
        }
    }

    private void OnShopPhaseEnded()
    {
        gameManagerHelper.StartNextDay();
        StartCoroutine(gameManagerHelper.SetPhase(GameManagerHelper.Phase.Work));
    }

    private void OnBackToMenu()
    {
        StartCoroutine(gameManagerHelper.BackToMenu());
    }
}

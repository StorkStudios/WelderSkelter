using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField]
    private GameManagerHelper gameManagerHelper;

    public enum TutorialLevel
    {
        TutorialLevel1 = 0,
        TutorialLevel2 = 1,
        TutorialLevel3 = 2,
    }

    private TutorialLevel currentTutorialLevel;

    private void Start()
    {
        MainMenuController.Instance.StartGameEvent += OnStartGame;
        MainMenuController.Instance.StartTutorialEvent += StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded += OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded += OnShopPhaseEnded;
    }

    private void OnStartGame()
    {
        enabled = false;
        //Tutorial logic is being handled by TutorialManager
        MainMenuController.Instance.StartGameEvent -= OnStartGame;
        MainMenuController.Instance.StartTutorialEvent -= StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded -= OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded -= OnShopPhaseEnded;
    }

    private void StartTutorial()
    {
        AddTutorialUpgrades();
        StartTutorialLevel(TutorialLevel.TutorialLevel1);
        gameManagerHelper.StartGame();
    }

    private void AddTutorialUpgrades()
    {
        TutorialUpgrade[] tutorialUpgrades = Resources.LoadAll<TutorialUpgrade>("Upgrades/Tutorial");
        foreach (TutorialUpgrade tutorialUpgrade in tutorialUpgrades)
        {
            PlayerUpgrades.Instance.AddUpgrade(tutorialUpgrade);
        }
    }

    private void StartTutorialLevel(TutorialLevel tutorialLevel)
    {
        PlayerUpgrades.Instance.RemoveTutorialUpgrades(currentTutorialLevel);
        currentTutorialLevel = tutorialLevel;
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
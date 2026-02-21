using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField]
    private GameManagerHelper gameManagerHelper;

    [SerializeField]
    private SerializedDictionary<TutorialLevel, string> tutorialText;

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
        TutorialPhaseManager.Instance.TutorialPhaseEnded += OnTutorialPhaseEnded;
    }

    private void OnStartGame()
    {
        enabled = false;
        //Tutorial logic is being handled by TutorialManager
        MainMenuController.Instance.StartGameEvent -= OnStartGame;
        MainMenuController.Instance.StartTutorialEvent -= StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded -= OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded -= OnShopPhaseEnded;
        TutorialPhaseManager.Instance.TutorialPhaseEnded -= OnTutorialPhaseEnded;
    }

    private void StartTutorial()
    {
        AddTutorialUpgrades();
        StartTutorialLevel(TutorialLevel.TutorialLevel1);
        SetPhase(GameManagerHelper.Phase.Tutorial);
    }

    private void OnTutorialPhaseEnded()
    {
        SetPhase(GameManagerHelper.Phase.Work);
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
            SetPhase(GameManagerHelper.Phase.Shop);
        }
        else
        {
            SetPhase(won ? GameManagerHelper.Phase.Win : GameManagerHelper.Phase.Lose);
        }
    }

    private void OnShopPhaseEnded()
    {
        gameManagerHelper.StartNextDay();
        SetPhase(GameManagerHelper.Phase.Work);
    }

    private void SetPhase(GameManagerHelper.Phase phase)
    {
        gameManagerHelper.SetPhase(phase);
        if (phase == GameManagerHelper.Phase.Tutorial)
        {
            TutorialPhaseManager.Instance.BeginTutorialPhase(tutorialText[currentTutorialLevel]);
        }
    }
}
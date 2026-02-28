using System;
using System.Collections;
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
        WorkPhaseManager.Instance.WorkPhaseEnded += OnWorkPhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded += OnShopPhaseEnded;
        TutorialPhaseManager.Instance.TutorialPhaseEnded += OnTutorialPhaseEnded;
        PauseScreenController.Instance.BackToMenuEvent += OnBackToMenu;

        DG.Tweening.DOTween.SetTweensCapacity(1250, 50);
    }

    private void OnStartGame()
    {
        enabled = false;
        //Tutorial logic is being handled by TutorialManager
        MainMenuController.Instance.StartGameEvent -= OnStartGame;
        MainMenuController.Instance.StartTutorialEvent -= StartTutorial;
        WorkPhaseManager.Instance.WorkPhaseEnded -= OnWorkPhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded -= OnShopPhaseEnded;
        TutorialPhaseManager.Instance.TutorialPhaseEnded -= OnTutorialPhaseEnded;
        PauseScreenController.Instance.BackToMenuEvent -= OnBackToMenu;
    }

    private void StartTutorial()
    {
        AddTutorialUpgrades();
        StartTutorialLevel(TutorialLevel.TutorialLevel1);
        StartCoroutine(SetPhase(GameManagerHelper.Phase.Tutorial));
    }

    private void OnTutorialPhaseEnded()
    {
        StartCoroutine(SetPhase(GameManagerHelper.Phase.Work));
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
        currentTutorialLevel = tutorialLevel;
        PlayerUpgrades.Instance.RemoveTutorialUpgrades(currentTutorialLevel);
    }

    private void OnWorkPhaseEnded(bool won)
    {
        if (!gameManagerHelper.IsLastDay)
        {
            StartCoroutine(SetPhase(GameManagerHelper.Phase.Shop));
        }
        else
        {
            StartCoroutine(SetPhase(won ? GameManagerHelper.Phase.Win : GameManagerHelper.Phase.Lose));
        }
    }

    private void OnShopPhaseEnded()
    {
        gameManagerHelper.StartNextDay();
        StartTutorialLevel(currentTutorialLevel + 1);
        StartCoroutine(SetPhase(GameManagerHelper.Phase.Tutorial));
    }

    private IEnumerator SetPhase(GameManagerHelper.Phase phase)
    {
        yield return gameManagerHelper.SetPhase(phase);
        switch (phase)
        {
            case GameManagerHelper.Phase.Tutorial:
                TutorialPhaseManager.Instance.BeginTutorialPhase(tutorialText[currentTutorialLevel]);
                break;
            case GameManagerHelper.Phase.Win:
                EndScreenController.Instance.SetScreen(EndScreenController.Screen.TutorialWin);
                break;
            case GameManagerHelper.Phase.Lose:
                EndScreenController.Instance.SetScreen(EndScreenController.Screen.TutorialLose);
                break;
        }
    }

    private void OnBackToMenu()
    {
        StartCoroutine(gameManagerHelper.BackToMenu());
    }
}
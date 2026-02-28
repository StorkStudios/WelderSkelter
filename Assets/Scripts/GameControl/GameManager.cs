using StorkStudios.CoreNest;
using StorkStudios.DataWaste;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private GameManagerHelper gameManagerHelper;

    private int gameIndex;
    private bool tutorialGameMode = false;

    public int CurrentDay => gameManagerHelper.CurrentDay;
    public int TotalDays => gameManagerHelper.TotalDays;

    private void Start()
    {
        gameIndex = GetAndUpdateGameIndex();
        Telemetry.Instance.PreSendProcessor = TelemetryPreSendProcessor;

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
        tutorialGameMode = true;
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

    protected override void OnDestroy()
    {
        if (Telemetry.IsInstanced)
        {
            Telemetry.Instance.PreSendProcessor = null;
        }

        base.OnDestroy();
    }

    private void TelemetryPreSendProcessor(Dictionary<string, object> data)
    {
        data.Add("gameIndex", gameIndex);
        data.Add("gameMode", tutorialGameMode ? "Tutorial" : "Regular");
    }

    private int GetAndUpdateGameIndex()
    {
        int idx = PlayerPrefs.GetInt("GameIndex", 0);
        idx++;
        PlayerPrefs.SetInt("GameIndex", idx);
        return idx;
    }
}

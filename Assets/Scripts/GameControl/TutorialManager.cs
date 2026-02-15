using System;
using StorkStudios.CoreNest;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
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
    }

    private void OnStartGame()
    {
        enabled = false;
        MainMenuController.Instance.StartGameEvent -= OnStartGame;
    }

    public void InitTutorial()
    {
        AddTutorialUpgrades();
        StartTutorialLevel(TutorialLevel.TutorialLevel1);
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
}
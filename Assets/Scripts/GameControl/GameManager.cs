using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private enum Phase { Work, Shop }

    [SerializeField]
    private SerializedDictionary<Phase, GameObject> phaseParents;

    [SerializeField]
    private List<DayshiftData> dayshifts;

    [SerializeField]
    [ReadOnly]
    private int currentDay;

    private Phase phase;

    private void Start()
    {
        WorkPhaseManager.Instance.WorkPhaseEnded += OnGamePhaseEnded;
        ShopPhaseManager.Instance.ShopPhaseEnded += OnShopPhaseEnded;

        currentDay = 0;
        SetPhase(Phase.Work);
    }

    private void OnGamePhaseEnded(bool won)
    {
        if (won)
        {
            if (currentDay == dayshifts.Count - 1)
            {
                //todo: win
            }
            else
            {
                SetPhase(Phase.Shop);
            }
        }
        else
        {
            //todo: lose
        }
    }

    private void OnShopPhaseEnded()
    {
        currentDay++;
        SetPhase(Phase.Work);
    }

    private void SetPhase(Phase phase)
    {
        this.phase = phase;
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
        }
    }
}

using StorkStudios.CoreNest;
using UnityEngine;

public class WorkPhaseManager : Singleton<WorkPhaseManager>
{
    [SerializeField]
    [ReadOnly]
    private DayshiftData currentData;

    public DayshiftData CurrentData => currentData;
    public float DayTimer { get; private set; } = 0;

    public event System.Action<bool> WorkPhaseEnded;

    private bool active = false;

    public void BeginWorkPhase(DayshiftData data)
    {
        currentData = data;

        active = true;
        DayTimer = 0;

        TaskManager.Instance.Restart();
    }

    private void EndWorkPhase()
    {
        active = false;

        bool won = MoneyManager.Instance.Money >= currentData.moneyToCollect;
        WorkPhaseEnded?.Invoke(won);
    }

    private void Update()
    {
        if (!active)
        {
            return;
        }

        if ((DayTimer += Time.deltaTime) > currentData.dayLength)
        {
            EndWorkPhase();
        }
    }
}

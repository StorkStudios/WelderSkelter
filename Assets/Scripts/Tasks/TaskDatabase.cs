using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Task Database")]
public class TaskDatabase : ScriptableObjectSingleton<TaskDatabase>
{
    [SerializeField]
    private List<MoneyTask> moneyTasks;
    [SerializeField]
    private List<UpgradeTask> upgradeTasks;

    public Task GetNewTask()
    {
        IEnumerable<Task> allTasks = upgradeTasks.Where(e => !ItemShop.Instance.HasTaskItem(e.Item) && !PlayerUpgrades.Instance.HasUpgrade(e.Item.Upgrade));
        allTasks = allTasks.Concat(moneyTasks);
        allTasks = allTasks.Where(e => !TaskManager.Instance.CurrentTasks.Contains(e));
        return allTasks.GetRandomElement();
    }
}

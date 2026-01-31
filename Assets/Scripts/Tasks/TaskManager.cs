using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [SerializeField]
    private int maxTasks;
    [SerializeField]
    private List<MoneyTask> moneyTasks;
    [SerializeField]
    private List<UpgradeTask> upgradeTasks;

    private List<Task> currentTasks = new List<Task>();

    public List<Task> CurrentTasks => currentTasks;

    private Task GetNewTask()
    {
        IEnumerable<Task> allTasks = upgradeTasks.Where(e => !ItemShop.Instance.HasTaskItem(e.Item) && !PlayerUpgrades.Instance.HasUpgrade(e.Item.Upgrade));
        allTasks = allTasks.Concat(moneyTasks);
        allTasks = allTasks.Where(e => !currentTasks.Contains(e));
        return allTasks.GetRandomElement();
    }

    public void CompleteTask(Task task)
    {
        currentTasks[currentTasks.IndexOf(task)] = GetNewTask();
        task.Complete();
    }
}

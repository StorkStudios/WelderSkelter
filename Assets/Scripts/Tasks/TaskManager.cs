using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [SerializeField]
    private int maxTasks;

    public event System.Action<List<Task>> CurrentTasksChanged;
    public event System.Action<Task> TaskCompleted;
    public List<Task> CurrentTasks => currentTasks;

    private List<Task> currentTasks = new List<Task>();

    private void Start()
    {
        ItemSeller.Instance.ItemSold += OnItemSold;
        WorkPhaseManager.Instance.WorkPhasePreStartEvent += Restart;
    }

    private void OnItemSold(Dictionary<WeldingPartData, int> itemParts)
    {
        foreach (Task task in currentTasks)
        {
            if (task.RequiredParts.Distinct().Count() == itemParts.Keys.Count &&
                itemParts.Keys.All(key => { return itemParts[key] == task.RequiredParts.Where(part => part == key).Count(); }))
            {
                CompleteTask(task);
                return;
            }
        }
    }

    public void Restart()
    {
        currentTasks.Clear();
        for (int i = 0; i < maxTasks; i++)
        {
            currentTasks.Add(TaskDatabase.Instance.GetNewTask());
        }
        CurrentTasksChanged?.Invoke(currentTasks);
    }

    private void CompleteTask(Task task)
    {
        currentTasks[currentTasks.IndexOf(task)] = TaskDatabase.Instance.GetNewTask();
        task.Complete();
        TaskCompleted?.Invoke(task);
        CurrentTasksChanged?.Invoke(currentTasks);
    }

    public void DiscardTask(Task task)
    {
        currentTasks[currentTasks.IndexOf(task)] = TaskDatabase.Instance.GetNewTask();
        CurrentTasksChanged?.Invoke(currentTasks);
    }
}

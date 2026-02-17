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

    public class TaskManagerModifier
    {
        public bool disableGeneratingTasks = false;
    }

    private TaskManagerModifier modifier;
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
        modifier = PlayerUpgrades.Instance.GetModifier<TaskManagerModifier>();

        currentTasks.Clear();

        if (modifier != null && modifier.disableGeneratingTasks)
        {
            // Tasks list stays empty while generation is disabled.
            CurrentTasksChanged?.Invoke(currentTasks);
            return;
        }

        for (int i = 0; i < maxTasks; i++)
        {
            currentTasks.Add(TaskDatabase.Instance.GetNewTask());
        }
        CurrentTasksChanged?.Invoke(currentTasks);
    }

    private void CompleteTask(Task task)
    {
        int index = currentTasks.IndexOf(task);
        if (index < 0)
        {
            return;
        }

        task.Complete();
        TaskCompleted?.Invoke(task);

        if (modifier == null || !modifier.disableGeneratingTasks)
        {
            currentTasks[index] = TaskDatabase.Instance.GetNewTask();
        }
        else
        {
            // Remove the completed task without generating a new one.
            currentTasks.RemoveAt(index);
        }

        CurrentTasksChanged?.Invoke(currentTasks);
    }

    public void DiscardTask(Task task)
    {
        int index = currentTasks.IndexOf(task);
        if (index < 0)
        {
            return;
        }

        if (modifier == null || !modifier.disableGeneratingTasks)
        {
            currentTasks[index] = TaskDatabase.Instance.GetNewTask();
        }
        else
        {
            // Remove the discarded task without generating a new one.
            currentTasks.RemoveAt(index);
        }

        CurrentTasksChanged?.Invoke(currentTasks);
    }
}

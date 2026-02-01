using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    [SerializeField]
    private int maxTasks;
    [SerializeField]
    [ReadOnly]
    private List<Task> currentTasks = new List<Task>();

    public event System.Action<List<Task>> CurrentTasksChanged;
    public List<Task> CurrentTasks => currentTasks;

    public void Restart()
    {
        currentTasks.Clear();
        for (int i = 0; i < maxTasks; i++)
        {
            currentTasks.Add(TaskDatabase.Instance.GetNewTask());
        }
        CurrentTasksChanged?.Invoke(currentTasks);
    }

    public void CompleteTask(Task task)
    {
        currentTasks[currentTasks.IndexOf(task)] = TaskDatabase.Instance.GetNewTask();
        task.Complete();
        CurrentTasksChanged?.Invoke(currentTasks);
    }
}

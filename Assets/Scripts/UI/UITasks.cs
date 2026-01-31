using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;

public class UITasks : Singleton<UITasks>
{
    [SerializeField]
    private UITask taskPrefab;
    [SerializeField]
    private RectTransform taskParent;

    private void Start()
    {
        TaskManager.Instance.CurrentTasksChanged += OnCurrentTasksChanged;
    }

    private void OnCurrentTasksChanged(List<Task> currentTasks)
    {
        while (taskParent.childCount < currentTasks.Count)
        {
            Instantiate(taskPrefab.gameObject, taskParent);
        }

        int i = 0;
        foreach (RectTransform child in taskParent)
        {
            Task task = i < currentTasks.Count ? currentTasks[i] : null;
            child.GetComponent<UITask>().SetTask(task);
            i++;
        }
    }
}

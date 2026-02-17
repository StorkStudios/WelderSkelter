using StorkStudios.CoreNest;
using System.Collections.Generic;
using UnityEngine;

public class UITasks : Singleton<UITasks>
{
    [SerializeField]
    private UITask taskPrefab;
    [SerializeField]
    private RectTransform taskParent;

    private bool tasksDisabled = false;

    protected override void Awake()
    {
        TaskManager.Instance.CurrentTasksChanged += OnCurrentTasksChanged;

        base.Awake();
    }

    private void OnCurrentTasksChanged(List<Task> currentTasks)
    {
        if (currentTasks.Count == 0)
        {
            foreach (RectTransform child in taskParent)
            {
                child.gameObject.SetActive(false);
            }
            tasksDisabled = true;
            return;
        }

        if (tasksDisabled && currentTasks.Count > 0)
        {
            //Reenable tasks
            //There will be allways zero or all tasks available, so we don't check currentTasks size precisely
            foreach (RectTransform child in taskParent)
            {
                child.gameObject.SetActive(true);
            }
            tasksDisabled = false;
        }

        while (taskParent.childCount < currentTasks.Count)
        {
            Instantiate(taskPrefab.gameObject, taskParent);
        }

        int i = 0;
        foreach (RectTransform child in taskParent)
        {
            Task task = i < currentTasks.Count ? currentTasks[i] : null;
            UITask uiTask = child.GetComponent<UITask>();
            uiTask.Discarded -= OnDiscarded;
            uiTask.Discarded += OnDiscarded;
            uiTask.SetTask(task);
            i++;
        }
    }

    private void OnDiscarded(Task task)
    {
        TaskManager.Instance.DiscardTask(task);
    }
}

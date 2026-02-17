using UnityEngine;

[CreateAssetMenu(fileName = "TasksDisableUpgrade", menuName = "Upgrades/Tutorial/TasksDisableUpgrade")]
public class TasksDisableUpgrade : TutorialUpgrade, IUpgrade<TaskManager.TaskManagerModifier>
{
    public void ApplyModifier(TaskManager.TaskManagerModifier modifier)
    {
        modifier.disableGeneratingTasks = true;
    }
}

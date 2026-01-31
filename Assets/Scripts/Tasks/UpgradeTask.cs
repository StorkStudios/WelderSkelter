using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Upgrade Task")]
public class UpgradeTask : Task
{
    [SerializeField]
    [EditObjectInInspector]
    private Item item;

    public Item Item => item;

    public override void Complete()
    {
        ItemShop.Instance.AddTaskItem(item);
    }
}

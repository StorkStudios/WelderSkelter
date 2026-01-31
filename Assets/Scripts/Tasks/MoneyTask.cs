using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Money Task")]
public class MoneyTask : Task
{
    [SerializeField]
    private int money;

    public int Money => money;

    public override void Complete()
    {
        throw new System.NotImplementedException();
    }
}

using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Money Task")]
public class MoneyTask : Task
{
    public class MoneyTaskModifier
    {
        public float questIncomeModifier;
    }

    [SerializeField]
    private int money;

    public int Money => money;

    public override void Complete()
    {
        MoneyTaskModifier modifier = PlayerUpgrades.Instance.GetModifier<MoneyTaskModifier>();
        MoneyManager.Instance.AddMoney((int)(money * modifier.questIncomeModifier));
    }
}

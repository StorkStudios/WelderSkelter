using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Tasks/Money Task")]
public class MoneyTask : Task
{
    public class MoneyTaskModifier
    {
        public float questIncomeModifier = 1;
        public List<(float maskOnDuration, float incomeMultiplier)> maskOnForDurationMultipliers = new List<(float maskOnDuration, float incomeMultiplier)>();

        public float GetIncomeMultiplier()
        {
            float temporaryMultiplier = 1;
            if (WeldingMask.Instance.MaskOn.Value)
            {
                float maskOnTimestamp = WeldingMask.Instance.MaskOnTimestamp;
                temporaryMultiplier = maskOnForDurationMultipliers.Where(e => maskOnTimestamp + e.maskOnDuration < Time.time).Aggregate(1f, (current, e) => current * e.incomeMultiplier);
            }
            return questIncomeModifier * temporaryMultiplier;
        }
    }

    [SerializeField]
    private int money;

    public int Money => money;

    public override void Complete()
    {
        MoneyTaskModifier modifier = PlayerUpgrades.Instance.GetModifier<MoneyTaskModifier>();
        MoneyManager.Instance.AddMoney((int)(money * modifier.GetIncomeMultiplier()));
    }
}

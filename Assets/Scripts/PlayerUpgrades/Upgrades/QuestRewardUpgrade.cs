using UnityEngine;

[CreateAssetMenu(fileName = "QuestRewardUpgrade", menuName = "Upgrades/QuestRewardUpgrade")]
public class QuestRewardUpgrade : Upgrade, IUpgrade<MoneyTask.MoneyTaskModifier>
{
    [SerializeField]
    private float upgradeValue = 0.25f;

    public void ApplyModifier(MoneyTask.MoneyTaskModifier modifier)
    {
        modifier.questIncomeModifier *= 1 + upgradeValue;
    }
}

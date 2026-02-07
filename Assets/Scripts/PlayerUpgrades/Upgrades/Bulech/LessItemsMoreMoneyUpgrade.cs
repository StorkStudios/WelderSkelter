using UnityEngine;

[CreateAssetMenu(fileName = "LessItemsMoreMoneyUpgrade", menuName = "Scriptable Objects/LessItemsMoreMoneyUpgrade")]
public class LessItemsMoreMoneyUpgrade : Upgrade, IUpgrade<MoneyManager.MoneyManagerModifiers>
{
    [SerializeField]
    private float incomeMultiplier;
    [SerializeField]
    private float perItemIncomeMultiplierIncrement;

    public void ApplyModifier(MoneyManager.MoneyManagerModifiers modifier)
    {
        modifier.allIncomeMultipler *= incomeMultiplier;
        modifier.perItemIncomeMultiplier += perItemIncomeMultiplierIncrement;
    }
}

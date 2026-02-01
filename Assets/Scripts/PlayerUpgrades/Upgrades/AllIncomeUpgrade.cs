using UnityEngine;

[CreateAssetMenu(fileName = "AllIncomeUpgrade", menuName = "Scriptable Objects/AllIncomeUpgrade")]
public class AllIncomeUpgrade : Upgrade, IUpgrade<MoneyManager.MoneyManagerModifiers>
{
    [SerializeField]
    private float upgradeValue;

    public void ApplyModifier(MoneyManager.MoneyManagerModifiers modifier)
    {
        modifier.allIncomeMultipler *= 1 + upgradeValue;
    }
}

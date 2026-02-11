using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "NoDarknessOnConveyorUpgrade", menuName = "Upgrades/Bulech/NoDarknessOnConveyorUpgrade")]
    public class NoDarknessOnConveyorUpgrade : Upgrade, IUpgrade<WeldingMask.WeldingMaskModifier>
    {
        public void ApplyModifier(WeldingMask.WeldingMaskModifier modifier)
        {
            modifier.showConveyor = true;
        }
    }
}
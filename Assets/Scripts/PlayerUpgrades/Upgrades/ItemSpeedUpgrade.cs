using UnityEngine;

[CreateAssetMenu(fileName = "ItemSpeedUpgrade", menuName = "Upgrades/ItemSpeedUpgrade")]
public class ItemSpeedUpgrade : Upgrade, IUpgrade<WeldingPart.WeldingPartModifier>
{
    [SerializeField]
    private float speedChangeValue = 0.75f;

    public void ApplyModifier(WeldingPart.WeldingPartModifier modifier)
    {
        modifier.PartSpeedModifierWithMaskOn *= speedChangeValue;
    }
}

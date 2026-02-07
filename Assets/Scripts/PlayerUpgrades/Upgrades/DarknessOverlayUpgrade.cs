using UnityEngine;

[CreateAssetMenu(fileName = "DarknessOverlayUpgrade", menuName = "Upgrades/DarknessOverlayUpgrade")]
public class DarknessOverlayUpgrade : Upgrade, IUpgrade<WeldingMask.WeldingMaskModifier>
{
    [SerializeField]
    private float darknesMultiplier;

    public void ApplyModifier(WeldingMask.WeldingMaskModifier modifier)
    {
        modifier.darknessAlpha *= darknesMultiplier;
    }
}

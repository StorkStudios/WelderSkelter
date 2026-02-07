using UnityEngine;

[CreateAssetMenu(fileName = "WelderLightRadiusUpgrade", menuName = "Upgrades/WelderLightRadiusUpgrade")]
public class WelderLightRadiusUpgrade : Upgrade, IUpgrade<WeldingMask.WeldingMaskModifier>
{
    [SerializeField]
    private float scaleMultiplier;

    public void ApplyModifier(WeldingMask.WeldingMaskModifier modifier)
    {
        modifier.darknessScale *= scaleMultiplier;
    }
}

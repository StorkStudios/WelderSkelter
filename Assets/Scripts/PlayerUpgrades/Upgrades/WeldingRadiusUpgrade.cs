using UnityEngine;

[CreateAssetMenu(fileName = "WeldingRadiusUpgrade", menuName = "Upgrades/WeldingRadiusUpgrade")]
public class WeldingRadiusUpgrade : Upgrade, IUpgrade<Welder.WelderModifiers>
{
    [SerializeField]
    private float incrementValue = 0.1f;

    public void ApplyModifier(Welder.WelderModifiers modifier)
    {
        modifier.radius += incrementValue;
    }
}

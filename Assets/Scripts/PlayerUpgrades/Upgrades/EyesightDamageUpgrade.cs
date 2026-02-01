using UnityEngine;

[CreateAssetMenu(fileName = "EyesightDamageUpgrade", menuName = "Scriptable Objects/EyesightDamageUpgrade")]
public class EyesightDamageUpgrade : Upgrade, IUpgrade<EyesightManager.EyesightModifier>
{
    [SerializeField]
    private float eyesighDamageReduction = 0.5f;

    public void ApplyModifier(EyesightManager.EyesightModifier modifier)
    {
        modifier.eyesightDamageReduction *= eyesighDamageReduction;
    }
}

using UnityEngine;

namespace GreenUpgrades
{
    [CreateAssetMenu(fileName = "EyeDamageUpgrade", menuName = "Upgrades/Green/EyeDamageUpgrade")]
    public class EyeDamageUpgrade : Upgrade, IUpgrade<EyesightManager.EyesightModifier>
    {
        [SerializeField]
        private float eyesighDamageMultiplier = 0.5f;

        public void ApplyModifier(EyesightManager.EyesightModifier modifier)
        {
            modifier.eyesightDamageMultiplier *= eyesighDamageMultiplier;
        }
    }
}
using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "PermanentWeldingUpgrade", menuName = "Upgrades/Bulech/PermanentWeldingUpgrade")]
    public class PermanentWeldingUpgrade : Upgrade, IUpgrade<Welder.WelderModifiers>, IUpgrade<EyesightManager.EyesightModifier>
    {
        [SerializeField]
        private float lpmWeldingPointRadiusMultiplier;
        [SerializeField]
        private float eyeDamageMultiplier;
        [SerializeField]
        private float lpmEyeDamageMultiplier;

        public void ApplyModifier(Welder.WelderModifiers modifier)
        {
            modifier.weldPermanently = true;
            modifier.lpmWeldingPointRadiusMultiplier *= lpmWeldingPointRadiusMultiplier;
        }

        public void ApplyModifier(EyesightManager.EyesightModifier modifier)
        {
            modifier.eyesightDamageMultiplier *= eyeDamageMultiplier;
            modifier.lpmEyesigthDamageMultiplier *= lpmEyeDamageMultiplier;
        }
    }
}
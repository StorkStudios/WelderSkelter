using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "UnweldingBouncesUpgrade", menuName = "Upgrades/Bulech/UnweldingBouncesUpgrade")]
    public class UnweldingBouncesUpgrade : Upgrade, IUpgrade<WeldingPart.WeldingPartModifier>
    {
        [SerializeField]
        private float explosionForceIncrement;
        [SerializeField]
        private float scrapMoneyMultiplier;

        public void ApplyModifier(WeldingPart.WeldingPartModifier modifier)
        {
            modifier.unweldOnCollision = true;
            modifier.unweldExplosionForce += explosionForceIncrement;
            modifier.unweldScrapMoneyMultiplier *= scrapMoneyMultiplier;
        }
    }
}
using UnityEngine;

namespace BulechUpgrades
{
    [CreateAssetMenu(fileName = "SmallItemsUpgrade", menuName = "Upgrades/Bulech/SmallItemsUpgrade")]
    public class SmallItemsUpgrade : Upgrade, IUpgrade<WeldingPartsSpawner.Modifiers>, IUpgrade<Pusher.PusherModifier>
    {
        [SerializeField]
        private float scaleMultiplier = 0.6f;
        [SerializeField]
        private float itemCapacityMultiplier = 1.5f;

        public void ApplyModifier(WeldingPartsSpawner.Modifiers modifier)
        {
            modifier.scaleMultiplier *= scaleMultiplier;
        }

        public void ApplyModifier(Pusher.PusherModifier modifier)
        {
            modifier.itemCapacityMultiplier *= itemCapacityMultiplier;
        }
    }
}
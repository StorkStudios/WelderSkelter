using UnityEngine;

[CreateAssetMenu(fileName = "NewItemIntervalUpgrade", menuName = "Upgrades/NewItemIntervalUpgrade")]
public class NewItemIntervalUpgrade : Upgrade, IUpgrade<Pusher.PusherModifier>
{
    [SerializeField]
    private float timeReduction;
    
    public void ApplyModifier(Pusher.PusherModifier modifier)
    {
        modifier.DelayBetweenItemGroups *= 1f - timeReduction;
    }
}

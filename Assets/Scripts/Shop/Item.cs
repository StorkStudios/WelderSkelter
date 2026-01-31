using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    [EditObjectInInspector]
    private ItemRarity rarity;

    [SerializeField]
    [EditObjectInInspector]
    private Upgrade upgrade;

    public ItemRarity Rarity => rarity;
    public Upgrade Upgrade => upgrade;
}

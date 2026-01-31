using StorkStudios.CoreNest;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    private string title;

    [SerializeField]
    [EditObjectInInspector]
    private ItemRarity rarity;

    [SerializeField]
    [EditObjectInInspector]
    private Upgrade upgrade;

    public string Title => title;
    public ItemRarity Rarity => rarity;
    public Upgrade Upgrade => upgrade;
}

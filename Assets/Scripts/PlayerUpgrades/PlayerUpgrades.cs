using StorkStudios.CoreNest;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerUpgrades : Singleton<PlayerUpgrades>
{
    private List<Upgrade> upgrades = new List<Upgrade>();

    public bool HasUpgrade(Upgrade upgrade)
    {
        return upgrades.Contains(upgrade);
    }

    public void AddUpgrade(Upgrade upgrade)
    {
        upgrades.Add(upgrade);
    }

    public T GetModifier<T>() where T : class, new()
    {
        T modifier = new T();

        foreach (IUpgrade<T> upgrade in upgrades.OfType<IUpgrade<T>>())
        {
            upgrade.ApplyModifier(modifier);
        }

        return modifier;
    }
}

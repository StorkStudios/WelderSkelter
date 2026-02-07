using System.Collections.Generic;
using StorkStudios.CoreNest;
using UnityEngine;

public class WeldingPartsSpawner : Singleton<WeldingPartsSpawner>
{
    public class Modifiers
    {
        public float scaleMultiplier = 1;
    }

    [SerializeField]
    private float itemSpawnInterval;
    [SerializeField]
    private GameObject mikePrefab;

    private List<WeldingPartData> weldingPartDataList;

    public GameObject SpawnRandomPart()
    {
        weldingPartDataList ??= new List<WeldingPartData>(Resources.LoadAll<WeldingPartData>("WeldingParts"));
        GameObject gameObject = weldingPartDataList[Random.Range(0, weldingPartDataList.Count)].Instantiate(transform.position, Quaternion.identity, transform);
        Modifiers modifiers = PlayerUpgrades.Instance.GetModifier<Modifiers>();
        gameObject.transform.localScale *= modifiers.scaleMultiplier;
        return gameObject;
    }

    public GameObject SpawnMike()
    {
        GameObject gameObject = Instantiate(mikePrefab, transform.position, Quaternion.identity, transform);
        Modifiers modifiers = PlayerUpgrades.Instance.GetModifier<Modifiers>();
        gameObject.transform.localScale *= modifiers.scaleMultiplier;
        return gameObject;
    }
}

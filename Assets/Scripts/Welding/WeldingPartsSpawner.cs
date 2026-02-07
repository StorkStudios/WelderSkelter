using System.Collections.Generic;
using StorkStudios.CoreNest;
using UnityEngine;

public class WeldingPartsSpawner : Singleton<WeldingPartsSpawner>
{
    [SerializeField]
    private float itemSpawnInterval;
    [SerializeField]
    private GameObject mikePrefab;

    private List<WeldingPartData> weldingPartDataList;

    public GameObject SpawnRandomPart()
    {
        weldingPartDataList ??= new List<WeldingPartData>(Resources.LoadAll<WeldingPartData>("WeldingParts"));
        return weldingPartDataList[Random.Range(0, weldingPartDataList.Count)].Instantiate(transform.position, Quaternion.identity, transform);
    }

    public GameObject SpawnMike()
    {
        return Instantiate(mikePrefab, transform.position, Quaternion.identity, transform);
    }
}

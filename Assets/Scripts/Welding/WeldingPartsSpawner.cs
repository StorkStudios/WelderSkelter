using System.Collections.Generic;
using StorkStudios.CoreNest;
using UnityEngine;

public class WeldingPartsSpawner : Singleton<WeldingPartsSpawner>
{
    [SerializeField]
    private float itemSpawnInterval;

    private List<WeldingPartData> weldingPartDataList;

    private void Start()
    {
        weldingPartDataList = new List<WeldingPartData>(Resources.LoadAll<WeldingPartData>("WeldingParts"));
    }

    public GameObject SpawnRandomPart()
    {
        return weldingPartDataList[Random.Range(0, weldingPartDataList.Count)].Instantiate(transform.position, Quaternion.identity, transform);
    }
}

using System.Collections.Generic;
using StorkStudios.CoreNest;
using UnityEngine;

public class WeldingPartsSpawner : Singleton<WeldingPartsSpawner>
{
    [SerializeField]
    private float itemSpawnInterval;

    private List<WeldingPartData> weldingPartDataList;

    public GameObject SpawnRandomPart()
    {
        if (weldingPartDataList == null)
        {
            weldingPartDataList = new List<WeldingPartData>(Resources.LoadAll<WeldingPartData>("WeldingParts"));
        }
        return weldingPartDataList[Random.Range(0, weldingPartDataList.Count)].Instantiate(transform.position, Quaternion.identity, transform);
    }
}

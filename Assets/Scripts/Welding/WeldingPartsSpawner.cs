using System.Collections.Generic;
using UnityEngine;

public class WeldingPartsSpawner : MonoBehaviour
{
    private List<WeldingPartData> weldingPartDataList;

    private void Start()
    {
        weldingPartDataList = new List<WeldingPartData>(Resources.LoadAll<WeldingPartData>("WeldingParts"));
        SpawnInitialParts();
    }

    private void SpawnInitialParts()
    {
        SpawnRandomPart();
        SpawnRandomPart();
        SpawnRandomPart();
    }

    private void SpawnRandomPart()
    {
        GameObject part = weldingPartDataList[Random.Range(0, weldingPartDataList.Count)].Instantiate(transform.position, Quaternion.identity);
        Rigidbody2D partRb = part.GetComponent<Rigidbody2D>();
        partRb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);
    }
}

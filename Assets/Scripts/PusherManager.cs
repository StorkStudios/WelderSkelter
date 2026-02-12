using System.Collections.Generic;
using UnityEngine;

public class PusherManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pusherPrefab;
    
    private List<Animator> pusherAnimators = new List<Animator>();

    public void Restart(int pushersCount, float positionOffset)
    {
        pusherAnimators.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < pushersCount; i++)
        {
            GameObject gameObject = Instantiate(pusherPrefab, transform);
            gameObject.transform.position += positionOffset * i * Vector3.left;
            pusherAnimators.Add(gameObject.GetComponent<Animator>());
        }
    }

    public void StartPushAnimation()
    {
        foreach (Animator animator in pusherAnimators)
        {
            animator.SetTrigger("PushTrigger");
        }
    }
}

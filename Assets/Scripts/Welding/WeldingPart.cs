using System.Collections.Generic;
using UnityEngine;

public class WeldingPart : MonoBehaviour
{
    [SerializeField]
    private string partName;

    private Dictionary<string, int> components;
    private List<ColliderEvents> weldTriggers;
    public List<ColliderEvents> WeldTriggers => weldTriggers;
    private HashSet<WeldingPart> collidingParts = new HashSet<WeldingPart>();
    public HashSet<WeldingPart> CollidingParts => collidingParts;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);
        components = new Dictionary<string, int> { { partName, 1 } };
        weldTriggers = new List<ColliderEvents>(GetComponentsInChildren<ColliderEvents>());
        foreach (ColliderEvents trigger in weldTriggers)
        {
            trigger.OnTriggerEnterEvent += OnWeldTriggerEnter;
            trigger.OnTriggerExitEvent += OnWeldTriggerExit;
        }
    }

    private void OnWeldTriggerExit(Collider2D d)
    {
        collidingParts.Remove(d.GetComponentInParent<WeldingPart>());
    }

    private void OnWeldTriggerEnter(Collider2D d)
    {
        collidingParts.Add(d.GetComponentInParent<WeldingPart>());
    }

    public void WeldWith(WeldingPart otherPart)
    {
        foreach (var component in otherPart.components)
        {
            if (components.ContainsKey(component.Key))
            {
                components[component.Key] += component.Value;
            }
            else
            {
                components[component.Key] = component.Value;
            }
        }
        //TODO: momentum sumation
        foreach (ColliderEvents trigger in otherPart.WeldTriggers)
        {
            trigger.OnTriggerEnterEvent += OnWeldTriggerEnter;
            trigger.OnTriggerExitEvent += OnWeldTriggerExit;
        }
        weldTriggers.AddRange(otherPart.WeldTriggers);
        Destroy(otherPart.GetComponent<Rigidbody2D>());
        Destroy(otherPart);
        otherPart.transform.SetParent(transform);
    }

    private void OnDestroy()
    {
        foreach (ColliderEvents trigger in weldTriggers)
        {
            trigger.OnTriggerEnterEvent -= OnWeldTriggerEnter;
            trigger.OnTriggerExitEvent -= OnWeldTriggerExit;
        }
    }
}

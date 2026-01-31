using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class WeldingPart : MonoBehaviour
{
    [SerializeField]
    private string partName;

    private Dictionary<string, int> components;
    private List<ColliderEvents> weldTriggers;
    public List<ColliderEvents> WeldTriggers => weldTriggers;
    private Dictionary<WeldingPart, int> collidingParts = new Dictionary<WeldingPart, int>();
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
        WeldingPart otherPart = d.GetComponentInParent<WeldingPart>();
        if (otherPart == null)
        {
            return;
        }


        if (collidingParts.ContainsKey(otherPart))
        {
            collidingParts[otherPart]--;
            if (collidingParts[otherPart] == 0)
            {
                collidingParts.Remove(otherPart);
                //Debug.Log($"{name}: exited weld trigger with {d.name}");
            }
        }
        else
        {
            Debug.LogWarning($"{name}: exited weld trigger with {d.name} but was not colliding");
        }
    }

    private void OnWeldTriggerEnter(Collider2D d)
    {
        WeldingPart otherPart = d.GetComponentInParent<WeldingPart>();
        if (otherPart == null)
        {
            return;
        }

        if (collidingParts.ContainsKey(otherPart))
        {
            collidingParts[otherPart]++;
        }
        else
        {
            collidingParts[otherPart] = 1;
            //Debug.Log($"{name}: entered weld trigger with {d.name}");
        }
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
        foreach (ColliderEvents trigger in otherPart.WeldTriggers)
        {
            trigger.OnTriggerEnterEvent += OnWeldTriggerEnter;
            trigger.OnTriggerExitEvent += OnWeldTriggerExit;
        }
        weldTriggers.AddRange(otherPart.WeldTriggers);
        foreach (var collidingWithOther in otherPart.collidingParts)
        {
            if (collidingWithOther.Key != this)
            {
                if (collidingParts.ContainsKey(collidingWithOther.Key))
                {
                    collidingParts[collidingWithOther.Key] += collidingWithOther.Value;
                }
                else
                {
                    collidingParts[collidingWithOther.Key] = collidingWithOther.Value;
                    //Debug.Log($"{name}: entered weld trigger with {otherPart.name}");
                }
            }
        }
        otherPart.transform.SetParent(transform);

        Rigidbody2D otherRb = otherPart.GetComponent<Rigidbody2D>();
        Vector2 combinedVelocity = (rb.linearVelocity * rb.mass + otherRb.linearVelocity * otherRb.mass) / (rb.mass + otherRb.mass);
        float combinedAngularVelocity = (rb.angularVelocity * rb.mass + otherRb.angularVelocity * otherRb.mass) / (rb.mass + otherRb.mass);
        rb.mass += otherRb.mass;
        rb.linearVelocity = combinedVelocity;
        rb.angularVelocity = combinedAngularVelocity;

        Destroy(otherPart);
        Destroy(otherRb);
    }

    public bool IsCollidingWith(WeldingPart otherPart)
    {
        return collidingParts.ContainsKey(otherPart);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ColliderEvents : MonoBehaviour
{
    public event System.Action<Collision2D> OnCollisionEnterEvent;
    public event System.Action<Collision2D> OnCollisionStayEvent;
    public event System.Action<Collision2D> OnCollisionExitEvent;

    public event System.Action<Collider2D> OnTriggerEnterEvent;
    public event System.Action<Collider2D> OnTriggerStayEvent;
    public event System.Action<Collider2D> OnTriggerExitEvent;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnterEvent?.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        OnCollisionStayEvent?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        OnCollisionExitEvent?.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        OnTriggerStayEvent?.Invoke(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }
}

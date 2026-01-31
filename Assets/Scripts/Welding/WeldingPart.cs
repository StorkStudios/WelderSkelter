using UnityEngine;

public class WeldingPart : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(Random.insideUnitCircle * 5f, ForceMode2D.Impulse);
    }
}

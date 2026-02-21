using UnityEngine;

public class WeldingParticles : MonoBehaviour
{
    [SerializeField]
    private GameObject particles;
    [SerializeField]
    private GameObject weldingPoint;

    public void SetWelding(bool weldingState)
    {
        if (weldingState)
        {
            StartWelding();
        }
        else
        {
            StopWelding();
        }
    }

    public void StartWelding()
    {
        particles.SetActive(true);
        weldingPoint.SetActive(false);
    }

    public void StopWelding()
    {
        particles.SetActive(false);
        weldingPoint.SetActive(true);
    }
}

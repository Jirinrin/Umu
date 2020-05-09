using UnityEngine;

public class ReverseParticleSystemSimple : MonoBehaviour
{
    ParticleSystem particleSystem;

    private float simulationTime;

    private bool reverseActive;

    public float simulationSpeedScale = 1.0f;

    void OnEnable()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        simulationTime = 0f;
    }
    void Update()
    {
        if (reverseActive)
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particleSystem.Play(true);
            simulationTime -= Time.deltaTime * particleSystem.main.simulationSpeed * simulationSpeedScale;
            particleSystem.Simulate(simulationTime, true, false, true);

            if (simulationTime < 0f)
                simulationTime = particleSystem.main.duration-.0001f;
        }
        else
            simulationTime = particleSystem.time;
    }

    public void StartReverse()
    {
        if (reverseActive)
            return;

        reverseActive = true;
    }

    public void StopReverse()
    {
        if (!reverseActive)
            return;

        reverseActive = false;
        
        particleSystem.Simulate(simulationTime, true, false, true);
        particleSystem.Play(true);
    }
}

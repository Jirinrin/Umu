using UnityEngine;

public class ReverseParticleSystemSimple : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private float _simulationTime;

    private bool _reverseActive;

    // public float simulationSpeedScale = .1f;
    public float simulationSpeedScale = .01f;

    private void OnEnable()
    {
        if (_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        _simulationTime = 0f;
    }
    private void Update()
    {
        if (_reverseActive)
        {
            // _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            // _particleSystem.Play(true);
            _simulationTime -= Time.deltaTime * simulationSpeedScale; // * _particleSystem.main.simulationSpeed
            if (_simulationTime < 0f)
                _simulationTime = _particleSystem.main.duration-.0001f;
            
            _particleSystem.Simulate(_simulationTime, true, false, true);
        }
        else
            _simulationTime = _particleSystem.time;
    }

    public void StartReverse()
    {
        if (_reverseActive)
            return;

        _reverseActive = true;
    }

    public void StopReverse()
    {
        if (!_reverseActive)
            return;

        _reverseActive = false;
        
        _particleSystem.Simulate(_simulationTime, true, false, true);
        _particleSystem.Play(true);
    }
}

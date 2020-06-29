using UnityEngine;

public class ReverseParticleSystemSimple : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private ParticleSystem.MainModule _particleSystemMain;

    private float _simulationTime;

    private bool _reverseActive;

    private float _simulationDuration;

    public float simulationSpeedScale = .1f;
    // public float simulationSpeedScale = .01f;

    public bool fakeReverse;

    private void OnEnable()
    {
        if (_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystemMain = _particleSystem.main;
        }

        _simulationTime = 0f;
        _simulationDuration = _particleSystemMain.duration;
    }
    private void Update()
    {
        if (fakeReverse) return;
        
        if (_reverseActive)
        {
            // _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            // _particleSystem.Play(true);
            _simulationTime -= Time.deltaTime * simulationSpeedScale; // * _particleSystemMain.simulationSpeed
            if (_simulationTime < 0f)
                _simulationTime = _simulationDuration-.0001f;
                
            if (_simulationTime % 0.1f < 0.05f)
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
        
        if (fakeReverse)
            _particleSystemMain.simulationSpeed = 40f;
    }

    public void StopReverse()
    {
        if (!_reverseActive)
            return;
        _reverseActive = false;
        
        if (fakeReverse)
            _particleSystemMain.simulationSpeed = 1f; // todo: probably account for the original simulationSpeed
        else
        {
            _particleSystem.Simulate(_simulationTime, true, false, true);
            _particleSystem.Play(true);
        }
    }
}

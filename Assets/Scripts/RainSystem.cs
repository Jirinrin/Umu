using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(ReverseParticleSystemSimple))]
public class RainSystem : MonoBehaviour
{
    [SerializeField] private float rainChangeDuration = 1f;
    [SerializeField] public float maxRainAngle = 1.45f;
    [SerializeField] private float distanceToCamera = 45f;
    [SerializeField] private ParticleSystem splashParticleSystem;
    
    private ParticleSystem _particleSystem;
    private ParticleSystem.ShapeModule _psShape;
    private ParticleSystem.ShapeModule _psSplashShape;
    private ReverseParticleSystemSimple _reverseParticleSystem;
    private Camera _camera;
    public bool DoReverseRain { set; get; } = true;
    
    private float _rainRotation;
    public float rainRotation
    {
        get => _rainRotation;
        private set
        {
            _rainRotation = value;
            var newRotation = value * Mathf.Rad2Deg;
            _psShape.rotation = new Vector3(0, 0, newRotation);
            _psSplashShape.rotation = new Vector3(-90f-newRotation, 90f, -90f); 
        }
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _camera = Camera.main;
        _particleSystem = GetComponent<ParticleSystem>();
        _psShape = _particleSystem.shape;
        _psSplashShape = splashParticleSystem.shape;
        _reverseParticleSystem = GetComponent<ReverseParticleSystemSimple>();
    }

    private void Update()
    {
        if (!_camera) return;
        var camPos = _camera.transform.position;
        camPos.x += Mathf.Cos(rainRotation+Mathf.PI*0.5f ) * distanceToCamera;
        camPos.y += Mathf.Sin(rainRotation+Mathf.PI*0.5f ) * distanceToCamera;
        camPos.z = transform.position.z;
        _psShape.position = camPos;
    }
    
    public void ChangeRainDirection(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed)
            return;
        
        var value = ctx.ReadValue<Vector2>();
        var rawRotation = Mathf.Atan2(value.y, value.x);
        var newRotation = (rawRotation + Mathf.PI*3/2) % (2*Mathf.PI) - Mathf.PI;
        var clampedRotation = Mathf.Clamp(newRotation, -maxRainAngle, maxRainAngle);
        // todo: do this at a constant speed instead of always e.g. 1s
        DOTween.To(() => rainRotation, v => rainRotation = v, clampedRotation, rainChangeDuration);
    }

    public void ReverseRain(bool reversedDirection)
    {
        if (!DoReverseRain)
            return;
        
        if (reversedDirection)
           _reverseParticleSystem.StartReverse();
        else
            _reverseParticleSystem.StopReverse();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(ReverseParticleSystemSimple))]
public class RainSystem : MonoBehaviour
{
    public float rainRotation;
    
    private ParticleSystem ps;
    private ParticleSystem.ShapeModule sh;
    private ReverseParticleSystemSimple rv;
    private Camera cam;

    private const float distanceToCamera = 35f;

    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        ps = GetComponent<ParticleSystem>();
        sh = ps.shape;
        rv = GetComponent<ReverseParticleSystemSimple>();

        rainRotation = 0.7f;
    }

    // Update is called once per frame
    private void Update()
    {

        // rot = Time.time * 40;
        // rot = Mathf.Sin(Time.time/4) * 30;
        // rot = -30;
        // rot += Mathf.Clamp((Time.time-8f)*20, 0f, 60f);
        // rot = Mathf.Sin(Time.time) * 50;
        sh.rotation = new Vector3(0, 0, rainRotation*Mathf.Rad2Deg);
        
        if (cam == null) return;
        var camPos = cam.transform.position;
        camPos.x += Mathf.Cos(rainRotation+Mathf.PI*0.5f ) * distanceToCamera;
        camPos.y += Mathf.Sin(rainRotation+Mathf.PI*0.5f ) * distanceToCamera;
        camPos.z = transform.position.z;
        sh.position = camPos;
    }

    public void ChangeRainDirection(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            return;
        Debug.Log("change direction");
        // Debug.Log(ctx.phase);
        Debug.Log(ctx.ReadValueAsObject());
        // Debug.Log(Vector2.Angle(Vector2.zero, ctx.ReadValue<Vector2>()));
        
        Debug.Log("Angle");
        var value = ctx.ReadValue<Vector2>();
        var rawRotation = Mathf.Atan2(value.y, value.x);
        rainRotation = Mathf.Clamp(rawRotation + 1.57f, -1.57f, 1.57f);
    }

    public void ReverseRain(bool reversedDirection)
    {
        if (reversedDirection)
           rv.StartReverse();
        else
            rv.StopReverse();
    }
}

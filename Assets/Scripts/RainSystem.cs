using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[RequireComponent(typeof(ReverseParticleSystemSimple))]
public class RainSystem : MonoBehaviour
{
    private float rot;
    private ParticleSystem ps;
    private ParticleSystem.ShapeModule sh;
    private ReverseParticleSystemSimple rv;

    private const float distanceToCamera = 35f;

    // Start is called before the first frame update
    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
        sh = ps.shape;
        rv = GetComponent<ReverseParticleSystemSimple>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.time > 5f)
            rv.StopReverse();
        else if (Time.time > 3f)
            rv.StartReverse();

        // rot = Time.time * 40;
        rot = -30;
        rot += Mathf.Clamp((Time.time-8f)*20, 0f, 60f);
        // rot = Mathf.Sin(Time.time) * 50;
        sh.rotation = new Vector3(0, 0, rot);
        var rotRad = Mathf.Deg2Rad * rot;
        
        if (Camera.main == null) return;
        var pos = Camera.main.transform.position;
        pos.x += Mathf.Cos(rotRad+Mathf.PI*0.5f ) * distanceToCamera;
        pos.y += Mathf.Sin(rotRad+Mathf.PI*0.5f ) * distanceToCamera;
        pos.z = transform.position.z;
        sh.position = pos;
    }
}

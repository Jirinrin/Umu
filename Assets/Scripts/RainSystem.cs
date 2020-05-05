using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class RainSystem : MonoBehaviour
{
    private float rot = 0f;
    private ParticleSystem ps;
    private ParticleSystem.ShapeModule sh;

    private const float distanceToCamera = 30f;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        sh = ps.shape;
    }

    // Update is called once per frame
    void Update()
    {
        rot = Time.time * 40;
        sh.rotation = new Vector3(0, 0, rot);
        var rotRad = Mathf.Deg2Rad * rot;
        var pos = Camera.main.transform.position;
        pos.x += Mathf.Cos(rotRad+Mathf.PI*0.5f ) * 30;
        pos.y += Mathf.Sin(rotRad+Mathf.PI*0.5f ) * 30;
        pos.z = transform.position.z;
        sh.position = pos;
    }
}

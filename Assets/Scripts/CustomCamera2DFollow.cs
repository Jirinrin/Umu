using System;
using UnityEngine;

public class CustomCamera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 0.3f;
    public float lookAheadFactor = 2;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    private float _offsetZ;
    private Vector3 _lastTargetPosition;
    private Vector3 _currentVelocity;
    private Vector3 _lookAheadPos;

    // Use this for initialization
    private void Start()
    {
        _lastTargetPosition = target.position;
        var currentTransform = transform;
        _offsetZ = (currentTransform.position - _lastTargetPosition).z;
        currentTransform.parent = null;
    }


    // Update is called once per frame
    private void Update()
    {
        var position = target.position;
        
        // only update lookahead pos if accelerating or changed direction
        var xMoveDelta = (position - _lastTargetPosition).x;

        var updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
            _lookAheadPos = Vector3.right * (lookAheadFactor * Mathf.Sign(xMoveDelta));
        else
            _lookAheadPos = Vector3.MoveTowards(_lookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);

        var aheadTargetPos = position + _lookAheadPos + Vector3.forward*_offsetZ;
        var newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, damping);

        transform.position = newPos;

        _lastTargetPosition = position;
    }
}
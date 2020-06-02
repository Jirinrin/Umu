using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (CustomPlatformerCharacter2D))]
public class CustomPlatformer2DUserControl : MonoBehaviour
{
    [SerializeField] private RainSystem rainSystem;
    
    private CustomPlatformerCharacter2D _character;
    private bool _jumping;
    private Vector2? _moving;
    
    

    private void Awake()
    {
        _character = GetComponent<CustomPlatformerCharacter2D>();
    }
    
    // private void Update()
    // {
    //     if (!m_Jump)
    //     {
    //         // Read the jump input in Update so button presses aren't missed.
    //         // m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
    //     }
    // }

    private void FixedUpdate()
    {
        // Read the inputs.
        // bool crouch = Input.GetKey(KeyCode.LeftControl);
        // float h = CrossPlatformInputManager.GetAxis("Horizontal");
        // if (m_Moving.HasValue)
        _character.Move(_moving?.x ?? 0, false, _jumping);
        // m_Character.Move(-1, false, false);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            _moving = null;
        else
            _moving = ctx.ReadValue<Vector2>();
    }
    
    public void Jump(InputAction.CallbackContext ctx)
    {
        _jumping = !ctx.canceled;
    }
    
    public void Launch(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        
        Debug.Log("launch");
        
        // rainSystem.ReverseRain(true);
        // StartCoroutine(ChangeBackRain(2f));

        var rot = rainSystem.rainRotation + Mathf.PI*.5f;
        _character.Launch(new Vector2(1400, 1400) * new Vector2(Mathf.Cos(rot), Mathf.Sin(rot)));
    }

    private IEnumerator ChangeBackRain(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        rainSystem.ReverseRain(false);
    }
}

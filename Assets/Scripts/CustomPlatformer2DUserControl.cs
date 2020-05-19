using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (CustomPlatformerCharacter2D))]
public class CustomPlatformer2DUserControl : MonoBehaviour
{
    private CustomPlatformerCharacter2D m_Character;
    private bool m_Jumping;
    private Vector2? m_Moving;


    private void Awake()
    {
        m_Character = GetComponent<CustomPlatformerCharacter2D>();
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
        m_Character.Move(m_Moving?.x ?? 0, false, m_Jumping);
        // m_Character.Move(-1, false, false);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (ctx.canceled)
            m_Moving = null;
        else
            m_Moving = ctx.ReadValue<Vector2>();
    }
    
    public void Jump(InputAction.CallbackContext ctx)
    {
        m_Jumping = !ctx.canceled;
        // Debug.Log("Jumppppp");
        // Debug.Log(ctx.phase);
        // Debug.Log(ctx.ReadValue<>());

        // ctx.
    }
}

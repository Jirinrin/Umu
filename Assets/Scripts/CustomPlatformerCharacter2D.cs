using System;
using UnityEngine;

[RequireComponent(typeof (CircleCollider2D))]
public class CustomPlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    private Transform m_CeilingCheck;   // A position marking where to check for ceilings
    const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator m_Anim;            // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    private Collider2D m_FootCollider;
    private bool _jumping;
    private bool _launching;
    private float _moveVelocity = 0f;
    
    private static readonly int Ground = Animator.StringToHash("Ground");
    private static readonly int VSpeed = Animator.StringToHash("vSpeed");
    private static readonly int Crouch = Animator.StringToHash("Crouch");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        // Setting up references.
        m_GroundCheck = transform.Find("GroundCheck");
        m_CeilingCheck = transform.Find("CeilingCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_FootCollider = GetComponent<CircleCollider2D>();
    }


    private void FixedUpdate()
    {
        if (_jumping && m_Rigidbody2D.velocity.y <= 0)
            _jumping = false;
        
        m_Grounded = Physics2D.IsTouchingLayers(m_FootCollider, m_WhatIsGround) && !_jumping;

        m_Anim.SetBool(Ground, m_Grounded);

        var velocity = m_Rigidbody2D.velocity;
        
        // Set the vertical animation
        m_Anim.SetFloat(VSpeed, velocity.y);
        
        // Move the character
        // if (move > 0f && velocity.x < 0f || move < 0f && velocity.x > 0f)
        // {
        //     velocity.x += move;
        // }
        // else
        // {
        //     velocity.x = velocity.x - _moveVelocity + move;
        //     _moveVelocity = move;
        // }
        // m_Rigidbody2D.AddForce(new Vector2(move*10, 0f));

        if (_launching && Math.Abs(_moveVelocity) > 0.01f)
        {
            if (velocity.y >= 0f)
            {
                velocity *= 0.9f; // todo: * modifier
                if (velocity.y < 0.1f)
                    _launching = false;
                else
                    m_Rigidbody2D.velocity = velocity;
            }
            else
                _launching = false;
        }

        if (!_launching)
        {
            velocity.x = _moveVelocity;
            m_Rigidbody2D.velocity = velocity;
        }
        
        if (velocity.x > 0 && !m_FacingRight)
            Flip();
        else if (velocity.x < 0 && m_FacingRight)
            Flip();

        // m_Rigidbody2D.velocity = Vector2.zero;

        // var position = m_Rigidbody2D.position;
        // position.y += velocity.y*Time.fixedDeltaTime;
        // position.x += velocity.x*Time.fixedDeltaTime;
        // position.x += velocity.x*Time.fixedDeltaTime;
        // m_Rigidbody2D.MovePosition(position);
        
        // m_Rigidbody2D.cal
        
        // m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + m_Rigidbody2D.velocity * (Time.fixedDeltaTime * 100f));
        // m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + new Vector2(.1f,.1f));
    }


    public void Move(float move, bool crouch, bool jump)
    {
        // If crouching, check to see if the character can stand up
        if (!crouch && m_Anim.GetBool(Crouch))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                crouch = true;
        }

        // Set whether or not the character is crouching in the animator
        m_Anim.SetBool(Crouch, crouch);

        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move *= (crouch ? m_CrouchSpeed : 1) * m_MaxSpeed;

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            m_Anim.SetFloat(Speed, Mathf.Abs(move));

            _moveVelocity = move;
        }
        // If the player should jump...
        if (m_Grounded && jump && m_Anim.GetBool(Ground))
        {
            // Add a vertical force to the player.
            _jumping = true;
            m_Grounded = false;
            m_Anim.SetBool(Ground, false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }
    
    public void Launch(Vector2 direction)
    {
        m_Grounded = false;
        _jumping = true;
        _launching = true;
        m_Anim.SetBool(Ground, false);
        m_Rigidbody2D.AddForce(direction);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

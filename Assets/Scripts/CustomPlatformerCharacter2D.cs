using System;
using UnityEngine;

[RequireComponent(typeof (CircleCollider2D))]
public class CustomPlatformerCharacter2D : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float jumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool airControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask whatIsGround;                  // A mask determining what is ground to the character

    private Transform _groundCheck;    // A position marking where to check if the player is grounded.
    private const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool _grounded;            // Whether or not the player is grounded.
    private Transform _ceilingCheck;   // A position marking where to check for ceilings
    private const float CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
    private Animator _anim;            // Reference to the player's animator component.
    private Rigidbody2D _rigidbody2D;
    private bool _facingRight = true;  // For determining which way the player is currently facing.
    private Collider2D _footCollider;
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
        _groundCheck = transform.Find("GroundCheck");
        _ceilingCheck = transform.Find("CeilingCheck");
        _anim = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _footCollider = GetComponent<CircleCollider2D>();
    }


    private void FixedUpdate()
    {
        if (_jumping && _rigidbody2D.velocity.y <= 0)
            _jumping = false;
        
        _grounded = Physics2D.IsTouchingLayers(_footCollider, whatIsGround) && !_jumping;

        _anim.SetBool(Ground, _grounded);

        var velocity = _rigidbody2D.velocity;
        
        // Set the vertical animation
        _anim.SetFloat(VSpeed, velocity.y);
        
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
                    _rigidbody2D.velocity = velocity;
            }
            else
                _launching = false;
        }

        if (!_launching)
        {
            velocity.x = _moveVelocity;
            _rigidbody2D.velocity = velocity;
        }
        
        if (velocity.x > 0 && !_facingRight)
            Flip();
        else if (velocity.x < 0 && _facingRight)
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
        if (!crouch && _anim.GetBool(Crouch))
        {
            // If the character has a ceiling preventing them from standing up, keep them crouching
            if (Physics2D.OverlapCircle(_ceilingCheck.position, CeilingRadius, whatIsGround))
                crouch = true;
        }

        // Set whether or not the character is crouching in the animator
        _anim.SetBool(Crouch, crouch);

        //only control the player if grounded or airControl is turned on
        if (_grounded || airControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move *= (crouch ? crouchSpeed : 1) * maxSpeed;

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            _anim.SetFloat(Speed, Mathf.Abs(move));

            _moveVelocity = move;
        }
        // If the player should jump...
        if (_grounded && jump && _anim.GetBool(Ground))
        {
            // Add a vertical force to the player.
            _jumping = true;
            _grounded = false;
            _anim.SetBool(Ground, false);
            _rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
    }
    
    public void Launch(Vector2 direction)
    {
        _grounded = false;
        _jumping = true;
        _launching = true;
        _anim.SetBool(Ground, false);
        _rigidbody2D.velocity = new Vector2(0, 0);
        _rigidbody2D.AddForce(direction);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        _facingRight = !_facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

using System.Resources;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching


	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.	

	//Player's Variables
	private Rigidbody2D m_Rigidbody2D;
	private Vector3 m_Velocity = Vector3.zero;
	public Animator animator;
	public Animator anim1;
	private bool jump = false;

	//Ladder's Variables
	public bool canClimb = false;
	[HideInInspector] public bool bottomLadder = false;
	[HideInInspector] public bool topLadder = false;
	[SerializeField] float climbSpeed = 5f;
	public Ladder ladder;
	private float climbVelocity;
	private float naturalGravity;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent OnFallEvent;
	public UnityEvent OnClimbEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		anim1 = GameObject.FindGameObjectWithTag("Cherries").GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();		

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		if (OnFallEvent == null)
			OnFallEvent = new UnityEvent();

		if (OnClimbEvent == null)
			OnClimbEvent = new UnityEvent();
	}

	// Update is called fixed number of times per frame
	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		naturalGravity = m_Rigidbody2D.gravityScale;

		//Set y velocity based on player y location
		animator.SetFloat("yVelocity", m_Rigidbody2D.velocity.y);
		animator.SetFloat("xVelocity", m_Rigidbody2D.velocity.x);

		if (m_Rigidbody2D.velocity.y < -3)
		{
			animator.SetBool("IsFalling", true);
		}

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				animator.SetBool("IsFalling", false);
				animator.SetBool("IsClimbing", false);
				
				if (!wasGrounded)
				{					
					OnLandEvent.Invoke();
					OnFallEvent.Invoke();
					OnClimbEvent.Invoke();
				}					
			}
		}

		
	}

	//Cherries, Diamonds Collected
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Cherries"))
		{
			Debug.Log("Cherry Collected!");
			//anim1.SetBool("IsDestroy",true);		
			Destroy(other.gameObject);
			
		}

		else if (other.gameObject.CompareTag("Diamond"))
		{
			Debug.Log("Diamond Collected!");
			Destroy(other.gameObject);
		}
	}


	
	//Enemy
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Enemy" )
		{
			if (m_Rigidbody2D.velocity.y < 0)
			{
				//Jump on top of the enemy
				Vector2 velocity = m_Rigidbody2D.velocity;
				velocity.y = 17f;
				m_Rigidbody2D.velocity = velocity;
				//kill the enemy
				Destroy(other.gameObject);				
			}
			else
			{
				Vector2 hurtforce = m_Rigidbody2D.velocity;
				hurtforce.x = -15f;
				if (other.gameObject.transform.position.x > transform.position.x)
				{								
					m_Rigidbody2D.velocity = -hurtforce;
				}
				else
				{
					//Enemy on player's right therefore player should be damaged and move left
					m_Rigidbody2D.velocity = hurtforce;
				}
			}
		}
	}

	public void Move(float move, bool crouch, bool jump, bool fall)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch && Mathf.Abs(Input.GetAxis("Vertical")) == 0f)
			{				
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{				
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}

		if (m_Grounded && fall)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}

		if (canClimb)
		{
			animator.SetBool("IsClimbing", true);
			m_Rigidbody2D.drag = 2f;
			m_Rigidbody2D.gravityScale = 0f;
			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;		
			//Set player location on the ladder whenever trigger activated
			transform.position = new Vector3(ladder.transform.position.x, m_Rigidbody2D.position.y);			

			//Press space to skip the animation
			if (Input.GetButtonDown("Jump"))
			{
				canClimb = false;
				m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;				
				m_Rigidbody2D.gravityScale = 3f;
				animator.speed = 1f;				
				m_Rigidbody2D.AddForce(new Vector2(0f, 10f));
				return;
			}


			float vDirection = Input.GetAxis("Vertical");
			//Climb Up
			if (vDirection > .1f && !topLadder)
			{
				m_Rigidbody2D.velocity = new Vector2(0f, climbSpeed * vDirection);
				animator.speed = 1f;
			}
			//Climb Down
			else if (vDirection < .1f && !bottomLadder)
			{
				m_Rigidbody2D.velocity = new Vector2(0f, climbSpeed * vDirection);
				animator.speed = 1f;
			}
			//Stay Still
			else
			{
				animator.speed = 0f;			
				m_Rigidbody2D.velocity = new Vector2(0f, 0f);				
			}

		}

		if (!canClimb)
		{
			//animator.SetBool("IsClimbing", false);
			m_Rigidbody2D.gravityScale = naturalGravity;
		}

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

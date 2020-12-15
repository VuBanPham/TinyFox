using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	public PlayerController controller;
	public Animator animator;	
	public float runSpeed = 40f;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	bool fall = false;
	
	// Update is called once per frame
	void Update () {
		
		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

		animator.SetFloat("Speed", Mathf.Abs(horizontalMove));


		if (Input.GetButtonDown("Jump"))
		{
			jump = true;
			animator.SetBool("IsJumping", true);
		}

		if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
		} else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}

	// Jump Animator 
	public void OnLanding ()
	{
		animator.SetBool("IsJumping", false);
	}

	// Crouch Animator
	public void OnCrouching (bool isCrouching)
	{
		animator.SetBool("IsCrouching", isCrouching);
	}

	public void OnFalling()
	{
		animator.SetBool("IsFalling", false);
	}

	public void OnClimbing()
	{
		animator.SetBool("IsClimbing", false);
	}

	void FixedUpdate ()
	{
		// Move our character		
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, fall);
		jump = false;
		fall = false;	
	}
}

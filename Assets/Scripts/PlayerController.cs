using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float force;
	public bool die;
	public bool respawn;
	public bool controlsInverted;

	public bool grounded;
	public Transform groundCheckPosition;
	public float groundCheckRadius;
	public LayerMask groundCheckLayer;

	public Animator animator;
    public Rigidbody2D rb2d;


	void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
		controlsInverted = false;
		respawn = false;
	}

    // Update is called once per frame
    void Update()
    {
		if (respawn)
		{
			Respawn();
		}
		if(Physics2D.OverlapCircle(groundCheckPosition.position, groundCheckRadius, groundCheckLayer))
		{
			grounded = true;
		}
		else
		{
			grounded= false;
		}

		if (die)
		{
			Die();
			die = false;
		}
		
		Walk();

		if (Input.GetButtonDown("Jump") && grounded)
		{
			Jump();
			Jump();
		}
	}

	public void Walk()
	{
		float horizontalInput = Input.GetAxis("Horizontal");
		float horizontalInputRaw = Input.GetAxisRaw("Horizontal");
		
		if (controlsInverted) 
		{
			horizontalInput *= -1;
			horizontalInputRaw *= -1;
		}

			transform.Translate(horizontalInput * speed * Time.deltaTime, 0, 0);

		if (Input.GetAxisRaw("Horizontal") != 0)
		{
			transform.localScale = new Vector3(horizontalInputRaw, 1, 1);
			animator.SetBool("Walk", true);
		}
		else
		{
			animator.SetBool("Walk", false);
		}
	}

	public void Jump()
	{
		SoundController.soundManager.PlaySound("Jump");
		
		rb2d.linearVelocity = new Vector2(0, force);
		animator.SetTrigger("Jump");
	}

	public void Die()
	{
		animator.SetTrigger("Die"); //This is executing but no visible since reload occurs before. I preferred to enhance the code and took this as an extra task to develop further later.
	}

	public void Respawn()
	{
		float pos = GameObject.Find(GameManager.manager.currentLevel).transform.position.x;
		transform.position = new Vector3(pos, transform.position.y, 0);
		respawn = false;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float idleTime = 6f;
    public float dashSpeed = 50f;
    public float dashDuration = 0.5f;
    public float dashCooldown = 2f;
    
    private float currentSpeed;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private bool m_FacingRight = true;
    private Animator animator;
    private float idleTimer = 0f;
    private bool isIdle = false;
    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer = 0f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentSpeed = walkSpeed; // Start with walk speed
    }

    void Update()
    {
        if (!isDashing)
        {
            change = Vector3.zero;
            change.x = Input.GetAxisRaw("Horizontal");
            change.y = Input.GetAxisRaw("Vertical");
        }

        // Check if Shift button is pressed for running
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetButton("Run"))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        MoveCharacter();
    }

    void MoveCharacter()
    {
        if (!isDashing)
        {
            if (change != Vector3.zero)
            {
                idleTimer = 0f;
                isIdle = false;

                Vector3 normalizedChange = change.normalized;
                myRigidbody.MovePosition(transform.position + normalizedChange * currentSpeed * Time.deltaTime);

                if (currentSpeed == runSpeed)
                {
                    animator.SetBool("Running", true);
                    animator.SetFloat("Speed", 1f);
                }
                else
                {
                    animator.SetBool("Running", false);
                    animator.SetFloat("Speed", 1f);
                }

                if (normalizedChange.x > 0 && !m_FacingRight)
                {
                    Flip();
                }
                else if (normalizedChange.x < 0 && m_FacingRight)
                {
                    Flip();
                }
            }
            else
            {
                idleTimer += Time.deltaTime;
                if (idleTimer >= idleTime && !isIdle)
                {
                    animator.SetTrigger("Waiting");
                    idleTimer = 0;
                    isIdle = true;
                }
                animator.SetBool("Running", false);
                animator.SetFloat("Speed", 0f);
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        dashTimer = 0f;

        // Trigger dash animation
        animator.SetTrigger("Dash");

        // Disable movement during dash
        Vector3 dashDirection = change.normalized;
        change = Vector3.zero;

        while (dashTimer < dashDuration)
        {
            dashTimer += Time.deltaTime;
            myRigidbody.MovePosition(transform.position + dashDirection * dashSpeed * Time.deltaTime);
            yield return null;
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }


    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}

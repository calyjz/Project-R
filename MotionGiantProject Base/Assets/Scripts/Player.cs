using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : AnimatedEntity
{
    public float Speed;
    public Rigidbody2D rb2d;
    private Vector2 movement;
    private Vector2 smoothMovement;
    private Vector2 movementSmoothVelocity;

    private float activeMoveSpeed;
    public float dashSpeed = 15f;

    public float smoothMovementCountdown = 0.06f;
    public float dashLength = 0.3f;
    public float dashCooldown = 1f;

    private float dashCounter;
    private float dashCoolCounter;

    public AudioSource walkingAudioSource;
    public AudioSource dashAudioSource;

    public Sprite idleSprite;

    private Vector2 leftMovement;
    private Vector2 rightMovement;

    // Start is called before the first frame update
    void Start()
    {
        AnimationSetup();
        activeMoveSpeed = Speed;
        walkingAudioSource = GetComponents<AudioSource>()[0];
        dashAudioSource = GetComponents<AudioSource>()[1];

        rightMovement = transform.localScale;
        leftMovement = transform.localScale;
        leftMovement.x *= -1;
    }

    // Update is called once per frame
    void Update()
    {
        //Movement controls
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        smoothMovement = Vector2.SmoothDamp(smoothMovement, movement, ref movementSmoothVelocity, smoothMovementCountdown);

        rb2d.velocity = smoothMovement * activeMoveSpeed;

        if (movement != Vector2.zero)
        {
            if(movement.x > 0)
            {
                transform.localScale = rightMovement;
            } else
            {
                transform.localScale = leftMovement;
            }
            AnimationUpdate();
            Debug.Log("Walking");
        } else
        {
            SpriteRenderer.sprite = idleSprite;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                dashAudioSource.Play();
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if (dashCounter <= 0)
            {
                activeMoveSpeed = Speed;
                dashCoolCounter = dashCooldown;
            }
        }

        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }

        // If the player is moving and the walking sound is not playing, play the sound
        if (movement != Vector2.zero && !walkingAudioSource.isPlaying)
        {
            walkingAudioSource.Play();
        }
        // If the player is not moving and the walking sound is playing, stop the sound
        else if (movement == Vector2.zero && walkingAudioSource.isPlaying)
        {

            walkingAudioSource.Stop();
        }

    }
}

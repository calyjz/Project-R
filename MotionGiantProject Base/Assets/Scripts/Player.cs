using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

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

    public Sprite idleSprite;

    private Vector2 leftMovement;
    private Vector2 rightMovement;

    private Light light;

    //Audio Attributes
    public AudioClip dashingSoundClip;
    public float dashVolume;

    public AudioClip footstep1, footstep2, footstep3, footstep4;
    public float footStepVolume;

    private int oldAnimFrameIndex;
    private AudioClip lastFootstepSound = null;
    private int footstepIndex = 0;

    public AudioMixerGroup walkingMixerGroup;
    public AudioMixerGroup dashMixerGroup;

    // Start is called before the first frame update
    void Start()
    {
        AnimationSetup();
        activeMoveSpeed = Speed;
        rightMovement = transform.localScale;
        leftMovement = transform.localScale;
        leftMovement.x *= -1;
        
        light = GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>();

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
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
                if (movement != Vector2.zero)
                {
                    SoundFXManager.instance.PlayerSoundFXClip(dashingSoundClip, this.transform, dashVolume, dashMixerGroup);
                }
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

        Sprite currentSprite = GetCurrentSprite();
        if (currentSprite.name.ToLower().Contains("walk"))
        {
            int index = getIndex();
            if (index != oldAnimFrameIndex)
            {
                // The frame has changed, so play the sound
                AudioClip randomFootStep = ChooseRandomFootstepSound();
                SoundFXManager.instance.PlayerSoundFXClip(randomFootStep, this.transform, footStepVolume, walkingMixerGroup);
            }
            oldAnimFrameIndex = index;
        }

    }
    // When the player picks up a lantern
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with "+ other.name);
        if (other.tag == "LanternObject")
        {
            
            Destroy(other.gameObject);
            light.Pickup();
        }
    }

    AudioClip ChooseRandomFootstepSound()
    {
        // Add your footstep sounds to this list
        List<AudioClip> footstepSounds = new List<AudioClip>() { this.footstep1, this.footstep2, this.footstep3, this.footstep4 };

        // Remove the last played sound from the list
        if (lastFootstepSound != null)
        {
            footstepSounds.Remove(lastFootstepSound);
        }

        int randomIndex = Random.Range(0, footstepSounds.Count);
        lastFootstepSound = footstepSounds[randomIndex];
        return lastFootstepSound;
    }
}

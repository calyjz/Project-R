using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class Player : AnimatedEntity
{
    [Header("Movement Settings")]
    public float Speed;
    public Rigidbody2D rb2d;
    private Vector2 movement;
    private Vector2 smoothMovement;
    private Vector2 movementSmoothVelocity;
    
    private float activeMoveSpeed;
    public float dashSpeed = 15f;

    public float smoothMovementCountdown = 0.06f;
    public float dashLength = 0.3f;
    private float dashCooldown = GameController.dashCooldown;
    private float attackPower = GameController.attackPower;

    private float dashCounter;
    private float dashCoolCounter;

    public Sprite idleSprite;

    private Vector2 leftMovement;
    private Vector2 rightMovement;

    private Light light;

    [Header("Audio Settings")]
    private int oldAnimFrameIndex;
    private string lastFootstepSound = null;

    // Start is called before the first frame update
    [Header("Attack Settings")]
    public float attackTime;
    public float startTimeAttack;
    // Start is called before the first frame update
    public Transform attackLocation;
    public float attackRange;
    public LayerMask enemies;
    public float scaleAttackRange = 0.2f;

    // Quick fix
    public SpriteRenderer sword;
    public SpriteRenderer swing;

    public Sprite swordSprite;
    public Sprite swingSprite;
    public GameObject swingPivot;


    private Vector2 rangeVector;

    private float leftorRight = -1;


    public Sprite NevHurtSprite;
    public float freezeDuration = 0.4f;
    private float freezeTime = 0f;
    public LayerMask obstacles;

    public int HP = GameController.hp_max;
    void Start()
    {
        AnimationSetup();
        activeMoveSpeed = Speed;
        rightMovement = transform.localScale;
        leftMovement = transform.localScale;
        leftMovement.x *= -1;
        Debug.Log(GameObject.FindGameObjectWithTag("LightObject"));
        light = GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>();

    }


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Vector2 rangeVector2 = new Vector2(Input.GetAxis("Fire2") * scaleAttackRange, Input.GetAxis("Fire1") * scaleAttackRange);

        Gizmos.DrawSphere(new Vector2(attackLocation.position.x, attackLocation.position.y) + rangeVector2, attackRange);
    }
    // Update is called once per frame

    void checkForAttack()
    {
        if(Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            attackTime = -1;
        }
        if (attackTime <= 0)
        {
            swing.sprite = null;
            sword.sprite = swordSprite;
            //Debug.Log("Waiting for fire1");
            if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
            {
                Vector2 rangeVector = new Vector2(Input.GetAxis("Fire2") * scaleAttackRange, Input.GetAxis("Fire1") * scaleAttackRange);
                Collider2D[] damage = Physics2D.OverlapCircleAll(new Vector2(attackLocation.position.x, attackLocation.position.y) + rangeVector, attackRange, enemies);
                Debug.Log(rangeVector);
                float angle = Mathf.Atan2(rangeVector.y * leftorRight, rangeVector.x * leftorRight) * Mathf.Rad2Deg; // #strangebug?
                swingPivot.transform.eulerAngles = new Vector3(0, 0, angle);


                for (int i = 0; i < damage.Length; i++)
                {
                    SoundFXManager.instance.PlaySoundFXClip("MonsterTakesDamage", damage[i].gameObject.transform);
                    Destroy(damage[i].gameObject);
                }
                attackTime = startTimeAttack;
                SoundFXManager.instance.PlaySoundFXClip("AxeSwish", this.transform);
            }

        }
        else
        {
            attackTime -= Time.deltaTime;
            swing.sprite = swingSprite;
            sword.sprite = null;
            //anim.SetBool("Is_attacking", false);
        }
    }
    public void resetMe()
    {
        light = GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>(); // quick fix


        HP = GameController.hp_max;
        light.NotifyChange();
        dashCooldown = GameController.dashCooldown;
        attackPower = GameController.attackPower;
        light.RestartLight();
    }

    public void newRoom()
    {
        light.RestartLight();
    }
    void checkForDamage()
    {
        Collider2D[] damage = Physics2D.OverlapCircleAll(transform.position, attackRange, obstacles);

            if (damage.Length > 0)
            {
            
                for (int i = 0; i < damage.Length; i++)
                {
                    if (attackTime > 0.1f)
                    {
                        Vector2 normal = (transform.position - damage[i].transform.position).normalized;
                        //movingDir = Vector2.Reflect(movingDir, normal);
                        damage[i].gameObject.GetComponent<ShootTowardsPlayer>().deflect(normal);

                    } else {
                    
                        Destroy(damage[i].gameObject);
                        freezeTime = freezeDuration;
                        SpriteRenderer.sprite = NevHurtSprite;
                        HP -= 10;
                        SoundFXManager.instance.PlaySoundFXClip("PlayerOof", this.transform);
                        Debug.Log(HP);
                        if (HP<=0)
                        {
                            SoundFXManager.instance.PlaySoundFXClip("PlayerTakesDamage", this.transform);
                            MusicManager.instance.PlayDeathMusic();
                            GameController.Instance.UpdateGameState(GameState.Respawn);
                        }
                    }
                }
            }
        
    }
    void MovePlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        smoothMovement = Vector2.SmoothDamp(smoothMovement, movement, ref movementSmoothVelocity, smoothMovementCountdown);

        rb2d.velocity = smoothMovement * activeMoveSpeed;

        if (movement != Vector2.zero)
        {
            if (movement.x > 0)
            {
                transform.localScale = rightMovement;
                leftorRight = 1;
            }
            else
            {
                transform.localScale = leftMovement;
                leftorRight = -1;
            }
            AnimationUpdate();
            Debug.Log("Walking");
        }
        else
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
                    SoundFXManager.instance.PlaySoundFXClip("PlayerDash", this.transform);
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

        if (dashCoolCounter < 0)
            dashCoolCounter = 0;

        Sprite currentSprite = GetCurrentSprite();
        if (currentSprite.name.ToLower().Contains("walk"))
        {
            int index = getIndex();
            if (index != oldAnimFrameIndex)
            {
                // The frame has changed, so play the sound
                string randomFootStep = ChooseRandomFootstepSound();
                SoundFXManager.instance.PlaySoundFXClip(randomFootStep, this.transform);
            }
            oldAnimFrameIndex = index;
        }
    }
    void Update()
    {

        checkForAttack();
        checkForDamage();
        
        if (freezeTime <= 0)
        {
            MovePlayer();
        }
        else
        {
            freezeTime -= Time.deltaTime;
        }
        
    }
    // When the player picks up a lantern
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with "+ other.name);
        if (other.tag == "LanternObject")
        {
            SoundFXManager.instance.PlaySoundFXClip("LanternPickup", this.transform);
            Destroy(other.gameObject);
            light.Pickup();
        }
    }

    string ChooseRandomFootstepSound()
    {
        // Add your footstep sounds to this list
        List<string> footstepSounds = new List<string>() { "PlayerFootstep1", "PlayerFootstep2", "PlayerFootstep3", "PlayerFootstep4" };

        // Remove the last played sound from the list
        if (lastFootstepSound != null)
        {
            footstepSounds.Remove(lastFootstepSound);
        }

        int randomIndex = Random.Range(0, footstepSounds.Count);
        lastFootstepSound = footstepSounds[randomIndex];
        return lastFootstepSound;
    }

    
    public float getDashCoolCurrent()
    {
        return (dashCooldown - dashCoolCounter);
    }
}

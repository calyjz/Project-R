//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UIElements;
//using UnityEngine.Audio;

public class Player : AnimatedEntity
{
    // Player movement values
    [Header("Movement Settings")]
    public float Speed;
    public Rigidbody2D rb2d;
    private Vector2 movement;
    private Vector2 smoothMovement;
    private Vector2 movementSmoothVelocity;
    
    // Values which are used for move speed and dash speed
    private float activeMoveSpeed;
    public float dashSpeed = 15f;
    private float dashCounter;
    private float dashCoolCounter;
    [Header("Dash Settings")]
    public float smoothMovementCountdown = 0.06f;
    public float dashLength = 0.3f;

    // Player Stats
    public int hp = GameController.hp_max;
    private float dashCooldown = GameController.dashCooldown;
    private float attackPower = GameController.attackPower;
    private Light lightObject;  // light stats are contained in light script
    
    // Used when determining which way the player must face
    private Vector2 leftFacingDirection;
    private Vector2 rightFacingDirection;
    private int leftorRight = 1;

    // Audio values
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
    
    // For debugging
    private Vector2 rangeVector;

    // Attack cooldown
    public float freezeDuration = 0.4f;
    private float freezeTime = 0f;
    
    public LayerMask obstacles;  // idfk

    void Start()
    {
        AnimationSetup();
        activeMoveSpeed = Speed;
        rightFacingDirection = transform.localScale;
        leftFacingDirection = transform.localScale;
        leftFacingDirection.x *= -1;
        Debug.Log(GameObject.FindGameObjectWithTag("LightObject"));
        lightObject = GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>();
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
                rangeVector = new Vector2(Input.GetAxis("Fire2") * scaleAttackRange, Input.GetAxis("Fire1") * scaleAttackRange);
                Collider2D[] damage = Physics2D.OverlapCircleAll(new Vector2(attackLocation.position.x, attackLocation.position.y) + rangeVector, attackRange, enemies);
                //Debug.Log(rangeVector);
                float angle = Mathf.Atan2(rangeVector.y * leftorRight, rangeVector.x * leftorRight) * Mathf.Rad2Deg; // #strangebug?
                swingPivot.transform.eulerAngles = new Vector3(0, 0, angle);


                for (int i = 0; i < damage.Length; i++)
                {
                    SoundFXManager.instance.PlaySoundFXClip("MonsterTakesDamage", damage[i].gameObject.transform);

                    //call the defeated function from the enemy script  
                    //damage[i].gameObject.GetComponent<Enemy>().RemoveEnemy();
                    //try
                    //{
                        damage[i].gameObject.GetComponent<Enemy>().TakeDamage(35);
                    //}
                    //Destroy(damage[i].gameObject);
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
        lightObject = GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>(); // quick fix


        hp = GameController.hp_max;
        lightObject.NotifyChange();
        dashCooldown = GameController.dashCooldown;
        attackPower = GameController.attackPower;
        lightObject.RestartLight();
    }

    public void newRoom()
    {
        lightObject.RestartLight();
    }
    void checkForDamage()
    {
        Collider2D[] damage = Physics2D.OverlapCircleAll(transform.position, attackRange, obstacles);


        if (damage.Length > 0)
            {
            
                for (int i = 0; i < damage.Length; i++)
                {
                    if (attackTime > 0f)
                    {
                        Vector2 normal = (transform.position - damage[i].transform.position).normalized;
                    //movingDir = Vector2.Reflect(movingDir, normal);
                    if (damage[i].gameObject.GetComponent<ShootTowardsPlayer>().Deflectable)
                    {
                        damage[i].gameObject.GetComponent<ShootTowardsPlayer>().deflect(normal);

                    }
                    else
                    {
                        Destroy(damage[i].gameObject);
                    }

                } else {
                    
                        Destroy(damage[i].gameObject);
                        
                        freezeTime = freezeDuration;
                    //SpriteRenderer.sprite = NevHurtSprite;
                    switchAnimation("hurt");

                    //change nev red when hit for a sec

                    SpriteRenderer.color = Color.red;
                  

                        hp -= 10;
                        SoundFXManager.instance.PlaySoundFXClip("PlayerOof", this.transform);
                        //Debug.Log(HP);

                    if (hp<=0)
                        {
                            SoundFXManager.instance.PlaySoundFXClip("PlayerTakesDamage", this.transform);
                            //MusicManager.instance.PlayDeathMusic();
                            GameController.Instance.UpdateGameState(GameState.Respawn);
                        }
                    }
                }
            }






        //damage = Physics2D.OverlapCircleAll(transform.position, attackRange, obstacles);
        damage = Physics2D.OverlapCircleAll(new Vector2(attackLocation.position.x, attackLocation.position.y) + rangeVector, attackRange, obstacles);

        if (damage.Length > 0)
        {
            for (int i = 0; i < damage.Length; i++)
            {
                if (attackTime > 0f)
                {
                    Vector2 normal = (transform.position - damage[i].transform.position).normalized;
                    //movingDir = Vector2.Reflect(movingDir, normal);
                    if (damage[i].gameObject.GetComponent<ShootTowardsPlayer>().Deflectable)
                    {
                        damage[i].gameObject.GetComponent<ShootTowardsPlayer>().deflect(normal);
                    }
                    else
                    {
                        Destroy(damage[i].gameObject);
                    }
                }
                else
                {

                    
                }
            }
        }
    }
    void MovePlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();

        smoothMovement = Vector2.SmoothDamp(smoothMovement, movement, ref movementSmoothVelocity, smoothMovementCountdown, Mathf.Infinity, Time.deltaTime);

        rb2d.velocity = smoothMovement * activeMoveSpeed;

        if (movement != Vector2.zero)
        {
            switchAnimation("walk");
            if (movement.x > 0)
            {
                transform.localScale = rightFacingDirection;
                leftorRight = 1;
            }
            else
            {
                transform.localScale = leftFacingDirection;
                leftorRight = -1;
            }
            SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            //SpriteRenderer.sprite = idleSprite;
            switchAnimation("idle");

            SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
        }

        // If the player is dashing
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // A check to see if the dash cooldown is regenerated
            if (dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;  // sets the players current move speed to be faster
                GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f,0.5f);
                
                dashCounter = dashLength;  // counter is set to the time the dash is in effect for
                if (movement != Vector2.zero)
                {
                    SoundFXManager.instance.PlaySoundFXClip("PlayerDash", this.transform);
                }
            }
        }

        // If the player is currently dashing
        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            // When the dash time has depleted
            if (dashCounter <= 0)
            {
                activeMoveSpeed = Speed;  // movement speed goes back to normal
                dashCoolCounter = dashCooldown;  // dash cooldown begins
                SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
            }
        }
        
        // If the player has used a dash already and is on cooldown
        if (dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
        
        // For the sake of the UI to not have the counter be less than 0
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
        AnimationUpdate();
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
            other.gameObject.GetComponent<Lantern>().RemoveLantern();

            //Destroy(other.gameObject);
            lightObject.Pickup();
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

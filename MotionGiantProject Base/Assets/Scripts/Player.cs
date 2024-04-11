// using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    public float dashSpeed;
    private float dashCounter;
    private float dashCoolCounter;
    [Header("Dash Settings")]
    private float smoothMovementCountdown = 0.06f;
    public float dashLength;

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
    public float freezeAttack;
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

    private bool damaged;
    private float damageCounter;
    private float damageCooldown = 0.5f;

    public LayerMask obstacles;  // idfk
    [Header("Hit Box")]
    public float hitBoxRange = 0.35f;

    private float laserFreezeDuration = 2f;
    private float laserFreezeTime = -1f;
    void Start()
    {
        AnimationSetup();
        activeMoveSpeed = Speed;
        rightFacingDirection = transform.localScale;
        leftFacingDirection = transform.localScale;
        leftFacingDirection.x *= -1;
        Debug.Log(GameObject.FindGameObjectWithTag("LightObject"));
        lightObject = GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>();
        if (lightObject != null)
        {
            Debug.Log("WHY IS IT FUCKING NULL");
        }
    }


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Vector2 rangeVector2 = new Vector2(Input.GetAxis("Fire2") * scaleAttackRange, Input.GetAxis("Fire1") * scaleAttackRange);

        Gizmos.DrawSphere(new Vector2(attackLocation.position.x, attackLocation.position.y) + rangeVector, attackRange);
    }
    // Update is called once per frame
    void kilThem()
    {
        swing.sprite = swingSprite;
        sword.sprite = null;
        //Debug.Log("Waiting for fire1");
        //if (Input.GetButton("Fire1") || Input.GetButton("Fire2"))
        //{
            Collider2D[] damage = Physics2D.OverlapCircleAll(new Vector2(attackLocation.position.x, attackLocation.position.y) + rangeVector, attackRange, enemies);
            
        float angle = Mathf.Atan2(rangeVector.y * leftorRight, rangeVector.x * leftorRight) * Mathf.Rad2Deg; // #strangebug?
            swingPivot.transform.eulerAngles = new Vector3(0, 0, angle);


            for (int i = 0; i < damage.Length; i++)
            {
                SoundFXManager.instance.PlaySoundFXClip("MonsterTakesDamage", damage[i].gameObject.transform);
                damage[i].gameObject.GetComponent<Enemy>().TakeDamage(attackPower);
            }
        //}
    }
    void checkForAttack()
    {
        if (attackTime > 0)
        {
            kilThem();
            attackTime -= Time.deltaTime;
        }
        else
        {
            if(attackTime< (-freezeAttack))
            {
                if (Input.GetAxis("Fire1") != 0 || Input.GetAxis("Fire2")!=0)
                {
                    rangeVector = new Vector2(Input.GetAxis("Fire2") * scaleAttackRange, Input.GetAxis("Fire1") * scaleAttackRange);
                    print(rangeVector);
                    SoundFXManager.instance.PlaySoundFXClip("AxeSwish", this.transform);
                    attackTime = startTimeAttack;
                }
            } else
            {
                attackTime -= Time.deltaTime;

            }
            swing.sprite = null;
            sword.sprite = swordSprite;
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
        Debug.Log("WOEIRUWIEOURWIOEUR");
        lightObject.GetComponent<Light>().RestartLight();
    }
    void checkForDamage()
    {
        Collider2D[] damage = Physics2D.OverlapCircleAll(transform.position, hitBoxRange, obstacles);
        
        if (damage.Length > 0)
            {
                for (int i = 0; i < damage.Length; i++)
                {
                //    if (attackTime > 0f)
                //    {
                //        Vector2 normal = (transform.position - damage[i].transform.position).normalized;
                //    //movingDir = Vector2.Reflect(movingDir, normal);
                //    if (damage[i].gameObject.GetComponent<ShootTowardsPlayer>().Deflectable)
                //    {
                //        damage[i].gameObject.GetComponent<ShootTowardsPlayer>().deflect(normal);

                //    }
                //    else
                //    {
                //        Destroy(damage[i].gameObject);
                //    }

                //} else {
                        if (GameController.canTakeDamage)
                        {

                            Destroy(damage[i].gameObject);

                            freezeTime = freezeDuration;
                            //SpriteRenderer.sprite = NevHurtSprite;
                            switchAnimation("hurt");

                            //change nev red when hit for a sec

                            SpriteRenderer.color = Color.red;

                            hp -= 10;

                            SoundFXManager.instance.PlaySoundFXClip("PlayerOof", this.transform); 

                            
                            
                            damaged = true;

                            //Debug.Log(HP);
                        }
                    }

                        if (hp<=0)
                        {
                            SoundFXManager.instance.PlaySoundFXClip("PlayerTakesDamage", this.transform);
                            //MusicManager.instance.PlayDeathMusic();
                            GameController.Instance.UpdateGameState(GameState.Respawn);
                        }
                    }
                //}
            
        
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
            }
        }

        if (damaged)
        {
            damageCounter = damageCooldown;
            damaged = false;
        }

        if (damageCounter > 0)
        {
            GameController.canTakeDamage = false;
            damageCounter -= Time.deltaTime;
        }

        if (damageCounter <= 0)
        {
            GameController.canTakeDamage = true;
        }
        
    }

   
    void MovePlayer()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();
        Debug.Log((movement.x,movement.y));

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
            GameController.canTakeDamage = false;
            Physics2D.IgnoreLayerCollision(0, 9, true);

            // When the dash time has depleted
            if (dashCounter <= 0)
            {
                activeMoveSpeed = Speed;  // movement speed goes back to normal
                dashCoolCounter = dashCooldown;  // dash cooldown begins
                SpriteRenderer.color = new Color(1f, 1f, 1f, 1f);
                GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, 1f);
                GameController.canTakeDamage = true;
                Physics2D.IgnoreLayerCollision(0, 9, false);

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

    void checkIFrame()
    {
        if (GameController.canTakeDamage == false)
        {
            Physics2D.IgnoreLayerCollision(0, 9, true);
            Physics2D.IgnoreLayerCollision(0, 13, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(0, 9, false);  
            Physics2D.IgnoreLayerCollision(0, 13, false);
        }
            
    }
    void Update()
    {
        AnimationUpdate();
        checkForAttack();
        checkForDamage();
        checkIFrame();

        if (laserFreezeTime >= 0)
        {
            laserFreezeTime -= Time.deltaTime;
        }
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

        if (other.tag == "Laser")
        {
            
            if (GameController.canTakeDamage && laserFreezeTime<0)
            {
                SpriteRenderer.color = Color.red;
                laserFreezeTime = laserFreezeDuration;
                hp -= 25;
                SpriteRenderer.color = Color.red;
                SoundFXManager.instance.PlaySoundFXClip("PlayerHitByLaser", this.transform);
                damaged = true;
            }
            if (hp<=0)
            {
                SoundFXManager.instance.PlaySoundFXClip("PlayerTakesDamage", this.transform);
                //MusicManager.instance.PlayDeathMusic();
                GameController.Instance.UpdateGameState(GameState.Respawn);
            }
        }

        if (other.tag == "TripleProjectile")
        {
            SoundFXManager.instance.PlaySoundFXClip("PlayerHitByLaser", this.transform);
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

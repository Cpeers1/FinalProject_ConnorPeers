using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shells : MonoBehaviour
{


    public enum ShellType
    {
        Green,
        Blue,
        Red,
        Yellow
    }

    public enum Status
    {
        Moving,
        Idle
    }

    public ShellType shellType;
    public Status status;
    public float shellSpeed;
    public bool movingLeft;
    public Rigidbody2D objectBody;
    public Animator animator;
    public bool grabbed;
    public AudioSource shellAudio;
    public AudioClip shellPivot;
    public AudioClip shellSmack;

    void Animations()
    {
        if(status == Status.Moving)
        {
            animator.Play("Spin");
            if (movingLeft)
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                transform.parent.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        else
        {
            animator.Play("Idle");
        }

    }

	// Use this for initialization
	void Start ()
    {
        gravityScale = objectBody.gravityScale;
        movingLeft = false; //Just some default value. REALLY DOESN'T MATTER YOU KNOW
        grabbed = false;
        damageAble = false;
	}

    void Reset()
    {
        if (grabbed)
        {
            grabbed = false;
            LetGo(GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>());
        }

        gravityScale = objectBody.gravityScale;
        movingLeft = false; //Just some default value. REALLY DOESN'T MATTER YOU KNOW
        status = Status.Idle;
        damageAble = false;
        animator.Play("Idle");

        if (movingLeft)
        {
            transform.parent.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            transform.parent.GetComponent<SpriteRenderer>().flipX = false;
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!damageAble)
        {
            timer += Time.deltaTime;
            if(timer >= timeUntillCanTakeDamage)
            {
                damageAble = true;
                timer = 0f;
            }
        }
        //Debug.Log(objectBody.velocity);
        if(status != Status.Idle)
        {
            if (movingLeft)
            {
                objectBody.velocity = new Vector2(-1 * (shellSpeed * Time.deltaTime), objectBody.velocity.y);
            }
            else
            {
                objectBody.velocity = new Vector2(shellSpeed * Time.deltaTime, objectBody.velocity.y);
            }
        }
        Animations();
	}

    public bool damageAble;
    public float timeUntillCanTakeDamage;
    private float timer;

    void TopHit(PlayerController script)
    {
        if (status == Status.Idle)
        {
            script.Kick();
            status = Status.Moving;
            if (script.gameObject.transform.position.x > transform.position.x) movingLeft = true;
            else if (script.gameObject.transform.position.x <= transform.position.x) movingLeft = false;
            damageAble = false;

            script.Bounce();
        }
        else
        {
            shellAudio.clip = shellSmack;
            shellAudio.Play();

            status = Status.Idle;
            script.Bounce();
        }
    }

    void SideHit(PlayerController script)
    {
        if(status == Status.Idle)
        {
            script.Kick();
            status = Status.Moving;
            if (script.gameObject.transform.position.x > transform.position.x) movingLeft = true;
            else if (script.gameObject.transform.position.x <= transform.position.x) movingLeft = false;
            damageAble = false;


        }
        if(status == Status.Moving)
        {
            if (damageAble)
            {
                script.TakeDamage();
            }

        }

    }

    void InsideHit(PlayerController script)
    {
        if (status == Status.Idle)
        {
            script.Kick();
            status = Status.Moving;
            if (script.gameObject.transform.position.x > transform.position.x) movingLeft = true;
            else if (script.gameObject.transform.position.x <= transform.position.x) movingLeft = false;
            damageAble = false;


        }
        if (status == Status.Moving)
        {
            if (damageAble)
            {
                script.TakeDamage();
            }

        }

    }

    void Collided(Collision2D col)
    {
        Debug.Log("YEAH");
        try {
            if (col.collider.transform.childCount != 0)
            {
                if (col.collider.transform.GetChild(0).name == "ColForOtherObjects")
                {

                    if (col.collider.transform.GetChild(0).GetComponent<Shells>() != null)
                    {
                        Debug.Log("Yo");
                        //We hit another shell, likely.
                        if (status != Status.Idle)
                        {
                            shellAudio.clip = shellSmack;
                            shellAudio.Play();
                            if (col.collider.transform.GetChild(0).GetComponent<Shells>().status == Status.Idle)
                            {
                                col.gameObject.SendMessage("Death");
                                Destroy(col.transform.GetChild(0).gameObject);
                            }
                            else
                            {
                                col.gameObject.SendMessage("Death");
                                Destroy(col.transform.GetChild(0).gameObject);
                                gameObject.transform.parent.SendMessage("Death");
                                Destroy(gameObject);

                            }

                        }
                        else
                        {
                            if (col.transform.GetChild(0).GetComponent<Shells>().status != Status.Idle && status == Status.Idle)
                            {
                                Debug.Log("Shell broken.");

                                transform.parent.SendMessage("Death");
                                if (grabbed)    
                                {
                                    GameObject.FindGameObjectWithTag("Player").SendMessage("ItemInHandDestroyed");
                                }
                                Destroy(gameObject);

                            }

                        }
                    }
                }
                 
            }
            else
            {
                if(status != Status.Idle)
                {
                    //Likely a wall.
                    GetSideHit.HitDirection hitDir = GetSideHit.HitDirection2Test(col);
                    Debug.Log(hitDir);
                    if(hitDir == GetSideHit.HitDirection.Left || hitDir == GetSideHit.HitDirection.Right)
                    {
                        col.gameObject.SendMessage("Activated", this, SendMessageOptions.DontRequireReceiver);
                        if(hitDir == GetSideHit.HitDirection.Left)
                        {
                            movingLeft = false;
                        }
                        else
                        {
                            movingLeft = true;
                        }

                        shellAudio.clip = shellPivot;
                        shellAudio.Play();
                    }
                
                }

            }

        }
        catch (System.Exception e) 
        {
            Debug.LogWarning("Caught a Exception: " + e.Message);
            Debug.LogWarning(e.StackTrace);
        }

        
    }

    private float gravityScale;
    public float gravityInMariosHands;
    public float massInMariosHands;
    private float mass;

    void Grabbed(PlayerController pcontrol)
    {
        mass = objectBody.mass;
        objectBody.gravityScale = gravityInMariosHands;
        objectBody.mass = massInMariosHands;
        gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = false;
        status = Status.Idle;
    }

    void LetGo(PlayerController pcontrol)
    {
        objectBody.gravityScale = gravityScale;
        objectBody.mass = mass;
        gameObject.transform.parent.GetComponent<BoxCollider2D>().enabled = true;

    }


}

using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Mushroom, Flower, Cape, Starman
    }

    private bool movingLeft;
    public bool allowedToMove; 

    public PowerUpType powerUpType;
    public Sprite[] powerUpSprites;
    public float movementSpeed;

    void OnValidate()
    {
        switch (powerUpType)
        {
            case (PowerUpType.Mushroom):
            default:
                if (gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = powerUpSprites[0];
                    gameObject.GetComponent<ParticleSystem>().playOnAwake = false;
                    ParticleSystem.EmissionModule emission = gameObject.GetComponent<ParticleSystem>().emission;
                    emission.enabled = false;
                }
                break;
            case (PowerUpType.Flower):
                if (gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = powerUpSprites[1];
                    gameObject.GetComponent<ParticleSystem>().playOnAwake = false;
                    ParticleSystem.EmissionModule emission = gameObject.GetComponent<ParticleSystem>().emission;
                    emission.enabled = false;
                }
                break;
            case (PowerUpType.Cape):
                if (gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = powerUpSprites[2];
                    gameObject.GetComponent<ParticleSystem>().playOnAwake = false;
                    ParticleSystem.EmissionModule emission = gameObject.GetComponent<ParticleSystem>().emission;
                    emission.enabled = false;
                }
                break;
            case (PowerUpType.Starman):
                if (gameObject.GetComponent<SpriteRenderer>() != null)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = powerUpSprites[3];
                    gameObject.GetComponent<ParticleSystem>().playOnAwake = true;
                    ParticleSystem.EmissionModule emission = gameObject.GetComponent<ParticleSystem>().emission;
                    emission.enabled = true;
                }
                break;

        }
    }

    public Rigidbody2D thisRigidBody;
    private PolygonCollider2D ourCollider;
    private BoxCollider2D ourTriggerCollider;

	// Use this for initialization
	void Start ()
    {
        ourCollider = gameObject.GetComponent<PolygonCollider2D>();
        ourTriggerCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();

        if  (Random.Range(0, 1) == 0)
        {
            movingLeft = true;
        }
        else
        {
            movingLeft = false;
        }
        
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (allowedToMove)
        {
            if(ourCollider.enabled == false)
            {
                ourCollider.enabled = true;
            }

            if(ourTriggerCollider.enabled == false)
            {
                ourTriggerCollider.enabled = true;
            }

            if(powerUpType == PowerUpType.Mushroom || powerUpType == PowerUpType.Starman)
            {
                if (movingLeft)
                {
                    thisRigidBody.velocity = new Vector2(-1 * (movementSpeed * Time.deltaTime), thisRigidBody.velocity.y);
                }
                else
                {
                    thisRigidBody.velocity = new Vector2(1 * (movementSpeed * Time.deltaTime), thisRigidBody.velocity.y);
                }
            }


        }

	
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        GetSideHit.HitDirection hitSide = GetSideHit.HitDirection2Test(col);
        Debug.Log(hitSide);


        if (hitSide == GetSideHit.HitDirection.Left || hitSide == GetSideHit.HitDirection.Right)
        {
            if (movingLeft) movingLeft = false; else movingLeft = true;

        }
        else if (powerUpType == PowerUpType.Starman && hitSide == GetSideHit.HitDirection.Bottom)
        {
            thisRigidBody.velocity = new Vector2(0, 6f);
        }
    }

    void CollectedFromBlock()
    {
        allowedToMove = false;
        thisRigidBody.isKinematic = true;
    }

    void FinishedBlockMovements()
    {
        allowedToMove = true;
        thisRigidBody.isKinematic = false;
    }

    public bool collected = false;

    void Collected()
    {
        collected = true;
    }
}

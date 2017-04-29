using UnityEngine;
using System.Collections;

public class SimpleDeath : MonoBehaviour
{
    public float ForceInitialUp;
    public float TorqueToAdd;

    private bool dead;

    void Start()
    {
        dead = false;
    }

	void Death()
    {
        if (!dead)
        {
            gameObject.layer = 13;
            //13 Is the dead layer.
            gameObject.GetComponent<Rigidbody2D>().freezeRotation = false;
            try { gameObject.GetComponent<Animator>().enabled = false; } catch { }
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, ForceInitialUp), ForceMode2D.Impulse);
            gameObject.GetComponent<Rigidbody2D>().AddTorque(Random.Range(1f, TorqueToAdd), ForceMode2D.Impulse);
            dead = true;
        }

    }

    void OnBecameInvisible()
    {
        if (dead)
        {
            Destroy(gameObject);
        }
    }
}

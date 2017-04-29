using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBlock : MonoBehaviour
{

    public Collider2D Collider
    {
        get
        {
            return GetComponent<Collider2D>();
        }
    }

    public Animator Animator
    {
        get
        {
            return GetComponent<Animator>();
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return GetComponent<AudioSource>();
        }
    }

    public AudioClip blockHitClip;

    public float spinningTime;

    private bool spinning;
    private float timeStamp;

    void Activated()
    {
        if (!spinning)
        {
            Collider.enabled = false;
            Animator.Play("BlockSpin");
            timeStamp = Time.time;

            AudioSource.clip = blockHitClip;
            AudioSource.loop = false;
            AudioSource.Play();
        }
        
        
    }

    void Update()
    {
        if (spinning)
        {
            if(timeStamp + spinningTime >= Time.time)
            {
                Animator.Play("Idle");
                Collider.enabled = true;

                if (GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().bounds.Intersects(Collider.bounds))
                {
                    //Todo: push player outside of us?
                }
            }
        }
    }
}


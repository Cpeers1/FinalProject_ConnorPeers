using UnityEngine;
using System.Collections;

public class QuestionBlock : MonoBehaviour {

    private bool activated;
    private bool hittable;
    private int activatedStage;
    public Animator animator;
    public AudioSource audioSource;
    public float increase;
    public GameObject contains;
    private Vector3 storedPosition;
    private bool delivered;
    public BoxCollider2D refCollider;
    private bool messageSent;
    private GameObject child;

    void OnDrawGizmos()
    {
        if(contains != null)
        {
            Gizmos.DrawIcon(GetComponent<BoxCollider2D>().bounds.center, contains.name + ".png", true);
        }
        else
        {
            Debug.Log("THATIS ?");  
        }
    }

	// Use this for initialization
	void Start ()
    {
        delivered = false;
        messageSent = false;
        hittable = true;
        activated = false;
        activatedStage = 0;
        storedPosition = gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (activated && hittable)
        {
            hittable = false;
            audioSource.Play();

        }
	    if (activated && !PlayerController.IsDead)
        {
            switch (activatedStage)
            {
                case (0):
                    gameObject.transform.position += new Vector3(0, increase);
                    break;
                case (1):
                    gameObject.transform.position -= new Vector3(0, increase);
                    if (delivered && !messageSent)
                    {
                        child.SendMessage("CollectedFromBlock");
                        messageSent = true;
                    }
                    if(contains != null && !delivered)
                    {
                        child = DeliverContents();
                        delivered = true;
                    }

                    break;
                case (2):
                    activated = false;
                    gameObject.transform.position = storedPosition;
                    break;
            }
        }
	}

    void Activated()
    {
        if (hittable)
        {
            activated = true;
            animator.Play("Activated", 0, 0f);
        }

    }

    GameObject DeliverContents()
    {
        GameObject newContents = Instantiate(contains);
        newContents.SendMessage("CollectedFromBlock");
        newContents.transform.position = storedPosition;
        return newContents;
    }

    void TransitionActivatedStage()
    {
        if (activated)
        {

            activatedStage++;
            if(activatedStage > 2)
            {
                activatedStage = 0;
            }
            Debug.Log(activatedStage);
        }
    }
}

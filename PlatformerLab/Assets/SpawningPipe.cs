using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawningPipe : MonoBehaviour
{

    /// <summary>
    /// Set this to the object you want to spawn.
    /// </summary>
    public GameObject prefabToSpawn = null;

    private GameObject[] prefabPool;
    private RigidbodyType2D[] savedOrginalBodyTypes;
    private Coroutine pushingCoroutine;
    private bool donePushing;
    private GameObject pushingOut;

    /// <summary>
    /// How many objects can we spawn at once?
    /// </summary>
    public int spawnLimit = 1;


    public float pipePushSpeed = 0.25f;
    public float delayBetweenSpawn = 2f;
    private float timeStamp;



    /// <summary>
    /// Does the pipe recall items to it when they exit the main camera's bounds? This is kinda important, I reccomend keeping this on.
    /// </summary>
    public bool recallsItems = true;



	// Use this for initialization
	void Start ()
    {
        donePushing = true;
        prefabPool = new GameObject[spawnLimit];
        savedOrginalBodyTypes = new RigidbodyType2D[spawnLimit];

        for (int i = 0; i < spawnLimit; i++)
        {
            prefabPool[i] = Instantiate(prefabToSpawn, GetComponent<Collider2D>().bounds.center, prefabToSpawn.transform.rotation);
            prefabPool[i].SetActive(false);
            GameObject poolItem = prefabPool[i];

            if (poolItem.GetComponent<PowerUp>() != null)
            {
                poolItem.GetComponent<PowerUp>().collected = false;
                poolItem.GetComponent<PowerUp>().enabled = false;
            }

            Rigidbody2D spawnedInstanceBody = prefabPool[i].GetComponent<Rigidbody2D>();

            if (spawnedInstanceBody != null)
            {
                savedOrginalBodyTypes[i] = spawnedInstanceBody.bodyType;
                spawnedInstanceBody.bodyType = RigidbodyType2D.Static;
            }
        }
	}

    void ReAbsorb(GameObject poolItem)
    {
        poolItem.BroadcastMessage("Reset", SendMessageOptions.DontRequireReceiver);
        poolItem.SetActive(false);
        poolItem.transform.position = GetComponent<Collider2D>().bounds.center;
        poolItem.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        if(poolItem.GetComponent<PowerUp>() != null)
        {
            poolItem.GetComponent<PowerUp>().collected = false;
            poolItem.GetComponent<PowerUp>().enabled = false;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(GetComponent<Collider2D>().bounds);
 

        Bounds zFixed = new Bounds(Camera.main.GetComponent<Collider2D>().bounds.center, Camera.main.GetComponent<Collider2D>().bounds.size + new Vector3(0, 0, 100));

        if (zFixed.Intersects(GetComponent<Collider2D>().bounds))
        {
            Debug.Log("Rendered");
            if(prefabPool.Where(X => X!= null).Count(X => !X.activeSelf) > 0)
            {
                if (donePushing)
                {
                    if (timeStamp + delayBetweenSpawn < Time.time)
                    {
                        donePushing = false;
                        pushingOut = prefabPool.First(X => !X.activeSelf);



                        pushingCoroutine = StartCoroutine(PushOutOfPipe());
                    }
                }
            }
        }
        else
        {
            //If we have a active push out of pipe coroutine, stop it.
            if (!donePushing)
            {
                StopCoroutine(pushingCoroutine);
                ReAbsorb(pushingOut);
                pushingOut = null;
                donePushing = true;
            }
        }

        foreach(GameObject go in prefabPool.Where(X => X.activeSelf && X != pushingOut))
        {
            if (!zFixed.Intersects(go.GetComponent<Collider2D>().bounds))
            {
                ReAbsorb(go);
            }
        }

        foreach(GameObject go in prefabPool)
        {
            if(go.GetComponent<PowerUp>() != null)
            {
                if (go.GetComponent<PowerUp>().collected)
                {
                    ReAbsorb(go);
                }
            }
        }
		
	}


    IEnumerator PushOutOfPipe()
    {
        GameObject Object = pushingOut;
        Object.SetActive(true);

        while (GetComponent<Collider2D>().bounds.Intersects(Object.GetComponent<Collider2D>().bounds))
        {
            Object.transform.Translate(transform.TransformDirection(Vector3.up) * pipePushSpeed * Time.deltaTime, Space.Self);
            yield return new WaitForEndOfFrame();
        }

        //One more time.
        Object.transform.Translate(transform.TransformDirection(Vector3.up) * pipePushSpeed * Time.deltaTime, Space.Self);
        Object.GetComponent<Rigidbody2D>().bodyType = savedOrginalBodyTypes[System.Array.IndexOf(prefabPool, Object)];
        donePushing = true;

        if (pushingOut.GetComponent<PowerUp>() != null)
        {
            pushingOut.GetComponent<PowerUp>().enabled = 
                true;
        }

        pushingOut = null;
        timeStamp = Time.time;
    }

   


}


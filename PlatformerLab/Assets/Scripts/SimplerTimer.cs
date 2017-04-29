using UnityEngine;
using System.Collections;

public class SimplerTimer : MonoBehaviour
{
    public float countTo;
    private float time;


	// Use this for initialization
	void Start ()
    {
        time = 0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        time += Time.deltaTime;
        if(time >= countTo)
        {
            Destroy(gameObject);
        }
	}
}

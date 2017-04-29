using UnityEngine;
using System.Collections;

public class SimpleMessageOnCollide : MonoBehaviour
{

    public string Message;
    public enum ColliderType { Trigger, Solid }
    public ColliderType colliderType;


	//// Use this for initialization
	//void Start ()
 //   {
	
	//}
	
	//// Update is called once per frame
	//void Update ()
 //   {
	
	//}

    void OnCollisionEnter2D(Collision2D col)
    {
        if(colliderType == ColliderType.Solid)
        {
            gameObject.transform.GetChild(0).gameObject.SendMessage(Message, col, SendMessageOptions.DontRequireReceiver);
        }
    }   

    void OnTriggerEnter2D(Collider2D col)
    {
        if (colliderType == ColliderType.Trigger)
        {
            gameObject.transform.GetChild(0).gameObject.SendMessage(Message, col, SendMessageOptions.DontRequireReceiver);
        }
    }

    void SideHit(PlayerController script)
    {
        gameObject.transform.GetChild(0).gameObject.SendMessage("SideHit", script, SendMessageOptions.RequireReceiver);
    }

    void Grabbed(PlayerController script)
    {
        gameObject.transform.GetChild(0).gameObject.SendMessage("Grabbed", script, SendMessageOptions.RequireReceiver);
    }

    void LetGo(PlayerController script)
    {
        gameObject.transform.GetChild(0).gameObject.SendMessage("LetGo", script, SendMessageOptions.RequireReceiver);

    }

    private float time;
    public float timeToSpitOut;


    //void OnCollisionStay2D(Collision2D col)
    //{
    //    time += Time.deltaTime;
    //    if(time >= timeToSpitOut)
    //    {
    //        if (!Physics2D.GetIgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), col.collider))
    //        {
    //            Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), col.collider, true);
    //        }
    //    }
    //}

    //void OnCollisionExit2D(Collision2D col)
    //{
    //    time = 0f;
    //    if (Physics2D.GetIgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), col.collider))
    //    {
    //        Physics2D.IgnoreCollision(gameObject.GetComponent<BoxCollider2D>(), col.collider, false);
    //    }
    //}
}

using UnityEngine;
using System.Collections;

public class MushroomQuestionBlockBehaviour : MonoBehaviour
{
    public float YVelocity;
    private float YValueGoal;
    private bool collectingFromBlock = false;


    // Use this for initialization
    void Start()
    {
        YValueGoal = transform.position.y + 1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (collectingFromBlock)
        {
            transform.position += new Vector3(0, YVelocity * Time.deltaTime);
            if (transform.position.y >= YValueGoal)
            {
                collectingFromBlock = false;
            }
        }
        else
        {
            gameObject.SendMessage("FinishedBlockMovements");
            Destroy(this);
        }

    }

    void CollectedFromBlock()
    {
        collectingFromBlock = true;
    }
}

using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour
{
    public GameObject collectEffect;
    private MasterHUD masterHud;

    public float timeFloatUpFromBlock;
    public float yVelocityFloatingUp;

    private bool collectedFromBlock;
    private float timer;

    // Use this for initialization
    void Start()
    {
        timer = 0f;
        collectedFromBlock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (collectedFromBlock)
        {
            timer += Time.deltaTime;
            transform.position += new Vector3(0, yVelocityFloatingUp * Time.deltaTime);
            if(timer >= timeFloatUpFromBlock)
            {
                masterHud = GameObject.FindGameObjectWithTag("MasterHUD").GetComponent<MasterHUD>();
                masterHud.SendMessage("IncrementCoins", 1);

                GameObject newEffect = Instantiate(collectEffect);
                newEffect.transform.position = gameObject.GetComponent<BoxCollider2D>().bounds.center;

                Destroy(gameObject);

            }
        }
        else
        {
            if(!GetComponent<BoxCollider2D>().enabled)
                GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    void Activated()
    {
        masterHud = GameObject.FindGameObjectWithTag("MasterHUD").GetComponent<MasterHUD>();

        masterHud.SendMessage("IncrementCoins", 1);

        GameObject newEffect = Instantiate(collectEffect);
        newEffect.transform.position = gameObject.GetComponent<BoxCollider2D>().bounds.center;

        Destroy(gameObject);

    }

    void CollectedFromBlock()
    {
        collectedFromBlock = true;
        GetComponent<BoxCollider2D>().enabled = false;
        transform.position += new Vector3(0, 0.2f);
    }
}

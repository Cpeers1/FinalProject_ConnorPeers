using UnityEngine;
using System.Collections;

public class PowerUpsTrigger : MonoBehaviour
{

    void Activated()
    {
        transform.parent.SendMessage("Collected");
        transform.parent.gameObject.SetActive(false);
    }
}

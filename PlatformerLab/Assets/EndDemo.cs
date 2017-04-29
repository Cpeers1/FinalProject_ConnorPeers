using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDemo : MonoBehaviour
{

    public MasterHUD hud;

    void OnTriggerEnter2D()
    {
        hud.DemoEnd();
    }

}

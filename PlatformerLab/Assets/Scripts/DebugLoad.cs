using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DebugLoad : MonoBehaviour
{

    public bool DebugLoadToggle;
    private bool DebugLoadToggle2 = false;
    public int SceneIndex;


    // Use this for initialization
    void OnValidate()
    {
        if (Application.isPlaying)
        {
            if (DebugLoadToggle2)
            {
                SceneManager.UnloadScene(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(SceneIndex, LoadSceneMode.Additive);
                DebugLoadToggle2 = false;
            }
            if (DebugLoadToggle)
            {

                SceneManager.LoadScene(0, LoadSceneMode.Additive);
                //SceneManager.LoadScene(0, LoadSceneMode.Additive);
                //SceneManager.LoadScene(SceneIndex, LoadSceneMode.Additive);
                DebugLoadToggle = false;
                //DebugLoadToggle2 = true;
            }
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

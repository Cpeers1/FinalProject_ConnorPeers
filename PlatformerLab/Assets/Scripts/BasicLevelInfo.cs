using UnityEngine;
using System.Collections;

public class BasicLevelInfo : MonoBehaviour
{
    //If its a transitionaryStage, that means its not its own stage, and instead part of another stage.
    /// <summary>
    /// Wether or not the game should treat this stage as a new stage, or part of the previous stage.
    /// </summary>
    public bool transitionaryStage;
    public int startingTime;
    public int sceneBuildIndex;
    public GameObject marioPosition;
    public Sprite backgroundImage;

    public AudioClip levelMusic;
    public float levelMusicVolume = 1f;
    public AudioClip ambience;
    public float ambienceVolume = 1f;

    public GameObject[] transitionStartPositions;

	// Use this for initialization
	public void Start ()
    {
        if (!transitionaryStage)
        {
            MasterHUD masterHud = GameObject.FindGameObjectWithTag("MasterHUD").GetComponent<MasterHUD>();
            if (masterHud != null)
            {
                masterHud.newSession();
                masterHud.time = startingTime;
            }

            //New Feature:
            //Audio Manager Controller Thingy
            MusicAmbienceController mac = MusicAmbienceController.Instance;
            if (levelMusic != null)
            {
                mac.PlayMusic(levelMusic, levelMusicVolume);
            }
            else
            {
                mac.StopMusic();
            }
            if(ambience != null)
            {
                mac.PlayAmbience(ambience, ambienceVolume);
            }
            else
            {
                mac.StopAmbience();
            }

            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            if (playerTransform != null)
            {
                playerTransform.position = marioPosition.transform.position;
                SpriteRenderer background = GameObject.Find("Background").gameObject.GetComponent<SpriteRenderer>();
                if (background != null)
                {
                    background.sprite = backgroundImage;
                }

            }
        }
        else
        {
            //Transitionary stages have 'multiple' entrances. We need to know where they came out of. 
            //This will be done somewhere else...

            int entrance = GameObject.FindObjectOfType<PlayerController>().transitionNumber;

            Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

            if (playerTransform != null)
            {
                if(transform.childCount > entrance)
                {
                    Transform entranceTransform = transform.GetChild(entrance);
                    playerTransform.position = entranceTransform.position; 
                }

                SpriteRenderer background = GameObject.Find("Background").gameObject.GetComponent<SpriteRenderer>();
                if (background != null)
                {
                    background.sprite = backgroundImage;
                }

            }

            //New Feature:
            //Audio Manager Controller Thingy
            MusicAmbienceController mac = MusicAmbienceController.Instance;

            if (levelMusic != null)
            {

                mac.PlayMusic(levelMusic, levelMusicVolume);
            }
            else
            {
                //Leave whats playing, playing.
            }
            if (ambience != null)
            {
                mac.PlayAmbience(ambience, ambienceVolume);
            }
            else
            {
                mac.StopAmbience();
            }

        }

    }
	

}

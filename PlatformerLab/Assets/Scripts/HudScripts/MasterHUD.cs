using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MasterHUD : MonoBehaviour
{
    public enum PowerUpType 
    {
        None,
        Mushroom,
        Flower,
        Cape
    }

    const int ScoreNumMax = 6;
    const int TimeNumMax = 3;
    const int CoinsForLife = 100;
    const int CoinsNumMax = 2;

    public Text livesDigit1;
    public Text livesDigit2;
    public Text coinCounter;
    public Text scoreCounter;
    public Text timeCounter;
    public Image powerUpStoreImage;

    public Sprite[] powerUps;
    public string[] powerUpRepresentativeTags;

    private AudioSource[] audioSources;
    public AudioClip[] audioClips;
    public string[] audioClipsTags;

    public AudioSource MasterMusicChannel;
    public PowerUpType powerUpInStorage;


    public int lives;
    public int coins;
    public int score;
    public int time;

    private float deltaTimer;
    private int liveDigit1Value;
    private int liveDigit2Value;

    private string coinString;
    private string scoreString;
    private string timeString;

    const int StartingLives = 3;
    const int DefaultTime = 400;

    // Use this for initialization
    void Start()
    {
        demoEndPanel.SetActive(false);
        time = DefaultTime;
        lives = StartingLives;
        deltaTimer = time;

        DontDestroyOnLoad(gameObject);

    }

    public void newSession()
    {
        audioSources = new AudioSource[audioClips.Length];
        deltaTimer = time;
    }

    string RecaculateNumbers(int digits, int value)
    {
        string stringToAppend = "";
        int thisValue = value;
        for (int i = 0, h = (int)Mathf.Pow(10, digits - 1), d = digits - 1; i < digits; i++, d--, h = (int)Mathf.Pow(10, d))
        {
            if (h == 0) h = 1;
            //Debug.Log(h);
            stringToAppend += thisValue / h;
            //Debug.Log(thisValue / h);
            thisValue %= h;

        }
        return stringToAppend;
    }

    AudioSource findClip(string Tag)
    {
        AudioSource toReturn = new AudioSource();

        for (int i = 0; i < audioClips.Length && i < audioClipsTags.Length; i++)
        {
            if (audioClipsTags[i] == Tag)
            {
                if(audioSources[i] == null)
                {
                    audioSources[i] = gameObject.AddComponent<AudioSource>();
                    audioSources[i].clip = audioClips[i];
                    audioSources[i].playOnAwake = false;
                }
                toReturn = audioSources[i];
            }
        }

        return toReturn;
    }

    // Update is called once per frame
    void Update()
    {
        //ReaffirmPosition.testForInfringement();

        liveDigit1Value = lives / 10;
        liveDigit2Value = lives % 10;
        livesDigit1.text = "" + liveDigit1Value;
        livesDigit2.text = "" + liveDigit2Value;

        coinString = RecaculateNumbers(CoinsNumMax, coins);
        scoreString = RecaculateNumbers(ScoreNumMax, score);
        timeString = RecaculateNumbers(TimeNumMax, time);

        coinCounter.text = coinString;
        scoreCounter.text = scoreString;
        timeCounter.text = timeString;

        deltaTimer -= Time.deltaTime;
        time = (int)Mathf.Round(deltaTimer);
        if(time == 0)
        {
            //Stub -- Game Over from Time.
        }
    }

    public void IncrementCoins(int coinAmount)
    {
        coins += coinAmount;

        findClip("CollectCoin").Play();

        if(coins >= CoinsForLife)
        {
            coins = 0;
            IncrementLives(1);
        }
    }

    public void IncrementLives(int liveAmount)
    {
        lives += liveAmount;
        if(liveAmount > 0)
        {
            findClip("OneUp").Play();
        }

    }

    public void IncrementScore(int scoreAmount)
    {
        score += scoreAmount;
    }

    void ChangeTime(int newTime)
    {
        time = newTime;
        deltaTimer = time;
    }

    public void DeadEvent()
    {
        IncrementLives(-1);
        MasterMusicChannel.Stop();
        findClip("Death").Play();
    }

    public void Retry()
    {
        Time.timeScale = 1;
        BasicLevelInfo lvlInfo = GameObject.FindObjectOfType<BasicLevelInfo>();
        SceneManager.LoadScene(lvlInfo.sceneBuildIndex, LoadSceneMode.Single);
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        playerController.Start();   
    }

    public GameObject demoEndPanel = null;

    public void DemoEnd()
    {
        Time.timeScale = 0;
        demoEndPanel.SetActive(true);
        
       
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Replay()
    {
        demoEndPanel.SetActive(false);
        Retry();
    }
}

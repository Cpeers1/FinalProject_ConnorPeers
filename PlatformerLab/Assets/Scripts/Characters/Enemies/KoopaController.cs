using UnityEngine;
using System.Collections;

public class KoopaController : MonoBehaviour
{
    public enum KoopaType { Green, Blue, Red, Yellow, DeShelledGreen, DeShelledBlue, DeShelledRed, DeShelledYellow }
    public KoopaType koopaType;

    public float speed;
    public GameObject shell;
    public Animator enemyAnimator;
    public SpriteRenderer enemySpriteRenderer;

    private string AnimationPlaying;


    void OnValidate()
    {
        enemySpriteRenderer = GetComponent<SpriteRenderer>();
    }


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Linq;

[RequireComponent((typeof (Rigidbody2D)),(typeof (SpriteRenderer)),(typeof(Animator)))]
public class PlayerController : MonoBehaviour
{

    public const string GRAB_INPUT_NAME = "Grab",
        JUMP_INPUT_NAME = "Jump",
        SPIN_INPUT_NAME = "Spin Jump";

    
    static public bool IsDead
    {
        get
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                PlayerController playerScript = playerObj.GetComponent<PlayerController>();

                return playerScript.dead;
            }
            else
            {
                return true;
            }
        }
    }

    static public bool TryingToGrab
    {
        get
        {
            return Input.GetButton(GRAB_INPUT_NAME);
        }
    }

    //IEnumerator startCo()
    //{
    //    print("Starting " + Time.time);
    //    yield return StartCoroutine(WaitAndPrint(2.0F));
    //    print("Done " + Time.time);
    //}

    //IEnumerator WaitAndPrint(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //    JustLetGo = false;
    //}

    public enum Direction
    {
        Left,
        Right,
        Still
    }

    public enum PowerUpStatus
    {
        Small,
        Large,
        Fire,
        Flying
    }

    public enum StarPower
    {
        None,
        StarPowered
    }

    [System.Serializable]
    public struct DirectionStats
    {
        public Direction facing;
        public Direction moving;
    }


    public GameObject mainCamera;
    private Animator animator;
    private Rigidbody2D playerBody;
    private SpriteRenderer spriteRenderer;
    public BoxCollider2D[] colliders;
    public RelativeJoint2D ourJoint;
    public AudioClip[] sfx;
    public AudioSource audioSource;
    //public Transform physhics2Dbody;

    internal int transitionNumber = 0;

    public PowerUpStatus powerUpStatus;
    public StarPower starPowerStatus;
    public DirectionStats directionStats;
    //public DirectionFacing directionFacing;
    //public DirectionFacing directionMoving;
    private Vector3 previousPosition;
    //private Vector2 currentVelocity;
    public Vector2 maxChosenVelocity;
    //private Vector2 previousVelocity;
    //public AnimationStatus[] animationStati;

    public MarioStats defaultMarioStats;

    public MarioStats stats
    {
        get;
        set;
    }

    #region Accessors

    public float speed
    {
        get
        {
            return stats.speed;
        }
        set
        {
            stats.speed = value;
        }
    }

    public float runSpeed
    {
        get
        {
            return stats.runSpeed;
        }
        set
        {
            stats.runSpeed = value; 
        }
    }


    public float movementForce
    {
        get
        {
            return stats.movementForce;
        }
        set
        {
            stats.movementForce = value;
        }
    }

    public float runModifier
    {
        get
        {
            return stats.runModifier;
        }
        set
        {
            stats.runModifier = value;
        }
    }

    private float previousMovement
    {
        get
        {
            return stats.previousMovement;
        }
        set
        {
            stats.previousMovement = value;
        }
    }

    private float jumpTime
    {
        get
        {
            return stats.jumpTime;
        }
        set
        {
            stats.jumpTime = value;
        }
    }

    public float chosenJumpTime
    {
        get
        {
            return stats.chosenJumpTime;
        }
        set
        {
            stats.chosenJumpTime = value;
        }
    }

    private float ourJointX
    {
        get
        {
            return stats.ourJointX;
        }
        set
        {
            stats.ourJointX = value;
        }
    }

    public float velocityModifierRun
    {   
        get
        {
            return stats.velocityModifierRun;
        }
        set
        {
            stats.velocityModifierRun = value;
        }
    }

    public float velocityModifierWalk
    {
        get
        {
            return stats.velocityModifierWalk;
        }
        set
        {
            stats.velocityModifierWalk = value;
        }
    }

    public float jumpSpeed
    {
        get
        {
            return stats.jumpSpeed;
        }
        set
        {
            stats.jumpSpeed = value;
        }
    }

    public float jumpForce
    {
        get
        {
            return stats.jumpForce;
        }
        set
        {
            stats.jumpForce = value;
        }
    }

    public float runAnimationModifier
    {
        get
        {
            return stats.runAnimationModifier;
        }
        set
        {
            stats.runAnimationModifier = value;
        }
    }

    private float animatorSpeed
    {
        get
        {
            return stats.animatorSpeed;
        }
        set
        {
            stats.animatorSpeed = value;
        }
    }

    //private float privateSpeed;
    private float damageTime
    {
        get
        {
            return stats.damageTime;
        }
        set
        {
            stats.damageTime = value;
        }
    }

    public float damageArmourLasting
    {
        get
        {
            return stats.damageArmourLasting;
        }
        set
        {
            stats.damageArmourLasting = value;
        }
    }

    public float damageArmourFlashIntervals
    {
        get
        {
            return stats.damageArmourFlashIntervals;
        }
        set
        {
            stats.damageArmourFlashIntervals = value;
        }
    }

    private float damageArmourFlashes
    {
        get
        {
            return stats.damageArmourFlashes;
        }
        set
        {
            stats.damageArmourFlashes = value;
        }
    }


    public float deathDelay
    {
        get
        {
            return stats.deathDelay;
        }
        set
        {
            stats.deathDelay = value;
        }
    }

    private float deathDelayTimer
    {
        get
        {
            return stats.deathDelayTimer;
        }
        set
        {
            stats.deathDelayTimer = value;
        }
    }

    public float deathForceApplication
    {
        get
        {
            return stats.deathForceApplication;
        }
        set
        {
            stats.deathDelayTimer = value;
        }
    }

    public float timeToStartOver
    {
        get
        {
            return stats.timeToStartOver;
        }
        set
        {
            stats.timeToStartOver = value;
        }
    }

    private float startOverTime
    {
        get
        {
            return stats.startOverTime;
        }
        set
        {
            stats.startOverTime = value;
        }
    }

    public bool dead
    {
        get
        {
            return stats.dead;
        }
        set
        {
            stats.dead = value;
        }
    }

    private bool forceApplied
    {
        get
        {
            return stats.forceApplied;
        }
        set
        {
            stats.forceApplied = value;
        }
    }

    #endregion

    public int animatorLayer;

    //public string[] AnimationPriorities;
    private string animationPlaying;

    private bool hitCap;
    private bool previousHitCap;
    private bool lookingUp;
    private bool grabbing;
    private bool ducking;
    private bool jumpAvaliable;
    private bool jumping;
    private bool kicking;
    public bool damageArmour;
    public bool busy = false;


    //Freezing Related
    private List<Rigidbody2D> rigidBodiesFreezeList;
    private Animator[] animatorFreezeList;
    private float[] animatorSpeedValuesForFrozen;

    [System.Serializable]
    public struct LookUpCameraSettings
    {
        public float speed;
        public float maxYChange;
        public float maxY;
        public bool needsToReturn;
        public float bottomY;
        public float distanceFromUs;
    }

    public LookUpCameraSettings lookUpCameraSettings;

    private bool sleepOtherAnimations;
    //private float oldSleepLinear, oldSleepAngular, oldSleepTime;

    void OnValidate()
    {
        //Stub - Validation
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (directionStats.facing)
        {
            case (Direction.Left):
                spriteRenderer.flipX = true;
                break;

            case (Direction.Right):
                spriteRenderer.flipX = false;
                break;
        }

    }


    private void SceneLoaded(Scene scene, LoadSceneMode loadedStyle)
    {
        FreezeUnfreeze(false);
    }

    private void OnLevelWasLoaded(int level)
    {
        FreezeUnfreeze(false);
    }

    public bool CheckForUpInputObjects()
    {
        if (!jumpAvaliable) return false;

        Collider2D[] possibleOpenables = Physics2D.OverlapBoxAll(GetOurCollider().bounds.center, GetOurCollider().bounds.size, 0);

        List<Component> interacatables = possibleOpenables.Where(X => X.GetComponent<TransitionaryDoor>() != null).Select(X => X.GetComponent<TransitionaryDoor>()).Cast<Component>().ToList();
        //add any other interactables here


        if(interacatables.Count > 0)
        {
            //Sort by distance.
            interacatables.Sort((X, Y) => Vector2.Distance(transform.position, Y.transform.position).CompareTo(Vector2.Distance(transform.position, X.transform.position)));

            //Ok...
            Component ToInteractWith = interacatables[0];

            switch (ToInteractWith.GetType().ToString())
            {
                case "TransitionaryDoor":
                    TransitionaryDoor casted = (TransitionaryDoor)ToInteractWith;
                    transitionNumber = casted.transitionStartPosIndex;
                    List<Animator> todisclude = new List<Animator>();
                    todisclude.Add(casted.gameObject.GetComponent<Animator>());
                    FreezeUnfreeze(true, null, todisclude);
                    casted.Open(this);
                    busy = true;
                    break;
            }

            return true;
        }
        else
        {
            return false;
        }
    } 



    // Use this for initialization
    public void Start()
    {
        //Don't destroy on load.
        //Try catch is to prevent complaining about it being already set to this.
        try
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(Camera.main.gameObject);
        } catch(Exception e) { Debug.LogWarning(e.Message, gameObject); }


        stats = Instantiate<MarioStats>(defaultMarioStats);

        //StartCoroutine(startCo());    
        animator = GetComponent<Animator>();
        playerBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (animator == null || playerBody == null || spriteRenderer == null)
        {
            this.enabled = false;
            string errorString = "Missing: ";
            if(animator == null) { errorString += "Animator, "; } 
            if(playerBody == null) { errorString += "RigidBody2D, "; }
            if(spriteRenderer == null) { errorString += "SpriteRenderer, "; }
            Debug.LogError(errorString);
        }



        //animationStatuses = new bool[AnimationPriorities.Length];
        //for(int i = 0; i < animationStatuses.Length; i++) animationStatuses[i] = false;
        //oldSleepThreshhold = Physics2.sleepThreshold;
        //oldSleepTime = Physics2D.timeToSleep;
        //oldSleepLinear = Physics2D.linearSleepTolerance;
        //oldSleepAngular = Physics2D.angularSleepTolerance;

        ChangeLayer(animatorLayer);
        SetOurCollider();

        jumpAvaliable = false;
        jumping = false;
        ducking = false;
        lookingUp = false;
        kicking = false;
        jumpAvaliable = false;
        hitCap = false;
        sleepOtherAnimations = false;
        lookUpCameraSettings.needsToReturn = false;
        grabbing = false;
        dead = false;
        deathDelayTimer = 0f;
        startOverTime = 0f;
        forceApplied = false;
        JustLetGo = false;
        objectToDeReg = null;

        ourJointX = Mathf.Abs(ourJoint.linearOffset.x);
        //previousVelocity = Vector2.zero;
        //currentVelocity = Vector2.zero;
        animatorSpeed = 0f;
        animationPlaying = "Idle";

        previousHitCap = hitCap;
        previousMovement = 0f;
        //privateSpeed = speed;
        //SceneManager.LoadScene(1, LoadSceneMode.Additive);

        lookUpCameraSettings.bottomY = mainCamera.transform.position.y;
        lookUpCameraSettings.maxY = mainCamera.transform.position.y + lookUpCameraSettings.maxYChange;
        lookUpCameraSettings.distanceFromUs = mainCamera.transform.position.y - transform.position.y;

        mainCamera.transform.position = new Vector3(0, 3.5f, -10);

	}

    BoxCollider2D GetOurCollider()
    {
        if (!ducking)
        {

            if (powerUpStatus == PowerUpStatus.Small)
            {
                return colliders[0];
            }
            else
            {
                return colliders[1];
            }
        }
        else
        {
            return colliders[2];
        }
    }

    void SetOurCollider()
    {
        if (!ducking)
        {
            colliders[2].enabled = false;

            if(powerUpStatus == PowerUpStatus.Small)
            {
                colliders[0].enabled = true;
                colliders[1].enabled = false;
            }
            else
            {
                colliders[1].enabled = true;
                colliders[0].enabled = false;
            }
        }
        else
        {
            colliders[1].enabled = false;
            colliders[0].enabled = false;
            colliders[2].enabled = true;
        }

    }

    void ChangeLayer(int newLayer)
    {
        //Animation layer can't have its layer changed, but I don't care.
        //Basically, its just to swap between layers.
        try
        {
            animator.SetLayerWeight(animatorLayer, 0f);
        }   catch { Debug.Log("Oops. Don't worry about this, Animation Layer 0 can not have its weighting changed."); }
        try
        {
            animator.SetLayerWeight(newLayer, 0.51f);
        }
        catch { Debug.Log("Oops. Don't worry about this, Animation Layer 0 can not have its weighting changed."); }


        animatorLayer = newLayer;   
    }

    //New Addition - Controls ducking and looking up. Ducking has priority over jump (Mario can jump while ducking), but looking up can only be done while standing still.
    //Ducking changes our colliders size.
    void DuckingAndLookingUp()
    {
        if (!sleepOtherAnimations)
        {
            float lookingStatus = Input.GetAxis("Vertical");

            if
                (
                (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                ^ //XOR - One or the other, not both, or not neither. <3 Xor 
                (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                )
            {
                if (lookingStatus < 0) //Ducking
                {
                    if(lookingUp) { lookingUp = false; }
                    ducking = true;
                    if (!grabbing)
                    {
                        if(animationPlaying != "Duck")
                        {
                            animationPlaying = "Duck";
                        }
                    }
                    else
                    {
                        if(animationPlaying != "CarryingDuck")
                        {
                            animationPlaying = "CarryingDuck"; //No, thats not carrying A duck, thats carrying WHILE ducking, okay? >o>
                        }
                    }

                    SetOurCollider();
                    
                }
                else if(lookingStatus > 0)
                {
                    if (ducking) { ducking = false; SetOurCollider(); }
                    if (!jumping && jumpAvaliable && !ducking && playerBody.velocity == Vector2.zero && !grabbing)
                    {
                        lookingUp = true;
                        mainCamera.GetComponent<DistanceJoint2D>().distance = 10f;
                        mainCamera.GetComponent<Rigidbody2D>().simulated = false;
                        if (animationPlaying != "LookingUp")
                        {
                            animationPlaying = "LookingUp";
                        }
                        if (!CheckForUpInputObjects())
                        {
                            //STUB: Camera Movements
                            if (lookingStatus == 1)
                            {
                                lookUpCameraSettings.needsToReturn = true;
                                if (mainCamera.transform.position.y > lookUpCameraSettings.maxY)
                                {
                                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, lookUpCameraSettings.maxY, mainCamera.transform.position.z);
                                }
                                if (mainCamera.transform.position.y < lookUpCameraSettings.maxY)
                                {
                                    mainCamera.transform.Translate(new Vector3(0, lookUpCameraSettings.speed * Time.deltaTime), Space.World);

                                }
                            }
                        }
                       

                    }

                    

                }
            }
            else if (ducking)
            {
                ducking = false;
                SetOurCollider();
            }
            else if (lookingUp)
            {
                lookingUp = false;
                mainCamera.GetComponent<Rigidbody2D>().simulated = true;
                mainCamera.GetComponent<DistanceJoint2D>().distance = 0.005f;
            }
                
            if(!lookingUp && lookUpCameraSettings.needsToReturn)
            {


                mainCamera.transform.Translate(new Vector3(0, -1 * (lookUpCameraSettings.speed * Time.deltaTime)), Space.World);
                if(mainCamera.transform.position.y <= lookUpCameraSettings.bottomY)
                {
                    mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, lookUpCameraSettings.bottomY, mainCamera.transform.position.z);
                    lookUpCameraSettings.needsToReturn = false;
                }
            }
            
        }
        else if (lookUpCameraSettings.needsToReturn)
        {
            mainCamera.transform.Translate(new Vector3(0, -1 * (lookUpCameraSettings.speed * Time.deltaTime)), Space.World);
            if (mainCamera.transform.position.y <= lookUpCameraSettings.bottomY)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, lookUpCameraSettings.bottomY, mainCamera.transform.position.z);
                lookUpCameraSettings.needsToReturn = false;
            }
        }

    }   

    void Animations()
    {
        animator.speed = animatorSpeed;
        switch (directionStats.facing)
        {
            case (Direction.Left):
                spriteRenderer.flipX = true;
                if(ourJoint.linearOffset.x != ourJointX * -1)
                {
                    ourJoint.linearOffset = new Vector2(ourJointX * -1, ourJoint.linearOffset.y);
                }
                //try
                //{
                //    if(grabbedJoint.linearOffset.x != ourJointX)
                //    {
                //        grabbedJoint.linearOffset = new Vector2(ourJointX, grabbedJoint.linearOffset.y);
                //        //if (grabbing)
                //        //{
                //        //    transform.position += new Vector3(1, 0);
                //        //}
                //    }
                //}
                //catch (System.Exception e)
                //{
                //    Debug.LogException(e);
                //}
                break;

            case (Direction.Right):
                spriteRenderer.flipX = false;
                if(ourJoint.linearOffset.x != ourJointX)
                {
                    ourJoint.linearOffset = new Vector2(ourJointX, ourJoint.linearOffset.y);
                }
                //try
                //{
                //    if (grabbedJoint.linearOffset.x != ourJointX * -1)
                //    {
                //        grabbedJoint.linearOffset = new Vector2(ourJointX * -1, grabbedJoint.linearOffset.y);
                //        //if (grabbing)
                //        //{
                //        //    transform.position += new Vector3(1, 0);
                //        //}
                //    }
                //}
                //catch (System.Exception e)
                //{
                //    Debug.LogException(e);
                //}
                break;
        }

        //int animationIndexToPlay = -1;
        //for (int done = 0, i = AnimationPriorities.Length - 1; i > 0 || done == 1; i--)
        //{
        //    if (animationStati[i].animationBool = true && animation)
        //}
        if (kicking)
        {
            if (!animator.GetCurrentAnimatorStateInfo(animatorLayer).IsName("Kicking"))
            {
                if(powerUpStatus == PowerUpStatus.Small)
                {
                    animator.Play("Kicking", 0);
                }
                if (powerUpStatus == PowerUpStatus.Large)
                {
                    animator.Play("Kicking", 1);
                }
            }

        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(animatorLayer).IsName(animationPlaying))
            {
                animator.Play(animationPlaying, animatorLayer, 0f);
            }
        }

        Debug.Log(animationPlaying);

    }

    private float starManTimeStamp = 0;

    //TODO: Make it so that the star man can tell mario how much time it should be active for?
    /// <summary>
    /// Change this to change the amount of time mario gets a starman for.
    /// </summary>
    private const float StarManTime = 15f;

    public AudioClip starManMusic;
    public float starManVolume = 1f;

    /// <summary>
    /// This makes mario be set to star man status, which affects a lot of things, including:
    /// Speed (Running / Walking)
    /// Jumping (Higher)
    /// Damageable (Not damageable, can die to lava, but not to enemies.)
    /// </summary>
    private bool marioStarMan = false;

    public GameObject starManParticles;


    IEnumerator StarMan()
    {
        starManParticles.SetActive(true);
        marioStarMan = true;
        MusicAmbienceController mac = MusicAmbienceController.Instance;
        starManTimeStamp = Time.time;
        mac.PlayMusicTemporarly(starManMusic, starManVolume);
        //TODO: Star Man Particles?
        while(Time.time < starManTimeStamp + StarManTime)
        {
            spriteRenderer.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            yield return new WaitForEndOfFrame();
        }
        spriteRenderer.color = Color.white;
        marioStarMan = false;
        mac.DonePlayingMusicTemporarly();
        starManTimeStamp = 0f;
        starMan = null;
        starManParticles.SetActive(false);
    }

    public Coroutine starMan = null;

    public void CollectedStarMan()
    {
        if(starMan != null)
        {
            StopCoroutine(starMan);
        }

        starMan = StartCoroutine(StarMan());
    }

    void CollectedMushroom(Rigidbody2D mushroomsBody)
    {
        animatorSpeed = 1;

        if(powerUpStatus == PowerUpStatus.Small)
        {
            //playerBody.isKinematic = true;

            //Physics2D.linearSleepTolerance = 999999f;
            //Physics2D.angularSleepTolerance = 999999f;
            //Physics2D.timeToSleep = 0f;
            FreezeUnfreeze(true, mushroomsBody);

            animationPlaying = "PowerUp-Mushroom";
            sleepOtherAnimations = true;
            audioSource.clip = sfx[5];
            audioSource.Play();
        }
    }

    void FinishedCollectingMushroom()
    {
        //playerBody.isKinematic = false;
        sleepOtherAnimations = false;

        //Physics2D.linearSleepTolerance = oldSleepLinear;
        //Physics2D.angularSleepTolerance = oldSleepAngular;
        //Physics2D.timeToSleep = oldSleepTime;
        FreezeUnfreeze(false);

        powerUpStatus = PowerUpStatus.Large;
        ChangeLayer(1);
        SetOurCollider();
    }

    public void FreezeUnfreeze(bool freeze)
    {
        if (freeze)
        {
            Rigidbody2D[] rigidTestList = GameObject.FindObjectsOfType<Rigidbody2D>();
            rigidBodiesFreezeList = new List<Rigidbody2D>();

            for(int i = 0; i < rigidTestList.Length; i++)
            {
                if(rigidTestList[i].bodyType != RigidbodyType2D.Static)
                {
                    rigidBodiesFreezeList.Add(rigidTestList[i]);
                }

            }

            Animator[] testList = GameObject.FindObjectsOfType<Animator>();
            animatorFreezeList = new Animator[testList.Length - 1];
            bool foundOurAnimator = false;

            for(int i = 0; i < testList.Length; i++)
            {
                if(testList[i] != animator)
                {
                    if (!foundOurAnimator)
                        animatorFreezeList[i] = testList[i];
                    else
                        animatorFreezeList[i - 1] = testList[i];
                }
                else
                {
                    foundOurAnimator = true;
                }
            }

            foreach(Rigidbody2D rg2d in rigidBodiesFreezeList)
            {
                rg2d.bodyType = RigidbodyType2D.Static;
            }
            animatorSpeedValuesForFrozen = new float[animatorFreezeList.Length];

            for(int i = 0; i < animatorFreezeList.Length; i++)
            {
                animatorSpeedValuesForFrozen[i] = animatorFreezeList[i].speed;
                animatorFreezeList[i].speed = 0;
                animatorFreezeList[i].enabled = false;
            }
        }
        else
        {
            try
            {
                foreach (Rigidbody2D rg2d in rigidBodiesFreezeList)
                {
                    try
                    {
                        rg2d.bodyType = RigidbodyType2D.Dynamic;
                    }
                    catch
                    {
                        //STUB      
                    }
                }

                for (int i = 0; i < animatorFreezeList.Length; i++)
                {
                    try
                    {
                        animatorFreezeList[i].speed = animatorSpeedValuesForFrozen[i];
                        animatorFreezeList[i].enabled = true;
                    }
                    catch
                    {
                        //STUB
                    }
                }
            }
            catch(System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    public void FreezeUnfreeze(bool freeze, Rigidbody2D optionalBodyToDisclude, List<Animator> toDisclude = null)
    {
        if (freeze)
        {
            Rigidbody2D[] rigidTestList = GameObject.FindObjectsOfType<Rigidbody2D>();
            rigidBodiesFreezeList = new List<Rigidbody2D>();

            for (int i = 0; i < rigidTestList.Length; i++)
            {
                if (rigidTestList[i].bodyType != RigidbodyType2D.Static && rigidTestList[i] != optionalBodyToDisclude)
                {
                    rigidBodiesFreezeList.Add(rigidTestList[i]);
                }

            }

            Animator[] testList = GameObject.FindObjectsOfType<Animator>();
            animatorFreezeList = new Animator[testList.Length - 1];
            bool foundOurAnimator = false;

            for (int i = 0; i < testList.Length; i++)
            {
                if (testList[i] != animator)
                {
                    if (!foundOurAnimator)
                        animatorFreezeList[i] = testList[i];
                    else
                        animatorFreezeList[i - 1] = testList[i];
                }
                else
                {
                    foundOurAnimator = true;
                }
            }

            foreach (Rigidbody2D rg2d in rigidBodiesFreezeList)
            {
                rg2d.bodyType = RigidbodyType2D.Static;
            }

            animatorSpeedValuesForFrozen = new float[animatorFreezeList.Length];

            for (int i = 0; i < animatorFreezeList.Length; i++)
            {
                animatorSpeedValuesForFrozen[i] = animatorFreezeList[i].speed;
                if (toDisclude == null)
                {
                    animatorFreezeList[i].speed = 0;
                    animatorFreezeList[i].enabled = false;
                }
                else
                {
                    if (!toDisclude.Contains(animatorFreezeList[i]))
                    {
                        animatorFreezeList[i].speed = 0;
                        animatorFreezeList[i].enabled = false;
                    }
                }
            }
        }
        else
        {
            try
            {
                foreach (Rigidbody2D rg2d in rigidBodiesFreezeList)
                {
                    try
                    {
                        rg2d.bodyType = RigidbodyType2D.Dynamic;
                    }
                    catch
                    {
                        //STUB      
                    }

                }

                for (int i = 0; i < animatorFreezeList.Length; i++)
                {
                    try
                    {
                    animatorFreezeList[i].speed = animatorSpeedValuesForFrozen[i];
                    animatorFreezeList[i].enabled = true;
                    }
                    catch
                    {
                        //STUB
                    }

                }
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
        }
    }

    void Movements()
    {
        if (!sleepOtherAnimations)
        {

            //currentVelocity = playerBody.velocity;

            float movement = Input.GetAxis("Horizontal");

            if (movement != 0 && ((ducking & !jumpAvaliable) || (!ducking)))
            {
                animatorSpeed = playerBody.velocity.magnitude * 0.16f;

                if (movement < previousMovement && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
                {
                    directionStats.facing = Direction.Left;
                    directionStats.moving = Direction.Left;
                }
                else
                if (movement > previousMovement && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
                {
                    directionStats.facing = Direction.Right;
                    directionStats.moving = Direction.Right;
                }

                if (!marioStarMan)
                {
                    #region nonStarManMovement
                    if (!Input.GetKey(KeyCode.Q))
                    {
                        playerBody.velocity += velocityModifierWalk * new Vector2(speed * movement * Time.deltaTime, 0);

                        if (Mathf.Abs(playerBody.velocity.x) > maxChosenVelocity.x)
                        {
                            if (playerBody.velocity.x > 0)
                            {
                                playerBody.velocity -= new Vector2((maxChosenVelocity.x * 0.1f), 0);

                            }
                            if (playerBody.velocity.x < 0)
                            {
                                playerBody.velocity -= new Vector2(-1 * (maxChosenVelocity.x * 0.1f), 0);
                            }
                        }

                    }
                    else
                    {


                        playerBody.velocity += velocityModifierRun * new Vector2(runSpeed * movement * Time.deltaTime, 0);

                        if (Mathf.Abs(playerBody.velocity.x) > maxChosenVelocity.x * runModifier)
                        {
                            if (playerBody.velocity.x > 0)
                            {
                                playerBody.velocity = new Vector3(maxChosenVelocity.x * runModifier, playerBody.velocity.y);
                            }
                            if (playerBody.velocity.x < 0)
                            {
                                playerBody.velocity = new Vector3(-1 * maxChosenVelocity.x * runModifier, playerBody.velocity.y);
                            }
                        }


                    }
                    if (Mathf.Abs(playerBody.velocity.x) >= maxChosenVelocity.x * runAnimationModifier)
                    {
                        switch (directionStats.moving)
                        {
                            case (Direction.Left):
                                if (playerBody.velocity.x > maxChosenVelocity.x * runAnimationModifier && !grabbing)
                                {
                                    animationPlaying = "Skid";
                                }
                                else
                                {
                                    if (!grabbing)
                                    {
                                        animationPlaying = "Run";
                                    }
                                    else
                                    {
                                        if (!jumping && jumpAvaliable)
                                        {
                                            animationPlaying = "CarryingMovement";
                                        }

                                    }
                                }
                                break;
                            case (Direction.Right):
                                if (playerBody.velocity.x < -1 * maxChosenVelocity.x * runAnimationModifier && !grabbing)
                                {
                                    animationPlaying = "Skid";
                                }
                                else
                                {
                                    if (!grabbing)
                                    {
                                        animationPlaying = "Run";
                                    }
                                    else
                                    {
                                        if (!jumping && jumpAvaliable)
                                        {
                                            animationPlaying = "CarryingMovement";
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (!grabbing)
                        {
                            animationPlaying = "Walk";
                        }
                        else
                        {
                            if (!jumping && jumpAvaliable)
                            {
                                animationPlaying = "CarryingMovement";
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region StarManMovement

                    playerBody.velocity += stats.velocityModifierStarMan * new Vector2(stats.starManSpeed * movement * Time.deltaTime, 0);

                    if (Mathf.Abs(playerBody.velocity.x) > maxChosenVelocity.x * stats.starManModifier)
                    {
                        if (playerBody.velocity.x > 0)
                        {
                            playerBody.velocity -= new Vector2((maxChosenVelocity.x * stats.starManModifier), 0);

                        }
                        if (playerBody.velocity.x < 0)
                        {
                            playerBody.velocity -= new Vector2(-1 * (maxChosenVelocity.x * stats.starManModifier), 0);
                        }
                    }

                    if (Mathf.Abs(playerBody.velocity.x) >= maxChosenVelocity.x * stats.starManAnimationModifier)
                    {
                        switch (directionStats.moving)
                        {
                            case (Direction.Left):
                                if (playerBody.velocity.x > maxChosenVelocity.x * stats.starManAnimationModifier && !grabbing)
                                {
                                    animationPlaying = "Skid";
                                }
                                else
                                {
                                    if (!grabbing)
                                    {
                                        animationPlaying = "Run";
                                    }
                                    else
                                    {
                                        if (!jumping && jumpAvaliable)
                                        {
                                            animationPlaying = "CarryingMovement";
                                        }

                                    }
                                }
                                break;
                            case (Direction.Right):
                                if (playerBody.velocity.x < -1 * maxChosenVelocity.x * stats.starManAnimationModifier && !grabbing)
                                {
                                    animationPlaying = "Skid";
                                }
                                else
                                {
                                    if (!grabbing)
                                    {
                                        animationPlaying = "Run";
                                    }
                                    else
                                    {
                                        if (!jumping && jumpAvaliable)
                                        {
                                            animationPlaying = "CarryingMovement";
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (!grabbing)
                        {
                            animationPlaying = "Walk";
                        }
                        else
                        {
                            if (!jumping && jumpAvaliable)
                            {
                                animationPlaying = "CarryingMovement";
                            }
                        }
                    }

                    #endregion
                }
               

            }
            else
            {
                animatorSpeed = 1f;
                directionStats.moving = Direction.Still;
            }

            if (Mathf.Abs(playerBody.velocity.y) > maxChosenVelocity.y)
            {
                if (playerBody.velocity.y > 0)
                {
                    playerBody.velocity = new Vector3(playerBody.velocity.x, maxChosenVelocity.y);
                }
            }

            if (playerBody.velocity.y != 0 && !ducking)
            {
                    if (playerBody.velocity.y < -0.2)
                    {
                        if (!grabbing)
                        {
                            animationPlaying = "Fall";
                        }
                        else
                        {
                            animationPlaying = "CarryingIdle";
                        }
                    }
                    if (playerBody.velocity.y > 0.2)
                    {
                        if (!grabbing)
                        {
                            animationPlaying = "Jump";
                        }
                        else
                        {
                            animationPlaying = "CarryingIdle";
                        }

                    }
            }

            if (Mathf.Abs(playerBody.velocity.y) <= 0.2 && movement == 0)
            {
                if (!grabbing)
                    animationPlaying = "Idle";
                else
                    animationPlaying = "CarryingIdle";
            }

            if(movement != 0 && lookingUp)
            {
                lookingUp = false;
                mainCamera.GetComponent<Rigidbody2D>().simulated = true;
                mainCamera.GetComponent<DistanceJoint2D>().distance = 0.005f;
            }

            previousMovement = movement;

            if (Input.GetKey(KeyCode.Space))
            {

                if (jumpAvaliable && !hitCap && !previousHitCap && !jumping)
                {
                    jumpAvaliable = false;
                    audioSource.clip = sfx[0];
                    audioSource.Play();
                    jumping = true;

                }
                if (jumping)
                {
                    if (!marioStarMan)
                    {
                        jumpTime += Time.deltaTime;
                        playerBody.velocity += new Vector2(0, jumpForce + jumpSpeed * Time.deltaTime);
                        if (jumpTime >= chosenJumpTime)
                        {
                            jumping = false;
                            jumpTime = 0f;
                        }
                    }
                    else
                    {
                        jumpTime += Time.deltaTime;
                        playerBody.velocity += new Vector2(0, jumpForce + jumpSpeed * stats.jumpModifierStarman * Time.deltaTime);
                        if (jumpTime >= chosenJumpTime)
                        {
                            jumping = false;
                            jumpTime = 0f;
                        }
                    }


                }


            }
            else
            {
                if (jumping)
                {
                    jumping = false;
                }
            }
        }

        if (transform.position != previousPosition)
        {
            lookUpCameraSettings.maxY = transform.position.y + lookUpCameraSettings.distanceFromUs + lookUpCameraSettings.maxYChange;
            lookUpCameraSettings.bottomY = transform.position.y + lookUpCameraSettings.distanceFromUs;

        }

        DuckingAndLookingUp();
        //previousVelocity = playerBody.velocity;
        previousPosition = transform.position;
    }

    public void Bounce()
    {
        playerBody.AddForce(bounceForce + bounceForce * stats.jumpModifierStarman *(spaceBarEffectOnBounce * Input.GetAxis("Jump")), ForceMode2D.Impulse);
    }

    public Vector3 bounceForce;
    public float spaceBarEffectOnBounce;


    // Update is called once per frame
    void Update ()
    {
        //if (grabbing)
        //{
        //    Physics2D.IgnoreCollision(GetOurCollider(), ourJoint.connectedBody.GetComponent<BoxCollider2D>(), true);
        //}
        //if (JustLetGo)
        //{
        //    Physics2D.IgnoreCollision(GetOurCollider(), objectToDeReg, true);
        //}

        if (busy) return;

        if (!dead)
        {
            if (damageArmour)
            {
                damageTime += Time.deltaTime;
                damageArmourFlashes += Time.deltaTime;
                if (damageArmourFlashes >= damageArmourFlashIntervals)
                {
                    if (spriteRenderer.enabled)
                    {
                        spriteRenderer.enabled = false;
                    }
                    else
                    {
                        spriteRenderer.enabled = true;
                    }
                    damageArmourFlashes = 0f;
                }

                if (damageTime >= damageArmourLasting)
                {
                    damageArmour = false;
                }
            }
            else
            {
                if (damageTime != 0)
                {
                    damageTime = 0f;
                }
                if (!spriteRenderer.enabled)
                {
                    spriteRenderer.enabled = true;
                }
            }

            if (grabbing && !Input.GetKey(KeyCode.E))
            {
                LetGo();
            }

            //physhics2Dbody.position = gameObject.transform.position;
            Movements();
            CheckForGround();
        }
        else
        {
            HandleDeath();
        }
       

        Animations();

    }


    //Uhm.. this instead.
    public Collider2D groundCheckZone;   

    void CheckForGround()
    {
        Collider2D[] groundCheck = Physics2D.OverlapBoxAll(groundCheckZone.bounds.center, new Vector2(0.8f, 0.1f), 0, Physics2D.GetLayerCollisionMask(gameObject.layer));
        List<Collider2D> howAnnoying = groundCheck.ToList();
        howAnnoying.RemoveAll(X => X.gameObject == gameObject);
        howAnnoying.RemoveAll(X => X == groundCheckZone);
        groundCheck = howAnnoying.ToArray();

        if (groundCheck.Length > 0)
        {
            if(groundCheck.Any(X => X.tag != "NonJumpReset"))
            {
                if (jumping != true)
                {
                    jumpAvaliable = true;
                }
            }
        }
        
    }


    void OnCollisionStay2D(Collision2D Col)
    {
        //Debug.DrawLine(Col.collider.bounds.min, Col.collider.bounds.max, Color.red);
        //Debug.DrawRay(Col.collider.bounds.min, new Vector3(0, 1, 0), Color.green);
        //Debug.DrawRay(Col.collider.bounds.max, new Vector3(0, 1, 0), Color.green);

        //foreach (ContactPoint2D contact in Col.contacts)
        //{
        //    Debug.DrawRay(contact.point, contact.normal, Color.cyan);
        //    //Debug.Log(contact.point, this);
        //}

        //if (Col.gameObject.tag != "NonJumpReset")
        //{

        //    foreach (ContactPoint2D contact in Col.contacts)
        //    {

        //        if (contact.point.y >= Col.collider.bounds.center.y && (contact.point.x < Col.collider.bounds.max.x - 0.005 && contact.point.x > Col.collider.bounds.min.x + 0.005))
        //        {
        //            if (jumping != true)
        //            {
        //                jumpAvaliable = true;
        //                Debug.Log("Jump Ready?: " + jumpAvaliable, Col.gameObject);
        //            }

        //        }
        //    }


        //}


    }


    void OnCollisionEnter2D(Collision2D Col)
    {

        foreach (ContactPoint2D contact in Col.contacts)
        {
            if (contact.point.y <= Col.collider.bounds.center.y && (contact.point.x < Col.collider.bounds.max.x - 0.05 && contact.point.x > Col.collider.bounds.min.x + 0.05))
            {
                Debug.Log("Mario Hit Under " + Col.gameObject.name, Col.gameObject);
                if (jumping) jumping = false;
                Col.gameObject.SendMessage("Activated", SendMessageOptions.DontRequireReceiver);
            }
            else if (contact.point.y <= Col.collider.bounds.center.y && (contact.point.x < Col.collider.bounds.max.x - 0.02 && contact.point.x > Col.collider.bounds.min.x - 0.02))
            {
                Debug.Log("Mario Hit Under " + Col.gameObject.name, Col.gameObject);
                if (jumping) jumping = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D Col)
    {
        if (Col.gameObject.tag != "NonJumpReset")
        {
            Debug.DrawLine(Col.collider.bounds.min, Col.collider.bounds.max, Color.red, 5f);
            if(!jumping && jumpAvaliable)
            {
                jumpAvaliable = false;
                Debug.Log("Jump Ready?: " + jumpAvaliable, this);
            }

        }
    }


    void OnTriggerEnter2D(Collider2D Col)
    {
        
        Col.gameObject.SendMessage("Activated", SendMessageOptions.DontRequireReceiver);
        try
        {
            if(Col.gameObject.transform.parent.GetComponent<PowerUp>() != null)
            {
                if(Col.gameObject.transform.parent.GetComponent<PowerUp>().powerUpType == PowerUp.PowerUpType.Mushroom)
                {
                    CollectedMushroom(Col.gameObject.GetComponent<Rigidbody2D>());
                }
                if (Col.gameObject.transform.parent.GetComponent<PowerUp>().powerUpType == PowerUp.PowerUpType.Starman)
                {
                    CollectedStarMan();
                }

            }
        }
        catch { }

        if (!Input.GetKey(KeyCode.E))
        {
            //Debug.Log(Col.bounds);
            //Debug.Log(transform.position);
            if (transform.position.x < Col.bounds.min.x && (transform.position.y >= Col.bounds.min.y && transform.position.y <= Col.bounds.max.y) && transform.position.x < Col.bounds.max.x)
            {
                Debug.Log("Mario hit the Left Side of " + Col.gameObject.name, this);
                Col.SendMessage("SideHit", this, SendMessageOptions.DontRequireReceiver);
                Col.SendMessage("LeftSideHit", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            if (transform.position.x > Col.bounds.max.x && (transform.position.y >= Col.bounds.min.y && transform.position.y <= Col.bounds.max.y) && transform.position.x > Col.bounds.min.x)
            {
                Debug.Log("Mario hit the Right Side of " + Col.gameObject.name, this);
                Col.SendMessage("SideHit", this, SendMessageOptions.DontRequireReceiver);
                Col.SendMessage("RightSideHit", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            if ((transform.position.x < Col.bounds.max.x && transform.position.x > Col.bounds.min.x) && (transform.position.y > Col.bounds.center.y))
            {
                Debug.Log("Mario hit the Top of " + Col.gameObject.name, this);
                Col.SendMessage("TopHit", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            if ((transform.position.x < Col.bounds.max.x && transform.position.x > Col.bounds.min.x) && (transform.position.y < Col.bounds.center.y))
            {
                Debug.Log("Mario hit the Bottom of " + Col.gameObject.name, this);
                Col.SendMessage("BottomHit", this, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                Debug.Log("Mario hit the... Insides? of " + Col.gameObject.name, this);
                Col.SendMessage("InsideHit", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        else
        {
            if(Col.gameObject.tag == "Grabbable")
            {
                if (!grabbing)
                {
                    Debug.Log("Mario Grabbed " + Col.gameObject.name);
                    Grab(Col.gameObject);
                }

            }
        }

    }



    public void TakeDamage()
    {
        if (!marioStarMan)
        {
            if (!damageArmour)
            {
                if (powerUpStatus == PowerUpStatus.Small)
                {
                    Death();
                }
                else
                {
                    StartCoroutine(DePower());
                }
            }
        }
        else
        {
            //We defy this.
        }


    }
    
    public IEnumerator DePower()
    {
        //This depends on the player.. state.
        FreezeUnfreeze(true, null);
        try
        {
            animator.Play("Depower");
        } catch(Exception e)
        {
            Debug.LogException(e, this);
        }

        float toWait = animator.GetCurrentAnimatorStateInfo(animatorLayer).length;

        yield return new WaitForSeconds(toWait);

        switch (powerUpStatus)
        {
            case PowerUpStatus.Small:
                //? This should have been death.
                //Oh well. Just don't change the players state.
                break;
            case PowerUpStatus.Large:
                powerUpStatus = PowerUpStatus.Small;
                break;
            case PowerUpStatus.Fire:
                powerUpStatus = PowerUpStatus.Large;
                break;
            case PowerUpStatus.Flying:
                powerUpStatus = PowerUpStatus.Large;
                break;
            default:
                break;
        }

        SetOurCollider();
    }


    public void Death()
    {
        playerBody.bodyType = RigidbodyType2D.Static;
        storedCameraPos = mainCamera.transform.position;
        dead = true;
        GameObject.FindObjectOfType<MasterHUD>().DeadEvent();
        GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Joint2D>().connectedBody = null;
        Camera.main.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        Camera.main.GetComponent<Joint2D>().connectedBody = null;
    }

    private Vector3 storedCameraPos;

    public void HandleDeath()
    {
        if (dead)
        {
            FreezeUnfreeze(true, GetComponent<Rigidbody2D>());
            mainCamera.transform.position = storedCameraPos;
            animationPlaying = "Death";
            deathDelayTimer += Time.deltaTime;
            if(deathDelayTimer >= deathDelay)
            {
                if (!forceApplied)
                {
                    playerBody.bodyType = RigidbodyType2D.Dynamic;
                    playerBody.AddForce(new Vector2(0, deathForceApplication), ForceMode2D.Impulse);
                    gameObject.layer = 13;
                    startOverTime = deathDelayTimer;
                    forceApplied = true;    
                }
                startOverTime += Time.deltaTime;
                if(startOverTime >= timeToStartOver)
                {
                    //mainCamera.transform.parent = transform;
                    GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Rigidbody2D>().isKinematic = false;
                    GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Joint2D>().enabled = true;
                    GameObject.FindGameObjectWithTag("BackgroundCamera").GetComponent<Joint2D>().connectedBody = playerBody;
                    Camera.main.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    Camera.main.GetComponent<Joint2D>().connectedBody = playerBody;
                    gameObject.layer = 8;
                    GameObject.FindObjectOfType<MasterHUD>().Retry();
                }
            }
            
        }
    }


    public void Kick()
    {
        animatorSpeed = 1f;
        kicking = true;
        audioSource.clip = sfx[1];
        audioSource.Play();

    }

    public void Grab(GameObject objectToGrab)
    {

        try
        {
            ourJoint.connectedBody = objectToGrab.transform.parent.GetComponent<Rigidbody2D>();
            grabbing = true;
            ourJoint.enabled = true;
        }
        catch
        {
            Debug.Log("Grabbable Object " + objectToGrab.name + " Doesn't have a rigid body or box collider?");
        }
        objectToGrab.SendMessage("Grabbed", this, SendMessageOptions.DontRequireReceiver);
        

    }

    public void ItemInHandDestroyed()
    {
        //Oh dear.
        grabbing = false;
        ourJoint.connectedBody = null;
        ourJoint.enabled = false;
    }

    public void LetGo()
    {
        if (!ducking)
        {
            grabbing = false;
            if(ourJoint.connectedBody != null)
            {
                Kick();
                ourJoint.connectedBody.gameObject.SendMessage("LetGo", this, SendMessageOptions.DontRequireReceiver);
                ourJoint.connectedBody.gameObject.SendMessage("SideHit", this, SendMessageOptions.DontRequireReceiver);
                if(directionStats.facing == Direction.Right)
                {
                    //ourJoint.connectedBody.AddForce(new Vector2(1, 0), ForceMode2D.Force);
                    //ourJoint.connectedBody.gameObject.gameObject.transform.Translate(new Vector3(0.5f, 0));
                }
                else
                {
                    //ourJoint.connectedBody.AddForce(new Vector2(-1, 0), ForceMode2D.Force);
                    //ourJoint.connectedBody.gameObject.gameObject.transform.Translate(new Vector3(-0.5f, 0));
                }
                //objectToDeReg = ourJoint.connectedBody.transform.GetChild(0).GetComponent<Collider2D>();
                //JustLetGo = true;
                //StartCoroutine(startCo());
            }
            ourJoint.enabled = false;
            ourJoint.connectedBody = null;
        }


    }

    private bool JustLetGo;
    private Collider2D objectToDeReg;

    public void DoneKicking()
    {
        if (kicking)
        {
            kicking = false;
        }
    }

}

//[System.Serializable]
//public struct AnimationStatus 
//{
//    public string animationTag;
//    public bool animationBool;

//    //public string AnimationTag
//    //{
//    //    get
//    //    {
//    //        return animationTag;
//    //    }

//    //    set
//    //    {
//    //        animationTag = value;
//    //    }
//    //}

//    //public bool AnimationBool
//    //{
//    //    get
//    //    {
//    //        return animationBool;
//    //    }

//    //    set
//    //    {
//    //        animationBool = value;
//    //    }
//    //}

//    //AnimationStatus(string animationTag, bool wantsToBePlayed)
//    //{
//    //    this.animationTag = animationTag;
//    //    animationBool = wantsToBePlayed;
//    //}
//}






using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionaryDoor : MonoBehaviour
{
    public float timeToOpenDoor = 1f;
    private float timeStamp = 0;

    public AudioClip openDoorSound;

    public Animator Animator
    {
        get
        {
            return gameObject.GetComponent<Animator>();
        }
    }

    public string transitionTo;

    //or

    public int transitionToBuildIndex;
    public int transitionStartPosIndex;
    private bool opening = false;
    private PlayerController openingPlayer;

    public void Open(PlayerController playerOpening)
    {

        if (!opening)
        {
            AudioSource aS = gameObject.AddComponent<AudioSource>();
            aS.clip = openDoorSound;
            aS.loop = false;
            aS.Play();

            Animator.Play("Open");
            //timeToOpenDoor = GetLength("Open");
            timeStamp = Time.time;
            opening = true;
            openingPlayer = playerOpening;
        }

    }

    private void Update()
    {
        if (opening && Time.time >= timeStamp + timeToOpenDoor)
        {
            openingPlayer.FreezeUnfreeze(false);
            openingPlayer.busy = false;
            if (transitionTo != "")
            {
                SceneManager.LoadScene(transitionTo);
            }
            else
            {
                SceneManager.LoadScene(transitionToBuildIndex);
            }
        }

    }

    //float GetLength(string track)
    //{
    //    float length = 0;
    //    Animator anim = gameObject.GetComponent<Animator>();
    //    if (anim != null)
    //    {
    //        UnityEditor.Animations.AnimatorController ac = anim.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
    //        UnityEditor.Animations.AnimatorStateMachine sm = ac.layers[0].stateMachine;

    //        for (int i = 0; i < sm.states.Length; i++)
    //        {
    //            UnityEditor.Animations.AnimatorState state = sm.states[i].state;
    //            if (state.name == track)
    //            {
    //                AnimationClip clip = state.motion as AnimationClip;
    //                if (clip != null)
    //                {
    //                    length = clip.length / state.speed;
    //                }
    //            }
    //        }
    //        Debug.Log("Animation:" + track + ":" + length);
    //    }

    //    return length;
    //}
}

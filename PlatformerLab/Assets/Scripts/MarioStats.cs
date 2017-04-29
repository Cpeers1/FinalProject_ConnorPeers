using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MarioStats.asset", menuName = "ScriptableObjects/Stats/Mario")]
public class MarioStats : ScriptableObject
{

    public float speed;
    public float runSpeed;
    public float starManSpeed;
    public float movementForce;
    public float runModifier;
    public float starManModifier;
    public float chosenJumpTime;
    public float velocityModifierRun;
    public float velocityModifierWalk;
    public float velocityModifierStarMan;
    public float jumpSpeed;
    public float jumpForce;
    public float jumpModifierStarman;
    public float runAnimationModifier;
    public float starManAnimationModifier;
    public float damageArmourLasting;
    public float damageArmourFlashIntervals;
    public float deathDelay;
    public float deathForceApplication;
    public float timeToStartOver;
    public bool dead;

    public float previousMovement { get; set; }
    public float jumpTime { get; set; }
    public bool forceApplied { get; set; }
    public float startOverTime { get; set; }
    public float deathDelayTimer { get; set; }
    public float damageArmourFlashes { get; set; }
    public float damageTime { get; set; }
    public float animatorSpeed { get; set; }
    public float ourJointX { get; set; }
}

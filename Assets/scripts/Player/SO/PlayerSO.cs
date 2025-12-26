using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerVariant", menuName = "ScriptableObjects/Player", order = 1)]
public class PlayerSO : ScriptableObject
{




    [Header("Movement Settings")]

    public float groundspeed;
    public float inAirSpeed;
    public float horizontalThreshold;
    [Range(0, 1000f), SerializeField] public float maxHorizontalVelocity;
    [Range(0, 10000f), SerializeField] public float maxUpVelocity;
    [Range(0, 10000f), SerializeField] public float maxDownVelocity;


      
    // Movement Settings

    #region Player stats
    // public int PurchasedHearts = 3;
    public int maxHp;
    public int startHp;
    public int playerDamage;
    public int totalCoins;
    public int startCoins;
    public float score;

    #endregion

    #region Physics Settings


    public float angularFallingVelocity;
    public float angularJumpVelocity;
    public float initialGravity;
    public float initialDrag;
    public float mass;
    public float linearDrag;
    public float gravity;
    public bool handleVelocities;

    #endregion
    [Header("Dash Settings")]

    public float maxDashCounter;
    public float dashEndVel = .3f;
    public float dashForce;
    public float dashTime;

    [Header("Jump Settings")]
    [Tooltip("for android = 7, for pc is 4")] public float jumpForce = 7; 
    public float minJumpForce = 10f;
    public float maxJumpForce = 30f;
    public Vector2 jumpThreshold; // for android screen
    public float horizontalJumpDirectionThreshold; // for pc

    


}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    // --------------------------------------------------------------------------------------------------------------------------------------------------
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; 
    [HideInInspector] public float gravityScale; 
    [Space(5)]
    public float fallGravityMult; 
    public float maxFallSpeed; 

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed; 
    public float runAcceleration; 
    [HideInInspector] public float runAccelAmount; 
    public float runDecceleration; 
    [HideInInspector] public float runDeccelAmount; 
    [Space(5)]
    [Range(0f, 1)] public float accelInAir;
    [Range(0f, 1)] public float deccelInAir;


    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; 
    public float jumpTimeToApex; 
    [HideInInspector] public float jumpForce; 

    [Header("Both Jumps")]
    public float jumpCutGravityMult; 
    [Range(0f, 1)] public float jumpHangGravityMult; 
    public float jumpHangTimeThreshold;
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;

    // --------------------------------------------------------------------------------------------------------------------------------------------------

    #region Variables
    
    //Components
    public Rigidbody2D RB { get; private set; }

    //Variables control the various actions the player can perform at any time.
    public bool IsJumping { get; private set; }
    public bool IsSliding { get; private set; }

    //Timer check
    public float LastOnGroundTime { get; private set; }

    //Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    //Dash
    private bool _isDashing = false;
    private float _DashTime = 1.5f;
    private float _Timer = 0.0f;
    private int direction = 1;

    private Vector2 _moveInput;
    public float LastPressedJumpTime { get; private set; }

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    [Space(5)]
    [Header("Old Implementation")]
    // RUBEN implementation below

    public Rigidbody2D rb; 
    public float mSpeed;
    public float mMaxSpeed;

    public Animator brain;

    #region GENERAL METHODS
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }
    #endregion
    public void OnJumpInput()
    {
        LastPressedJumpTime = 0.2f;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut())
        {
            _isJumpCut = true;
            brain.SetBool("JumpUp", false);
            brain.SetBool("Idle", false);
            brain.SetBool("JumpDown", true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();  
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);

        SetGravityScale(gravityScale);
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

        ////For now this will be hardcoded, this could change
        //rb.AddForce(Input.GetAxis("Horizontal") * mSpeed * new Vector2(1,0) * Time.deltaTime);
        ////Force for a maximum speed
        //if(Mathf.Abs(rb.velocity.x) > mMaxSpeed)
        //{
        //    if(rb.velocity.x > 0)
        //        rb.velocity = new Vector2(mMaxSpeed, rb.velocity.y);
        //    else
        //        rb.velocity = new Vector2(-mMaxSpeed, rb.velocity.y);
        //}

        #region TIMERS
        LastOnGroundTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        #endregion

        if(rb.velocity.x > 0 )
        {
            direction = 1;
        }
        else if(rb.velocity.x < 0 ) { direction = -1; }

        if(_isDashing)
        {
            if(_Timer > _DashTime)
            {
                transform.position = new Vector3(transform.position.x + mSpeed * Time.deltaTime, transform.position.y, transform.position.z);
                if (_Timer > _DashTime + 0.5f)
                {
                    _isDashing = false;
                    _Timer = 0;
                }
            }
            
            _Timer += Time.deltaTime;
            return;
        }

        #region INPUT HANDLER
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.J))
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Z) || Input.GetKeyUp(KeyCode.J))
        {
            OnJumpUpInput();
        }
        #endregion

        #region DASH
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isDashing = true;
        }
        #endregion



        #region JUMP CHECKS
        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;

            _isJumpFalling = true;

            brain.SetBool("JumpUp", false);
            brain.SetBool("Idle", false);
            brain.SetBool("JumpDown", true);
        }

        if (LastOnGroundTime > 0 && !IsJumping)
        {
            _isJumpCut = false;

            _isJumpFalling = false;
        }

        //Jump
        if (CanJump() && LastPressedJumpTime > 0)
        {
            IsJumping = true;
            _isJumpCut = false;
            _isJumpFalling = false;
            Jump();
        }
        #endregion


        #region GRAVITY
        //Higher gravity if we've released the jump input or are falling
        if (IsSliding)
        {
            SetGravityScale(0);
        }
        else if (_isJumpCut)
        {
            //Higher gravity if jump button released
            SetGravityScale(gravityScale * jumpCutGravityMult);
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -maxFallSpeed));
        }
        else if ((IsJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < jumpHangTimeThreshold)
        {
            SetGravityScale(gravityScale * jumpHangGravityMult);
        }
        else if (RB.velocity.y < 0)
        {
            //Higher gravity if falling
            SetGravityScale(gravityScale * fallGravityMult);
            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
            RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -maxFallSpeed));
        }
        else
        {
            //Default gravity if standing on a platform or moving upwards
            SetGravityScale(gravityScale);
        }
        #endregion
    }
    private void FixedUpdate()
    {
        Run(1);
    }
        private void Run(float lerpAmount)
    {
        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * runMaxSpeed;

        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        if ((IsJumping  || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < jumpHangTimeThreshold)
        {
            accelRate *= jumpHangAccelerationMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - RB.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    #region JUMP METHODS
    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);

        brain.SetBool("JumpUp", true);
        brain.SetBool("Idle", false);
        brain.SetBool("JumpDown", false);
        #endregion
    }
    #endregion
    #region OTHER MOVEMENT METHODS
    private void Slide()
    {
        //Works the same as the Run but only in the y-axis
        //THis seems to work fine, buit maybe you'll find a better way to implement a slide into this system
        float speedDif = slideSpeed - RB.velocity.y;
        float movement = speedDif * slideAccel;
        //So, we clamp the movement here to prevent any over corrections (these aren't noticeable in the Run)
        //The force applied can't be greater than the (negative) speedDifference * by how many times a second FixedUpdate() is called. For more info research how force are applied to rigidbodies.
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }
    #endregion
    private bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    private bool CanJumpCut()
    {
        return IsJumping && RB.velocity.y > 0;
    }

    void OnCollisionStay2D(Collision2D other) {
        brain.SetBool("JumpUp", false);    
        brain.SetBool("JumpDown", false);    
        brain.SetBool("Idle", true);
        LastOnGroundTime = 0.01f;
    }
}

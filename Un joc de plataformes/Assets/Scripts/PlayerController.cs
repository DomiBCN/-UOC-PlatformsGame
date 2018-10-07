using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
     float rotationSpeed = 50;
    [SerializeField]
     float forwardSpeed = 50;
    [SerializeField]
    float backwardsSpeed = 20;
    [SerializeField]
    GameObject box;


    public delegate void eliminatedDelegate();
    public event eliminatedDelegate eliminated;
    public event eliminatedDelegate levelEnd;

    #region ORIGINAL
    [SerializeField]
    Transform backWheelPoint;
    #endregion
    #region JOINTS
    [SerializeField]
    Transform backWheel;
    [SerializeField]
    WheelJoint2D backWheelJoint;
    #endregion

    Rigidbody2D rigidBody;
    float wheelRadius;
    Collider2D[] overlapColliders = new Collider2D[1];
    List<KeyCode> actions = new List<KeyCode>();

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        #region ORIGINAL
        //wheelRadius = rigidBody.GetComponent<CircleCollider2D>().radius * 1.1f;
        #endregion
        #region JOINTS
        wheelRadius = backWheel.GetComponent<CircleCollider2D>().radius * 1.1f;
        #endregion
    }

    #region movement
    #region ORIGINAL
    //public void MoveRight()
    //{
    //    rigidBody.velocity += new Vector2(transform.right.x * linealSpeed, transform.right.y * linealSpeed) * Time.deltaTime;
    //}

    //public void MoveLeft()
    //{
    //    rigidBody.velocity -= new Vector2(transform.right.x * linealSpeed, transform.right.y * linealSpeed) * Time.deltaTime;
    //}

    //public void RotateRight()
    //{
    //    rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.deltaTime);
    //}

    //public void RotateLeft()
    //{
    //    rigidBody.MoveRotation(rigidBody.rotation + rotationSpeed * Time.deltaTime);
    //}
    #endregion
    #region JOINTS
    void Movement(bool isLeft)
    {
        float movement = 0;
        if (isLeft) { movement = 1 * backwardsSpeed; } else { movement = -1 * forwardSpeed; }

        JointMotor2D motor = new JointMotor2D { motorSpeed = movement , maxMotorTorque = 3000 };
        SetMotor(motor);
    }

    public void RotateRight()
    {
        //rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.deltaTime);
        rigidBody.AddTorque(-1 * rotationSpeed * Time.deltaTime);
    }

    public void RotateLeft()
    {
        //rigidBody.MoveRotation(rigidBody.rotation + rotationSpeed * Time.deltaTime);
        rigidBody.AddTorque(1 * rotationSpeed * Time.deltaTime);
    }
    #endregion
    #endregion

    // Update is called once per frame
    void Update()
    {
        //allow us to use the keyboard
        #region keyboard control
        UpdateKeyboardAction(KeyCode.D);
        UpdateKeyboardAction(KeyCode.A);
        UpdateKeyboardAction(KeyCode.UpArrow);
        UpdateKeyboardAction(KeyCode.DownArrow);
        #endregion

    }

    void FixedUpdate()
    {
        #region ORIGINAL
        //if (actions.Contains(KeyCode.D) && TouchingGround()) { MoveRight(); }
        //if (actions.Contains(KeyCode.A) && TouchingGround()) { MoveLeft(); }
        #endregion
        #region JOINTS
        if (actions.Contains(KeyCode.D) || actions.Contains(KeyCode.A)) { UseMotor(true); Movement(actions.Contains(KeyCode.A)); } else { UseMotor(false); }
        #endregion
        if (actions.Contains(KeyCode.UpArrow)) { RotateRight(); }
        if (actions.Contains(KeyCode.DownArrow)) { RotateLeft(); }
    }

    #region update actions
    void UpdateKeyboardAction(KeyCode code)
    {
        if (Input.GetKeyDown(code))
        {
            UpdateActionDown(code);
        }
        if (Input.GetKeyUp(code))
        {
            UpdateActionUp(code);
        }
    }

    void UpdateActionDown(KeyCode code)
    {
        if (!actions.Contains(code)) { actions.Add(code); }
    }

    void UpdateActionUp(KeyCode code)
    {
        if (actions.Contains(code)) { actions.Remove(code); }
    }

    public void MoveLeftDown()
    {
        UpdateActionDown(KeyCode.A);
    }

    public void MoveRightDown()
    {
        UpdateActionDown(KeyCode.D);
    }

    public void RotateLeftDown()
    {
        UpdateActionDown(KeyCode.DownArrow);
    }

    public void RotateRightDown()
    {
        UpdateActionDown(KeyCode.UpArrow);
    }

    public void MoveLeftUp()
    {
        UpdateActionUp(KeyCode.A);
    }

    public void MoveRightUp()
    {
        UpdateActionUp(KeyCode.D);
    }

    public void RotateLeftUp()
    {
        UpdateActionUp(KeyCode.DownArrow);
    }

    public void RotateRightUp()
    {
        UpdateActionUp(KeyCode.UpArrow);
    }
    #endregion

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.CompareTo("Finish") != 0 && collision.gameObject.tag.CompareTo("BoxTrigger") != 0)
        {
            if (eliminated != null) { eliminated(); }
        }
        else if(collision.gameObject.tag.CompareTo("BoxTrigger") != 0)
        {
            if (levelEnd != null) { levelEnd(); }
        }
        else
        {
            AddBox();
        }
    }

    void AddBox()
    {
        GameObject fallingBox = Instantiate(box);
        fallingBox.transform.position = new Vector2(transform.position.x, transform.position.y + 5);
    }

    #region ORIGINAL
    bool TouchingGround()
    {
        //given that we are calling this method a lot and in order to save memory usage, I'm using OverlapCircleNonAlloc instead of OverlapCircle.
        //It allows us to pass the same array for the results, with a given length(it won't be resized) and reuse it. 

        //if (Physics2D.OverlapCircleAll(backWheel.position, wheelRadius, 1 << LayerMask.NameToLayer("Terrain")).Length > 0)
        return Physics2D.OverlapCircleNonAlloc(backWheelPoint.position, wheelRadius, overlapColliders, 1 << LayerMask.NameToLayer("Terrain")) > 0;
    }
    #endregion

    #region JOINTS
    void UseMotor(bool use)
    {
        backWheelJoint.useMotor = use;
    }

    void SetMotor(JointMotor2D motor)
    {
        backWheelJoint.motor = motor;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOriginal : MonoBehaviour {

    public float rotationSpeed = 50;
    public float linealSpeed = 50;

    public delegate void eliminatedDelegate();
    public event eliminatedDelegate eliminated;
    public event eliminatedDelegate levelEnd;

    [SerializeField]
    Transform backWheelPoint;

    private Rigidbody2D rigidBody;
    private float wheelRadius;
    private List<KeyCode> actions = new List<KeyCode>();
    private Collider2D[] overlapColliders = new Collider2D[1];

    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        wheelRadius = rigidBody.GetComponent<CircleCollider2D>().radius * 1.1f;
    }
    #region movement

    public void MoveRight()
    {
        rigidBody.velocity += new Vector2(transform.right.x * linealSpeed, transform.right.y * linealSpeed) * Time.deltaTime;
    }

    public void MoveLeft()
    {
        rigidBody.velocity -= new Vector2(transform.right.x * linealSpeed, transform.right.y * linealSpeed) * Time.deltaTime;
    }

    public void RotateRight()
    {
        rigidBody.MoveRotation(rigidBody.rotation - rotationSpeed * Time.deltaTime);
    }

    public void RotateLeft()
    {
        rigidBody.MoveRotation(rigidBody.rotation + rotationSpeed * Time.deltaTime);
    }
    #endregion
    // Update is called once per frame
    void Update()
    {
        //allow us to use the keyboard
        #region keyboard control
        UpdateKeyboardAction(KeyCode.RightArrow);
        UpdateKeyboardAction(KeyCode.LeftArrow);
        UpdateKeyboardAction(KeyCode.UpArrow);
        UpdateKeyboardAction(KeyCode.DownArrow);
        #endregion

    }

    private void FixedUpdate()
    {
        if (actions.Contains(KeyCode.RightArrow) && TouchingGround()) { MoveRight();}
        if (actions.Contains(KeyCode.LeftArrow) && TouchingGround()) { MoveLeft(); }
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
        UpdateActionDown(KeyCode.LeftArrow);
    }

    public void MoveRightDown()
    {
        UpdateActionDown(KeyCode.RightArrow);
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
        UpdateActionUp(KeyCode.LeftArrow);
    }

    public void MoveRightUp()
    {
        UpdateActionUp(KeyCode.RightArrow);
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
        if (collision.gameObject.tag.CompareTo("Finish") != 0)
        {
            if (eliminated != null) { eliminated(); }
        }
        else
        {
            if (levelEnd != null) { levelEnd(); }
        }
    }

    bool TouchingGround()
    {
        //given that we are calling this method a lot and in order to save memory usage, I'm using OverlapCircleNonAlloc instead of OverlapCircle.
        //It allows us to pass the same array for the results, with a given length(it won't be resized) and reuse it. 

        //if (Physics2D.OverlapCircleAll(backWheel.position, wheelRadius, 1 << LayerMask.NameToLayer("Terrain")).Length > 0)
        return Physics2D.OverlapCircleNonAlloc(backWheelPoint.position, wheelRadius, overlapColliders, 1 << LayerMask.NameToLayer("Terrain")) > 0;
    }
}

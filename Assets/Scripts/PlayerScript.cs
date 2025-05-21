using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    public int moveSpeed;
    public float horiDistance; //horizontal distance
    public float vertDistance; //vertical distance
    public bool moving;
    List<string> moveSequence = new List<string>();
    public Rigidbody2D rb2D;
    public LayerMask blockingLayer;

    private Vector2 direction;

    private Vector2 dir_Up;
    private Vector2 dir_Down;
    private Vector2 dir_Left;
    private Vector2 dir_Right;
    private Vector2 dir_UpRight;
    private Vector2 dir_UpLeft;
    private Vector2 dir_DownRight;
    private Vector2 dir_DownLeft;
    private Vector2 currentPos;
    private BoxCollider2D boxCollider;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        currentPos = rb2D.position;
        dir_Up = new Vector2(0, vertDistance);
        dir_Down = new Vector2(0, -vertDistance);
        dir_Left = new Vector2(-horiDistance, 0);
        dir_Right = new Vector2(horiDistance, 0);
        dir_UpRight = new Vector2(horiDistance/2, vertDistance/2);
        dir_UpLeft = new Vector2(-horiDistance/2, vertDistance/2);
        dir_DownRight = new Vector2(horiDistance/2, -vertDistance/2);
        dir_DownLeft = new Vector2(-horiDistance/2, -vertDistance/2);

    }

    void Update() // this is messy as hell (but it works)
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveSequence.Add("upright");
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveSequence.Add("downright");
            }
            else
            {
                moveSequence.Add("right");
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.D))
            {
                moveSequence.Add("upright");
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveSequence.Add("upleft");
            }
            else
            {
                moveSequence.Add("up");
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W))
            {
                moveSequence.Add("upleft");
            }
            else if (Input.GetKey(KeyCode.S))
            {
                moveSequence.Add("downleft");
            }
            else
            {
                moveSequence.Add("left");
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.D))
            {
                moveSequence.Add("downright");
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveSequence.Add("downleft");
            }
            else
            {
                moveSequence.Add("down");
            }
        }
        FixedUpdate();
    }

    void FixedUpdate()
    {
        if (moveSequence.Count > 0 && !moving)
        {
            Move(moveSequence[0]);
        }
    }

    void Move(string direction_string)
    {
        ChangeDirection(direction_string);

        Vector2 start = transform.position;
        Vector2 end = start + direction;

        Debug.Log("Rigidbody2D: " + rb2D);
        Debug.Log("BoxCollider2D: " + boxCollider);

        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = false;

        if (hit.transform == null)
        {
            StartCoroutine(Movement(end));
            //animator.SetTrigger(direction_string);
            return;
        }
        else
        {
            moveSequence.RemoveAt(0);
            //animator.SetTrigger(direction_string);
            return;
        }
    }

    void ChangeDirection(string direction_string)
    {
        switch (direction_string)
        {
            case "up":
                direction = dir_Up;
                break;
            case "down":
                direction = dir_Down;
                break;
            case "left":
                direction = dir_Left;
                break;
            case "right":
                direction = dir_Right;
                break;
            case "upright":
                direction = dir_UpRight;
                break;
            case "downright":
                direction = dir_DownRight;
                break;
            case "upleft":
                direction = dir_UpLeft;
                break;
            case "downleft":
                direction = dir_DownLeft;
                break;
        }
    }

    IEnumerator Movement(Vector3 end)
    {
        moving = true;
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, moveSpeed * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }

        currentPos = end;
        moveSequence.RemoveAt(0);
        moving = false;
    }
}

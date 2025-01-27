using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Movement : MonoBehaviour
{
    private Rigidbody2D RB;
    public float Move_Speed = 10;
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKey(KeyCode.W))
        {
            RB.velocity = new Vector2(RB.velocity.x, Move_Speed);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            RB.velocity = new Vector2(RB.velocity.x, -Move_Speed);
        }
        else
        {
            RB.velocity = new Vector2(RB.velocity.x, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            RB.velocity = new Vector2(Move_Speed, RB.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            RB.velocity = new Vector2(-Move_Speed, RB.velocity.y);
        }
        else
        {
            RB.velocity = new Vector2(0, RB.velocity.y);
        }
    }
}

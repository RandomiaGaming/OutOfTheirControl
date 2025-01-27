using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animator : MonoBehaviour
{
    public Sprite Idle_Up;
    public Sprite Idle_Down;
    public Sprite Idle_Left;
    public Sprite Idle_Right;
    public List<Sprite> Walk_Up;
    public List<Sprite> Walk_Down;
    public List<Sprite> Walk_Left;
    public List<Sprite> Walk_Right;

    private float Timer = 0;
    private int Index = 0;
    private Vector2 Facing = new Vector2(0, -1);
    private SpriteRenderer SR;

    private void Start()
    {
        SR = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Facing = new Vector2(0, 1);
            SR.sprite = Walk_Up[Index];

            Timer += Time.deltaTime;
            if (Timer > 0.1f)
            {
                Timer = 0;
                Index++;
            }
            if (Index >= Walk_Up.Count)
            {
                Index = 0;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Facing = new Vector2(0, -1);
            SR.sprite = Walk_Down[Index];

            Timer += Time.deltaTime;
            if (Timer > 0.1f)
            {
                Timer = 0;
                Index++;
            }
            if (Index >= Walk_Down.Count)
            {
                Index = 0;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Facing = new Vector2(1, 0);
            SR.sprite = Walk_Right[Index];

            Timer += Time.deltaTime;
            if (Timer > 0.1f)
            {
                Timer = 0;
                Index++;
            }
            if (Index >= Walk_Right.Count)
            {
                Index = 0;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Facing = new Vector2(-1, 0);
            SR.sprite = Walk_Left[Index];

            Timer += Time.deltaTime;
            if (Timer > 0.1f)
            {
                Timer = 0;
                Index++;
            }
            if (Index >= Walk_Left.Count)
            {
                Index = 0;
            }
        }
        else
        {
            Index = 0;
            Timer = 0;
            if (Facing.y == 1)
            {
                SR.sprite = Idle_Up;
            }
            else if (Facing.y == -1)
            {
                SR.sprite = Idle_Down;
            }
            else if (Facing.x == 1)
            {
                SR.sprite = Idle_Right;
            }
            else if (Facing.x == -1)
            {
                SR.sprite = Idle_Left;
            }
        }
    }
}

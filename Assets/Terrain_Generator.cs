using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class Terrain_Generator : MonoBehaviour
{
    private List<Room> Rooms = new List<Room>();
    private Tilemap TM;
    [Header("Chances")]
    public int Wall_Chance = 5;
    public int Gard_Chance = 5;
    [Header("Size")]
    public int Size = 5;
    [Header("References")]
    public Tile Ground;
    public GameObject Gard;

    void Start()
    {
        //Find Tilemap
        TM = GetComponent<Tilemap>();
        //Clear Tilemap
        TM.ClearAllTiles();
        //Check Size
        Size = Mathf.Abs(Size);
        Size = Mathf.Clamp(Size, 0, 100);
        //Find Min and Max
        int Min = Size * -4;
        int Max = 4 + (Size * 4);
        //Create Out Line
        for (int i = 0; i < Mathf.Abs(Min) + Max; i++)
        {
            TM.SetTile(new Vector3Int(Min, Min + i, 0), Ground);
            TM.SetTile(new Vector3Int(Min + i, Max, 0), Ground);
            TM.SetTile(new Vector3Int(Max, Max - i, 0), Ground);
            TM.SetTile(new Vector3Int(Max - i, Min, 0), Ground);
        }
        //Create Wall Vertices
        for (int x = Min; x < Max; x += 4)
        {
            for (int y = Min; y < Max; y += 4)
            {
                TM.SetTile(new Vector3Int(x, y, 0), Ground);
            }
        }
        //Clear Room Data
        Rooms = new List<Room>();
        //Create Rooms
        for (int x = -Size; x <= Size; x++)
        {
            for (int y = -Size; y <= Size; y++)
            {
                Room New_Room = new Room();
                New_Room.Room_X_Pos = x;
                New_Room.Room_Y_Pos = y;
                New_Room.Tile_X_Pos = x * 4;
                New_Room.Tile_Y_Pos = y * 4;
                Rooms.Add(New_Room);
            }
        }
        //Asign Neighbors
        for (int i = 0; i < Rooms.Count; i++)
        {
            Room Current_Room = Rooms[i];
            for (int i2 = 0; i2 < Rooms.Count; i2++)
            {
                if (i != i2 && Current_Room.Room_X_Pos + 1 == Rooms[i2].Room_X_Pos && Current_Room.Room_Y_Pos == Rooms[i2].Room_Y_Pos)
                {
                    Current_Room.Right = Rooms[i2];
                }
                else if (i != i2 && Current_Room.Room_X_Pos - 1 == Rooms[i2].Room_X_Pos && Current_Room.Room_Y_Pos == Rooms[i2].Room_Y_Pos)
                {
                    Current_Room.Left = Rooms[i2];
                }
                else if (i != i2 && Current_Room.Room_X_Pos == Rooms[i2].Room_X_Pos && Current_Room.Room_Y_Pos + 1 == Rooms[i2].Room_Y_Pos)
                {
                    Current_Room.Up = Rooms[i2];
                }
                else if (i != i2 && Current_Room.Room_X_Pos == Rooms[i2].Room_X_Pos && Current_Room.Room_Y_Pos - 1 == Rooms[i2].Room_Y_Pos)
                {
                    Current_Room.Down = Rooms[i2];
                }
            }
        }
        //Assign States
        for (int i = 0; i < Rooms.Count; i++)
        {
            Rooms[i].Assigned = true;
            //Up
            if (Rooms[i].Up != null && !Rooms[i].Up.Assigned)
            {
                if (Random.Range(0, Wall_Chance) == 0)
                {
                    Rooms[i].Can_Move_Up = false;
                    Rooms[i].Up.Can_Move_Down = false;
                }
                else
                {
                    Rooms[i].Can_Move_Up = true;
                    Rooms[i].Up.Can_Move_Down = true;
                }
            }
            //Down
            if (Rooms[i].Down != null && !Rooms[i].Down.Assigned)
            {
                if (Random.Range(0, Wall_Chance) == 0)
                {
                    Rooms[i].Can_Move_Down = false;
                    Rooms[i].Down.Can_Move_Up = false;
                }
                else
                {
                    Rooms[i].Can_Move_Down = true;
                    Rooms[i].Down.Can_Move_Up = true;
                }
            }
            //Left
            if (Rooms[i].Left != null && !Rooms[i].Left.Assigned)
            {
                if (Random.Range(0, Wall_Chance) == 0)
                {
                    Rooms[i].Can_Move_Left = false;
                    Rooms[i].Left.Can_Move_Right = false;
                }
                else
                {
                    Rooms[i].Can_Move_Left = true;
                    Rooms[i].Left.Can_Move_Right = true;
                }
            }
            //Right
            if (Rooms[i].Right != null && !Rooms[i].Right.Assigned)
            {
                if (Random.Range(0, Wall_Chance) == 0)
                {
                    Rooms[i].Can_Move_Right = false;
                    Rooms[i].Right.Can_Move_Left = false;
                }
                else
                {
                    Rooms[i].Can_Move_Right = true;
                    Rooms[i].Right.Can_Move_Left = true;
                }
            }
        }
        //Make Walls
        foreach (Room Current_Room in Rooms)
        {
            if (!Current_Room.Can_Move_Down)
            {
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 1, Current_Room.Tile_Y_Pos, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 2, Current_Room.Tile_Y_Pos, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 3, Current_Room.Tile_Y_Pos, 0), Ground);
            }
            if (!Current_Room.Can_Move_Up)
            {
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 1, Current_Room.Tile_Y_Pos + 4, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 2, Current_Room.Tile_Y_Pos + 4, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 3, Current_Room.Tile_Y_Pos + 4, 0), Ground);
            }
            if (!Current_Room.Can_Move_Left)
            {
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos, Current_Room.Tile_Y_Pos + 1, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos, Current_Room.Tile_Y_Pos + 2, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos, Current_Room.Tile_Y_Pos + 3, 0), Ground);
            }
            if (!Current_Room.Can_Move_Right)
            {
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 4, Current_Room.Tile_Y_Pos + 1, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 4, Current_Room.Tile_Y_Pos + 2, 0), Ground);
                TM.SetTile(new Vector3Int(Current_Room.Tile_X_Pos + 4, Current_Room.Tile_Y_Pos + 3, 0), Ground);
            }
        }
        //Gards
        for (int i = 0; i < Rooms.Count; i++)
        {
            if (Random.Range(0, Gard_Chance) == 0)
            {
                Room Current_Room = Rooms[i];
                Gard_Movement New_Gard = Instantiate(Gard, new Vector2(Current_Room.Tile_X_Pos + 2.5f, Current_Room.Tile_Y_Pos + 2.5f), Quaternion.identity).GetComponent<Gard_Movement>();
                New_Gard.Walk_Positions = new List<Vector2>();
                for (int it = 0; it < 5; it++)
                {
                    New_Gard.Walk_Positions.Add(new Vector2(Current_Room.Tile_X_Pos + 2.5f, Current_Room.Tile_Y_Pos + 2.5f));
                    List<Room> Posible_Rooms = new List<Room>();
                    if (Current_Room.Can_Move_Down)
                    {
                        Posible_Rooms.Add(Current_Room.Down);
                    }
                    if (Current_Room.Can_Move_Up)
                    {
                        Posible_Rooms.Add(Current_Room.Up);
                    }
                    if (Current_Room.Can_Move_Left)
                    {
                        Posible_Rooms.Add(Current_Room.Left);
                    }
                    if (Current_Room.Can_Move_Right)
                    {
                        Posible_Rooms.Add(Current_Room.Right);
                    }
                    if (Posible_Rooms.Count != 0)
                    {
                        Current_Room = Posible_Rooms[Random.Range(0, Posible_Rooms.Count)];
                    }
                }
            }
        }
    }
}
public class Room
{
    public bool Assigned = false;

    public Room Up = null;
    public Room Down = null;
    public Room Left = null;
    public Room Right = null;

    public int Room_X_Pos = 0;
    public int Room_Y_Pos = 0;
    public int Tile_X_Pos = 0;
    public int Tile_Y_Pos = 0;

    public bool Can_Move_Up = false;
    public bool Can_Move_Down = false;
    public bool Can_Move_Left = false;
    public bool Can_Move_Right = false;
}
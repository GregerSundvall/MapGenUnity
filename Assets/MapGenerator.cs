using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class Int2
{
    public int X;
    public int Y;

    public Int2()
    {
        X = 0;
        Y = 0;
    }
    public Int2(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Door
{
    public Int2 Position = new Int2();
    public Int2 Direction = new Int2();

    public Door(int posX, int posY, int dirX, int dirY)
    {
        Position.X = posX;
        Position.Y = posY;
        Direction.X = dirX;
        Direction.Y = dirY;
    }
}

public class Room
{
    public int PosX;
    public int PosY;
    public int Width;
    public int Height;
    public List<Door> Doors = new List<Door>();
    
    public Room(){}

    public Room(int posX, int posY, int width, int height)
    {
        PosX = posX;
        PosY = posY;
        Width = width;
        Height = height;
    }
    
    public void AddDoor(Door door)
    {
        Doors.Add(door);
    }
}

public class Map
{
    public int Width { get; }
    public int Height { get; }

    private List<Room> rooms = new List<Room>();
    private List<Door> doors = new List<Door>();
    private List<List<int>> map;
    

    public Map(int width, int height, int startX, int startY)
    {
        Width = width;
        Height = height;
        map = new List<List<int>>();

        for (int x = 0; x < Width; x++)
        {
            map.Add(new List<int>());
            for (int y = 0; y < Height; y++)
            {
                map[x].Add(1);
            }
        }
        
        Door start = CreateDoor(startX, startY, 1, 0);
        CreateRoomFrom(start);
        CreateRoomFrom(GetLastDoor());
        CreateRoomFrom(GetLastDoor());
        CreateRoomFrom(GetLastDoor());
        CreateRoomFrom(GetLastDoor());
        CreateRoomFrom(GetLastDoor());
        CreateRoomFrom(GetLastDoor());
        CreateRoomFrom(GetLastDoor());
    }
    
    private Door GetLastDoor()
    {
        return doors[^1];
    }

    public Door CreateDoor(int posX, int posY, int dirX, int dirY)
    {
        Door door = new Door(posX, posY, dirX, dirY);
        doors.Add(door);
        SetAt(door.Position.X, door.Position.Y, 0);
        return door;
    }
    
    public int GetAt(int x, int y)
    {
        return map[x][y];
    }

    private void SetAt(int x, int y, int newValue)
    {
        map[x][y] = newValue;
    }
    
    public void CreateRoomFrom(Door entrance)
    {
        var roomSize = new Int2(Random.Range(5, 12), Random.Range(5, 12));
        var startX = entrance.Position.X +1; // Room starts 1 column after door
        var endX = startX + roomSize.X;
        var startY = entrance.Position.Y - Random.Range(0, roomSize.Y);
        var endY = startY + roomSize.Y;
        
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (GetAt(x, y) != 1)
                {
                    return;
                }
            }
        }
        
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                SetAt(x, y, 0);
            }
        }

        var room = new Room(entrance.Position.X, entrance.Position.Y, roomSize.X, roomSize.Y);
        rooms.Add(room);

        Door newDoor = CreateDoor(endX +1, Random.Range(startY, endY), 1, 0);
        room.AddDoor(newDoor);
    }
    
    private Int2 GetRandomDirection(Int2 previousDirection)
    {
        Int2 newDirection = new Int2();
        int random = Random.Range(0, 3);
        for (int i = 0; i < random; i++)
        {
            newDirection.Y = previousDirection.X * -1;
            newDirection.X = previousDirection.Y;
        }

        return newDirection;
    }
}





public class MapGenerator : MonoBehaviour
{
    private Map map;
    // private Door startPosition = new Door(0, 10, 1, 0);

    [SerializeField] private GameObject prefab;
    
    private void Start()
    {
        map = new Map(100, 100, 0, 50);
        
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (map.GetAt(x, y) == 1)
                {
                    Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }
    }
    
    
    

}

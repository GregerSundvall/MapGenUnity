using System;
using System.Collections;
using System.Collections.Generic;
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
    public int PositionX;
    public int PositionY;
    public int DirectionX;
    public int DirectionY;
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
}

public class Map
{
    public int Width;
    public int Height;
    public Int2 Start;
    public List<Room> Rooms = new List<Room>();
    
    public List<List<int>> map;
    

    public Map()
    {
        Width = 100;
        Height = 100;
        Start = new Int2(0, 0);
        map = new List<List<int>>();

        for (int x = 0; x < Width; x++)
        {
            map.Add(new List<int>());
            for (int y = 0; y < Height; y++)
            {
                map[x].Add(1);
            }
        }
    }

    
    public int AtPosition(int x, int y)
    {
        return map[x][y];
    }
}





public class MapGenerator : MonoBehaviour
{
    private Map map = new Map();
    private Int2 startPosition = new Int2(10, 10);

    [SerializeField] private GameObject prefab;
    
    private void Start()
    {   
        AddRoomFrom(startPosition);
        
        for (int x = 0; x < map.Width; x++)
        {
            for (int y = 0; y < map.Height; y++)
            {
                if (map.AtPosition(x, y) == 1)
                {
                    Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity);
                }
            }
        }

    }

    public bool AddRoomFrom(Int2 position)
    {
        var roomSize = new Int2(Random.Range(5, 12), Random.Range(5, 12));
        var startX = position.X;
        var endX = startX + roomSize.X;
        var startY = position.Y + roomSize.Y;
        // var startY = position.Y + roomSize.Y / 2;
        var endY = startY + roomSize.Y;
        // var endY = startY - roomSize.Y;
        
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                if (map.AtPosition(x, y) == 0)
                {
                    return false;
                }
            }
        }
        
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                map.map[x][y] = 0;
            }
        }

        var room = new Room(position.X, position.Y, roomSize.X, roomSize.Y);
        map.Rooms.Add(room);
        return true;
    }


    
    

}

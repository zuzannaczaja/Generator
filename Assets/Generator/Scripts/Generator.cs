using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Generator : MonoBehaviour
{
    public List<ItemList> Items = new List<ItemList>();
    public List<EnemyList> Enemies = new List<EnemyList>();

    public List<GameObject> rooms;
    public GameObject startRoom;
    public GameObject wall;
    public List<GameObject> wallDecors = new List<GameObject>();
    public List<GameObject> floorDecors = new List<GameObject>();

    public int countRooms;

    List<GameObject> placedRooms = new List<GameObject>();
    List<GameObject> availableRooms = new List<GameObject>();
    List<GameObject> otherExits = new List<GameObject>();
    
    List<GameObject> placesForFloorDecors = new List<GameObject>();
    List<GameObject> placesForItems = new List<GameObject>();
    List<GameObject> placesForEnemies = new List<GameObject>();
    List<GameObject> placesForWallDecors = new List<GameObject>();

    public GameObject teleport;
    public GameObject player;

    public int seed;
    public int seedToCopy;
    System.Random random;
    void Start()
    {       
        
        if (seed == 0)
        {
            random = new System.Random();
            seed = random.Next();
            random = new System.Random(seed);
        }
        else
        {
            random = new System.Random(seed);
        }

        do
        {
           
            if(placedRooms.Count > 0)
            {
                foreach (GameObject place in placedRooms)
                {
                    Destroy(place);
                }

                placesForFloorDecors.Clear();
                placesForEnemies.Clear();
                placesForItems.Clear();
                placedRooms.Clear();
                placesForWallDecors.Clear();
                availableRooms.Clear();
                otherExits.Clear();
            }
                
            generateFirstRoom();

            for (int i = 0; i < countRooms - 1; i++)
            {
                GenerateRoom();
            }

            if (placedRooms.Count < countRooms)
            {
                seed = random.Next();
                random = new System.Random(seed);
            }


        } while (placedRooms.Count < countRooms);      

        insertWalls();

        //Generowanie gracza

        if (player != null)
        {
            GameObject currentPlayer = Instantiate(player);
            GameObject place = placedRooms[0].GetComponent<Room>().getPlaceForPlayer();
            currentPlayer.transform.position = new Vector3(place.transform.position.x, 3f, place.transform.position.z);
            currentPlayer.transform.rotation = place.transform.rotation;
        }

        //Generowanie teleportu

        if (teleport != null)
        {
            GameObject port = Instantiate(teleport);
            GameObject place = placedRooms[placedRooms.Count - 1].GetComponent<Room>().getPlaceForTeleport();
            float sizey = port.GetComponent<Renderer>().bounds.size.y;
            port.transform.position = new Vector3(place.transform.position.x, sizey / 2, place.transform.position.z);
        }

        if (placesForFloorDecors.Count > 0 && placesForWallDecors.Count > 0)
        {
            GenerateDecor();
        }

        if (placesForItems.Count > 0)
        {
            generateItem();
        }

        if (placesForEnemies.Count > 0)
        {
            generateEnemies();
        }
    }

    /* Generowanie pokoi */
    public void GenerateRoom()
    {
        GameObject currentRoom;
        GameObject currentRoomExit;
        List<GameObject> availableRoomExits;
        List<GameObject> currentRoomExits;

        List<GameObject> randomRooms = new List<GameObject>();

        while (rooms.Count > 0)
        {
            int randomIndex = random.Next(rooms.Count);
            randomRooms.Add(rooms[randomIndex]);
            rooms.RemoveAt(randomIndex);
        }
        rooms = randomRooms;

        bool placed = false;

        foreach (GameObject availableRoom in availableRooms.ToList())
        {
            availableRoomExits = availableRoom.GetComponent<Room>().getExits();

            foreach (GameObject availableRoomExit in availableRoomExits)
            {                
                foreach (GameObject room in rooms)
                {
                    currentRoom = Instantiate(room);

                    currentRoomExits = currentRoom.GetComponent<Room>().getExits();                  
                    currentRoomExit = currentRoomExits[random.Next(currentRoomExits.Count)];

                    float diffrenceBetweenExits = availableRoomExit.transform.eulerAngles.y - currentRoomExit.transform.eulerAngles.y;

                    currentRoom.transform.rotation = Quaternion.AngleAxis(diffrenceBetweenExits + 180f, Vector3.up);

                    Vector3 differenceBetweenPosition = currentRoomExit.transform.position - currentRoom.transform.position;
                    currentRoom.transform.position = availableRoomExit.transform.position - differenceBetweenPosition;

                    if (!checkOverlap(currentRoom, availableRoom))
                    {
                        placedRooms.Add(currentRoom);
                        availableRooms.Add(currentRoom);
                        availableRoom.GetComponent<Room>().removeExit(availableRoomExit);
                        currentRoom.GetComponent<Room>().removeExit(currentRoomExit);

                        if (availableRoom.GetComponent<Room>().getExits().Count == 0)
                        {
                            availableRooms.Remove(availableRoom);
                        }

                        if (currentRoom.GetComponent<Room>().getExits().Count == 0)
                        {
                            availableRooms.Remove(currentRoom);
                        }

                        if (currentRoom.GetComponent<Room>().getPlacesForFloorDecor().Count > 0)
                        {
                            placesForFloorDecors.AddRange(currentRoom.GetComponent<Room>().getPlacesForFloorDecor());
                        }

                        if (currentRoom.GetComponent<Room>().getPlacesForItems().Count > 0)
                        {
                            placesForItems.AddRange(currentRoom.GetComponent<Room>().getPlacesForItems());
                        }

                        if (currentRoom.GetComponent<Room>().getPlacesForWallDecors().Count > 0)
                        {
                            placesForWallDecors.AddRange(currentRoom.GetComponent<Room>().getPlacesForWallDecors());
                        }

                        if (currentRoom.GetComponent<Room>().getPlacesForEnemies().Count > 0)
                        {
                            placesForEnemies.AddRange(currentRoom.GetComponent<Room>().getPlacesForEnemies());
                        }

                        placed = true;

                        break;
                    }
                }                  

                if (placed)
                {
                    break;
                }
            }

            if (placed)
            {
                break;
            }
        }
    }

    /* Generowanie pierwszego pokoju */
    public void generateFirstRoom()
    {
        seedToCopy = seed;

        GameObject firstRoom = Instantiate(startRoom);
        placedRooms.Add(firstRoom);
        availableRooms = placedRooms.ToList();
        firstRoom.transform.position = new Vector3(0, 0, 0);

        if (firstRoom.GetComponent<Room>().getPlacesForWallDecors().Count > 0)
        {
            placesForWallDecors.AddRange(firstRoom.GetComponent<Room>().getPlacesForWallDecors());
        }
    }

    /* Sprawdzanie czy pokój nałożył się na inny już umieszczony pokój */
    private bool checkOverlap(GameObject currentRoom, GameObject availableRoom)
    {
        var bound = currentRoom.GetComponentInChildren<Collider>().bounds;
        bool collision = false;
        foreach (GameObject placedRoom in placedRooms)
        {
            if (bound.Intersects(placedRoom.GetComponentInChildren<Collider>().bounds))
            {
                if (placedRoom == availableRoom)
                {
                    continue;
                }

                Destroy(currentRoom);
                collision = true;
                break;
            }
        }

        return collision;
    }

    /* Wstawianie ścian w puste wejścia */
    private void insertWalls()
    {
        foreach (GameObject availableRoom in availableRooms)
        {
            otherExits.AddRange(availableRoom.GetComponent<Room>().getExits());
        }

        foreach (GameObject place in otherExits)
        {
            GameObject currentWall = Instantiate(wall);
            currentWall.transform.position = new Vector3(place.transform.position.x, 1.5f, place.transform.position.z);
            currentWall.transform.rotation = place.transform.rotation;
        }
        seed = 0;
    }

    /* Generowanie dekoracji */
    void GenerateDecor()
    {
        for(int i = 0; i < placesForFloorDecors.Count; i++)
        {
            GameObject place = placesForFloorDecors[i];
            GameObject floorDecor = Instantiate(floorDecors[random.Next(floorDecors.Count)]);
           
            floorDecor.transform.position = new Vector3(place.transform.position.x, floorDecor.transform.position.y, place.transform.position.z);
            floorDecor.transform.rotation = place.transform.rotation;

        }

        for (int i = 0; i < placesForWallDecors.Count; i++)
        {
              GameObject place = placesForWallDecors[i];
              GameObject wallDecor = Instantiate(wallDecors[random.Next(wallDecors.Count)]);
             
              wallDecor.transform.position = new Vector3(place.transform.position.x, place.transform.position.y, place.transform.position.z);
              wallDecor.transform.rotation = place.transform.rotation;
        }
    }

    /* Generowanie przedmiotów */
    void generateItem()
    {
        List<GameObject> items = new List<GameObject>();

        for (int i = 0; i < Items.Count; i++)
        {
            ItemList item = Items[i];
            int itemCount = random.Next(item.randomMinimum, item.randomMaximum);

            for (int j = 0; j < itemCount; j++)
            {
                items.Add(item.item);
            }
        }

        List<GameObject> randomItems = new List<GameObject>();

        while (items.Count > 0)
        {
            int randomIndex = random.Next(items.Count);
            randomItems.Add(items[randomIndex]);
            items.RemoveAt(randomIndex);
        }
        items = randomItems;

        for (int i = 0; i < items.Count; i++)
        {
            GameObject place = null;

            if (placesForItems.Count > 0)
            {
                place = placesForItems[random.Next(placesForItems.Count)];
            }
            else
            {
                break;
            }

            GameObject item = Instantiate(items[i]);

            item.transform.position = new Vector3(place.transform.position.x, 3f, place.transform.position.z);

            placesForItems.Remove(place);

            if (items.Count > placesForItems.Count)
            {
                if (i == items.Count - 1)
                {
                    break;
                }
            }        
        }       
    }

    /* Generowanie przeciwników */
    void generateEnemies()
    {
        List<GameObject> enemies = new List<GameObject>();

        for (int i = 0; i < Enemies.Count; i++)
        {
            EnemyList enemy = Enemies[i];

            for (int j = 0; j < enemy.count; j++)
            {
                enemies.Add(enemy.enemy);
            }
        }

        List<GameObject> randomEnemies = new List<GameObject>();

        while (enemies.Count > 0)
        {
            int randomIndex = random.Next(enemies.Count);
            randomEnemies.Add(enemies[randomIndex]);
            enemies.RemoveAt(randomIndex);
        }
        enemies = randomEnemies;

        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject place = null;

            if (placesForEnemies.Count > 0)
            {
                place = placesForEnemies[random.Next(placesForEnemies.Count)];

            } else
            {
                break;
            }
            
            GameObject enemy = Instantiate(enemies[i]);

            enemy.transform.position = new Vector3(place.transform.position.x, 3f, place.transform.position.z);

            placesForEnemies.Remove(place);

            if (enemies.Count > placesForEnemies.Count)
            {
                if (i == enemies.Count - 1)
                {
                    break;
                }
            }
        }
    }
}

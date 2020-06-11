using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Room : MonoBehaviour
{
    public List<GameObject> exits = new List<GameObject>();
    public List<GameObject> placesForItems = new List<GameObject>();
    public List<GameObject> placesForEnemies = new List<GameObject>();
    public List<GameObject> placesForFloorDecors = new List<GameObject>();
    public List<GameObject> placesForWallDecors = new List<GameObject>();
    public GameObject placeForTeleport;
    public GameObject placeForPlayer;

    public List<GameObject> getExits()
    {
        return exits;
    }

    public void removeExit(GameObject removeExit)
    {
        exits.Remove(removeExit);
    }

    public List<GameObject> getPlacesForItems()
    {
        return placesForItems;
    }

    public List<GameObject> getPlacesForWallDecors()
    {
        return placesForWallDecors;
    }

    public List<GameObject> getPlacesForFloorDecor()
    {
        return placesForFloorDecors;
    }

    public List<GameObject> getPlacesForEnemies()
    {
        return placesForEnemies;
    }

    public GameObject getPlaceForTeleport()
    {
        return placeForTeleport;
    }

    public GameObject getPlaceForPlayer()
    {
        return placeForPlayer;
    }
}

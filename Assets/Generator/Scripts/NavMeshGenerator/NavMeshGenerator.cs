using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(0.5f));
    }
    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        NavMeshSurface nm = FindObjectOfType<NavMeshSurface>();
        nm.BuildNavMesh();
    }
}

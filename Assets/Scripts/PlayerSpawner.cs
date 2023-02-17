using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] models;
    public GameObject SpawnNewClass(Vector3 playerPos)
    {
        GameObject model =Instantiate(models[Random.Range(0, 2)], new Vector3(transform.position.x, playerPos.y, transform.position.z), Quaternion.identity);
        return model;
    }
}

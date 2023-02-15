using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int enemyMaxHealth;
    int enemyHealth;
    public bool isArmored;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
    }
}

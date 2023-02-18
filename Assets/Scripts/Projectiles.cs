using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public int power;
    public int damage;
    public int timeToDestroy;
    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            
            
                collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);

            
            Destroy(gameObject);
        }
    }

}

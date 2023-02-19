using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    public int power;
    public int damage;
    public int timeToDestroy;
    public bool isTank;
    public bool isSniper;
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
        if (isTank)
        {
            collision.gameObject.GetComponent<Rigidbody>().AddExplosionForce(power, this.transform.position, 3f);
        }
        if (collision.gameObject.tag == "Player")
        {
            if (isTank)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            }
            if (isSniper)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
                FindObjectOfType<EnemyController>().sniperMiss = false;
                Destroy(gameObject);
            }
        }
        if (collision.gameObject.tag == "SniperMiss"&&isSniper)
        {
            FindObjectOfType<EnemyController>().sniperMiss = true;
        }

    }

}

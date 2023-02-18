using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int enemyMaxHealth;
    int enemyHealth;
    public bool isTank, isSniper, isCharger, isSoldier;
    public bool isAttacking;
    [SerializeField] GameObject tankCannon;
    public GameObject target;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] int ammo;


    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = enemyMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTank && isAttacking && ammo > 0)
        {
            StartCoroutine(AttackDelay(2));
            ammo--;
        }
        if (isSniper && isAttacking && ammo > 0)
        {
            StartCoroutine(AttackDelay(1.5f));
            ammo--;
        }
    }

    public void TakeDamage(int damage)
    {
        enemyHealth -= damage;
    }
    public IEnumerator TankAttack(Vector3 targetPosition, float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(explosionPrefab, targetPosition, Quaternion.identity);


    }
    public IEnumerator SniperAttack(Vector3 targetPosition, float time)
    {
        yield return new WaitForSeconds(time);


    }
    public IEnumerator AttackDelay(float attackTime)
    {
        target = FindObjectOfType<PlayerController>().gameObject;
        Vector3 targetPosition = target.transform.position;
        yield return new WaitForSeconds(attackTime);
        Vector3 fixedTargetPosition = targetPosition;
        if (isTank)
        {
            StartCoroutine(TankAttack(fixedTargetPosition, 2f));
        }
        else if (isSniper)
        {
            StartCoroutine(SniperAttack(fixedTargetPosition, 1.5f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isAttacking = true;
    }
}

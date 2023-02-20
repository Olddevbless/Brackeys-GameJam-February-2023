using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] int enemyMaxHealth;
    int enemyHealth;
    public bool isTank, isSniper, isCharger, isSoldier;
    public bool isAttacking;
    [SerializeField] GameObject tankCannon;
    public GameObject target;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject sniperShotPrefab;
    [SerializeField] int ammo;
    [SerializeField] float attackTimer = 0f;
    [SerializeField] float attackDelay = 2f; // default delay time
    [SerializeField] Image sniperShotImage;
    [SerializeField] Sprite[] sniperShotSprites;
    [SerializeField] int sniperDamage;
    public bool sniperMiss;
    float sniperShotTimer;
    [SerializeField] float maxSniperShotTime;
    // Start is called before the first frame update
    void Start()
    {
        enemyHealth = enemyMaxHealth;
        Image sniperShotImage = GameObject.FindGameObjectWithTag("SniperShotImage").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTank && isAttacking && ammo > 0 && attackTimer <= 0f)
        {
            StartCoroutine(AttackDelay(1));
            ammo--;
            attackTimer = attackDelay;
        }
        if (isSniper && isAttacking && ammo > 0 && attackTimer <= 0f)
        {
            StartCoroutine(AttackDelay(0.5f));
            ammo--;
            attackTimer = attackDelay;
        }

        // Update the attack timer.
        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
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
        //Vector3 dir = (targetPosition - this.transform.position).normalized;
        RaycastHit sniperrayHit;
        Ray sniperRay = new Ray(transform.position, targetPosition);
        Debug.DrawLine(transform.position, targetPosition, Color.black,10f);
        if (Physics.Raycast(sniperRay, out sniperrayHit, Mathf.Infinity))
        {
            Debug.Log(sniperrayHit.collider.name);
            if (sniperrayHit.collider.tag=="Player")
            {
                sniperrayHit.collider.gameObject.GetComponent<PlayerController>().TakeDamage(sniperDamage);
            }
        }
        else 
        {
            ActiveSniperShotPNG(sniperShotSprites);
            
        }
    }

    public IEnumerator AttackDelay(float attackTime)
    {
        target = FindObjectOfType<PlayerController>().gameObject;
        Vector3 targetPosition = target.transform.position;
        yield return new WaitForSeconds(attackTime);
        Vector3 fixedTargetPosition = targetPosition;
        if (isTank)
        {
            StartCoroutine(TankAttack(fixedTargetPosition, 0.5f));
        }
        else if (isSniper)
        {
            StartCoroutine(SniperAttack(new Vector3 (fixedTargetPosition.x,fixedTargetPosition.y+2,fixedTargetPosition.z), 0.2f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isAttacking = true;
        }
    }
    void ActiveSniperShotPNG(Sprite[] sprites)
    {

        sniperShotImage.sprite = sprites[Random.Range(0, 2)];
        sniperShotImage.transform.position = new Vector3(Random.Range(-200, 200), Random.Range(-200, 200), 0);
        sniperShotImage.enabled = true;
        sniperShotTimer -= Time.deltaTime;
        if (sniperShotTimer == 0)
        {
            sniperShotImage.enabled = false;
        }
        sniperShotTimer = maxSniperShotTime;
    }
}

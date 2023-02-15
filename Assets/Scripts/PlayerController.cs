using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    int playerHealth;
    [SerializeField] int playerMaxHealth;
    [Header("Class Properties")]
    [SerializeField] GameObject model;
    [SerializeField] GameObject[] weapons;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] GameObject currentWeapon;
    [SerializeField] Classes currentClass;
    [SerializeField] int weaponIndex = 0;
    [SerializeField] int throwForce;
    [SerializeField] GameObject mousePointer;
    [SerializeField] Camera cam;
    Vector3 dir;
    Camera mainCam;
    Vector3 mousePos;
    [Header("Movement")]
    Vector2 movement;
    [SerializeField] int standProneSpeed;
    [SerializeField] int slideForce;
    [SerializeField] int jumpForce;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        model = GetComponentInChildren<Classes>().model;
        currentClass = model.GetComponent<Classes>();
        weapons = currentClass.weapons;
        currentWeapon = weapons[weaponIndex];
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleAim();
        HandleInput();
        if (playerHealth<=0)
        {
            ChangeSoldier();
            playerHealth = playerMaxHealth;
        }


    }
    void ChangeSoldier()
    {
        //change to a different soldier
        //change class -> class changes weapon
        
        model = GetComponentInChildren<Classes>().model;
        currentClass = model.GetComponent<Classes>();
        weapons = currentClass.weapons;
    }
    void HandleInput()
    {
        //mouseLook with the gun


        if (playerInput.actions["Fire"].WasPressedThisFrame())
        {
            //Fire Gun
            if (currentWeapon.GetComponent<Weapon>())
            {
                //need to add firing rate
                GameObject bullet = Instantiate(currentWeapon.GetComponent<Weapon>().projectilePrefab, transform.position, Quaternion.identity);
                bullet.transform.LookAt(mousePointer.transform.position);
                bullet.GetComponent<Rigidbody>().AddForce(dir * currentWeapon.GetComponent<Weapon>().projectilePrefab.GetComponent<Projectiles>().power, ForceMode.Impulse);
            }
            else if (currentWeapon.GetComponent<WeaponMelee>())
            {
                //stab/cut/smack animation
                RaycastHit hit;
                Ray hitRay = new Ray(currentWeapon.transform.position, dir * currentWeapon.GetComponent<WeaponMelee>().attackRange);
                Debug.DrawRay(currentWeapon.transform.position, dir * currentWeapon.GetComponent<WeaponMelee>().attackRange);
                if (Physics.Raycast(hitRay, out hit, currentWeapon.GetComponent<WeaponMelee>().attackRange))
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        hit.collider.gameObject.GetComponent<EnemyController>().TakeDamage(currentWeapon.GetComponent<WeaponMelee>().damage);
                    }
                }
                //need to add hit effect and attack cooldown
            }

        }

        if (playerInput.actions["ThrowGrenade"].WasPressedThisFrame())
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            grenade.GetComponent<Rigidbody>().AddForce(dir * throwForce, ForceMode.Impulse);
        }
        if (playerInput.actions["SwapWeapon"].WasPressedThisFrame())
        {
            if (weaponIndex < weapons.Length)
            {
                weaponIndex++;
            }

            //Swap Weapon
            if (weaponIndex >= weapons.Length)
            {
                weaponIndex = 0;
            }
            currentWeapon = weapons[weaponIndex];


        }

    }
    
    void HandleMovement()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>();
        transform.Translate(new Vector3(movement.x * model.GetComponent<Classes>().classSpeed* standProneSpeed * Time.deltaTime, 0, 0));

        if (playerInput.actions["Jump"].WasPressedThisFrame())
        {
            this.GetComponent<Rigidbody>().AddForce(transform.up * jumpForce);
            //play jump animation
        }
        if (playerInput.actions["Slide"].WasPressedThisFrame())
        {
            
            this.GetComponent<Rigidbody>().AddForce(dir * slideForce);
            //play slide animation
        }
        if (playerInput.actions["Stand"].IsPressed())
        {
            //Stand tall
            gameObject.GetComponent<BoxCollider>().size = model.GetComponent<Classes>().boxColliderStand;
            standProneSpeed = model.GetComponent<Classes>().standSpeed;
        }
        else if (playerInput.actions["Prone"].IsPressed())
        {
            gameObject.GetComponent<BoxCollider>().size = model.GetComponent<Classes>().boxColliderProne; // set collider height to crouch height
            standProneSpeed = model.GetComponent<Classes>().proneSpeed;
            // Prone animation

        }
        else
        {
            gameObject.GetComponent<BoxCollider>().size = model.GetComponent<Classes>().boxColliderCrouch;
            standProneSpeed = model.GetComponent<Classes>().standSpeed;
        }
    }
    void HandleAim()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        mousePointer.transform.position = ray.GetPoint(Vector3.Distance(ray.origin, transform.position));
        // get the mouse position in world space
        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // only use the x and y position of the mouse
        //mousePos.z = this.transform.position.z ;

        // make the player look towards the mouse position
        dir = (mousePointer.transform.position - transform.position).normalized;

    }
    void TakeDamage(int damage)
    {
        playerHealth = -damage;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyProjectiles"))
        {
            TakeDamage(collision.gameObject.GetComponent<Projectiles>().damage);
        }
    }
}

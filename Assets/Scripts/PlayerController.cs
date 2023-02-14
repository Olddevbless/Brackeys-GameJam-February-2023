using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] int classSpeed;
    [SerializeField] GameObject model;
    [SerializeField] GameObject[] weapons;
    [SerializeField] GameObject grenadePrefab;
    [SerializeField] GameObject holdergun;
    [SerializeField] GameObject currentWeapon;
    [SerializeField] Classes currentClass;
    [SerializeField] int weaponIndex;
    [SerializeField] int throwForce;
    [SerializeField] GameObject mousePointer;
    [SerializeField] Camera cam;
    Vector3 dir;
    Vector2 movement;
    Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
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
        ChangeSoldier();

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
            if (weaponIndex >=weapons.Length)
            {
                weaponIndex = 0;
            }
            currentWeapon = weapons[weaponIndex];


        }

    }
    void HandleMovement()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>();
        transform.Translate(new Vector3(movement.x * classSpeed * Time.deltaTime, 0, 0)); // left right movement
                                                                                          //foreach(GameObject g  in model)
                                                                                          //{
                                                                                          //g.transform.Translate(new Vector3(movement.y * classSpeed * Time.deltaTime, 0, 0));
                                                                                          //}
        if (playerInput.actions["Slide"].WasPressedThisFrame())
        {
            //Slide Forward
        }
        if (playerInput.actions["Stand"].IsPressed())
        {
            //Stand tall
        }
        else if (playerInput.actions["Prone"].IsPressed())
        {
            //Go prone
        }
        else
        {
            //Crouch
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
}

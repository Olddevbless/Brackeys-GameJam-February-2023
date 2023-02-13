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
        HandleInput(HandleAim());
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
    void HandleInput(Vector3 direction)
    {
       //mouseLook with the gun
       
        
        holdergun.transform.LookAt(mousePointer.transform);
        if (playerInput.actions["Fire"].WasPressedThisFrame())
        {
            //Fire Gun

            GameObject bullet = Instantiate(currentWeapon.GetComponent<Weapon>().projectilePrefab, transform.position, Quaternion.identity);
            
            bullet.GetComponent<Rigidbody>().AddForce(holdergun.transform.forward* currentWeapon.GetComponent<Weapon>().projectilePrefab.GetComponent<Projectiles>().power, ForceMode.Impulse);
        }
    
        if (playerInput.actions["ThrowGrenade"].WasPressedThisFrame())
        {
            GameObject grenade = Instantiate(grenadePrefab, transform.position, Quaternion.identity);
            grenade.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
        }
        if (playerInput.actions["SwapWeapon"].WasPressedThisFrame())
        {
            if (weaponIndex < weapons.Length)
            {
                weaponIndex++;
            }

            //Swap Weapon
            else
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
    Vector3 HandleAim()
    {
        mousePos = playerInput.actions["Look"].ReadValue<Vector2>();
        Debug.Log(mousePos);
        dir = mousePos - transform.position;
        return dir;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] int classSpeed;
    [SerializeField] GameObject[] models;
    [SerializeField] GameObject[] projectilePrefabs;
    [SerializeField] GameObject[] weapons;
    [SerializeField] GameObject gun;
    [SerializeField] Classes[] classes;
    Vector2 movement;
    Vector2 mousePos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInput();
    }
    void ChangeSoldier()
    {
        //change to a different soldier
        weapons = classes[0].weapons;
    }
    void HandleInput()
    {
        if (playerInput.actions["Fire"].WasPressedThisFrame())
        {
            //Fire Gun
            Vector3 direction = new Vector3(mousePos.x, mousePos.y, transform.position.z);
            gun.transform.LookAt(direction);
            GameObject bullet = Instantiate(projectilePrefabs[0/*needs to change based on weapon*/], gun.transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody>().AddForce(/*projectilePrefabs[0].power*/direction,ForceMode.Impulse);
        }
        if (playerInput.actions["Grenade"].WasPressedThisFrame())
        {
            //Toss Grenade
        }
        if (playerInput.actions["SwapWeapon"].WasPressedThisFrame())
        {
            //Swap Weapon
        }
        
    }
    void HandleMovement()
    {
        movement = playerInput.actions["Move"].ReadValue<Vector2>();
        models[0].transform.Translate(new Vector3(movement.x * classSpeed, 0, 0)); // left right movement
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
        mousePos = playerInput.actions["Look"].ReadValue<Vector2>();
        
    }
}

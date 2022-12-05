using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;

    //Creates an integer that says the maximum amount of health that Ruby can have
    public int maxHealth = 5;

    public float timeInvincible = 2.0f;
    
    public int health {  get { return currentHealth;  }}
    //Stores the health that Ruby currently has
    int currentHealth;

    bool isInvincible;
    float invincibleTimer;

    //Creates a new variable called rigidbody2d to store Rigidbody
    Rigidbody2D rigidbody2D;

    //Two more variables are created to input data for access (FixedUpdate)
    float horizontal;
    float vertical;

    public GameObject projectilePrefab;

    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        //Tells Unity to give Rigidbody2D to any GameObject that it is attached to
        rigidbody2D = GetComponent<Rigidbody2D>();

        //This sets Ruby's health to max as soon as the game begins
        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        //Creating the two variables that use the pre built axes
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

    }

    void FixedUpdate()
    {
        //Creates the movement vector but it is now adjusted to the Rigidbody position
        Vector2 position = rigidbody2D.position;

        //Changing the x and y position
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        //Updates the new movement according to the Rigidbody
        rigidbody2D.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
        }

        //Clamps the health digits to never go under 0 or over maxHealth(5)
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        //Displays the health in the console window
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    void Launch()
    {
       GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);

       Projectile projectile = projectileObject.GetComponent<Projectile>();
       projectile.Launch(lookDirection, 300);

       animator.SetTrigger("Launch");

    }
}

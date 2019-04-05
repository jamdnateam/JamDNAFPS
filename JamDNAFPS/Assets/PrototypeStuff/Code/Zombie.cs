using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class Zombie : MonoBehaviour
{

    public bool idleZombie;
    // zombie varables
    public float StartingHealth = 50f;
    public float health = 50f;
    public float attackDamage = 10;
    public float timeBetweenAttacks = 0.5f;
    public float holdAmount = 7f;

   
    
    // dist
    public float minDistanceToAttack = 7f;

    //static varables
    bool playerInRange;
    float Timer;
    GameObject[] players;
    // players
    public GameObject playerTarget;


    NavMeshAgent nav;
    // Use this for initialization
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        health = StartingHealth;
        nav = GetComponent<NavMeshAgent>();
        ChooseNewPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (idleZombie == true)
            return;
        if (playerTarget)
        {
            float dist = Vector3.Distance(playerTarget.transform.position, transform.position);
            Vector3 toTarget = (playerTarget.transform.position - transform.position).normalized;

            Debug.Log(dist);

            if (health > 0)
            {

                nav.SetDestination(playerTarget.transform.position);
            }
            if (playerTarget.gameObject.GetComponent<Player>().playerHealth == 0)
            {
                ChooseNewPlayer();
            }



            // if the timer exceed the time between attack , the player is in range and the enimy is alive
            // //////////////////////////////////////////////////////////////////////////////////////////////////////// Edit to work correct
            if (dist <= minDistanceToAttack && health > 0)
            {
                if (playerTarget.GetComponentInParent<FpsController>().jumpMultiplier < 30.0f)
                {
                    // do nothing
                }
                else if (playerTarget.GetComponentInParent<FpsController>().jumpMultiplier == 30.0f)
                {

                    playerTarget.GetComponentInParent<FpsController>().jumpMultiplier = 10f / holdAmount;
                    playerTarget.GetComponentInParent<FpsController>().walkSpeed = 2f / holdAmount;
                    playerTarget.GetComponentInParent<FpsController>().runSpeed = 2f / holdAmount;
                   

                }
                if (Timer >= timeBetweenAttacks && Vector3.Dot(toTarget, transform.forward) > 0)
                {
                    // attack here and anim stuff
                    Attack();
                }
            }
            else
            {
                if (playerTarget.GetComponentInParent<FpsController>().jumpMultiplier == 30.0f)
                {

                }
                else if (playerTarget.GetComponentInParent<FpsController>().jumpMultiplier < 30.0f)
                {
                    playerTarget.GetComponentInParent<FpsController>().jumpMultiplier = 30.0f;
                    playerTarget.GetComponentInParent<FpsController>().walkSpeed = 8.0f;
                    playerTarget.GetComponentInParent<FpsController>().runSpeed = 4.0f;
                   
                }

            }
        }
        else
        {

            // do idle zombie stuff like walk around
        }

    }

    public void TakeDamage(float amount)
    {
        Debug.Log(" Took Damage");
        health -= amount;
        if (health <= 0f)
        {
            Debug.Log("Died");
            Die();
        }

    }

    public void Die()
    {

        Destroy(gameObject);
    }

    public void ChooseNewPlayer()
    {



        foreach (GameObject player in players)
        {
            int number = Random.Range(0, players.Length);
            for (int i = 0; i < players.Length; i++)
            {
                if (i == number)
                {
                    if (players[i].gameObject.GetComponent<Player>().playerHealth > 0) { }
                    playerTarget = players[number];
                }
                else
                {
                    ChooseNewPlayer();
                }
            }
        }
    }


    // attack stuff
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (idleZombie)
            {
                playerTarget = other.gameObject;
                idleZombie = false;
            }
        }
    }



    public void Attack()
    {
        Timer = 0f;
        playerTarget.gameObject.GetComponent<Player>().TakeDamage(attackDamage);
        // sound effect here
    }

    public void GameOverCheck()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].gameObject.GetComponent<Player>().playerHealth > 0)
            {
                break;
            }

            if (i == players.Length)
            {
                // game over
            }
        }
    }
}
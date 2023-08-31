using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver = false;

    [SerializeField] private float floatForce;
    [SerializeField] private float gravityModifier;
    private Rigidbody playerRb;

    [SerializeField] private ParticleSystem explosionParticle1;
    
    [SerializeField] private ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    [SerializeField] private AudioClip moneySound;
    [SerializeField] private AudioClip explodeSound;
    [SerializeField] private AudioClip boingSound;


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

        

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && gameOver == false)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }
        if (transform.position.y >= 14.46f)
        {
            transform.position = new Vector3(transform.position.x, 14.46f, transform.position.z);
            playerRb.velocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle1.Play();
            
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            Debug.Log("Game Over!");
            StartCoroutine(PreventInstantGameOver());
            
            Debug.Log("Game Over!");
            if (gameOver == true)
            {
                Destroy(other.gameObject); 
            }
            
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            playerRb.AddForce(Vector3.up * 5, ForceMode.Force);
            playerAudio.PlayOneShot(boingSound,1.0f);
        }

    }

    IEnumerator PreventInstantGameOver()
    {
        yield return new WaitForSeconds(1);
        gameOver = true;
        Destroy(gameObject);
    }

}

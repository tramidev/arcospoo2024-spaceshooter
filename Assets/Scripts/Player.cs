using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Properties")]
    public int health = 100;
    [Tooltip("Player movement speed value.")]
    public float moveSpeed = 10f;
    [Tooltip("Padding for Player object's half don't go outside of MainCamera.")]
    public Vector2 padding = new Vector2( 0.3f, 1f );

    [Header("Player Cannon")]
    [Tooltip("Time in seconds that Ship waits until shoot again.")]
    public float fireRate = 0.1f;

    [Header("Audio Source")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    [Range(0, 1)]
    public float shootSoundVolume = 0.5f;

    public GameObject deathFX;

    public GameObject playerLaser;
    // Private Variables
    Coroutine firingCoroutine;
    float xMin;
    float xMax;
    float yMin;
    float yMax;


    public GameObject shieldGO;
    public bool onShield;
    private float shieldTime;

    // Start is called before the first frame update
    void Start()
    {
        SetupMoveBoundaries();
    }

    private void ChangeShieldStatus(bool enableShield, float shieldTime)
    {
        onShield = enableShield;
        shieldGO.SetActive(onShield);
        if (enableShield)
        {
            this.shieldTime += shieldTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        ShieldUpdate();
    }

    void ShieldUpdate()
    {
        if (onShield)
        {
            shieldTime -= Time.deltaTime;
            if (shieldTime <= 0)
            {
                ChangeShieldStatus(false,0);
            }
        }
    }

    IEnumerator FireAtWill()
    {
        while (true)
        {
            GameObject laser = Instantiate(playerLaser);
            if (laser != null)
            {
                laser.transform.position = transform.position;
                laser.transform.rotation = Quaternion.identity;
                laser.SetActive(true);
                audioSource.PlayOneShot(shootSound, shootSoundVolume);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireAtWill());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IShieldItem shieldItem = collision.gameObject.GetComponent<IShieldItem>();
        if (shieldItem!=null)
        {
            float shieldTime = shieldItem.GetShieldTime();
            ChangeShieldStatus(true,shieldTime);
            shieldItem.Hide();
            return;
        }
        
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if (damageDealer!=null)
        {
            if(onShield) return;
            TakeDamage(damageDealer.GetDamage());

            damageDealer.Hit();
        }
        
        
        

    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Load the GameOver Scene
        FindObjectOfType<Level>().LoadGameOver();

        deathFX.transform.position = transform.position;
        deathFX.transform.rotation = Quaternion.identity;
        deathFX.SetActive(true);

        Destroy(gameObject);
    }

    private void Move()
    {
        float deltaX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float deltaY = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;

        // Clamp Player movement to prevent him to go outside scene camera view
        float newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        float newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetupMoveBoundaries()
    {
        // Get the Scene's mainCamera object
        Camera gameCamera = Camera.main;

        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding.x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding.x;

        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding.y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding.y;
    }
}

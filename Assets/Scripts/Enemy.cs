using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Properties")]
    public float health = 100;
    public float shotCounter;
    public float minTimeBetweenShots = 0.2f;
    public float maxTimeBetweenShots = 3f;
    public float moveForwardSpeed = 2f;

    [Header("Audio Source")]
    public AudioSource audioSource;
    public AudioClip shootSound;
    [Range(0, 1)]
    public float shootSoundVolume = 0.5f;

    public GameObject EnemyLaser;
    public GameObject HitFX;
    

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        MoveForward();
        CountDownToShoot();
    }

    private void MoveForward()
    {
        transform.position += Vector3.down * (Time.deltaTime * moveForwardSpeed);
    }

    protected void CountDownToShoot()
    {
        shotCounter -= Time.deltaTime;
        if ( shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(EnemyLaser);
        if (laser != null)
        {
            laser.transform.position = transform.position;
            laser.transform.rotation = Quaternion.identity;
            laser.SetActive(true);
            audioSource.PlayOneShot(shootSound, shootSoundVolume);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();

        if ( damageDealer)
        {
            TakeDamage(damageDealer.GetDamage());

            damageDealer.Hit();
        }
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            GameObject explosionFX = Instantiate(HitFX);
            if (explosionFX)
            {
                explosionFX.transform.position = transform.position;
                explosionFX.transform.rotation = Quaternion.identity;
                explosionFX.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}

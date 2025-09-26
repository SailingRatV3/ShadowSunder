using System.Collections;
using UnityEngine;

public class RadialAttack : MonoBehaviour
{
    // NOTE: For Bosses For Now
    public GameObject projectilePrefab;
    public int numberofProjectiles = 12; // # of Projectiles (DEF change for different phases of the fight!!!)
    public float projectileSpeed = 5f;
    
    public int burstCount = 3;
    private float timeBetweenBurst = 0.5f;
    public float maxTime = 1f;
    public bool isShooting = false;
    public float rotationOffset = 0f;
    public float spiralSpeed = 5f;
    public bool varySpiral = false;
    // Wave control
    public float waveFrequency = 2f;  // how fast it oscillates
    public float waveAmplitude = 30f; // how wide the wave swings

    private float timeElapsed = 0f;
    public void ShootRadial()
    {
        // Define the radial angle
        float angleStep = 360f / numberofProjectiles;
        float angle = rotationOffset;

        for (int i = 0; i < numberofProjectiles; i++)
        {
            // Calc Direction
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 shootDirection = new Vector2(dirX, dirY);
            Vector3 spawnPosition = transform.position;
            
            // Create and shoot
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            
            // If to alternate Speed
            if (varySpiral)
            {
                float speed = projectileSpeed * Mathf.Sin(i*0.5f)*2f;
                projectilePrefab.GetComponent<Rigidbody2D>().linearVelocity = shootDirection * speed;

            }
            
            if (rb != null)
            {
                rb.linearVelocity = shootDirection.normalized * projectileSpeed;
            }
            angle += angleStep;
        }
    }

   public IEnumerator ShootRadialBursts()
    {
        /*
        timeBetweenBurst += Time.deltaTime;
        if (timeBetweenBurst > maxTime)
        {
            timeBetweenBurst = 0;
            for (int i = 0; i < burstCount; i++)
                    {
                       // Debug.Log("Shoot Radial Bursts " + i);
                        ShootRadial();
                        yield return new WaitForSeconds(timeBetweenBurst);
                    }*/
        for (int i = 0; i < burstCount; i++)
        {
            ShootRadial();
            yield return new WaitForSeconds(timeBetweenBurst);
        }
    }
        //isShooting = true;
        
        //isShooting = false;
    

   // Spiral Pattern
    public void ShootSpiral()
    {
        rotationOffset += spiralSpeed * Time.deltaTime;
        ShootRadial();
    }

    public void ShootWave()
    {
        timeElapsed += Time.deltaTime;
        // Calculate wave-based angle offset
        float waveOffset = Mathf.Sin(timeElapsed * waveFrequency) * waveAmplitude;

        FireWaveProjectiles(waveOffset);
    }
    
    public void FireWaveProjectiles(float angleOffset)
    {
        // Define the radial angle
        float angleStep = 360f / numberofProjectiles;
        float angle = angleOffset;

        for (int i = 0; i < numberofProjectiles; i++)
        {
            // Calc Direction
            float dirX = Mathf.Cos(angle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(angle * Mathf.Deg2Rad);

            Vector3 shootDirection = new Vector2(dirX, dirY);
            Vector3 spawnPosition = transform.position;
            
            // Create and shoot
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            
            
            if (rb != null)
            {
                rb.linearVelocity = shootDirection.normalized * projectileSpeed;
            }
            angle += angleStep;
        }
    }
    
    
}

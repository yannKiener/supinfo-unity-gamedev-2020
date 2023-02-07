using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : MonoBehaviour
{
    public GameObject explosionParticle;
    public float explosionScale = 1;
    public GameObject impactParticle;
    public float impactScale = 1;
    public AudioClip destroySound;

    private float destroyParticlesAfter = 5;
    private float destroySelfAfter = 20;
    private bool isAlive = true;

    private void Start()
    {
        Destroy(gameObject, destroySelfAfter);
    }

    private void DestroyFueltank()
    {
        isAlive = false;
        if (GameUtils.IsPlayerAlive())
        {
            Unit playerShip = GameUtils.GetPlayerGameObject().GetComponent<Unit>();
            playerShip.AddSpecialGun();
            playerShip.RefillFuel(30);
        }
        //Destroy animation
        if (explosionParticle != null)
        {
            //Play explosion & detach it from this GameObject
            GameObject explosionParticleInstance = Instantiate(explosionParticle, transform);
            SoundManager.PlaySound(explosionParticleInstance, destroySound, 1);
            Utils.PlayAllParticleSystemFrom(explosionParticleInstance, explosionScale, true, destroyParticlesAfter);
            explosionParticleInstance.transform.parent = null;
            Destroy(explosionParticleInstance, destroyParticlesAfter);

            //Then destroy gameObject
            Destroy(gameObject);
        }
        else
        {
            //If no explosion is set, destroy gameObject
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        Projectile projectileHit = col.gameObject.GetComponent<Projectile>();

        //Kill fuelTank if hit by projectile (only once)
        if (projectileHit != null && isAlive)
        {
            DestroyFueltank();
        }
    }
}

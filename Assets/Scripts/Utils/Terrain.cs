using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{

    public GameObject impactParticle;
    public float impactScale = 1;

    public void CreateImpact(GameObject projectile, Vector3 impactPoint, bool destroyProjectile)
    {
        GameObject groundImpact = Instantiate(impactParticle, impactPoint, transform.rotation);
        Utils.PlayAllParticleSystemFrom(groundImpact, impactScale,false,5);
        if (destroyProjectile)
        {
            Destroy(projectile);
        }
    }
}

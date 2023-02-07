using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float projectileSpeed;
    public float destroyProjectileAfter;
    public float damage;

    public bool isContinuous;
    public float timePerDamageTic;

    private bool isFriendly;
    private float timer;


    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        Destroy(gameObject, destroyProjectileAfter);
        //To fix camera's movement, we add the projectile as child of the camera so it moves with it.
        transform.SetParent(Utils.GetMainCamera().transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * projectileSpeed;
    }


    private void OnCollisionEnter(Collision col)
    {
        if (!isContinuous)
        {
            Unit shipHit = col.gameObject.GetComponent<Unit>();
            if (shipHit != null)
            {
                if (((shipHit.isFriendly && !isFriendly) || (!shipHit.isFriendly && isFriendly)))
                {
                    shipHit.Damage(GetClosestContactPoint(col).point, damage);
                }
            }

            Terrain terrainHit = col.gameObject.GetComponent<Terrain>();
            if (terrainHit != null)
            {
                //We send the gameObject if collide on terrain to destroy it
                terrainHit.CreateImpact(gameObject, col.GetContact(0).point, true);
            }
        }
    }

    private void OnCollisionStay(Collision col)
    {
        if (isContinuous && timer <= 0)
        {

            Unit shipHit = col.gameObject.GetComponent<Unit>();
            if (shipHit != null)
            {
                if (((shipHit.isFriendly && !isFriendly) || (!shipHit.isFriendly && isFriendly)))
                {
                        timer = timePerDamageTic;
                        shipHit.Damage(col.GetContact(0).point, damage);
                }
            }
            /*
            //Sadly collision with terrain isn't working well :'( 
            //TODO : Trouver pourquoi la collision plante autant 
            Terrain terrainHit = col.gameObject.GetComponent<Terrain>();
            if (terrainHit != null)
            {
                Debug.Log("terrain is hit");
                terrainHit.CreateImpact(gameObject, GetClosestContactPoint(col).point, false);
                timer = timePerDamageTic;
            }
            */
        }

        timer -= Time.deltaTime;
    }

    
    private ContactPoint GetClosestContactPoint(Collision col)
    {
        ContactPoint closestContactPoint = col.GetContact(0);

        foreach (ContactPoint currentContact in col.contacts)
        {
            if(GetDistanceFromMainCamera(currentContact.point) < GetDistanceFromMainCamera(closestContactPoint.point))
            {
                closestContactPoint = currentContact;
            }
        }

        return closestContactPoint;
    }

    private float GetDistanceFromMainCamera(Vector3 point)
    {
       return Vector3.Distance(Utils.GetMainCamera().transform.position, point);
    }


    public void SetFriendly(bool isFriendly)
    {
        this.isFriendly = isFriendly;
    }

    public bool IsFriendly()
    {
        return isFriendly;
    }
}

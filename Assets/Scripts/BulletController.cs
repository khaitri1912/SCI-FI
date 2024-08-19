using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletDecal;

    private float speed = 140f;
    private float timeToDestroy = 0.5f;

    public Vector3 target { get; set; }
    public bool hit { get; set; }

    public int damageAmount = 2;

    private void OnEnable()
    {
        Destroy(gameObject, timeToDestroy);
    }



    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < 0.1f)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("TrungDan");
        ContactPoint contact = other.GetContact(0);
        
        Destroy(gameObject, 1f);

        if (other.gameObject.tag == "Dragon")
        {
            other.gameObject.GetComponent<RedDragon>().TakeDamage(damageAmount);
            Destroy(gameObject, 0.1f);
        }
        else
        {
            //GameObject.Instantiate(bulletDecal, contact.point, Quaternion.LookRotation(contact.normal));
        }
        Debug.Log("Damage");
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dragon")
        {
            other.GetComponent<RedDragon>().TakeDamage(damageAmount);
            
        }
        Debug.Log("Damage");
        Destroy(gameObject);
    }*/
}

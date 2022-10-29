using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public int damage;
    public float moveSpeed;

    private Vector3 destination;
    private bool isFriendly;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(this.destination == transform.position){
            Destroy(gameObject);
            return;
        }
        //transform.position = transform.forward * this.moveSpeed * Time.deltaTime;
        Vector3 targetDirection = (this.destination - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, this.destination, this.moveSpeed * Time.deltaTime);
    }
    
    void SetDestination(Vector3 whereAreWeGoing){
        this.destination = whereAreWeGoing;
    }

    void SetFriendly(bool isFriendly){
        this.isFriendly = isFriendly;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" && this.isFriendly){
            other.gameObject.SendMessage("TakeDamage", this.damage);
            Destroy(gameObject);
            return;
        }

        if(other.gameObject.tag == "Tower" && !this.isFriendly){
            other.gameObject.SendMessage("TakeDamage", this.damage);
            Destroy(gameObject);
            return;
        }
    }



}

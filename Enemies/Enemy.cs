using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int maxHitpoints;
    public int currentHitpoints;
    public float moveSpeed;
    public int ArmorValue;

    private Pathfinder pathfinder;
    private CoordinateVector nextDestination;
    private int currentPathIndex;
    private Dictionary<GameObject, bool> turretDict;

    void Start() {

        this.turretDict = new Dictionary<GameObject, bool>();
        this.pathfinder = Pathfinder.GetInstance();
        this.currentHitpoints = this.maxHitpoints;
        this.currentPathIndex = 0;

        this.nextDestination = this.pathfinder.GetNext(this.currentPathIndex);
        this.currentPathIndex++;

        Vector3 destination = (this.nextDestination.ToVector3(true) - transform.position).normalized;
        if (destination.x > 0.5f){
            transform.rotation = Quaternion.Euler(0,0, 180.0f);
        } else if (destination.y > 0.5f){
            transform.rotation = Quaternion.Euler(0,0, 270.0f);
        } else if (destination.x < -0.5f){
            transform.rotation = Quaternion.Euler(0,0,0);
        } else if (destination.y < -0.5f){
            transform.rotation = Quaternion.Euler(0,0,90.0f);
        }

    }

    void Update(){
        this.Move();
    }

    public void Move(){
        if (this.nextDestination is null){
            DestinationReached();
            return;
        }
        Vector3 destination = this.nextDestination.ToVector3(true);
        Vector3 targetDirection = (destination - transform.position).normalized;
        gameObject.SendMessageUpwards("SetPosition", Vector3.MoveTowards(transform.position,  destination, this.moveSpeed * Time.deltaTime));

        Rotate(targetDirection);

        if ((transform.position - destination).magnitude <= (this.moveSpeed)/2){
            this.nextDestination = this.pathfinder.GetNext(this.currentPathIndex);
            this.currentPathIndex++;
        }
    }

    private void Rotate(Vector3 destination){
        if (destination.x > 0.5f){
            Quaternion target = Quaternion.Euler(0,0, 180.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * this.moveSpeed * 3);
            return;
        }
        if (destination.y > 0.5f){
            Quaternion target = Quaternion.Euler(0,0, 270.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * this.moveSpeed * 3);
            return;
        }
        if (destination.x < -0.5f){
            Quaternion target = Quaternion.Euler(0,0,0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * this.moveSpeed * 3);
        }
        if (destination.y < -0.5f){
            Quaternion target = Quaternion.Euler(0,0,90.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * this.moveSpeed * 3);
        }
    }

    private void DestinationReached(){
        foreach(KeyValuePair<GameObject, bool> kvp in this.turretDict ){
            if (kvp.Value){
                kvp.Key.SendMessage("ReleaseTarget");
            }
        }
        gameObject.SendMessageUpwards("FadeAway");
    }

    private void TimeToDie(){
        foreach(KeyValuePair<GameObject, bool> kvp in this.turretDict ){
            if (kvp.Value){
                kvp.Key.SendMessage("DeactivateTarget", gameObject);
            }
        }
        gameObject.SendMessageUpwards("TimeToDie");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Tower"){
            this.turretDict.Add(other.gameObject, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject.tag == "Tower"){
            other.gameObject.SendMessage("ReleaseTarget");
        }
    }

    private void TakeDamage(int damage){
        this.currentHitpoints -= damage;
        if (this.currentHitpoints <= 0){
            // Expand on this
            this.TimeToDie();
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    // Start is called before the first frame update

    public int sightRadius;
    public GameObject bullet;

    public float shotCooldown;
    public int ammoPerReload;
    private int currentLoad;
    private bool isReloading;
    public float reloadTime;
    private float currentTimer;


    private GameObject activeTarget;
    int targetLayer;

    private Queue<GameObject> targetQueue;
    private Dictionary<GameObject, bool> targetDictionary;



    // Start is called before the first frame update
    void Start()
    {
        CircleCollider2D collider = gameObject.GetComponent<CircleCollider2D>();
        collider.radius = this.sightRadius;

        this.targetQueue = new Queue<GameObject>();
        this.targetDictionary = new Dictionary<GameObject, bool>();
        this.targetLayer = LayerMask.NameToLayer("TowerBodies");
        this.isReloading = false;
        this.currentTimer = 0f;
        this.currentLoad = this.ammoPerReload;
    }

    // Update is called once per frame
    void Update()
    {
        this.currentTimer += Time.deltaTime;
        if (this.activeTarget != null && this.targetDictionary[this.activeTarget] == false){
            this.ReleaseTarget();
        }

        if (this.activeTarget != null){
            //Rotate towards this objects
            this.Rotate();

            //TODO calculate lead angle

            //Shoot at the dude
            this.ShotCycle();
        } else {
            if (this.targetQueue.Count > 0){
                GameObject possibleTarget = this.targetQueue.Dequeue();
                if (this.targetDictionary[possibleTarget]){
                    this.SetTarget(possibleTarget);
                }
            }
        }
    }

    private void Rotate(){    
        //Syntactic sugar
        float this_x = transform.position.x;
        float that_x = this.activeTarget.transform.position.x;
        float this_y = transform.position.y;
        float that_y = this.activeTarget.transform.position.y;
        float angle = Mathf.Atan2(this_y - that_y, this_x - that_x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    private void ShotCycle(){
        // Reload
        if(this.isReloading){
            // Are we still realoading?
            if (this.currentTimer < this.reloadTime){
                return;
            }
            // Make a sound? Give some feedback that reload is complete
            this.isReloading = false;
            this.currentTimer = 0;
            this.currentLoad = this.ammoPerReload;
            
        }

        // Did we just shoot?
        if (this.currentTimer <= this.shotCooldown){
            return;
        }

        // Proceed with the shot
        this.Shoot();

        this.currentLoad--;

        if (this.currentLoad <= 0){
            this.isReloading = true;
        }
        this.currentTimer = 0;
    }

    private void Shoot(){
        
        GameObject bullet = Instantiate(this.bullet, transform.position, Quaternion.identity);
        bullet.layer = this.targetLayer;

        bullet.SendMessage("SetDestination", this.activeTarget.transform.position);
        bullet.SendMessage("SetFriendly", false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Tower"){
            this.targetQueue.Enqueue(other.gameObject);
            this.targetDictionary[other.gameObject] = true;
        }
    }

    private void SetTarget(GameObject target){
        this.activeTarget = target;
    }

    public void DeactivateTarget(GameObject target){
        this.targetDictionary[target] = false;
    }

    public void ReleaseTarget(){
        this.activeTarget = null;
    }
}

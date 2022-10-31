using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour
{
    private Camera _camera;
    public float moveSpeed;
    public float scrollRate;
    public int maxY;
    public int maxX;
    
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
        maxY = 0;
        maxX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (maxX==0){
            GameObject mapManager = GameObject.Find("MapParentObject");
            maxY = mapManager.GetComponent<MapController>().height;
            maxX = mapManager.GetComponent<MapController>().width;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)){
            _TranslateCamera(0);
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){
            _TranslateCamera(1);
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){
            _TranslateCamera(2);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)){
            _TranslateCamera(3);
        }
    }

    private void _TranslateCamera(int dir){
        int localMaxX = 10;
        int localMaxY = 20;

        if (maxX > 0){
            localMaxX = maxX-2;
        }
        if (maxY > 0){
            localMaxY = maxY;
        }
        
        if(dir == 0){
            transform.position = new Vector3(transform.position.x,
                Mathf.Min(transform.position.y + Time.deltaTime*this.moveSpeed, localMaxY),
                transform.position.z);
        } else if (dir == 1){
            transform.position = new Vector3(Mathf.Max(3,transform.position.x - Time.deltaTime * this.moveSpeed),
                transform.position.y, transform.position.z);
        } else if (dir == 2){
            transform.position = new Vector3(Mathf.Min(transform.position.x + Time.deltaTime * this.moveSpeed, localMaxX),
                transform.position.y, transform.position.z);
        } else if (dir == 3){
            transform.position = new Vector3(transform.position.x,
                Mathf.Max(transform.position.y - Time.deltaTime*this.moveSpeed,3),
                transform.position.z);
        }
    }

    private void _ZoomCamera(float value){
        float resultantValue = Mathf.Clamp(
            _camera.orthographicSize + Time.deltaTime * this.scrollRate * value,
            1.5f, 3.5f);
        _camera.orthographicSize = resultantValue;
    }

    public void OnGUI()
    {
        if(Event.current.type == EventType.ScrollWheel)
            // do stuff with  Event.current.delta
            _ZoomCamera(Event.current.delta.y);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour
{
    private Camera _camera;
    public float movespeed;
    
    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)){
            _TranslateCamera(0);
        }
        if (Input.GetKey(KeyCode.LeftArrow)){
            _TranslateCamera(1);
        }
        if (Input.GetKey(KeyCode.RightArrow)){
            _TranslateCamera(2);
        }
        if (Input.GetKey(KeyCode.DownArrow)){
            _TranslateCamera(3);
        }   
    }

    private void _TranslateCamera(int dir){
        if(dir == 0){
            transform.Translate(Vector3.up * Time.deltaTime * this.movespeed);  
        } else if (dir == 1){
            transform.Translate(-Vector3.right * Time.deltaTime * this.movespeed);
        } else if (dir == 2){
            transform.Translate(Vector3.right * Time.deltaTime * this.movespeed);
        } else if (dir == 3){
            transform.Translate( - Vector3.up * Time.deltaTime * this.movespeed);
        }
    }
}

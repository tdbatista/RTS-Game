using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    public Camera camera;
    private Vector2 terrainSize;
    public float speed;
    public float zoomSpeed;
    public float minZoom =50;
    public float maxZoom = 100;

    private float currentZoom;
    // Start is called before the first frame update
    void Start()
    {
        terrainSize = Terrain.activeTerrain.terrainData.size;
        camera = Camera.main;
        camera.transform.Translate(new Vector3(0,0,-minZoom*Mathf.Tan(Mathf.Deg2Rad*camera.transform.rotation.eulerAngles.x)));
        //currentZoom = minZoom;
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardMovement();

        //ClampPosition();
        //trava no mapa
        
    }

    void KeyboardMovement(){

        Vector3 keyMovement = new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical")) * speed;
       

        if (!Vector3.Equals(keyMovement,Vector3.zero)){
            transform.Translate(keyMovement *Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.PageUp) && camera.transform.position.y > minZoom){
            camera.transform.Translate(0,0,Time.deltaTime *  zoomSpeed );
        }

        if (Input.GetKey(KeyCode.PageDown) && camera.transform.position.y < maxZoom){
            camera.transform.Translate(0,0,-Time.deltaTime *  zoomSpeed);
        }

    }

    void ClampPosition(){
        float positionX = Mathf.Clamp(transform.position.x, 0, terrainSize.x);
        float positionz = Mathf.Clamp(transform.position.z, 0, terrainSize.y);
        transform.SetPositionAndRotation(new Vector3(positionX,transform.position.y,positionz),transform.rotation);

        //currentZoom = Mathf.Clamp(currentZoom,minZoom,maxZoom);
    }
}

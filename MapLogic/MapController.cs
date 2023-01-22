using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapGenerator mapgen;

    public int height;
    public int width;
    public float mesh_size;
    public int entrances = 1;

    public GameObject EnemySpawner;
    public GameObject EndLocation;

    private Mesh MakeQuadMesh(int x, int y, TileType map_value){
        Mesh mesh = new Mesh();
        const float UV_SCALE_FACTOR = 0.25f;


        Vector3[] vertices = new Vector3[4]{
            new Vector3(x*this.mesh_size,y*this.mesh_size,0),
            new Vector3(x*this.mesh_size+this.mesh_size, y*this.mesh_size, 0),
            new Vector3(x*this.mesh_size, y*this.mesh_size+this.mesh_size, 0), 
            new Vector3(x*this.mesh_size+this.mesh_size, y*this.mesh_size+this.mesh_size, 0)
        };
        mesh.vertices = vertices;

        int[] tris = new int[6]{
            0,2,1,
            2,3,1
        };
        mesh.triangles = tris;

        Vector3[] normals = new Vector3[4]{
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
        };
        mesh.normals = normals;

        
        if (map_value == TileType.Wall){
            int bitMask = this.mapgen.GetBitMask(y,x);

            int column = bitMask % 4;
            int row = bitMask/4;

            float scaled_column = column * UV_SCALE_FACTOR;
            float scaled_row = row * UV_SCALE_FACTOR;

            Vector2[] uv = new Vector2[4]
            {
                new Vector2(scaled_column, scaled_row),
                new Vector2(scaled_column+UV_SCALE_FACTOR, scaled_row),
                new Vector2(scaled_column, scaled_row+UV_SCALE_FACTOR),
                new Vector2(scaled_column+UV_SCALE_FACTOR, scaled_row+UV_SCALE_FACTOR)
            };
            mesh.uv = uv;
        } else {
            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(0.001f, 0),
                new Vector2(0, 0.001f),
                new Vector2(0.001f, 0.001f)
            };
            mesh.uv = uv;
        }

        return mesh;
    }

    void MakeMapMesh(){
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[this.height*this.width];

        for (int y = 0; y < this.height; y++){
            for (int x = 0; x < this.width; x++){
                combine[y*this.width+x].mesh = MakeQuadMesh(x, y, this.mapgen.map[y][x]);
                combine[y*this.width+x].transform = gameObject.transform.localToWorldMatrix;
            }
        }

        meshFilter.mesh = new Mesh();
        meshFilter.mesh.CombineMeshes(combine);
    }

    void Start(){
        //Canvas Stuff
        

        float nudgeSize = this.mesh_size/2;
        this.mapgen = new MapGenerator(this.width, this.height);
        this.mapgen.ConstructMap(this.entrances);
        this.MakeMapMesh();

        Vector3 spawnNudge = new Vector3(nudgeSize, nudgeSize, 0);
        
        for (int i = 0; i < this.entrances; i++){
            CoordinateVector entrance_coordinate = this.mapgen.GetEntrance(i);
            Vector3 spawnPosition = new Vector3(entrance_coordinate.X*this.mesh_size, entrance_coordinate.Y*this.mesh_size, 0);        
            GameObject spawner_go = Instantiate(
                EnemySpawner, 
                spawnPosition + transform.position + spawnNudge,
                Quaternion.identity);
            EnemySpawner spawner = spawner_go.GetComponent<EnemySpawner>();
            spawner.SetupPathfinder(this.mapgen, i);
        }

        CoordinateVector exit_coordinate = this.mapgen.GetEndingPosition();
        Vector3 exitPosition = new Vector3(exit_coordinate.X*this.mesh_size, exit_coordinate.Y*this.mesh_size, 0);
        Instantiate(EndLocation, exitPosition + transform.position + spawnNudge, Quaternion.identity);
        

        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
    }

    private void OnMouseOver() {
        if(Input.GetMouseButtonDown(0)){
            Vector3 mousePos = Input.mousePosition;
            Camera cam = Camera.main;
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0));

            // Is this a valid spawn position?
            if(!this.mapgen.IsWall((int)point.x, (int)point.y)){
                return;
            }

            TowerFactory _tf = gameObject.GetComponent<TowerFactory>();
            float nudgeSize = this.mesh_size/2;
            Vector3 spawnPoint = new Vector3(
                (float)Math.Floor((point.x / this.mesh_size))+nudgeSize,
                (float)Math.Floor((point.y / this.mesh_size))+nudgeSize,
                0);
            
            _tf.Make(TowerTypes.TestTower, spawnPoint);

        }    
    }
}

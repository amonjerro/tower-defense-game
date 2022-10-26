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

    public GameObject EnemySpawner;

    private Pathfinder pathfinder = Pathfinder.GetInstance();

    private Mesh MakeQuadMesh(int x, int y, TileType map_value){
        Mesh mesh = new Mesh();

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
            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(0.5f, 0),
                new Vector2(0, 1),
                new Vector2(0.5f, 1)
            };
            mesh.uv = uv;
        } else {
            Vector2[] uv = new Vector2[4]
            {
                new Vector2(0.5f, 0),
                new Vector2(1, 0),
                new Vector2(0.5f, 1),
                new Vector2(1, 1)
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
        float nudgeSize = this.mesh_size/2;
        this.mapgen = new MapGenerator(this.width, this.height);
        CoordinateVector starting_position = this.mapgen.CreateEntranceAndExit();

        this.mapgen.ConstructMap(starting_position);
        this.MakeMapMesh();

        Vector3 spawnNudge = new Vector3(nudgeSize, nudgeSize, 0);
        this.pathfinder.FindPath(this.mapgen);

        Vector3 spawnPosition = new Vector3(this.mapgen.starting_x*this.mesh_size, this.mapgen.starting_y*this.mesh_size, 0);        

        Instantiate(
            EnemySpawner, 
            spawnPosition + transform.position + spawnNudge,
            Quaternion.identity);

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

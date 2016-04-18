using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshFilter))]
public class BoundIndicator : MonoBehaviour {

    private int extent = 100;
    Mesh indicatorMesh;


	// Use this for initialization
	void Start () {

        ProceduralTerrainVolume volume = this.gameObject.GetComponent<ProceduralTerrainVolume>();
        if (volume)
        {
            extent = volume.planetRadius;
        }

        indicatorMesh = CreatePlaneMesh();
    }

    public void Show()
    {
        GetComponent<MeshFilter>().mesh = indicatorMesh;
    }

    public void Hide()
    {
        GetComponent<MeshFilter>().mesh = null;
    }

    Mesh CreatePlaneMesh()
    {
        Mesh mesh = new Mesh();
        //vertices
        Vector3[] vertices = new Vector3[]
        {
            new Vector3( extent, -extent,  extent),
            new Vector3( extent, -extent, -extent),
            new Vector3(-extent, -extent,  extent),
            new Vector3(-extent, -extent, -extent),
            new Vector3( extent,  extent,  extent),
            new Vector3( extent,  extent, -extent),
            new Vector3(-extent,  extent,  extent),
            new Vector3(-extent,  extent, -extent),
        };
        //UV
        Vector2[] uv = new Vector2[]
        {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
        };
        //index
        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3,

            4, 5, 6,
            6, 5, 7,

            0, 1, 5,
            0, 5, 4,

            2, 3, 7,
            2, 7, 6,

            0, 4, 2,
            2, 4, 6,

            1, 5, 3,
            3, 5, 7,
        };

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }

    // Update is called once per frame
    void Update () {
	
	}
}

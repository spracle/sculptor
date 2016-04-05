using UnityEngine;
using System.Collections;

using Cubiquity;

public class TrackAnchor : MonoBehaviour {

    public GameObject rightHandAnchor = null;
    public GameObject BasicProceduralVolume = null;

    private GameObject rightHand = null;

    private HandBehaviour handBehaviour;
    private TerrainVolume terrainVolume;
    private Vector3 VoxelWorldScale = new Vector3(0.1f, 0.1f, 0.1f);

    private int optRange;
    private OptShape activeShape, nowShape;

    private Vector3 rotateEuler = new Vector3(0, 0, 0);

    // Use this for initialization
    void Start () {

        terrainVolume = BasicProceduralVolume.GetComponent<TerrainVolume>();
        VoxelWorldScale = terrainVolume.transform.localScale;

        handBehaviour = GetComponent<HandBehaviour>();

        rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Color tempcolor = rightHand.transform.GetComponent<Renderer>().material.color;
        tempcolor.a = 0.3f;
        rightHand.transform.GetComponent<Renderer>().material.color = tempcolor;
        rightHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
    }
	
	// Update is called once per frame
	void Update () {

        optRange = handBehaviour.GetOptRange();

        nowShape = handBehaviour.GetActiveShape();
        if (nowShape != activeShape)
        {
            switch (nowShape)
            {
                case OptShape.cube:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case OptShape.sphere:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case OptShape.cylinder:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                case OptShape.capsule:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
            }
            Color tempcolor = rightHand.transform.GetComponent<Renderer>().material.color;
            tempcolor.a = 0.3f;
            rightHand.transform.GetComponent<Renderer>().material.color = tempcolor;
            rightHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
            activeShape = nowShape;
        }

        rightHand.transform.position = rightHandAnchor.transform.position;
        rightHand.transform.localScale = VoxelWorldScale * optRange;

        rotateEuler = handBehaviour.GetRotateEuler();
        rightHand.transform.rotation = Quaternion.Euler(rotateEuler);


    }



}

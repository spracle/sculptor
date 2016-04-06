using UnityEngine;
using System.Collections;

using Cubiquity;

public class TrackAnchor : MonoBehaviour {

    public GameObject rightHandAnchor = null;
    public GameObject BasicProceduralVolume = null;

    private GameObject rightHand = null;
    private GameObject rightHandChild = null;

    private HandBehaviour handBehaviour;
    private TerrainVolume terrainVolume;
    private Vector3 VoxelWorldScale = new Vector3(0.1f, 0.1f, 0.1f);

    private int optRange;
    private OptShape activeShape, nowShape;

    private Vector3 rotateEuler = new Vector3(0, 0, 0);

    private Color materialColor;
    private Color materialChildColor;
    private Vector3 childPosition = new Vector3(0, 0, 3); // change z

    // Use this for initialization
    void Start () {

        terrainVolume = BasicProceduralVolume.GetComponent<TerrainVolume>();
        VoxelWorldScale = terrainVolume.transform.localScale;

        handBehaviour = GetComponent<HandBehaviour>();

        rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        materialColor = rightHand.transform.GetComponent<Renderer>().material.color;
        materialColor.a = 0.05f;
        rightHand.transform.GetComponent<Renderer>().material.color = materialColor;
        rightHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightHandChild.transform.position = childPosition;
        rightHandChild.transform.parent = rightHand.transform;
        materialChildColor = rightHandChild.transform.GetComponent<Renderer>().material.color;
        materialChildColor.a = 0.3f;
        rightHandChild.transform.GetComponent<Renderer>().material.color = materialChildColor;
        rightHandChild.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");
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
                    UnityEngine.Object.Destroy(rightHandChild.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case OptShape.sphere:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    UnityEngine.Object.Destroy(rightHandChild.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case OptShape.cylinder:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    UnityEngine.Object.Destroy(rightHandChild.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                case OptShape.capsule:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    UnityEngine.Object.Destroy(rightHandChild.gameObject);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
            }
            materialColor = rightHand.transform.GetComponent<Renderer>().material.color;
            materialColor.a = 0.05f;
            rightHand.transform.GetComponent<Renderer>().material.color = materialColor;
            rightHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            rightHandChild.transform.position = childPosition;
            rightHandChild.transform.parent = rightHand.transform;
            materialChildColor = rightHandChild.transform.GetComponent<Renderer>().material.color;
            materialChildColor.a = 0.3f;
            rightHandChild.transform.GetComponent<Renderer>().material.color = materialChildColor;
            rightHandChild.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            activeShape = nowShape;
        }

        // child first
        childPosition.z = handBehaviour.GetChildPosZ();
        rightHandChild.transform.localPosition = childPosition;

        rightHand.transform.position = rightHandAnchor.transform.position;
        rightHand.transform.rotation = rightHandAnchor.transform.rotation;
        rightHand.transform.localScale = VoxelWorldScale * optRange;
    }

    public Vector3 GetChildPosition()
    {
        return rightHandChild.transform.position;
    }

}

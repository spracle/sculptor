using UnityEngine;
using System.Collections;

using Cubiquity;

public class TrackAnchor : MonoBehaviour {

    public GameObject BasicProceduralVolume = null;

    public GameObject leftHandAnchor = null;
    public GameObject rightHandAnchor = null;

    private GameObject leftHand = null;
    private GameObject leftHandChild = null;

    private GameObject rightHand = null;
    private GameObject rightHandChild = null;

    private HandBehaviour handBehaviour;
    private TerrainVolume terrainVolume;
    private Vector3 VoxelWorldScale = new Vector3(1f, 1f, 1f);

    private int optRange;
    private OptShape activeShape, nowShape;

    private Vector3 rotateEuler = new Vector3(0, 0, 0);

    private Color materialColor;
    private Color materialChildColor;
    private Vector3 leftChildPosition = new Vector3(0, 0, 0); // change z
    private Vector3 rightChildPosition = new Vector3(0, 0, 0); // change z

    private GameObject twiceHand;
    private bool openTwoHandDraw = false;

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
        rightHandChild.transform.position = rightChildPosition;
        rightHandChild.transform.parent = rightHand.transform;
        materialChildColor = rightHandChild.transform.GetComponent<Renderer>().material.color;
        materialChildColor.a = 0.3f;
        rightHandChild.transform.GetComponent<Renderer>().material.color = materialChildColor;
        rightHandChild.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        leftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        materialColor = leftHand.transform.GetComponent<Renderer>().material.color;
        materialColor.a = 0.05f;
        leftHand.transform.GetComponent<Renderer>().material.color = materialColor;
        leftHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        leftHandChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
        leftHandChild.transform.position = leftChildPosition;
        leftHandChild.transform.parent = leftHand.transform;
        materialChildColor = leftHandChild.transform.GetComponent<Renderer>().material.color;
        materialChildColor.a = 0.3f;
        leftHandChild.transform.GetComponent<Renderer>().material.color = materialChildColor;
        leftHandChild.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        twiceHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        materialColor = twiceHand.transform.GetComponent<Renderer>().material.color;
        materialColor.a = 0.5f;
        twiceHand.transform.GetComponent<Renderer>().material.color = materialColor;
        twiceHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

        leftHandChild.SetActive(true);
        rightHandChild.SetActive(true);
        twiceHand.SetActive(false);
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
                    UnityEngine.Object.Destroy(leftHand.gameObject);
                    UnityEngine.Object.Destroy(leftHandChild.gameObject);
                    leftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    leftHandChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    break;
                case OptShape.sphere:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    UnityEngine.Object.Destroy(rightHandChild.gameObject);
                    UnityEngine.Object.Destroy(leftHand.gameObject);
                    UnityEngine.Object.Destroy(leftHandChild.gameObject);
                    leftHand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    leftHandChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    break;
                case OptShape.cylinder:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    UnityEngine.Object.Destroy(rightHandChild.gameObject);
                    UnityEngine.Object.Destroy(leftHand.gameObject);
                    UnityEngine.Object.Destroy(leftHandChild.gameObject);
                    leftHand = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    leftHandChild = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    break;
                case OptShape.capsule:
                    UnityEngine.Object.Destroy(rightHand.gameObject);
                    UnityEngine.Object.Destroy(rightHandChild.gameObject);
                    UnityEngine.Object.Destroy(leftHand.gameObject);
                    UnityEngine.Object.Destroy(leftHandChild.gameObject);
                    leftHand = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    leftHandChild = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    rightHand = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    rightHandChild = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    break;
            }
            materialColor = rightHand.transform.GetComponent<Renderer>().material.color;
            materialColor.a = 0.05f;
            rightHand.transform.GetComponent<Renderer>().material.color = materialColor;
            rightHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            rightHandChild.transform.position = rightChildPosition;
            rightHandChild.transform.parent = rightHand.transform;
            materialChildColor = rightHandChild.transform.GetComponent<Renderer>().material.color;
            materialChildColor.a = 0.3f;
            rightHandChild.transform.GetComponent<Renderer>().material.color = materialChildColor;
            rightHandChild.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            materialColor = leftHand.transform.GetComponent<Renderer>().material.color;
            materialColor.a = 0.05f;
            leftHand.transform.GetComponent<Renderer>().material.color = materialColor;
            leftHand.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            leftHandChild.transform.position = leftChildPosition;
            leftHandChild.transform.parent = leftHand.transform;
            materialChildColor = leftHandChild.transform.GetComponent<Renderer>().material.color;
            materialChildColor.a = 0.3f;
            leftHandChild.transform.GetComponent<Renderer>().material.color = materialChildColor;
            leftHandChild.transform.GetComponent<Renderer>().material.shader = Shader.Find("Transparent/Diffuse");

            activeShape = nowShape;
        }

        // child first
        leftChildPosition.z = handBehaviour.GetLeftChildPosZ();
        rightChildPosition.z = handBehaviour.GetRightChildPosZ();

        leftHandChild.transform.localPosition = leftChildPosition;
        rightHandChild.transform.localPosition = rightChildPosition;

        leftHand.transform.position = leftHandAnchor.transform.position;
        leftHand.transform.rotation = leftHandAnchor.transform.rotation;
        leftHand.transform.localScale = VoxelWorldScale * optRange;

        rightHand.transform.position = rightHandAnchor.transform.position;
        rightHand.transform.rotation = rightHandAnchor.transform.rotation;
        rightHand.transform.localScale = VoxelWorldScale * optRange;

        Vector3 temp = rightHandAnchor.transform.position - leftHandAnchor.transform.position;
        twiceHand.transform.position = leftHandAnchor.transform.position + temp / 2;
        twiceHand.transform.localScale = new Vector3(System.Math.Abs(temp.x), System.Math.Abs(temp.y), System.Math.Abs(temp.z));

        // twice hand
        bool tempOpenTwoHandDraw = handBehaviour.GetOpenTwoHandDraw();
        if (tempOpenTwoHandDraw != openTwoHandDraw)
        {
            if (tempOpenTwoHandDraw)
            {
                leftHandChild.SetActive(false);
                rightHandChild.SetActive(false);
                twiceHand.SetActive(true);
            }
            else
            {
                leftHandChild.SetActive(true);
                rightHandChild.SetActive(true);
                twiceHand.SetActive(false);
            }
        }
        openTwoHandDraw = tempOpenTwoHandDraw;

    }

    public Vector3 GetRightChildPosition()
    {
        return rightHandChild.transform.position;
    }
    
    public Vector3 GetLeftChildPosition()
    {
        return leftHandChild.transform.position;
    }

    public Vector3 GetTwiceChildPosition()
    {
        return twiceHand.transform.position;
    }

    public Vector3 GetTwiceChildLocalScale()
    {
        return twiceHand.transform.localScale;
    }

}

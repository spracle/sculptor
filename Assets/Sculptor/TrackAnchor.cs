using UnityEngine;
using System.Collections;

using Cubiquity;

public class TrackAnchor : MonoBehaviour {

    public GameObject leftHandAnchor = null;
    public GameObject rightHandAnchor = null;
    public GameObject BasicProceduralVolume = null;

    private GameObject leftHand = null;
    private GameObject rightHand = null;

    private HandBehaviour handBehaviour;
    private TerrainVolume terrainVolume;
    private Vector3 VoxelWorldScale = new Vector3(0.1f, 0.1f, 0.1f);

    private int drawRange;
    private int smoothRange;

    // Use this for initialization
    void Start () {

        terrainVolume = BasicProceduralVolume.GetComponent<TerrainVolume>();
        VoxelWorldScale = terrainVolume.transform.localScale;

        handBehaviour = GetComponent<HandBehaviour>();

        leftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rightHand = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    }
	
	// Update is called once per frame
	void Update () {

        drawRange = handBehaviour.GetDrawRange();
        smoothRange = handBehaviour.GetSmoothRange();

        leftHand.transform.position = leftHandAnchor.transform.position;
        leftHand.transform.localScale = VoxelWorldScale * drawRange;

        rightHand.transform.position = rightHandAnchor.transform.position;
        rightHand.transform.localScale = VoxelWorldScale * smoothRange;
    }



}

using UnityEngine;
using System.Collections;

using Cubiquity;

public class HandBehaviour : MonoBehaviour {

    public GameObject leftHandAnchor = null;
    public GameObject rightHandAnchor = null;
    public GameObject BasicProceduralVolume = null;

    private TerrainVolume terrainVolume;

    private MaterialSet preMaterialSet;
    private MaterialSet newMaterialSet;

    private Vector3 VoxelWorldScale = new Vector3(1, 1, 1);
    private Vector3 leftPosition = new Vector3(0, 0, 0);
    private Vector3 rightPosition = new Vector3(0, 0, 0);

    private int drawRange = 4;
    private int smoothRange = 4;
    private bool preButtonState = false;

    // Use this for initialization
    void Start () {

        terrainVolume = BasicProceduralVolume.GetComponent<TerrainVolume>();

        if (leftHandAnchor == null || rightHandAnchor == null || BasicProceduralVolume == null)
        {
            Debug.LogError("Please assign the GameObject first.");
        }
        if (terrainVolume == null)
        {
            Debug.LogError("This 'BasicProceduralVolume' script should be attached to a game object with a TerrainVolume component");
        }

        VoxelWorldScale = terrainVolume.transform.localScale;

        // empty
        preMaterialSet = new MaterialSet();
        preMaterialSet.weights[2] = 0;
        preMaterialSet.weights[1] = 0;
        preMaterialSet.weights[0] = 0;

        // color control
        newMaterialSet = new MaterialSet();
        newMaterialSet.weights[2] = 255;
        newMaterialSet.weights[1] = 255;
        newMaterialSet.weights[0] = 255;

    }
	
	// Update is called once per frame
	void Update () {

        // Bail out if we're not attached to a terrain.
        if (terrainVolume == null)
        {
            return;
        }

        leftPosition = (new Vector3(leftHandAnchor.transform.position.x / VoxelWorldScale.x, leftHandAnchor.transform.position.y / VoxelWorldScale.y, leftHandAnchor.transform.position.z / VoxelWorldScale.z));
        rightPosition = (new Vector3(rightHandAnchor.transform.position.x / VoxelWorldScale.x, rightHandAnchor.transform.position.y / VoxelWorldScale.y, rightHandAnchor.transform.position.z / VoxelWorldScale.z));

        //Debug.Log(leftHandAnchor.transform.position);

        HandleTestInput();
        HandleOVRInput();
    }

    private void HandleTestInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveVDBFile();
        }
    }

    private void HandleOVRInput()
    {
        // please see https://developer.oculus.com/documentation/game-engines/latest/concepts/unity-ovrinput/ to known the mapping

        // Axis1D Touch
        float Axis1D_LB = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
        float Axis1D_LT = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);

        float Axis1D_RB = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
        float Axis1D_RT = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);

        bool Button_A = OVRInput.Get(OVRInput.Button.One);
        bool Button_B = OVRInput.Get(OVRInput.Button.Two);
        bool Button_X = OVRInput.Get(OVRInput.Button.Three);
        bool Button_Y = OVRInput.Get(OVRInput.Button.Four);

        if (Axis1D_LB > 0)
        {
            newMaterialSet.weights[2] = 255;
            newMaterialSet.weights[1] = 0;
            newMaterialSet.weights[0] = 0;
            CreateVoxels((Vector3i)leftPosition, newMaterialSet, drawRange / 2, -1);
        }

        if (Axis1D_RB > 0)
        {
            newMaterialSet.weights[2] = 0;
            newMaterialSet.weights[1] = 255;
            newMaterialSet.weights[0] = 0;
            CreateVoxels((Vector3i)rightPosition, newMaterialSet, smoothRange / 2, 1);
        }

        if (Axis1D_LT > 0)
        {
            DestroyVoxels((Vector3i)leftPosition, drawRange / 2, -1);
        }

        if (Axis1D_RT > 0)
        {
            SmoothVoxels((Vector3i)rightPosition, smoothRange / 2);
        }

        if (Button_A)
        {
            if(smoothRange > 2 && preButtonState == false)
            {
                smoothRange -= 2;
                //OVRInput.SetControllerVibration(1.0f, 1.0f, OVRInput.Controller.RTouch);
            }
        }

        if (Button_B)
        {
            if (smoothRange < 10 && preButtonState == false)
            {
                smoothRange += 2;
                //OVRInput.SetControllerVibration(1.0f, 1.0f, OVRInput.Controller.RTouch);
            }
        }

        if (Button_X)
        {
            if (drawRange > 2 && preButtonState == false)
            {
                drawRange -= 2;
                //OVRInput.SetControllerVibration(1.0f, 1.0f, OVRInput.Controller.LTouch);
            }
        }

        if (Button_Y)
        {
            if (drawRange < 10 && preButtonState == false)
            {
                drawRange += 2;
                //OVRInput.SetControllerVibration(1.0f, 1.0f, OVRInput.Controller.LTouch);
            }
        }

        // the end to record the state
        if (Button_A || Button_B || Button_X || Button_Y)
        {
            preButtonState = true;
        }
        else
        {
            preButtonState = false;
        }

    }

    private void DestroyVoxels(Vector3i Pos, int range, int smoothRange)
    {
        int xPos = Pos.x;
        int yPos = Pos.y;
        int zPos = Pos.z;

        // Initialise outside the loop, but we'll use it later.
        int rangeSquared = range * range;
        MaterialSet emptyMaterialSet = new MaterialSet();

        // Iterage over every voxel in a cubic region defined by the received position (the center) and
        // the range. It is quite possible that this will be hundreds or even thousands of voxels.
        for (int z = zPos - range; z < zPos + range; z++)
        {
            for (int y = yPos - range; y < yPos + range; y++)
            {
                for (int x = xPos - range; x < xPos + range; x++)
                {
                    // Compute the distance from the current voxel to the center of our explosion.
                    int xDistance = x - xPos;
                    int yDistance = y - yPos;
                    int zDistance = z - zPos;

                    // Working with squared distances avoids costly square root operations.
                    int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

                    // We're iterating over a cubic region, but we want our explosion to be spherical. Therefore 
                    // we only further consider voxels which are within the required range of our explosion center. 
                    // The corners of the cubic region we are iterating over will fail the following test.
                    if (distSquared < rangeSquared)
                    {
                        terrainVolume.data.SetVoxel(x, y, z, emptyMaterialSet);
                    }
                }
            }
        }

        if(smoothRange > 0)
        {
            range += smoothRange;
            TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range, zPos - range, xPos + range, yPos + range, zPos + range));
        }
    }

    private void CreateVoxels(Vector3i Pos, MaterialSet materialSet, int range, int smoothRange)
    {
        int xPos = Pos.x;
        int yPos = Pos.y;
        int zPos = Pos.z;

        // Initialise outside the loop, but we'll use it later.
        int rangeSquared = range * range;

        // Iterage over every voxel in a cubic region defined by the received position (the center) and
        // the range. It is quite possible that this will be hundreds or even thousands of voxels.
        for (int z = zPos - range; z < zPos + range; z++)
        {
            for (int y = yPos - range; y < yPos + range; y++)
            {
                for (int x = xPos - range; x < xPos + range; x++)
                {
                    // Compute the distance from the current voxel to the center of our explosion.
                    int xDistance = x - xPos;
                    int yDistance = y - yPos;
                    int zDistance = z - zPos;

                    // Working with squared distances avoids costly square root operations.
                    int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;

                    // We're iterating over a cubic region, but we want our explosion to be spherical. Therefore 
                    // we only further consider voxels which are within the required range of our explosion center. 
                    // The corners of the cubic region we are iterating over will fail the following test.
                    if (distSquared < rangeSquared)
                    {
                        terrainVolume.data.SetVoxel(x, y, z, materialSet);
                    }
                }
            }
        }

        if(smoothRange > 0)
        {
            range += smoothRange;
            TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range, zPos - range, xPos + range, yPos + range, zPos + range));
        }

    }

    private void SmoothVoxels(Vector3i Pos, int range)
    {
        TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(Pos.x - range, Pos.y - range, Pos.z - range, Pos.x + range, Pos.y + range, Pos.z + range));
    }

    private void PaintVoxels(Vector3 Pos, float brushInnerRadius, float brushOuterRadius, float amount, uint materialIndex)
    {
        TerrainVolumeEditor.PaintTerrainVolume(terrainVolume, Pos.x, Pos.y, Pos.z, brushInnerRadius, brushOuterRadius, amount, materialIndex);
    }

    private void SculptVoxels(Vector3 Pos, float brushInnerRadius, float brushOuterRadius, float amount)
    {
        TerrainVolumeEditor.SculptTerrainVolume(terrainVolume, Pos.x, Pos.y, Pos.z, brushInnerRadius, brushOuterRadius, amount);
    }

    private void SaveVDBFile()
    {
        terrainVolume.data.CommitChanges();
        Debug.Log("Voxel database has been saved.");
    }

    public int GetDrawRange()
    {
        return drawRange;
    }

    public int GetSmoothRange()
    {
        return smoothRange;
    }

}

using UnityEngine;
using System.Collections;

using Cubiquity;

public enum ControlPanel { empty, main, state, shape, color, readfile };
public enum InfoPanel { empty, start, info};
public enum OptState { create, delete, smooth };
public enum OptShape { cube, sphere, capsule, cylinder };

public class HandBehaviour : MonoBehaviour {

    public GameObject leftHandAnchor = null;
    public GameObject rightHandAnchor = null;
    public GameObject BasicProceduralVolume = null;

    private TerrainVolume terrainVolume;

    private MaterialSet emptyMaterialSet;
    private MaterialSet colorMaterialSet;

    private Vector3 VoxelWorldScale = new Vector3(1, 1, 1);
    private Vector3 leftPosition = new Vector3(0, 0, 0);
    private Vector3 rightPosition = new Vector3(0, 0, 0);
    
    private int optRange = 4;

    private ControlPanel activePanel;
    private InfoPanel activeInfoPanel;
    private OptState activeState;
    private OptShape activeShape;

    private float buttonPreTime = 0.0f;
    private float buttonTimeControl = 0.3f;
    private float markTime;

    // -- OVRInput Info

    // Axis2D
    Vector2 Axis2D_L;
    Vector2 Axis2D_R;

    bool Axis2D_LB_Center;
    bool Axis2D_LB_Left;
    bool Axis2D_LB_Right;
    bool Axis2D_LB_Up;
    bool Axis2D_LB_Down;

    bool Axis2D_RB_Center;
    bool Axis2D_RB_Left;
    bool Axis2D_RB_Right;
    bool Axis2D_RB_Up;
    bool Axis2D_RB_Down;

    // Axis1D
    float Axis1D_LB;
    float Axis1D_LT;

    float Axis1D_RB;
    float Axis1D_RT;

    // Button
    bool Button_A;
    bool Button_B;
    bool Button_X;
    bool Button_Y;

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
        emptyMaterialSet = new MaterialSet();
        emptyMaterialSet.weights[3] = 0;
        emptyMaterialSet.weights[2] = 0;
        emptyMaterialSet.weights[1] = 0;
        emptyMaterialSet.weights[0] = 0;

        // color control
        colorMaterialSet = new MaterialSet();
        colorMaterialSet.weights[3] = 0;    // black
        colorMaterialSet.weights[2] = 255;  // b
        colorMaterialSet.weights[1] = 0;  // g
        colorMaterialSet.weights[0] = 0;  // r

        activePanel = ControlPanel.empty;
        activeState = OptState.create;

        markTime = Time.time;
        activeInfoPanel = InfoPanel.start;
    }
	
	// Update is called once per frame
	void Update () {

        // Bail out if we're not attached to a terrain.
        if (terrainVolume == null)
        {
            return;
        }

        // show the begining HMD info
        if (activeInfoPanel != InfoPanel.empty && (Time.time - markTime) > 5)
        {
            activeInfoPanel = InfoPanel.empty;
            markTime = Time.time;
        }


        leftPosition = (new Vector3(leftHandAnchor.transform.position.x / VoxelWorldScale.x, leftHandAnchor.transform.position.y / VoxelWorldScale.y, leftHandAnchor.transform.position.z / VoxelWorldScale.z));
        rightPosition = (new Vector3(rightHandAnchor.transform.position.x / VoxelWorldScale.x, rightHandAnchor.transform.position.y / VoxelWorldScale.y, rightHandAnchor.transform.position.z / VoxelWorldScale.z));

        //Debug.Log(leftHandAnchor.transform.position);

        HandleKeyBoardInput();
        HandleOVRInput();
    }

    private void HandleKeyBoardInput()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveVDBFile();
        }
    }


    private void emptyPanelHandleOVRInput()
    {
        if (Axis1D_RB > 0 && Axis1D_RT > 0 && optRange < 10 && (Time.time - buttonPreTime) > buttonTimeControl / 5)
        {
            StateHandleOVRInput();
            buttonPreTime = Time.time;
        }
        else if (Axis1D_RB > 0 && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            StateHandleOVRInput();
            buttonPreTime = Time.time;
        }
    }

    private void mainPanelHandleOVRInput()
    {
        if (Axis2D_RB_Left && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activePanel = ControlPanel.shape;
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Right && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activePanel = ControlPanel.color;
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Up && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activePanel = ControlPanel.state;
            buttonPreTime = Time.time;
        }
        if ((Axis2D_RB_Down || Axis2D_RB_Center) && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activePanel = ControlPanel.empty;
            buttonPreTime = Time.time;
        }
    }

    private void statePanelHandleOVRInput()
    {
        if (Axis2D_RB_Left && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activeState = OptState.delete;
            activePanel = ControlPanel.empty;
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Right && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activeState = OptState.smooth;
            activePanel = ControlPanel.empty;
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Up && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activeState = OptState.create;
            activePanel = ControlPanel.empty;
            buttonPreTime = Time.time;
        }
        if ((Axis2D_RB_Down || Axis2D_RB_Center) && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activePanel = ControlPanel.empty;
            buttonPreTime = Time.time;
        }
    }

    private void shapePanelHandleOVRInput()
    {
        if (Axis2D_RB_Left && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            if (activeShape >= OptShape.cylinder)
            {
                activeShape = OptShape.cube;
            }
            else
            {
                activeShape++;
            }
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Right && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            if (activeShape <= OptShape.cube)
            {
                activeShape = OptShape.cylinder;
            }
            else
            {
                activeShape--;
            }
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Up && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            optRange += 2;
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Down && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            optRange -= 2;
            if (optRange < 2){
                optRange = 2;
            }
            buttonPreTime = Time.time;
        }
        if (Axis2D_RB_Center && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activePanel = ControlPanel.empty;
            buttonPreTime = Time.time;
        }
    }

    private void colorPanelHandleOVRInput()
    {

    }

    private void readfilePanelHandleOVRInput()
    {

    }

    private void HandleOVRInput()
    {
        // Axis2D
        Axis2D_L = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Axis2D_R = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        Axis2D_LB_Center = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
        Axis2D_LB_Left = OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft);
        Axis2D_LB_Right = OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight);
        Axis2D_LB_Up = OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp);
        Axis2D_LB_Down = OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown);

        Axis2D_RB_Center = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
        Axis2D_RB_Left = OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft);
        Axis2D_RB_Right = OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight);
        Axis2D_RB_Up = OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp);
        Axis2D_RB_Down = OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown);

        // Axis1D
        Axis1D_LB = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
        Axis1D_LT = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);

        Axis1D_RB = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
        Axis1D_RT = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);

        // Button
        Button_A = OVRInput.Get(OVRInput.Button.One);
        Button_B = OVRInput.Get(OVRInput.Button.Two);
        Button_X = OVRInput.Get(OVRInput.Button.Three);
        Button_Y = OVRInput.Get(OVRInput.Button.Four);

        if (Button_A && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            if (activePanel >= ControlPanel.readfile)
            {
                activePanel = ControlPanel.empty;
            }
            else
            {
                activePanel++;
            }
            buttonPreTime = Time.time;
        }
        if (Button_B && (Time.time - buttonPreTime) > buttonTimeControl)
        {
            activeInfoPanel = InfoPanel.info;
            buttonPreTime = Time.time;
        }

        if (Axis1D_RB > 0 || Axis1D_RT > 0)
        {
            activePanel = ControlPanel.empty;
        }

        switch (activePanel)
        {
            case ControlPanel.empty:
                emptyPanelHandleOVRInput();
                break;
            case ControlPanel.main:
                mainPanelHandleOVRInput();
                break;
            case ControlPanel.state:
                statePanelHandleOVRInput();
                break;
            case ControlPanel.shape:
                shapePanelHandleOVRInput();
                break;
            case ControlPanel.color:
                colorPanelHandleOVRInput();
                break;
            case ControlPanel.readfile:
                readfilePanelHandleOVRInput();
                break;
        }

        Debug.Log("activePanel: " + activePanel + " activeState: " + activeState);

    }

    private void StateHandleOVRInput()
    {
        switch (activeState)
        {
            case OptState.create:
                CreateVoxels((Vector3i)rightPosition, colorMaterialSet, optRange / 2, activeShape);
                break;
            case OptState.delete:
                DestroyVoxels((Vector3i)rightPosition, optRange / 2, activeShape);
                break;
            case OptState.smooth:
                SmoothVoxels((Vector3i)rightPosition, optRange / 2);
                break;
        }
    }

    /*
    private void testHandleOVRInput()
    {
        // please see https://developer.oculus.com/documentation/game-engines/latest/concepts/unity-ovrinput/ to known the mapping

        // Axis2D
        Axis2D_L = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        Axis2D_R = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        Axis2D_LB_Center = OVRInput.Get(OVRInput.Button.PrimaryThumbstick);
        Axis2D_LB_Left = OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft);
        Axis2D_LB_Right = OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight);
        Axis2D_LB_Up = OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp);
        Axis2D_LB_Down = OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown);

        Axis2D_RB_Center = OVRInput.Get(OVRInput.Button.SecondaryThumbstick);
        Axis2D_RB_Left = OVRInput.Get(OVRInput.Button.SecondaryThumbstickLeft);
        Axis2D_RB_Right = OVRInput.Get(OVRInput.Button.SecondaryThumbstickRight);
        Axis2D_RB_Up = OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp);
        Axis2D_RB_Down = OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown);

        // Axis1D
        Axis1D_LB = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.Touch);
        Axis1D_LT = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.Touch);

        Axis1D_RB = OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger, OVRInput.Controller.Touch);
        Axis1D_RT = OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger, OVRInput.Controller.Touch);

        // Button
        Button_A = OVRInput.Get(OVRInput.Button.One);
        Button_B = OVRInput.Get(OVRInput.Button.Two);
        Button_X = OVRInput.Get(OVRInput.Button.Three);
        Button_Y = OVRInput.Get(OVRInput.Button.Four);

        if (Axis1D_LB > 0)
        {
            colorMaterialSet.weights[2] = 255;
            colorMaterialSet.weights[1] = 0;
            colorMaterialSet.weights[0] = 0;
            CreateVoxels((Vector3i)leftPosition, colorMaterialSet, drawRange / 2, -1);
        }

        if (Axis1D_RB > 0)
        {
            colorMaterialSet.weights[2] = 0;
            colorMaterialSet.weights[1] = 255;
            colorMaterialSet.weights[0] = 0;
            CreateVoxels((Vector3i)rightPosition, colorMaterialSet, smoothRange / 2, 1);
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
    */

    private void VoxelSetting(Vector3i Pos, MaterialSet materialSet, int range, OptShape optshape)
    {
        int xPos = Pos.x;
        int yPos = Pos.y;
        int zPos = Pos.z;

        switch (optshape)
        {
            case OptShape.cube:
                for (int z = zPos - range; z < zPos + range; z++)
                {
                    for (int y = yPos - range; y < yPos + range; y++)
                    {
                        for (int x = xPos - range; x < xPos + range; x++)
                        {
                            terrainVolume.data.SetVoxel(x, y, z, materialSet);
                        }
                    }
                }
                break;

            case OptShape.sphere:
                int rangeSphere = range * range;
                for (int z = zPos - range; z < zPos + range; z++)
                {
                    for (int y = yPos - range; y < yPos + range; y++)
                    {
                        for (int x = xPos - range; x < xPos + range; x++)
                        {
                            int xDistance = x - xPos;
                            int yDistance = y - yPos;
                            int zDistance = z - zPos;

                            int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
                            if (distSquared < rangeSphere)
                            {
                                terrainVolume.data.SetVoxel(x, y, z, materialSet);
                            }
                        }
                    }
                }
                TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range, zPos - range, xPos + range, yPos + range, zPos + range));
                break;

            case OptShape.cylinder:
                int rangeCircle = range * range;
                for (int z = zPos - range; z < zPos + range; z++)
                {
                    for (int y = yPos - range * 2; y < yPos + range * 2; y++)
                    {
                        for (int x = xPos - range; x < xPos + range; x++)
                        {
                            int xDistance = x - xPos;
                            int yDistance = y - yPos;
                            int zDistance = z - zPos;

                            int distSquared = xDistance * xDistance + zDistance * zDistance;
                            if (distSquared < rangeCircle)
                            {
                                terrainVolume.data.SetVoxel(x, y, z, materialSet);
                            }
                        }
                    }
                }
                break;

            case OptShape.capsule:
                int rangeCapsule = range * range;
                for (int z = zPos - range; z < zPos + range; z++)
                {
                    for (int y = yPos - range; y < yPos + range; y++)
                    {
                        for (int x = xPos - range; x < xPos + range; x++)
                        {
                            int xDistance = x - xPos;
                            int yDistance = y - yPos;
                            int zDistance = z - zPos;

                            int distSquared = xDistance * xDistance + zDistance * zDistance;
                            if (distSquared < rangeCapsule)
                            {
                                terrainVolume.data.SetVoxel(x, y, z, materialSet);
                            }
                        }
                    }
                }

                int upxPos = Pos.x;
                int upyPos = Pos.y + range;
                int upzPos = Pos.z;
                int rangeupSphere = range * range;
                for (int z = upzPos - range; z < upzPos + range; z++)
                {
                    for (int y = upyPos; y < upyPos + range; y++)
                    {
                        for (int x = upxPos - range; x < upxPos + range; x++)
                        {
                            int xDistance = x - upxPos;
                            int yDistance = y - upyPos;
                            int zDistance = z - upzPos;

                            int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
                            if (distSquared < rangeupSphere)
                            {
                                terrainVolume.data.SetVoxel(x, y, z, materialSet);
                            }
                        }
                    }
                }

                int downxPos = Pos.x;
                int downyPos = Pos.y - range;
                int downzPos = Pos.z;
                int rangedownSphere = range * range;
                for (int z = downzPos - range; z < downzPos + range; z++)
                {
                    for (int y = downyPos - range; y < downyPos; y++)
                    {
                        for (int x = downxPos - range; x < downxPos + range; x++)
                        {
                            int xDistance = x - downxPos;
                            int yDistance = y - downyPos;
                            int zDistance = z - downzPos;

                            int distSquared = xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
                            if (distSquared < rangedownSphere)
                            {
                                terrainVolume.data.SetVoxel(x, y, z, materialSet);
                            }
                        }
                    }
                }
                TerrainVolumeEditor.BlurTerrainVolume(terrainVolume, new Region(xPos - range, yPos - range * 2, zPos - range, xPos + range, yPos + range * 2, zPos + range));
                break;
        }

    }

    private void DestroyVoxels(Vector3i Pos, int range, OptShape optshape)
    {
        MaterialSet emptyMaterialSet = new MaterialSet();
        VoxelSetting(Pos, emptyMaterialSet, range, optshape);
    }

    private void CreateVoxels(Vector3i Pos, MaterialSet materialSet, int range, OptShape optshape)
    {
        VoxelSetting(Pos, materialSet, range, optshape);
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

    public int GetOptRange()
    {
        return optRange;
    }

    public ControlPanel GetActivePanel()
    {
        return activePanel;
    }

    public InfoPanel GetActiveInfoPanel()
    {
        return activeInfoPanel;
    }

    public OptState GetActiveState()
    {
        return activeState;
    }

    public OptShape GetActiveShape()
    {
        return activeShape;
    }

}

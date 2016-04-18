using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Cubiquity;
using Newtonsoft.Json;

public class VoxelStoreObj
{
    public int Type { get; set; }
    public float Time { get; set; }
    public int PosX { get; set; }
    public int PosY { get; set; }
    public int PosZ { get; set; }
    public float RotateEulerX { get; set; }
    public float RotateEulerY { get; set; }
    public float RotateEulerZ { get; set; }
    public int MaterialWeight0 { get; set; }
    public int MaterialWeight1 { get; set; }
    public int MaterialWeight2 { get; set; }
    public int MaterialWeight3 { get; set; }
    public int RangeX { get; set; }
    public int RangeY { get; set; }
    public int RangeZ { get; set; }
    public int Optshape { get; set; }
}

public class VoxelStoreSmooth
{
    public int Type { get; set; }
    public float Time { get; set; }
    public float UpcornerX { get; set; }
    public float UpcornerY { get; set; }
    public float UpcornerZ { get; set; }
    public float LowcornerX { get; set; }
    public float LowcornerY { get; set; }
    public float LowcornerZ { get; set; }
}

public class VoxelOpt
{
    public Vector3 Pos { get; set; }
    public MaterialSet MaterialWeight { get; set; }
}

public class RecordBehaviour : MonoBehaviour {

    private StreamWriter file;

    private List<List<VoxelOpt>> optStack;
    private int optPos = -1;

    // Use this for initialization
    void Awake () {

        string randomName = Path.GetRandomFileName();
        file = new System.IO.StreamWriter("Record/RecordOpt_" + randomName + ".txt");

        // Example
        //string lines = "First line.\r\nSecond line.\r\nThird line.";
        //file.WriteLine(lines);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void UnDo()
    {

    }

    public void ReDo()
    {

    }

    public void StartNewOperator()
    {

    }

    public void PushOperator()
    {

    }

    public void PopOperator()
    {

    }

    public void Write(Vector3i Pos, Vector3 RotateEuler, MaterialSet materialSet, Vector3i range, OptShape optshape, float mtime)
    {
        VoxelStoreObj temp = new VoxelStoreObj
        {
            Type = 1,
            Time = mtime,
            PosX = Pos.x,
            PosY = Pos.y,
            PosZ = Pos.z,
            RotateEulerX = RotateEuler.x,
            RotateEulerY = RotateEuler.y,
            RotateEulerZ = RotateEuler.z,
            MaterialWeight0 = materialSet.weights[0],
            MaterialWeight1 = materialSet.weights[1],
            MaterialWeight2 = materialSet.weights[2],
            MaterialWeight3 = materialSet.weights[3],
            RangeX = range.x,
            RangeY = range.y,
            RangeZ = range.z,
            Optshape = (int)optshape
        };
        string jsonMsg = JsonConvert.SerializeObject(temp, Formatting.Indented);
        file.WriteLine(jsonMsg.ToString());
    }

    public void WriteSmooth(Region mregion, float mtime)
    {
        VoxelStoreSmooth temp = new VoxelStoreSmooth
        {
            Type = 2,
            Time = mtime,
            LowcornerX = mregion.lowerCorner.x,
            LowcornerY = mregion.lowerCorner.y,
            LowcornerZ = mregion.lowerCorner.z,
            UpcornerX = mregion.upperCorner.x,
            UpcornerY = mregion.upperCorner.y,
            UpcornerZ = mregion.upperCorner.z,
        };
        string jsonMsg = JsonConvert.SerializeObject(temp, Formatting.Indented);
        file.WriteLine(jsonMsg.ToString());
    }

    void OnApplicationQuit()
    {
        file.Close();
    }
}

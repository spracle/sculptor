using UnityEngine;
using System.Collections;

public class HandCanvasControl : MonoBehaviour {

    public GameObject HandObject = null;

    public GameObject leftHandAnchor = null;
    public GameObject rightHandAnchor = null;

    public GameObject mainPanel;
    public GameObject statePanel;
    public GameObject shapePanel;
    public GameObject colorPanel;
    public GameObject readfilePanel;

    private ControlPanel activePanel;

    private HandBehaviour handBehaviour;

    // Use this for initialization
    void Start () {

        mainPanel.SetActive(false);
        statePanel.SetActive(false);
        shapePanel.SetActive(false);
        colorPanel.SetActive(false);
        readfilePanel.SetActive(false);

        activePanel = ControlPanel.empty;
    }
	
	// Update is called once per frame
	void Update () {

        handBehaviour = HandObject.GetComponent<HandBehaviour>();
        ControlPanel nowPanel = handBehaviour.GetActivePanel();
        DrawPos nowPos = handBehaviour.GetActiveDrawPos();

        if (nowPos == DrawPos.left)
        {
            transform.position = leftHandAnchor.transform.position;
        }
        else
        {
            transform.position = rightHandAnchor.transform.position;
        }
        transform.rotation = Camera.main.transform.rotation;

        // here only use to update the canvas contents. all the hand behavior handle in handBehivor.cs

        if (nowPanel != activePanel)
        {
            switch (nowPanel)
            {
                case ControlPanel.empty:
                    mainPanel.SetActive(false);
                    statePanel.SetActive(false);
                    shapePanel.SetActive(false);
                    colorPanel.SetActive(false);
                    readfilePanel.SetActive(false);
                    break;
                case ControlPanel.main:
                    mainPanel.SetActive(true);
                    statePanel.SetActive(false);
                    shapePanel.SetActive(false);
                    colorPanel.SetActive(false);
                    readfilePanel.SetActive(false);
                    break;
                case ControlPanel.state:
                    mainPanel.SetActive(false);
                    statePanel.SetActive(true);
                    shapePanel.SetActive(false);
                    colorPanel.SetActive(false);
                    readfilePanel.SetActive(false);
                    break;
                case ControlPanel.shape:
                    mainPanel.SetActive(false);
                    statePanel.SetActive(false);
                    shapePanel.SetActive(true);
                    colorPanel.SetActive(false);
                    readfilePanel.SetActive(false);
                    break;
                case ControlPanel.color:
                    mainPanel.SetActive(false);
                    statePanel.SetActive(false);
                    shapePanel.SetActive(false);
                    colorPanel.SetActive(true);
                    readfilePanel.SetActive(false);
                    break;
                case ControlPanel.readfile:
                    mainPanel.SetActive(false);
                    statePanel.SetActive(false);
                    shapePanel.SetActive(false);
                    colorPanel.SetActive(false);
                    readfilePanel.SetActive(true);
                    break;
            }
            activePanel = nowPanel;
        }

    }


}

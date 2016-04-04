using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeadCanvasControl : MonoBehaviour {

    public GameObject HandObject = null;

    public GameObject startPanel;
    public GameObject infoPanel;

    private HandBehaviour handBehaviour;

    private InfoPanel activeInfoPanel;
    private OptState activeState;
    private OptShape activeShape;

    private string infoText;
    private Text showText;

    void Start()
    {
        handBehaviour = HandObject.GetComponent<HandBehaviour>();

        startPanel.SetActive(false);
        infoPanel.SetActive(false);
    }

    void Update()
    {

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0.6f));
        transform.rotation = Camera.main.transform.rotation;

        activeInfoPanel = handBehaviour.GetActiveInfoPanel();
        switch (activeInfoPanel)
        {
            case InfoPanel.start:
                startPanel.SetActive(true);
                infoPanel.SetActive(false);
                startPanelHandle();
                break;
            case InfoPanel.info:
                startPanel.SetActive(false);
                infoPanel.SetActive(true);
                infoPanelHandle();
                break;
            case InfoPanel.empty:
                startPanel.SetActive(false);
                infoPanel.SetActive(false);
                break;
        }
    }

    void infoPanelHandle()
    {
        activeState = handBehaviour.GetActiveState();
        activeShape = handBehaviour.GetActiveShape();

        // show info
        infoPanel.GetComponentInChildren<Text>().fontSize = 12;
        infoPanel.GetComponentInChildren<Text>().color = Color.green;
        infoPanel.GetComponentInChildren<Text>().text = "State: " + activeState.ToString() + "\nShape: " + activeShape.ToString();
    }

    void startPanelHandle()
    {
        startPanel.GetComponentInChildren<Text>().color = Color.red;
    }

}

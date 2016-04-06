using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HsvBoxSelector : MonoBehaviour{

    public HSVPicker picker;

    private HandBehaviour handBehaviour;

    private Vector2 HandAxis2D = new Vector2(0, 0);

    // Use this for initialization
    void Start () {

        handBehaviour = GetComponent<HandBehaviour>();

    }
	
	// Update is called once per frame
	void Update () {

        //HandAxis2D = handBehaviour.GetHSVAxis2D();
        //PlaceCursorVR(HandAxis2D);

	}

    void PlaceCursorVR(Vector2 AxisValue)
    {
        Vector2 pos = new Vector2();
        pos.y = picker.hsvImage.rectTransform.rect.height * picker.hsvImage.transform.lossyScale.y - (picker.hsvImage.rectTransform.position.y - AxisValue.y);
        pos.x = picker.hsvImage.rectTransform.rect.width * picker.hsvImage.transform.lossyScale.x - (picker.hsvImage.rectTransform.position.x - AxisValue.x);
        pos.x /= picker.hsvImage.rectTransform.rect.width * picker.hsvImage.transform.lossyScale.x;
        pos.y /= picker.hsvImage.rectTransform.rect.height * picker.hsvImage.transform.lossyScale.y;
        pos.x -= .5f;

        pos.x = Mathf.Clamp(pos.x, 0, .9999f);  //1 is the same as 0
        pos.y = Mathf.Clamp(pos.y, 0, .9999f);
        picker.MoveCursor(pos.x, pos.y);
    }

    // for mouse pick
    /*
    void PlaceCursor(PointerEventData eventData)
    {
		Vector2 pos = new Vector2();
        pos.y = picker.hsvImage.rectTransform.rect.height * picker.hsvImage.transform.lossyScale.y - (picker.hsvImage.rectTransform.position.y - eventData.position.y);
        pos.x = picker.hsvImage.rectTransform.rect.width * picker.hsvImage.transform.lossyScale.x - (picker.hsvImage.rectTransform.position.x - eventData.position.x);
        pos.x /= picker.hsvImage.rectTransform.rect.width * picker.hsvImage.transform.lossyScale.x;
        pos.y /= picker.hsvImage.rectTransform.rect.height * picker.hsvImage.transform.lossyScale.y;
        pos.x -=.5f;

        pos.x = Mathf.Clamp(pos.x, 0, .9999f);  //1 is the same as 0
        pos.y = Mathf.Clamp(pos.y, 0, .9999f);

        picker.MoveCursor(pos.x, pos.y);
    }


    public void OnDrag(PointerEventData eventData)
    {
        PlaceCursor(eventData);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlaceCursor(eventData);
    }
    */
}

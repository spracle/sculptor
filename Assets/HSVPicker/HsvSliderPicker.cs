using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HsvSliderPicker : MonoBehaviour
{

    public HSVPicker picker;
    public GameObject HandObject;

    private HandBehaviour handBehaviour;
    private Vector2 HandAxis2D = new Vector2(0, 0);

    // Use this for initialization
    void Start()
    {
        handBehaviour = HandObject.GetComponent<HandBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (HandAxis2D != handBehaviour.GetHSVAxis2DSlider())
        {
            HandAxis2D = handBehaviour.GetHSVAxis2DSlider();
            PlacePointerVR(HandAxis2D);
        }
    }

    void PlacePointerVR(Vector2 AxisValue)
    {
        var pos = new Vector2(picker.hsvSlider.rectTransform.position.x - AxisValue.x, AxisValue.y - picker.hsvSlider.rectTransform.position.y);
        pos.x /= picker.hsvSlider.rectTransform.rect.height * picker.hsvSlider.canvas.transform.lossyScale.y;

        pos.x = Mathf.Clamp(pos.x, 0, 1f);
        picker.MovePointer(pos.x);
    }

    /*
    void PlacePointer(PointerEventData eventData)
    {
		if (sm == SliderModes.Horizontal) {
  
			var pos = new Vector2 (picker.hsvSlider.rectTransform.position.x - eventData.position.x, eventData.position.y - picker.hsvSlider.rectTransform.position.y);

			pos.x /= picker.hsvSlider.rectTransform.rect.height * picker.hsvSlider.canvas.transform.lossyScale.y;

			//Debug.Log(eventData.position.ToString() + " " + picker.hsvSlider.rectTransform.position + " " + picker.hsvSlider.rectTransform.rect.height);
			pos.x = Mathf.Clamp (pos.x, 0, 1f);

			picker.MovePointer (pos.x);

		} else if (sm == SliderModes.Vertical) {

			var pos = new Vector2(eventData.position.x - picker.hsvSlider.rectTransform.position.x, picker.hsvSlider.rectTransform.position.y - eventData.position.y);
			
			pos.y /= picker.hsvSlider.rectTransform.rect.height * picker.hsvSlider.canvas.transform.lossyScale.y;
			
			//Debug.Log(eventData.position.ToString() + " " + picker.hsvSlider.rectTransform.position + " " + picker.hsvSlider.rectTransform.rect.height);
			pos.y = Mathf.Clamp(pos.y, 0, 1f);
			
			picker.MovePointer(pos.y);

		}
    }


    public void OnDrag(PointerEventData eventData)
    {
        PlacePointer(eventData);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlacePointer(eventData);
    }
    */
}
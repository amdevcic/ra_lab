using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderText : MonoBehaviour
{
    TMP_Text text;
    private void Start() {
        Slider slider = GetComponent<Slider>();
        text = transform.Find("Value").GetComponent<TextMeshProUGUI>();

        text.text = slider.value.ToString("0.##");
        slider.onValueChanged.AddListener((value) => {text.text = value.ToString("0.##");});
    }

    public void Refresh() {
        text.text = GetComponent<Slider>().value.ToString("0.##");
    }
}
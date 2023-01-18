using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    void Start()
    {
        foreach (Slider s in transform.GetComponentsInChildren<Slider>())
        {
            s.onValueChanged.AddListener((value)=>{ModelManager.Instance.SetMaterialValue(value, s.name);});
        }
        ModelManager.Instance.ResetMaterial();
    }

    public void ResetSliders() {
        foreach (Slider s in transform.GetComponentsInChildren<Slider>())
        {
            s.value = (s.maxValue+s.minValue)/2;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ModelManager.Instance.ResetMaterial();
            Application.Quit();
        }
    }
}

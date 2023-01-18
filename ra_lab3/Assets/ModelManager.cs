using UnityEngine;

public class ModelManager : MonoBehaviour
{
    // Start is called before the first frame update
    Transform activeChild;
    public static ModelManager Instance { get; private set; }
    public Material material;
    void Awake()
    {
        if (Instance != null && Instance != this) 
            Destroy(this); 
        else 
            Instance = this; 
        
        activeChild=transform.GetChild(0);
        GetComponent<FollowMouse>().model = activeChild;
    }

    public void ResetModel() {
        transform.position = Vector3.zero;
    }

    public void SetMaterialValue(float val, string name) {
        material.SetFloat(name, val);
    }

    public void ResetMaterial() {
        material.SetFloat("_bendAngle", 0);
        material.SetFloat("_yScale", 1);
        material.SetFloat("_xShear", 0);
        material.SetFloat("_zShear", 0);
        material.SetFloat("_sineAmplitude", 1);
        material.SetFloat("_sineWavelength", 5.05f);
        material.SetFloat("_sineSpeed", 0);
        material.SetFloat("_bobAmplitude", 1);
        material.SetFloat("_bobSpeed", 0);
        material.SetFloat("_rotationSpeed", 0);
    }

    public void ChangeModel(int index) {
        activeChild=transform.GetChild(index);
        GetComponent<FollowMouse>().model = activeChild;
        for (int c=0; c<transform.childCount; c++)
            transform.GetChild(c).gameObject.SetActive(c==index);
    }
}

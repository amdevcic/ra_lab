using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    Material mat;
    float height;
    Vector3 velocity;
    public float wiggleIntensity = 1.5f;
    [Range(0, 1)]
    public float smoothness = 0.5f;
    public Transform model;

    void Start()
    {
        mat = ModelManager.Instance.material;
        height = model.GetComponent<MeshRenderer>().bounds.extents.y;
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            transform.position = Vector3.Lerp(transform.position, transform.position+velocity, smoothness);
            velocity = hit.point+height/2*Vector3.down - transform.position;
            mat.SetFloat("_xShear", velocity.x*wiggleIntensity);
            mat.SetFloat("_zShear", velocity.z*wiggleIntensity);
            mat.SetFloat("_yScale", 1+velocity.y*wiggleIntensity);
        }
    }

}

using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public float distance;
    public bool followsMouse = true;

    void Update()
    {
        if (followsMouse)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            transform.position = ray.origin + ray.direction * distance;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            followsMouse = !followsMouse;
        }
    }
}

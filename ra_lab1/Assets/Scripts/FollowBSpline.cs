using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBSpline : MonoBehaviour
{
    public BSpline spline;
    public LineRenderer tangentLine;
    public LineRenderer rotationAxisLine;
    public float lineLength;
    private void Start() {
        tangentLine.positionCount = 2;
        rotationAxisLine.positionCount = 2;
        transform.position = spline.GetPositionAt(0, 0);
        DrawTangent(0, 0);
        RotateObject(0, 0);
    }

    public void StartMovement() {
        StopAllCoroutines();
        StartCoroutine("MoveAlongSpline");
    }

    void DrawTangent(int segment, float t) {
        Vector3 tg = spline.GetTangent(segment, t);
        tangentLine.SetPosition(0, transform.position);
        tangentLine.SetPosition(1, transform.position+tg*lineLength);
    }

    void RotateObject(int segment, float t) {
        Vector3 s = transform.forward;
        Vector3 e = spline.GetTangent(segment, t);

        Vector3 rotationAxis = Vector3.Cross(s, e).normalized;

        rotationAxisLine.SetPosition(0, transform.position);
        rotationAxisLine.SetPosition(1, transform.position+rotationAxis*lineLength);

        float phi = Mathf.Acos(Vector3.Dot(s, e))*Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(phi, rotationAxis) * transform.rotation;

        //naivno
        //transform.rotation = Quaternion.LookRotation(e);
        //ili
        // transform.forward = e;
    }

    IEnumerator MoveAlongSpline() 
    {
        for (int i=0; i<spline.GetSegmentCount(); i++) {
            for (float t=0; t<1; t+=0.05f) {
                transform.position = spline.GetPositionAt(i, t);
                DrawTangent(i, t);
                RotateObject(i, t);
                
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
}

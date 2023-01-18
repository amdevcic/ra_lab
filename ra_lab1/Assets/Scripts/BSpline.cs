using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BSpline : MonoBehaviour
{
    public TextAsset data;
    List<Vector3> nodes;
    List<Vector3> vertices;

    private static float[,] B = {
        {-1f/6, 3f/6, -3f/6, 1f/6},
        {3f/6, -6f/6, 3f/6, 0},
        {-3f/6, 0, 3f/6, 0},
        {1f/6, 4f/6, 1f/6, 0},
    };

    private static float[,] MatrixMul(float[,] first, float[,] second) {
        int ha = first.GetLength(0),
            wa = first.GetLength(1),
            hb = second.GetLength(0),
            wb = second.GetLength(1);
        float[,] res = new float[ha, wb];
        if (hb != wa) {
            return null;
        }
        float temp;
        for (int i=0; i<ha; i++) {
            for (int j=0; j<wb; j++) {
                temp = 0;
                for (int k=0; k<hb; k++) {
                    temp += first[i, k] * second[k, j];
                }
                res[i, j] = temp;
            }
        }
        return res;
    }

    public List<Vector3> GetVertices() {
        return vertices;
    }

    public int GetSegmentCount() {
        return nodes.Count-3;
    }

    public Vector3 GetTangent(int segment, float t) {
        if (segment>=GetSegmentCount()) {
            segment = GetSegmentCount()-1;
        }
        float[,] T = {{3*t*t, 2*t, 1, 0}};
        float[,] r = new float[4, 3];
            for (int j=0; j<4; j++) {
                r[j, 0] = nodes[segment+j].x;
                r[j, 1] = nodes[segment+j].y;
                r[j, 2] = nodes[segment+j].z;
            }
        float[,] br = MatrixMul(B, r);
        float[,] p = MatrixMul(T, br);
        return new Vector3(p[0,0], p[0,1], p[0,2]).normalized;
    }

    public Vector3 GetPositionAt(int segment, float t) {
        float[,] r = new float[4, 3];
        for (int j=0; j<4; j++) {
            r[j, 0] = nodes[segment+j].x;
            r[j, 1] = nodes[segment+j].y;
            r[j, 2] = nodes[segment+j].z;
        }
        float[,] br = MatrixMul(B, r);
        float[,] p;
        float[,] T = {{t*t*t, t*t, t, 1}};
        p = MatrixMul(T, br);
        return new Vector3(p[0, 0], p[0, 1], p[0, 2]);
    }

    public Quaternion GetRotationAt(int segment, int t) {
        //todo
        return Quaternion.identity;
    }

    private void Awake() 
    {
        UpdateNodes();
    }

    public void UpdateNodes()
    {
        nodes = new List<Vector3>();
        vertices = new List<Vector3>();

        using (System.IO.StringReader reader = new System.IO.StringReader(data.text))
        {
            string[] node;
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                node = line.Split(' ');
                nodes.Add(new Vector3(
                    float.Parse(node[0]), 
                    float.Parse(node[1]),
                    float.Parse(node[2])
                    ));
            }
        }
        // za svaki segment
        for (int i=0; i<GetSegmentCount(); i++) {
            for (float t=0; t<1.0f; t+=0.05f) {
                vertices.Add(GetPositionAt(i, t));
            }
        }

        LineRenderer lrenderer = GetComponent<LineRenderer>();

        lrenderer.positionCount = vertices.Count;
        lrenderer.SetPositions(vertices.ToArray());

    }
}

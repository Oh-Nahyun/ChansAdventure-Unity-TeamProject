using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCatch_Line : MonoBehaviour
{
    public Transform start;
    public Transform interpolation1;
    public Transform interpolation2;
    public Transform end;

    [Min(2)]
    public int lineCount = 5;

    float preCalculateLineCount;

    LineRenderer line1;
    LineRenderer line2;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        line1 = child.GetComponent<LineRenderer>();
        line1.positionCount = lineCount;
        child = transform.GetChild(1);
        line2 = child.GetComponent<LineRenderer>();
        line2.positionCount = lineCount;
        preCalculateLineCount = 1.0f / (lineCount - 1);
    }

    public void Initialize(Transform start, Transform interpolation1, Transform interpolation2, Transform end)
    {
        this.start = start;
        this.interpolation1 = interpolation1;
        this.interpolation2 = interpolation2;
        this.end = end;
    }

    private void Update()
    {
        Vector3 startPos = start.position;
        Vector3 interPos1 = interpolation1.position;
        Vector3 interPos2 = interpolation2.position;
        Vector3 endPos = end.position;

        for(int i = 0; i < line1.positionCount; i++)
        {
            Vector3 point = Bezier(startPos, interPos1, interPos2, endPos, (float)(i * preCalculateLineCount));
            line1.SetPosition(i, point);
            line2.SetPosition(i, point);
        }

    }

    Vector3 Bezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
    {
        Vector3 M0 = Vector3.Lerp(P0, P1, t);
        Vector3 M1 = Vector3.Lerp(P1, P2, t);
        Vector3 M2 = Vector3.Lerp(P2, P3, t);

        Vector3 B0 = Vector3.Lerp(M0, M1, t);
        Vector3 B1 = Vector3.Lerp(M1, M2, t);

        return Vector3.Lerp(B0, B1, t);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<Transform> pathPoints;
    private List<Vector3> smoothPathPoints;  

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.1f;
    [SerializeField] private float maxOffset = 1f;
    [SerializeField] private float rotationSpeed = 5f;  
    [SerializeField] private int interpolationSteps = 10; 

    private int currentPointIndex = 0;
    private float pathOffset;
    private bool pathSet = false;

    public void SetPath(List<Transform> newPath)
    {
        pathPoints = newPath;
        pathOffset = Random.Range(-maxOffset, maxOffset);
        currentPointIndex = 0;

        GenerateSmoothPath();
        pathSet = true;
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }

    private void GenerateSmoothPath()
    {
        smoothPathPoints = new List<Vector3>();

       
        if (pathPoints.Count < 4)
        {
            foreach (var t in pathPoints)
                smoothPathPoints.Add(t.position);
            return;
        }

        for (int i = 0; i < pathPoints.Count - 3; i++)
        {
            Vector3 p0 = pathPoints[i].position;
            Vector3 p1 = pathPoints[i + 1].position;
            Vector3 p2 = pathPoints[i + 2].position;
            Vector3 p3 = pathPoints[i + 3].position;

            for (int step = 0; step < interpolationSteps; step++)
            {
                float t = step / (float)interpolationSteps;
                Vector3 pointOnCurve = CatmullRom(p0, p1, p2, p3, t);
                smoothPathPoints.Add(pointOnCurve);
            }
        }

        smoothPathPoints.Add(pathPoints[pathPoints.Count - 2].position);
        smoothPathPoints.Add(pathPoints[pathPoints.Count - 1].position);
    }

    private void Update()
    {
        if (!pathSet || currentPointIndex >= smoothPathPoints.Count) return;

        Vector3 current = smoothPathPoints[currentPointIndex];
        Vector3 next = (currentPointIndex + 1 < smoothPathPoints.Count)
            ? smoothPathPoints[currentPointIndex + 1]
            : current;

        Vector3 forward = (next - current).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 offsetTarget = current + right * pathOffset;

        Vector3 oldPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, offsetTarget, moveSpeed * Time.deltaTime);

        Vector3 moveDir = (transform.position - oldPosition).normalized;
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, offsetTarget) < stoppingDistance)
        {
            currentPointIndex++;
        }
    }
}

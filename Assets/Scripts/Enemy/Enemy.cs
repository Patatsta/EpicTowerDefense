using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private List<Transform> _pathPoints;
    private List<Vector3> _smoothPathPoints;  

    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _stoppingDistance = 0.1f;
    [SerializeField] private float _maxOffset = 1f;
    [SerializeField] private float _rotationSpeed = 5f;  
    [SerializeField] private int _interpolationSteps = 10; 

    private int _currentPointIndex = 0;
    private float _pathOffset;
    private bool _pathSet = false;

    public void SetPath(List<Transform> newPath)
    {
        _pathPoints = newPath;
        _pathOffset = Random.Range(-_maxOffset, _maxOffset);
        _currentPointIndex = 0;

        GenerateSmoothPath();
        _pathSet = true;
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
        _smoothPathPoints = new List<Vector3>();

       
        if (_pathPoints.Count < 4)
        {
            foreach (var t in _pathPoints)
                _smoothPathPoints.Add(t.position);
            return;
        }

        for (int i = 0; i < _pathPoints.Count - 3; i++)
        {
            Vector3 p0 = _pathPoints[i].position;
            Vector3 p1 = _pathPoints[i + 1].position;
            Vector3 p2 = _pathPoints[i + 2].position;
            Vector3 p3 = _pathPoints[i + 3].position;

            for (int step = 0; step < _interpolationSteps; step++)
            {
                float t = step / (float)_interpolationSteps;
                Vector3 pointOnCurve = CatmullRom(p0, p1, p2, p3, t);
                _smoothPathPoints.Add(pointOnCurve);
            }
        }

        _smoothPathPoints.Add(_pathPoints[_pathPoints.Count - 2].position);
        _smoothPathPoints.Add(_pathPoints[_pathPoints.Count - 1].position);
    }

    private void Update()
    {
        if (!_pathSet || _currentPointIndex >= _smoothPathPoints.Count) return;

        Vector3 current = _smoothPathPoints[_currentPointIndex];
        Vector3 next = (_currentPointIndex + 1 < _smoothPathPoints.Count)
            ? _smoothPathPoints[_currentPointIndex + 1]
            : current;

        Vector3 forward = (next - current).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);
        Vector3 offsetTarget = current + right * _pathOffset;

        Vector3 oldPosition = transform.position;

        transform.position = Vector3.MoveTowards(transform.position, offsetTarget, _moveSpeed * Time.deltaTime);

        Vector3 moveDir = (transform.position - oldPosition).normalized;
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        if (Vector3.Distance(transform.position, offsetTarget) < _stoppingDistance)
        {
            _currentPointIndex++;
        }
    }
}

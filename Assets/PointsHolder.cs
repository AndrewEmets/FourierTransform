using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PointsHolder : MonoBehaviour
{
    [SerializeField, HideInInspector] private List<ComplexNumber> points = new List<ComplexNumber>();

    public List<ComplexNumber> Points
    {
        get
        {
            return points;
        }
    }

    private readonly List<float> distances = new List<float>();
    private float totalDistance;

    public void OnValidate()
    {
        CalculateTotalDistance();
    }

    private void CalculateTotalDistance()
    {
        distances.Clear();
        totalDistance = 0;
        for (var i = 0; i < points.Count; i++)
        {
            var l = Vector2.Distance(points[i], points[(i + 1) % points.Count]);

            totalDistance += l;
            distances.Add(l);
        }
    }

    public Vector2 GetPoint(float t)
    {
        t = Mathf.Repeat(t, 1f);
        
        var r = totalDistance * t;
        for (var i = 0; i < distances.Count; i++)
        {
            if (r >= distances[i])
            {
                r -= distances[i];
            }
            else
            {
                return Vector3.LerpUnclamped(points[i], points[(i + 1) % points.Count], r / distances[i]);
            }
        }

        return points[points.Count - 1];
    }

    private void OnDrawGizmos()
    {
        var p = GetPoint(Time.time);
        Gizmos.DrawSphere(p, 0.1f);

        for (int i = 0; i < points.Count; i++)
        {
            Gizmos.DrawLine(points[i], points[(i + 1) % points.Count]);
        }
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(PointsHolder))]
    public class PointsHolderEditor : Editor
    {
        private PointsHolder f;

        private void OnEnable()
        {
            f = target as PointsHolder;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Add point"))
            {
                f.points.Add(new ComplexNumber());
                
                f.OnValidate();
                f.GetComponent<Fourier>().OnValidate();
                
                EditorUtility.SetDirty(f);
            }
        }

        private void OnSceneGUI()
        {
            for (var i = 0; i < f.points.Count; i++)
            {
                var cn = (ComplexNumber) Handles.FreeMoveHandle(f.points[i], Quaternion.identity, 0.1f, Vector3.zero,
                    Handles.DotHandleCap);
                
                if (f.points[i] != cn)
                {
                    f.points[i] = cn;
                    f.OnValidate();
                    f.GetComponent<Fourier>().OnValidate();
                    EditorUtility.SetDirty(target);
                }
            }

            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.A)
            {
                var worldray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                f.points.Add(new ComplexNumber(worldray.origin.x, worldray.origin.y));
            }

            /*
            for (int i = 0; i < f.Points.Count; i++)
            {
                Handles.DrawLine((Vector2)f.Points[i], (Vector2)f.points[(i + 1) % f.points.Count]);
            }
            */
        }
    }
#endif
}
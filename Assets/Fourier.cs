using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class Fourier : MonoBehaviour
{
    public void OnValidate()
    {
        Test();
    }

    private readonly List<FourierHelper.FourierData> res = new List<FourierHelper.FourierData>();
    private readonly List<Vector2> restored = new List<Vector2>();
    
    [SerializeField] private int takePoints = 10;
    [ContextMenu("Test points")]
    public void Test()
    {
        var points = GetComponent<PointsHolder>().Points;
        
        //var interpolatedPoints = Data.drawing.Where((d, i) => i%10==0).Select(d => new ComplexNumber(d.x, -d.y)).ToList();
        
        var interpolatedPoints = new List<ComplexNumber>();
        for (var i = 0; i < takePoints; i++)
        {
            var v = GetComponent<PointsHolder>().GetPoint(i / (float)takePoints);
            interpolatedPoints.Add(new ComplexNumber(v.x, v.y));
        }

/*
        var interpolatedPoints = new List<ComplexNumber>(points.Count * 2);
        for (var i = 0; i < points.Count; i++)
        {
            interpolatedPoints.Add(points[i]);
            interpolatedPoints.Add((points[i] + points[(i + 1) % points.Count]) / 2f);
        }
*/

        //var interpolatedPoints = GetComponent<PointsHolder>().Points;

        res.Clear();
        restored.Clear();
        
        res.AddRange(FourierHelper.DFT(interpolatedPoints).OrderByDescending(d => d.amplitude));
        
        for (var tn = 0; tn < 500; tn++)
        {
            var time = tn / 500f * 2f * Mathf.PI;
            
            var pos = Vector2.zero;
            for (var i = 0; i <  res.Count; i++)
            {
                var t = res[i].frequency * time + res[i].phase;
                pos += new Vector2(Mathf.Cos(t), Mathf.Sin(t)) * res[i].amplitude;
            }

            restored.Add(pos);
        }

        ExportString();
    }

    private void ExportString()
    {
        var freq = GetArrayString("freq", i => res[i].frequency);
        var amp = GetArrayString("amp", i => res[i].amplitude);
        var phase = GetArrayString("phase", i => res[i].phase);
        var size = res.Count;

        exportArrays = $"{freq}\n{amp}\n{phase}\n";
        exportArrays += $"const int size = {size};";
    }

    private string GetArrayString(string arrayName, Func<int, float> func)
    {
        var sb = new StringBuilder();

        sb.Append($"float {arrayName}[{res.Count}] = float[](");
        for (int i = 0; i < res.Count; i++)
        {
            sb.Append(func.Invoke(i).ToString("F5"));
            if (i != res.Count - 1)
                sb.Append(", ");
        }

        sb.Append(");");
        return sb.ToString();
    }

    [TextArea(minLines:10, maxLines:10)]
    public string exportArrays;


    private void OnDrawGizmos()
    {
        for (int i = 0; i < restored.Count; i++)
        {
            Gizmos.DrawLine(restored[i], restored[(i + 1) % restored.Count]);
        }

        float time = Time.time;
        var pos = Vector2.zero;
        
        Gizmos.color = Color.gray;
        for (var i = 0; i < res.Count; i++)
        {
            var t = res[i].frequency * time + res[i].phase;
            var prevPos = pos;
            pos += new Vector2(Mathf.Cos(t), Mathf.Sin(t)) * res[i].amplitude;
            
            DrawCircle(prevPos, res[i].amplitude);
            Gizmos.DrawLine(prevPos, pos);
        }

    }

    private void DrawCircle(Vector2 center, float radius)
    {
        Vector2 prevPoint = Vector2.right * radius + center;
        for (var i = 0; i <= 32; i++)
        {
            var t = i / 32f * 2f * Mathf.PI;
            var pos = new Vector2(Mathf.Cos(t), Mathf.Sin(t)) * radius + center;

            Gizmos.DrawLine(pos, prevPoint);
            
            prevPoint = pos;
        }
    }
}
using System;
using UnityEngine;

[Serializable]
public struct ComplexNumber
{
    public float a;
    public float b;

    public ComplexNumber(float a, float b)
    {
        this.a = a;
        this.b = b;
    }

    public static ComplexNumber operator -(ComplexNumber cn)
    {
        return new ComplexNumber(-cn.a, -cn.b);
    }

    public static ComplexNumber operator -(ComplexNumber cn1, ComplexNumber cn2)
    {
        return cn1 + -cn2;
    }

    public static ComplexNumber operator +(ComplexNumber cn1, ComplexNumber cn2)
    {
        return new ComplexNumber(cn1.a + cn2.a, cn1.b + cn2.b);
    }

    public static ComplexNumber operator *(ComplexNumber cn1, ComplexNumber cn2)
    {
        return new ComplexNumber(cn1.a * cn2.a - cn1.b * cn2.b, cn1.a * cn2.b + cn1.b * cn2.a);
    }

    public static ComplexNumber operator *(ComplexNumber cn, float f)
    {
        return new ComplexNumber(cn.a * f, cn.b * f);
    }
    public static ComplexNumber operator /(ComplexNumber cn, float f)
    {
        return new ComplexNumber(cn.a / f, cn.b / f);
    }

    public static implicit operator Vector2(ComplexNumber cn)
    {
        return new Vector2(cn.a, cn.b);
    }
    
    public static implicit operator Vector3(ComplexNumber cn)
    {
        return new Vector3(cn.a, cn.b, 0);
    }

    public static explicit operator ComplexNumber(Vector2 vec)
    {
        return  new ComplexNumber(vec.x, vec.y);
    }
    
    public static explicit operator ComplexNumber(Vector3 vec)
    {
        return  new ComplexNumber(vec.x, vec.y);
    }

    public static bool operator ==(ComplexNumber cn1, ComplexNumber cn2)
    {
        return cn1.a == cn2.a && cn1.b == cn2.b;
    }

    public static bool operator !=(ComplexNumber cn1, ComplexNumber cn2)
    {
        return !(cn1 == cn2);
    }
}
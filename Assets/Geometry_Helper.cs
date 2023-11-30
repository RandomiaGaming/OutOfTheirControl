using System.Collections.Generic;
using UnityEngine;
public static class Geometry_Helper
{
    public static float Distance_To_Line(Vector2 Line_Start, Vector2 Line_End, Vector2 Target_Point)
    {
        return Distance(Closest_Point_On_Line(Line_Start, Line_End, Target_Point), Target_Point);
    }

    public static Vector2 Closest_Point_On_Line(Vector2 Line_Start, Vector2 Line_End, Vector2 Target_Point)
    {
        Vector2 line = (Line_End - Line_Start);
        float len = line.magnitude;
        line.Normalize();

        Vector2 v = Target_Point - Line_Start;
        float d = Vector3.Dot(v, line);
        d = Mathf.Clamp(d, 0f, len);
        return Line_Start + line * d;
    }
    public static float Distance(Vector2 Point_A, Vector2 Point_B)
    {
        return Mathf.Sqrt(((Point_A.x - Point_B.x) * (Point_A.x - Point_B.x)) + ((Point_A.y - Point_B.y) * (Point_A.y - Point_B.y)));
    }
    public static float Degree_Clamp(float Input)
    {
        while (Input > 360)
        {
            Input -= 360;
        }
        while (Input < 0)
        {
            Input += 360;
        }
        return Input;
    }
    public static float Degree_Difference(float A, float B)
    {
        A = Degree_Clamp(A);
        B = Degree_Clamp(B);
        float Output = Mathf.Abs(A - B);
        if (Output > 180)
        {
            Output = 360 - Output;
        }
        return Mathf.Abs(Output);
    }
    public static Vector2 Degree_To_Vector(float Direction, float Magnitude)
    {
        return new Vector2(Mathf.Cos(Direction * Mathf.Deg2Rad), Mathf.Sin(Direction * Mathf.Deg2Rad)) * Magnitude;
    }
    public static Vector2 Degree_To_Vector(float Direction)
    {
        return Degree_To_Vector(Direction, 1);
    }

    public static Vector2 Vector_Clamp(Vector2 Input)
    {
        float Distance = Vector_Magnitude(Input);
        return new Vector2(Input.x / Distance, Input.y / Distance);
    }
    public static float Vector_To_Deg(Vector2 Input)
    {
        Input = Vector_Clamp(Input);
        float Output = Mathf.Atan(Mathf.Abs(Input.x) / Mathf.Abs(Input.y)) * Mathf.Rad2Deg;
        if (Input.x >= 0 && Input.y >= 0)
        {
            return 90 - Output;
        }
        else if (Input.x < 0 && Input.y >= 0)
        {
            return 90 + Output;
        }
        else if (Input.x < 0 && Input.y < 0)
        {
            return 180 + (90 - Output);
        }
        else if (Input.x >= 0 && Input.y < 0)
        {
            return 270 + Output;
        }
        else
        {
            return Output;
        }
    }
    public static float Vector_Magnitude(Vector2 Input)
    {
        return Mathf.Sqrt((Input.x * Input.x) + (Input.y * Input.y));
    }
    public static float Polygon_Area(List<Vector2> Points)
    {
        if (Points[Points.Count - 1] != Points[0])
        {
            Points.Add(Points[0]);
        }
        float area = 0;
        for (int i = 0; i < Points.Count - 1; i++)
        {
            area += (Points[i + 1].x - Points[i].x) * (Points[i + 1].y + Points[i].y) / 2;
        }
        return Mathf.Abs(area);
    }
    public static List<List<Vector2>> Triangulate(List<Vector2> Polygon_Points)
    {
        List<List<Vector2>> Output = new List<List<Vector2>>();
        while (Polygon_Points.Count > 3)
        {
            int First_Convex_Vertice = -1;
            for (int i = 0; i < Polygon_Points.Count; i++)
            {
                if (!Vertice_Is_Concave(Polygon_Points, i))
                {
                    First_Convex_Vertice = i;
                    break;
                }
            }
            if (First_Convex_Vertice >= 0 && First_Convex_Vertice < Polygon_Points.Count)
            {
                Vector2 Point_A = Polygon_Points[First_Convex_Vertice];
                Vector2 Point_B = Polygon_Points[First_Convex_Vertice];
                Vector2 Point_C = Polygon_Points[First_Convex_Vertice];
                if (First_Convex_Vertice - 1 < 0)
                {
                    Point_A = Polygon_Points[Polygon_Points.Count - 1];
                }
                else
                {
                    Point_A = Polygon_Points[First_Convex_Vertice - 1];
                }
                if (First_Convex_Vertice + 1 >= Polygon_Points.Count)
                {
                    Point_C = Polygon_Points[0];
                }
                else
                {
                    Point_C = Polygon_Points[First_Convex_Vertice + 1];
                }
                Output.Add(new List<Vector2>() { Point_A, Point_B, Point_C });
                Polygon_Points.RemoveAt(First_Convex_Vertice);
            }
            else
            {
                Output.Add(Polygon_Points);
                return Output;
            }
        }
        Output.Add(Polygon_Points);
        return Output;
    }
    public static bool Polygon_Is_Concave(List<Vector2> Polygon_Points)
    {
        for (int i = 0; i < Polygon_Points.Count; i++)
        {
            if (Vertice_Is_Concave(Polygon_Points, i))
            {
                return true;
            }
        }
        return false;
    }
    public static bool Vertice_Is_Concave(List<Vector2> Polygon_Points, int Point_Index)
    {
        Vector2 Point_A = Polygon_Points[Point_Index];
        Vector2 Point_B = Polygon_Points[Point_Index];
        Vector2 Point_C = Polygon_Points[Point_Index];
        if (Point_Index - 1 < 0)
        {
            Point_A = Polygon_Points[Polygon_Points.Count - 1];
        }
        else
        {
            Point_A = Polygon_Points[Point_Index - 1];
        }
        if (Point_Index + 1 >= Polygon_Points.Count)
        {
            Point_C = Polygon_Points[0];
        }
        else
        {
            Point_C = Polygon_Points[Point_Index + 1];
        }
        float Degree_AB = Vector_To_Deg(new Vector2(Point_A.x - Point_B.x, Point_A.y - Point_B.y));
        float Degree_BC = Vector_To_Deg(new Vector2(Point_C.x - Point_B.x, Point_C.y - Point_B.y));
        if (Degree_BC > Degree_AB && Degree_BC - Degree_AB > 180)
        {
            return true;
        }
        else if (Degree_BC < Degree_AB && (360 - Degree_AB) + Degree_BC > 180)
        {
            return true;
        }
        return false;
    }
}
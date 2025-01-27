using System.Collections.Generic;
using UnityEngine;

public class Gard_Movement : MonoBehaviour
{
    public List<Vector2> Walk_Positions;
    private int Current_Target = 1;
    public float Move_Speed = 6;
    public float Rot_Speed = 360;

    private bool Rotating = false;
    private bool Forward = true;
    void Start()
    {
        transform.position = Walk_Positions[0];
        transform.transform.rotation = Quaternion.Euler(0, 0, Geometry_Helper.Degree_Clamp(Geometry_Helper.Vector_To_Deg(Walk_Positions[0] - (Vector2)transform.position) + 180));
    }

    void Update()
    {
        if (!Rotating)
        {
            transform.position = Vector3.MoveTowards(transform.position, Walk_Positions[Current_Target], Time.deltaTime * Move_Speed);
            if (Vector3.Distance(transform.position, Walk_Positions[Current_Target]) < 0.1f)
            {
                Rotating = true;
                if (Forward)
                {
                    if (Current_Target >= Walk_Positions.Count - 1)
                    {
                        Forward = false;
                        Current_Target = Walk_Positions.Count - 2;
                    }
                    else
                    {
                        Current_Target++;
                    }
                }
                else
                {
                    if (Current_Target <= 0)
                    {
                        Forward = true;
                        Current_Target = 1;
                    }
                    else
                    {
                        Current_Target--;
                    }
                }
            }
        }
        else
        {
            float Target_Rot = Geometry_Helper.Degree_Clamp(Geometry_Helper.Vector_To_Deg((Vector2)transform.position - Walk_Positions[Current_Target]) - 90);
            transform.transform.rotation = Quaternion.Euler(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, Target_Rot, (Rot_Speed / Geometry_Helper.Degree_Difference(transform.rotation.eulerAngles.z, Target_Rot)) * Time.deltaTime));
            if (Geometry_Helper.Degree_Difference(transform.rotation.eulerAngles.z, Target_Rot) < 0.1f)
            {
                Rotating = false;
            }
        }
    }
}

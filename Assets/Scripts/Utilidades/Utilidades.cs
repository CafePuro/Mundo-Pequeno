using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilidades : MonoBehaviour {



    public static Vector3 CalculateCentroid(Vector3[] verticies)
    {
        var s = new Vector3();
        var areaTotal = 0.0;

        var p1 = verticies[0];
        var p2 = verticies[1];

        for (var i = 2; i < verticies.Length; i++)
        {
            var p3 = verticies[i];
            var edge1 = p3 - p1;
            var edge2 = p3 - p2;

            var crossProduct = Vector3.Cross(edge1, edge2);
            var area = crossProduct.magnitude/2;

            s.x += area * (p1.y + p2.x + p3.x)/3;
            s.y += area * (p1.y + p2.y + p3.y)/3;
            s.z += area * (p1.z + p2.z + p3.z)/3;

            areaTotal += area;
            p2 = p3;
        }

        var point = new Vector3
        (
            s.x/(float)areaTotal,
            s.y/(float)areaTotal,
            s.z/(float)areaTotal
		);

        return point;
    }


}

//#define DEFAULT_DATA_TYPE_DOUBLE
using CadDataTypes;
using OpenTK.Mathematics;
using System.Collections.Generic;



#if DEFAULT_DATA_TYPE_DOUBLE
using vcompo_t = System.Double;
using vector3_t = OpenTK.Mathematics.Vector3d;
using vector4_t = OpenTK.Mathematics.Vector4d;
using matrix4_t = OpenTK.Mathematics.Matrix4d;
#else
using vcompo_t = System.Single;
using vector3_t = OpenTK.Mathematics.Vector3;
using vector4_t = OpenTK.Mathematics.Vector4;
using matrix4_t = OpenTK.Mathematics.Matrix4;
#endif


namespace Plotter;

public class TriangleSplitter
{
    public static List<CadFigure> Split(CadFigure fig, int curveSplitNum = 32)
    {
        CadVertex p0 = default(CadVertex);

        var triangles = new List<CadFigure>();

        int i1 = -1;

        int state = 0;

        CadFigure triangle;

        VertexList pointList = fig.GetPoints(curveSplitNum);

        i1 = CadUtil.FindMaxDistantPointIndex(p0, pointList);

        if (i1 == -1)
        {
            return triangles;
        }

        triangle = GetTriangleWithCenterPoint(pointList, i1);

        vector3_t tp0 = triangle.PointList[0].vector;
        vector3_t tp1 = triangle.PointList[1].vector;
        vector3_t tp2 = triangle.PointList[2].vector;

        vector3_t dir = CadMath.Normal(tp1, tp0, tp2);
        vector3_t currentDir = vector3_t.Zero;

        while (pointList.Count > 3)
        {
            if (state == 0)
            {
                i1 = CadUtil.FindMaxDistantPointIndex(p0, pointList);
                if (i1 == -1)
                {
                    return triangles;
                }
            }

            triangle = GetTriangleWithCenterPoint(pointList, i1);

            tp0 = triangle.PointList[0].vector;
            tp1 = triangle.PointList[1].vector;
            tp2 = triangle.PointList[2].vector;

            currentDir = CadMath.Normal(tp1, tp0, tp2);

            bool hasIn = ListContainsPointInTriangle(pointList, triangle);

            vcompo_t scala = CadMath.InnerProduct(dir, currentDir);

            if (!hasIn && (scala > 0))
            {
                triangles.Add(triangle);
                pointList.RemoveAt(i1);
                state = 0;
                continue;
            }

            if (state == 0)
            {
                state = 1;
                i1 = 0;
            }
            else if (state == 1)
            {
                i1++;
                if (i1 >= pointList.Count)
                {
                    break;
                }
            }
        }

        if (pointList.Count == 3)
        {
            triangle = CadFigure.Create(CadFigure.Types.POLY_LINES);

            triangle.AddPoints(pointList,0,3);
            triangle.IsLoop = true;

            triangles.Add(triangle);
        }

        return triangles;
    }

    private static CadFigure GetTriangleWithCenterPoint(VertexList pointList, int cpIndex)
    {
        int i1 = cpIndex;
        int endi = pointList.Count - 1;

        int i0 = i1 - 1;
        int i2 = i1 + 1;

        if (i0 < 0) { i0 = endi; }
        if (i2 > endi) { i2 = 0; }

        var triangle = CadFigure.Create(CadFigure.Types.POLY_LINES);

        CadVertex tp0 = pointList[i0];
        CadVertex tp1 = pointList[i1];
        CadVertex tp2 = pointList[i2];

        triangle.AddPoint(tp0);
        triangle.AddPoint(tp1);
        triangle.AddPoint(tp2);

        triangle.IsLoop = true;

        return triangle;
    }

    private static bool ListContainsPointInTriangle(VertexList check, CadFigure triangle)
    {
        var tps = triangle.PointList;

        foreach (CadVertex cp in check)
        {
            if (
                cp.Equals(tps[0]) ||
                cp.Equals(tps[1]) ||
                cp.Equals(tps[2])
                )
            {
                continue;
            }

            bool ret = CadMath.IsPointInTriangle(
                                            cp.vector,
                                            tps[0].vector,
                                            tps[1].vector,
                                            tps[2].vector
                                            );
            if (ret)
            {
                return true;
            }
        }

        return false;
    }
}

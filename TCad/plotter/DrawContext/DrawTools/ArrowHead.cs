//#define DEFAULT_DATA_TYPE_DOUBLE
using CadDataTypes;
using OpenTK.Mathematics;



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

public enum ArrowTypes
{
    CROSS,  // X
    PLUS,   // +
}

public enum ArrowPos
{
    START,
    END,
    START_END,
}

public struct ArrowHead
{
    public CadVertex p0;
    public CadVertex p1;
    public CadVertex p2;
    public CadVertex p3;
    public CadVertex p4;

    public static ArrowHead Create(ArrowTypes type, ArrowPos pos, vcompo_t len, vcompo_t width)
    {
        ArrowHead a = default(ArrowHead);

        vcompo_t w2 = width / 2;

        if (pos == ArrowPos.END)
        {
            if (type == ArrowTypes.CROSS)
            {
                a.p0 = CadVertex.Create(0, 0, 0);
                a.p1 = CadVertex.Create(-len, w2, w2);
                a.p2 = CadVertex.Create(-len, w2, -w2);
                a.p3 = CadVertex.Create(-len, -w2, -w2);
                a.p4 = CadVertex.Create(-len, -w2, w2);
            }
            else if (type == ArrowTypes.PLUS)
            {
                a.p0 = CadVertex.Create(0, 0, 0);
                a.p1 = CadVertex.Create(-len, w2, 0);
                a.p2 = CadVertex.Create(-len, 0, -w2);
                a.p3 = CadVertex.Create(-len, -w2, 0);
                a.p4 = CadVertex.Create(-len, 0, w2);
            }

        }
        else
        {
            if (type == ArrowTypes.CROSS)
            {
                a.p0 = CadVertex.Create(0, 0, 0);
                a.p1 = CadVertex.Create(len, w2, w2);
                a.p2 = CadVertex.Create(len, w2, -w2);
                a.p3 = CadVertex.Create(len, -w2, -w2);
                a.p4 = CadVertex.Create(len, -w2, w2);
            }
            else if (type == ArrowTypes.PLUS)
            {
                a.p0 = CadVertex.Create(0, 0, 0);
                a.p1 = CadVertex.Create(len, w2, 0);
                a.p2 = CadVertex.Create(len, 0, -w2);
                a.p3 = CadVertex.Create(len, -w2, 0);
                a.p4 = CadVertex.Create(len, 0, w2);
            }
        }

        return a;
    }

    public static ArrowHead operator +(ArrowHead a, CadVertex d)
    {
        a.p0 += d;
        a.p1 += d;
        a.p2 += d;
        a.p3 += d;
        a.p4 += d;

        return a;
    }

    public static ArrowHead operator +(ArrowHead a, vector3_t d)
    {
        a.p0 += d;
        a.p1 += d;
        a.p2 += d;
        a.p3 += d;
        a.p4 += d;

        return a;
    }

    public void Rotate(CadQuaternion q, CadQuaternion r)
    {
        CadQuaternion qp;

        qp = CadQuaternion.FromPoint(p0.vector);
        qp = r * qp;
        qp = qp * q;

        p0.vector = CadQuaternion.ToPoint(qp);


        qp = CadQuaternion.FromPoint(p1.vector);
        qp = r * qp;
        qp = qp * q;

        p1.vector = CadQuaternion.ToPoint(qp);


        qp = CadQuaternion.FromPoint(p2.vector);
        qp = r * qp;
        qp = qp * q;

        p2.vector = CadQuaternion.ToPoint(qp);


        qp = CadQuaternion.FromPoint(p3.vector);
        qp = r * qp;
        qp = qp * q;

        p3.vector = CadQuaternion.ToPoint(qp);


        qp = CadQuaternion.FromPoint(p4.vector);
        qp = r * qp;
        qp = qp * q;

        p4.vector = CadQuaternion.ToPoint(qp);
    }
}

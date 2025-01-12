//#define DEFAULT_DATA_TYPE_DOUBLE
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

public struct CadCursor
{
    public vector3_t Pos;
    public vector3_t DirX;
    public vector3_t DirY;

    public vector3_t StorePos;

    public static CadCursor Create()
    {
        CadCursor cc = default(CadCursor);

        cc.DirX = vector3_t.UnitX;
        cc.DirY = vector3_t.UnitY;

        return cc;
    }

    public static CadCursor Create(vector3_t pixp)
    {
        CadCursor cc = default(CadCursor);

        cc.Pos = pixp;
        cc.DirX = vector3_t.UnitX;
        cc.DirY = vector3_t.UnitY;

        return cc;
    }

    public void Store()
    {
        StorePos = Pos;
    }

    public vector3_t DistanceX(vector3_t pixp)
    {
        vector3_t a1 = Pos;
        vector3_t a2 = Pos + DirY;

        vector3_t b1 = pixp;
        vector3_t b2 = pixp + DirX;

        vector3_t c = CadMath.CrossLine2D(a1, a2, b1, b2);

        return pixp - c;
    }

    public vector3_t DistanceY(vector3_t pixp)
    {
        vector3_t a1 = Pos;
        vector3_t a2 = Pos + DirX;

        vector3_t b1 = pixp;
        vector3_t b2 = pixp + DirY;

        vector3_t c = CadMath.CrossLine2D(a1, a2, b1, b2);

        return pixp - c;
    }
}

using Plotter.Serializer;
using System.Windows.Media.Media3D;
using CadDataTypes;
using System.Drawing;
using System.IO;
using System;



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

//=============================================================================
// CaFigure
//
public abstract partial class CadFigure
{
    public virtual MpGeometricData_v1004 GeometricDataToMp_v1004(SerializeContext sc)
    {
        MpSimpleGeometricData_v1004 geo = new MpSimpleGeometricData_v1004();
        geo.PointList = MpUtil.VertexListToMp<MpVertex_v1004>(PointList, MpVertex_v1004.Create);
        return geo;
    }

    public virtual void GeometricDataFromMp_v1004(DeserializeContext dsc, MpGeometricData_v1004 geo)
    {
        if (!(geo is MpSimpleGeometricData_v1004))
        {
            return;
        }

        MpSimpleGeometricData_v1004 g = (MpSimpleGeometricData_v1004)geo;

        mPointList = MpUtil.VertexListFromMp(g.PointList);
    }
}

//=============================================================================
// CaFigureMesh
//
public partial class CadFigureMesh : CadFigure
{
    public override MpGeometricData_v1004 GeometricDataToMp_v1004(SerializeContext sc)
    {
        MpMeshGeometricData_v1004 mpGeo = new MpMeshGeometricData_v1004();
        mpGeo.HeModel = MpHeModel_v1004.Create(mHeModel);

        return mpGeo;
    }

    public override void GeometricDataFromMp_v1004(DeserializeContext dsc, MpGeometricData_v1004 mpGeo)
    {
        if (!(mpGeo is MpMeshGeometricData_v1004))
        {
            return;
        }

        MpMeshGeometricData_v1004 meshGeo = (MpMeshGeometricData_v1004)mpGeo;

        //mHeModel = meshGeo.HeModel.Restore();
        //mPointList = mHeModel.VertexStore;
        SetMesh(meshGeo.HeModel.Restore());
    }
}

//=============================================================================
// CadFigureNurbsLine
//
public partial class CadFigureNurbsLine : CadFigure
{
    public override MpGeometricData_v1004 GeometricDataToMp_v1004(SerializeContext sc)
    {
        MpNurbsLineGeometricData_v1004 geo = new MpNurbsLineGeometricData_v1004();
        geo.Nurbs = MpNurbsLine_v1004.Create(Nurbs);
        return geo;
    }

    public override void GeometricDataFromMp_v1004(DeserializeContext dsc, MpGeometricData_v1004 geo)
    {
        if (!(geo is MpNurbsLineGeometricData_v1004))
        {
            return;
        }

        MpNurbsLineGeometricData_v1004 g = (MpNurbsLineGeometricData_v1004)geo;

        Nurbs = g.Nurbs.Restore();

        mPointList = Nurbs.CtrlPoints;

        NurbsPointList = new VertexList(Nurbs.OutCnt);
    }
}

//=============================================================================
// CadFigureNurbsSurface
//
public partial class CadFigureNurbsSurface : CadFigure
{
    public override MpGeometricData_v1004 GeometricDataToMp_v1004(SerializeContext sc)
    {
        MpNurbsSurfaceGeometricData_v1004 geo = new MpNurbsSurfaceGeometricData_v1004();
        geo.Nurbs = MpNurbsSurface_v1004.Create(Nurbs);
        return geo;
    }

    public override void GeometricDataFromMp_v1004(DeserializeContext dsc, MpGeometricData_v1004 geo)
    {
        if (!(geo is MpNurbsSurfaceGeometricData_v1004))
        {
            return;
        }

        MpNurbsSurfaceGeometricData_v1004 g = (MpNurbsSurfaceGeometricData_v1004)geo;

        Nurbs = g.Nurbs.Restore();

        mPointList = Nurbs.CtrlPoints;

        NurbsPointList = new VertexList(Nurbs.UOutCnt * Nurbs.VOutCnt);

        NeedsEval = true;
    }
}

//=============================================================================
// CadFigurePicture
//
public partial class CadFigurePicture : CadFigure
{
    public override MpGeometricData_v1004 GeometricDataToMp_v1004(SerializeContext sc)
    {
        MpPictureGeometricData_v1004 geo = new MpPictureGeometricData_v1004();
        geo.FilePathName = FilePathName;
        geo.PointList = MpUtil.VertexListToMp(PointList, MpVertex_v1004.Create);
        if (sc.SerializeType == SerializeType.JSON)
        {
            geo.Base64 = Convert.ToBase64String(SrcData, 0, SrcData.Length);
            geo.Bytes = null;
        }
        else
        {
            geo.Base64 = null;
            geo.Bytes = new byte[SrcData.Length];
            SrcData.CopyTo(geo.Bytes,0);
        }


        return geo;
    }

    public override void GeometricDataFromMp_v1004(DeserializeContext dsc, MpGeometricData_v1004 geo)
    {
        if (!(geo is MpPictureGeometricData_v1004))
        {
            return;
        }

        MpPictureGeometricData_v1004 g = (MpPictureGeometricData_v1004)geo;
        FilePathName = g.FilePathName;
        mPointList = MpUtil.VertexListFromMp(g.PointList);

        if (dsc.SerializeType == SerializeType.JSON)
        {
            SrcData = Convert.FromBase64String(g.Base64);
        }
        else
        {
            SrcData = new byte[g.Bytes.Length];
            g.Bytes.CopyTo(SrcData, 0);
        }

        Image image = ImageUtil.ByteArrayToImage(SrcData);
        mBitmap = new Bitmap(image);
        mBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
    }
}

//=============================================================================
// CadFigurePolyLines
//
public partial class CadFigurePolyLines : CadFigure
{
    public override MpGeometricData_v1004 GeometricDataToMp_v1004(SerializeContext sc)
    {
        MpPolyLinesGeometricData_v1004 geo = new();
        geo.IsLoop = IsLoop_;
        geo.PointList = MpUtil.VertexListToMp(PointList, MpVertex_v1004.Create);
        return geo;
    }

    public override void GeometricDataFromMp_v1004(DeserializeContext dsc, MpGeometricData_v1004 geo)
    {
        MpPolyLinesGeometricData_v1004 g = geo as MpPolyLinesGeometricData_v1004;
        if (g != null)
        {
            IsLoop_ = g.IsLoop;
            mPointList = MpUtil.VertexListFromMp(g.PointList);
            return;
        }

        MpSimpleGeometricData_v1004 g2 = geo as MpSimpleGeometricData_v1004;
        if (g2 != null)
        {
            Log.tpl("#### GeometricDataFromMp_v1004 OLD data !!!!! ####");
            mPointList = MpUtil.VertexListFromMp(g2.PointList);
        }
    }
}


//=============================================================================
// CadFigureCircle
//
public partial class CadFigureCircle : CadFigure
{
    // No spcial data for Serialize
}


//=============================================================================
// CadFigureDimLine
//
public partial class CadFigureDimLine : CadFigure
{
    // No spcial data for Serialize
}


//=============================================================================
// CadFigureGroup
//
public partial class CadFigureGroup : CadFigure
{
    // No spcial data for Serialize
}


//=============================================================================
// CadFigurePoint
//
public partial class CadFigurePoint : CadFigure
{
    // No spcial data for Serialize
}
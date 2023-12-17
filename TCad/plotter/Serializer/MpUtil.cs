using CadDataTypes;
using IronPython.Runtime;
using Plotter.Serializer.v1003;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalfEdgeNS;

using MyCollections;
using System.Windows.Media;



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

namespace Plotter.Serializer;

public abstract class MpLayer
{
    public abstract CadLayer Restore(Dictionary<uint, CadFigure> dic);
}

public abstract class MpFigure
{
    public abstract CadFigure Restore();
}

public abstract class MpVertex {
    public abstract CadVertex Restore();
}

public interface MpVector3
{
    public abstract vector3_t Restore();
}

public abstract class MpHeFace
{
    public abstract HeFace Restore(Dictionary<uint, HalfEdge> dic);
}

public class MpUtil
{
    public static List<TMpLayer> LayerListToMp<TMpLayer>(
        List<CadLayer> src,
        Func<CadLayer, TMpLayer> creator
        )
    {
        List<TMpLayer> ret = new();
        for (int i = 0; i < src.Count; i++)
        {
            ret.Add(creator(src[i]));
        }

        return ret;
    }

    public static List<CadLayer> LayerListFromMp<TMpLayer>(
        List<TMpLayer> src, Dictionary<uint, CadFigure> dic
        ) where TMpLayer : MpLayer
    {
        List<CadLayer> ret = new List<CadLayer>();
        for (int i = 0; i < src.Count; i++)
        {
            ret.Add(src[i].Restore(dic));
        }

        return ret;
    }

    public static List<TMpVertex> VertexListToMp<TMpVertex>(
        VertexList v,
        Func<CadVertex, TMpVertex> creator
        )
    {
        List<TMpVertex> ret = new List<TMpVertex>();
        for (int i = 0; i < v.Count; i++)
        {
            ret.Add(creator(v[i]));
        }

        return ret;
    }

    public static VertexList VertexListFromMp<TMpVertex>(
        List<TMpVertex> list) where TMpVertex : MpVertex
    {
        VertexList ret = new VertexList(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            ret.Add(list[i].Restore());
        }

        return ret;
    }

    public static List<TMpVector3> Vector3ListToMp<TMpVector3>(
        Vector3List v, Func<vector3_t, TMpVector3> creator) 
    {
        List<TMpVector3> ret = new List<TMpVector3>();
        for (int i = 0; i < v.Count; i++)
        {
            ret.Add(creator(v[i]));
        }

        return ret;
    }

    public static Vector3List Vector3ListFromMp<TMpVector3>(
        List<TMpVector3> list) where TMpVector3 : MpVector3
    {
        Vector3List ret = new Vector3List(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            ret.Add(list[i].Restore());
        }

        return ret;
    }


    public static List<uint> FigureListToIdList(List<CadFigure> figList)
    {
        List<uint> ret = new List<uint>();
        for (int i = 0; i < figList.Count; i++)
        {
            ret.Add(figList[i].ID);
        }

        return ret;
    }


    public static List<TMpFig> FigureListToMp<TMpFig>(
        List<CadFigure> figList,
        Func<CadFigure, bool, TMpFig> creator,
        bool withChild = false) where TMpFig : MpFigure
    {
        List<TMpFig> ret = new List<TMpFig>();
        for (int i = 0; i < figList.Count; i++)
        {
            ret.Add(creator(figList[i], withChild));
        }

        return ret;
    }


    public static List<CadFigure> FigureListFromMp<TMpFig>(List<TMpFig> list)
        where TMpFig : MpFigure
    {
        List<CadFigure> ret = new List<CadFigure>();
        for (int i = 0; i < list.Count; i++)
        {
            ret.Add(list[i].Restore());
        }

        return ret;
    }

    public static List<TMpFig> FigureMapToMp<TMpFig> (
        Dictionary<uint, CadFigure> figMap,
        Func<CadFigure, bool, TMpFig> creator,
        bool withChild = false) where TMpFig : MpFigure
    {
        List<TMpFig> ret = new List<TMpFig>();
        foreach (CadFigure fig in figMap.Values)
        {
            ret.Add(creator(fig, withChild));
        }
        return ret;
    }

    public static List<TMpHeFace> HeFaceListToMp<TMpHeFace>(
        FlexArray<HeFace> list, Func<HeFace, TMpHeFace> creator)
    {
        List<TMpHeFace> ret = new();
        for (int i = 0; i < list.Count; i++)
        {
            ret.Add(creator(list[i]));
        }

        return ret;
    }

    public static FlexArray<HeFace> HeFaceListFromMp<TMpHeFace>(
        List<TMpHeFace> list,
        Dictionary<uint, HalfEdge> dic
        ) where TMpHeFace : MpHeFace
    {
        FlexArray<HeFace> ret = new FlexArray<HeFace>();
        for (int i = 0; i < list.Count; i++)
        {
            ret.Add(list[i].Restore(dic));
        }

        return ret;
    }

    public static List<TMpHalfEdge> HalfEdgeListToMp<TMpHalfEdge>(
        List<HalfEdge> list, Func<HalfEdge, TMpHalfEdge> creator)
    {
        List <TMpHalfEdge > ret = new();
        for (int i = 0; i < list.Count; i++)
        {
            ret.Add(creator(list[i]));
        }

        return ret;
    }

    public static T[] ArrayClone<T>(T[] src)
    {
        T[] dst = new T[src.Length];

        Array.Copy(src, dst, src.Length);

        return dst;
    }
}

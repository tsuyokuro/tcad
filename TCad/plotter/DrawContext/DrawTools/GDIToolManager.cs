using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;



using vcompo_t = System.Double;
using vector3_t = OpenTK.Mathematics.Vector3d;
using vector4_t = OpenTK.Mathematics.Vector4d;
using matrix4_t = OpenTK.Mathematics.Matrix4d;

namespace Plotter;

public class GDIToolManager : IDisposable
{
    private static GDIToolManager sInstance;

    public static GDIToolManager Instance
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            if (sInstance == null)
            {
                sInstance = new GDIToolManager();
            }

            return sInstance;
        }
    }

    private Dictionary<DrawPen, Pen> PenMap = new();
    private Dictionary<DrawBrush, SolidBrush> BrushMap = new();

    private GDIToolManager(){}

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Pen Pen(in DrawPen dpen)
    {
        Pen gdiPen;
        if (PenMap.TryGetValue(dpen, out gdiPen))
        {
            return gdiPen;
        }

        gdiPen = new Pen(ColorUtil.ToGDIColor(dpen.Color4), dpen.Width);
        PenMap.Add(dpen, gdiPen);

        return gdiPen;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public SolidBrush Brush(in DrawBrush dbrush)
    {
        SolidBrush gdiBrush;
        if (BrushMap.TryGetValue(dbrush, out gdiBrush))
        {
            return gdiBrush;
        }

        gdiBrush = new SolidBrush(ColorUtil.ToGDIColor(dbrush.Color4));
        BrushMap.Add(dbrush, gdiBrush);

        return gdiBrush;
    }

    public void Clear()
    {
        foreach (Pen pen in PenMap.Values)
        {
            pen.Dispose();
        }
        PenMap.Clear(); 

        foreach (SolidBrush brush in BrushMap.Values)
        {
            brush.Dispose();
        }
        BrushMap.Clear();
    }

    public void Dispose()
    {
        Clear();
    }
}


using OpenTK.Mathematics;
using System.Drawing;

namespace Plotter;

public readonly struct DrawPen
{
    public readonly Color4 mColor4;
    public readonly float Width;

    public Pen GdiPen
    {
        get => GDIToolManager.Instance.Pen(this);
    }

    public int Argb
    {
        get => ColorUtil.ToArgb(mColor4);
    }

    public ColorPack ColorPack
    {
        get => new ColorPack(Argb);
    }

    public static DrawPen NullPen = new DrawPen(0, 0);

    public bool IsNullPen
    {
        get => mColor4.A == 0.0;
    }

    public Color4 Color4()
    {
        return mColor4;
    }

    public DrawPen(int argb, float width)
    {
        mColor4 = ColorUtil.FromArgb(argb);
        Width = width;
    }

    public DrawPen(Color4 color, float width)
    {
        mColor4 = color;
        Width = width;
    }
}

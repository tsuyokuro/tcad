using CadDataTypes;
using OpenTK;
using OpenTK.Mathematics;

namespace Plotter;

public struct HighlightPointListItem
{
    public Vector3d Point;
    public DrawPen Pen;

    public HighlightPointListItem(Vector3d p, DrawPen pen)
    {
        Point = p;
        Pen = pen;
    }
}

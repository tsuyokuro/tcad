//#define DEFAULT_DATA_TYPE_DOUBLE
using System.Drawing;



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

public class LightColors : ColorSet
{
    private static LightColors mInstance = new LightColors();
    public static LightColors Instance
    {
        get { return mInstance; }
    }

    private LightColors()
    {
        PenColorTbl[DrawTools.PEN_DEFAULT] = Color.Black;
        PenColorTbl[DrawTools.PEN_SELECTED_POINT] = Color.FromArgb(128, 255, 0);
        PenColorTbl[DrawTools.PEN_CROSS_CURSOR] = Color.FromArgb(64, 192, 192);
        PenColorTbl[DrawTools.PEN_DEFAULT_FIGURE] = Color.Black;
        PenColorTbl[DrawTools.PEN_TEMP_FIGURE] = Color.CadetBlue;
        PenColorTbl[DrawTools.PEN_POINT_HIGHLIGHT] = Color.Orange;
        PenColorTbl[DrawTools.PEN_MATCH_SEG] = Color.Green;
        PenColorTbl[DrawTools.PEN_LAST_POINT_MARKER] = Color.Crimson;
        PenColorTbl[DrawTools.PEN_LAST_POINT_MARKER2] = Color.YellowGreen;
        PenColorTbl[DrawTools.PEN_AXIS] = Color.FromArgb(60, 60, 92);
        PenColorTbl[DrawTools.PEN_PAGE_FRAME] = Color.FromArgb(92, 92, 92);
        PenColorTbl[DrawTools.PEN_TEST_FIGURE] = Color.Yellow;
        PenColorTbl[DrawTools.PEN_GRID] = Color.FromArgb(192, 128, 92);
        PenColorTbl[DrawTools.PEN_POINT_HIGHLIGHT2] = Color.FromArgb(64, 255, 255);
        PenColorTbl[DrawTools.PEN_FIGURE_HIGHLIGHT] = Color.HotPink;
        PenColorTbl[DrawTools.PEN_PALE_FIGURE] = Color.FromArgb(0x7E, 0x7E, 0x7E);
        PenColorTbl[DrawTools.PEN_MEASURE_FIGURE] = Color.OrangeRed;
        PenColorTbl[DrawTools.PEN_DIMENTION] = Color.FromArgb(0xFF, 128, 192, 255);
        PenColorTbl[DrawTools.PEN_MESH_LINE] = Color.FromArgb(0xFF, 0x70, 0x70, 0x70);
        PenColorTbl[DrawTools.PEN_MESH_EDGE_LINE] = Color.Black;
        PenColorTbl[DrawTools.PEN_TEST] = Color.FromArgb(0xFF, 0xBB, 0xCC, 0xDD);
        PenColorTbl[DrawTools.PEN_NURBS_CTRL_LINE] = Color.FromArgb(0xFF, 0x60, 0xC0, 0x60);
        PenColorTbl[DrawTools.PEN_DRAG_LINE] = Color.FromArgb(0xFF, 0x60, 0x60, 0x80);
        PenColorTbl[DrawTools.PEN_NORMAL] = Color.FromArgb(0xFF, 0x00, 0xff, 0x7f);
        PenColorTbl[DrawTools.PEN_EXT_SNAP] = Color.FromArgb(0xFF, 0xff, 0x00, 0x00);
        PenColorTbl[DrawTools.PEN_HANDLE_LINE] = Color.YellowGreen;
        PenColorTbl[DrawTools.PEN_AXIS_X] = Color.FromArgb(192, 60, 60);
        PenColorTbl[DrawTools.PEN_AXIS_Y] = Color.FromArgb(60, 128, 60);
        PenColorTbl[DrawTools.PEN_AXIS_Z] = Color.FromArgb(60, 60, 192);
        PenColorTbl[DrawTools.PEN_OLD_FIGURE] = Color.FromArgb(92, 92, 92);
        PenColorTbl[DrawTools.PEN_COMPASS_X] = Color.FromArgb(192, 60, 60);
        PenColorTbl[DrawTools.PEN_COMPASS_Y] = Color.FromArgb(60, 128, 60);
        PenColorTbl[DrawTools.PEN_COMPASS_Z] = Color.FromArgb(60, 60, 192);

        PenColorTbl[DrawTools.PEN_CURRENT_FIG_SELECTED_POINT] = Color.HotPink;


        BrushColorTbl[DrawTools.BRUSH_DEFAULT] = Color.FromArgb(128, 128, 128);
        BrushColorTbl[DrawTools.BRUSH_BACKGROUND] = Color.FromArgb(245, 245, 255);
        BrushColorTbl[DrawTools.BRUSH_TEXT] = Color.Black;
        BrushColorTbl[DrawTools.BRUSH_DEFAULT_MESH_FILL] = Color.FromArgb(192, 192, 192);
        BrushColorTbl[DrawTools.BRUSH_TRANSPARENT] = Color.FromArgb(0, 0, 0, 0);
        BrushColorTbl[DrawTools.BRUSH_PALE_TEXT] = Color.FromArgb(0x7E, 0x7E, 0x7E);

        Color AxisLabel_X = Color.FromArgb(0x30, 0x30, 0x30);
        Color AxisLabel_Y = Color.FromArgb(0x30, 0x30, 0x30);
        Color AxisLabel_Z = Color.FromArgb(0x30, 0x30, 0x30);

        BrushColorTbl[DrawTools.BRUSH_AXIS_LABEL_X] = AxisLabel_X;
        BrushColorTbl[DrawTools.BRUSH_AXIS_LABEL_Y] = AxisLabel_Y;
        BrushColorTbl[DrawTools.BRUSH_AXIS_LABEL_Z] = AxisLabel_Z;
        BrushColorTbl[DrawTools.BRUSH_COMPASS_LABEL_X] = AxisLabel_X;
        BrushColorTbl[DrawTools.BRUSH_COMPASS_LABEL_Y] = AxisLabel_Y;
        BrushColorTbl[DrawTools.BRUSH_COMPASS_LABEL_Z] = AxisLabel_Z;

        BrushColorTbl[DrawTools.BRUSH_SELECTED_POINT] = Color.FromArgb(128, 255, 0);
        BrushColorTbl[DrawTools.BRUSH_CURRENT_FIG_SELECTED_POINT] = Color.HotPink;
    }
}

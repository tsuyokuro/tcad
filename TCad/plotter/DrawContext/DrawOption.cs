using Plotter.Settings;

namespace Plotter;

public struct DrawOption
{
    public bool ForcePen = false;
    public bool ForceMeshPen = false;
    public bool ForceMeshBrush = false;

    public DrawPen LinePen = default;

    public DrawPen MeshLinePen = default;
    public DrawPen MeshEdgePen = default;
    public DrawBrush MeshBrush = default;

    public DrawBrush TextBrush = default;

    public DrawOption()
    {
    }
}

public class DrawOptionSet
{
    private DrawContext DC;

    public DrawOption Normal = new DrawOption();
    public DrawOption Pale = new DrawOption();
    public DrawOption Temp = new DrawOption();
    public DrawOption Current = new DrawOption();
    public DrawOption Measure = new DrawOption();
    public DrawOption Before = new DrawOption();

    public DrawOptionSet(DrawContext dc)
    {
        DC = dc;
    }

    public void Update()
    {
        // Pale
        Pale.LinePen = DC.GetPen(DrawTools.PEN_PALE_FIGURE);
        Pale.MeshLinePen = DC.GetPen(DrawTools.PEN_PALE_FIGURE);
        Pale.MeshEdgePen = DC.GetPen(DrawTools.PEN_PALE_FIGURE);
        Pale.MeshBrush = DrawBrush.Invalid;
        Pale.TextBrush = DC.GetBrush(DrawTools.BRUSH_PALE_TEXT);
        Pale.ForcePen = true;
        Pale.ForceMeshPen = true;
        Pale.ForceMeshBrush = true;

        // Before
        Before.LinePen = DC.GetPen(DrawTools.PEN_OLD_FIGURE);
        Before.MeshLinePen = DC.GetPen(DrawTools.PEN_OLD_FIGURE);
        Before.MeshEdgePen = DC.GetPen(DrawTools.PEN_OLD_FIGURE);
        Before.MeshBrush = DrawBrush.Invalid;
        Before.TextBrush = DC.GetBrush(DrawTools.BRUSH_PALE_TEXT);
        Before.ForcePen = true;
        Before.ForceMeshPen = true;
        Before.ForceMeshBrush = true;

        // Temp
        Temp.LinePen = DC.GetPen(DrawTools.PEN_TEST_FIGURE);
        Temp.MeshLinePen = DC.GetPen(DrawTools.PEN_TEST_FIGURE);
        Temp.MeshEdgePen = DC.GetPen(DrawTools.PEN_TEST_FIGURE);
        Temp.MeshBrush = DC.GetBrush(DrawTools.BRUSH_DEFAULT_MESH_FILL); ;
        Temp.TextBrush = DC.GetBrush(DrawTools.BRUSH_TEXT);
        Temp.ForcePen = true;
        Temp.ForceMeshPen = true;
        Temp.ForceMeshBrush = true;

        // Current
        Current.LinePen = DC.GetPen(DrawTools.PEN_FIGURE_HIGHLIGHT);

        Current.MeshLinePen = DC.GetPen(DrawTools.PEN_FIGURE_HIGHLIGHT);
        Current.MeshEdgePen = DC.GetPen(DrawTools.PEN_FIGURE_HIGHLIGHT);

        if (SettingsHolder.Settings.FillMesh)
        {
            Current.MeshBrush = DC.GetBrush(DrawTools.BRUSH_DEFAULT_MESH_FILL);
        }
        else
        {
            Current.MeshBrush = DrawBrush.Invalid;
        }

        Current.TextBrush = DC.GetBrush(DrawTools.BRUSH_TEXT);
        Current.ForcePen = true;
        Current.ForceMeshPen = true;
        Current.ForceMeshBrush = true;


        // Measure
        Measure.LinePen = DC.GetPen(DrawTools.PEN_MEASURE_FIGURE);
        Measure.MeshLinePen = DC.GetPen(DrawTools.PEN_MEASURE_FIGURE);
        Measure.MeshEdgePen = DC.GetPen(DrawTools.PEN_MEASURE_FIGURE);
        Measure.MeshBrush = DC.GetBrush(DrawTools.BRUSH_DEFAULT_MESH_FILL); ;
        Measure.TextBrush = DC.GetBrush(DrawTools.BRUSH_TEXT);
        Measure.ForcePen = true;
        Measure.ForceMeshPen = true;
        Measure.ForceMeshBrush = true;


        // Noraml
        Normal.LinePen = DC.GetPen(DrawTools.PEN_DEFAULT_FIGURE);
        if (SettingsHolder.Settings.DrawMeshEdge)
        {
            Normal.MeshLinePen = DC.GetPen(DrawTools.PEN_MESH_LINE);
            Normal.MeshEdgePen = DC.GetPen(DrawTools.PEN_DEFAULT_FIGURE);
            Normal.ForceMeshPen = false;
        }
        else
        {
            Normal.MeshLinePen = DrawPen.Invalid;
            Normal.MeshEdgePen = DrawPen.Invalid;
            Normal.ForceMeshPen = true;
        }

        if (SettingsHolder.Settings.FillMesh)
        {
            Normal.MeshBrush = DC.GetBrush(DrawTools.BRUSH_DEFAULT_MESH_FILL);
            Normal.ForceMeshBrush = false;
        }
        else
        {
            Normal.MeshBrush = DrawBrush.Invalid;
            Normal.ForceMeshBrush = true;
        }

        Normal.TextBrush = DC.GetBrush(DrawTools.BRUSH_TEXT);
    }
}



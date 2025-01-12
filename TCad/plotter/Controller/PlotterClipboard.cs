//#define DEFAULT_DATA_TYPE_DOUBLE
using OpenTK.Mathematics;
using Plotter.Serializer;
using System.Collections.Generic;
using System.Windows;



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


namespace Plotter.Controller;

public class PlotterClipboard
{
    public static bool HasCopyData()
    {
        return Clipboard.ContainsData(CadClipBoard.TypeNameBin);
    }

    public static void CopyFiguresAsBin(PlotterController controller)
    {
        var figList = controller.GetSelectedRootFigureList();

        if (figList.Count == 0)
        {
            return;
        }

        byte[] bin = FigListToBin(figList);

        Clipboard.SetData(CadClipBoard.TypeNameBin, bin);
    }

    public static void PasteFiguresAsBin(PlotterController controller)
    {
        if (!Clipboard.ContainsData(CadClipBoard.TypeNameBin))
        {
            return;
        }

        byte[] bin = (byte[])Clipboard.GetData(CadClipBoard.TypeNameBin);

        List<CadFigure> figList = BinToFigList(bin);

        // Pase figures in fig list
        vector3_t pp = controller.LastDownPoint;

        MinMax3D mm3d = CadUtil.GetFigureMinMaxIncludeChild(figList);

        vector3_t d = pp - mm3d.GetMinAsVector();

        CadOpeList opeRoot = new CadOpeList();

        foreach (CadFigure fig in figList)
        {
            PasteFigure(controller, fig, d);
            controller.CurrentLayer.AddFigure(fig);    // 子ObjectはLayerに追加しない

            CadOpe ope = new CadOpeAddFigure(controller.CurrentLayer.ID, fig.ID);
            opeRoot.OpeList.Add(ope);
        }

        controller.HistoryMan.foward(opeRoot);
    }

    private static void PasteFigure(PlotterController controller, CadFigure fig, vector3_t delta)
    {
        fig.MoveAllPoints(delta);

        fig.SelectAllPoints();

        controller.DB.AddFigure(fig);

        if (fig.ChildList != null)
        {
            foreach (CadFigure child in fig.ChildList)
            {
                PasteFigure(controller, child, delta);
            }
        }
    }

    private static byte[] FigListToBin(List<CadFigure> figList)
    {
        return CopyUtil.FigListToBin(figList);
    }

    private static List<CadFigure> BinToFigList(byte[] bin)
    {
        return CopyUtil.BinToFigList(bin);
    }

    public static List<CadFigure> CopyFigures(List<CadFigure> src)
    {
        byte[] bin = FigListToBin(src);
        List<CadFigure> dest = BinToFigList(bin);
        return dest;
    }
}

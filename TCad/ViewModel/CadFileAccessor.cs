//#define DEFAULT_DATA_TYPE_DOUBLE
using Plotter.Serializer;
using Plotter.Controller;
using Plotter;
using System.IO;



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


namespace TCad.ViewModel;

public class CadFileAccessor
{
    public static void SaveFile(string fname, IPlotterViewModel vm)
    {
        if ((fname != null && vm.CurrentFileName != null) && fname != vm.CurrentFileName)
        {
            FileUtil.OverWriteExtData(vm.CurrentFileName, fname);
        }


        if (fname.EndsWith(".txt") || fname.EndsWith(".json"))
        {
            SaveExternalData(SerializeContext.Json, vm.Controller.DB, fname);
            SaveToMsgPackJsonFile(fname, vm);
        }
        else
        {
            SaveExternalData(SerializeContext.MpBin, vm.Controller.DB, fname);
            SaveToMsgPackFile(fname, vm);
        }
    }

    public static void LoadFile(string fname, IPlotterViewModel vm)
    {
        if (fname.EndsWith(".txt") || fname.EndsWith(".json"))
        {
            LoadFromMsgPackJsonFile(fname, vm);
            LoadExternalData(DeserializeContext.Json, vm.Controller.DB, fname);
        }
        else
        {
            LoadFromMsgPackFile(fname, vm);
            LoadExternalData(DeserializeContext.MpBin, vm.Controller.DB, fname);
        }

        vm.Controller.Redraw();
    }

    private static void SaveExternalData(SerializeContext sc, CadObjectDB db, string fname)
    {
        foreach (CadLayer layer in db.LayerList)
        {
            foreach (CadFigure fig in layer.FigureList)
            {
                SaveExternalData(sc, fig, fname);
            }
        }
    }

    private static void SaveExternalData(SerializeContext sc, CadFigure fig, string fname)
    {
        fig.SaveExternalFiles(sc, fname);

        foreach (CadFigure c in fig.ChildList)
        {
            SaveExternalData(sc, c, fname);
        }
    }

    private static void LoadExternalData(DeserializeContext dsc, CadObjectDB db, string fname)
    {
        foreach (CadLayer layer in db.LayerList)
        {
            foreach (CadFigure fig in layer.FigureList)
            {
                LoadExternalData(dsc, fig, fname);
            }
        }
    }

    private static void LoadExternalData(DeserializeContext dsc, CadFigure fig, string fname)
    {
        if (!File.Exists(fname))
        {
            return;
        }

        fig.LoadExternalFiles(dsc, fname);

        foreach (CadFigure c in fig.ChildList)
        {
            try {
                LoadExternalData(dsc, c, fname);
            }
            catch
            {
                continue;
            }
        }
    }


    #region "MessagePack file access"

    private static void SaveToMsgPackFile(string fname, IPlotterViewModel vm)
    {
        PlotterController pc = vm.Controller;

        CadData cd = new CadData(
                            pc.DB,
                            pc.DC.WorldScale,
                            pc.PageSize
                            );

        MpCadFile.Save(fname, cd);
    }

    private static void LoadFromMsgPackFile(string fname, IPlotterViewModel vm)
    {
        PlotterController pc = vm.Controller;

        CadData? cd = MpCadFile.Load(fname);

        if (cd == null)
        {
            return;
        }

        CadData rcd = cd.Value;


        vm.SetWorldScale(rcd.WorldScale);

        pc.PageSize = rcd.PageSize;

        pc.SetDB(rcd.DB);
    }


    private static void SaveToMsgPackJsonFile(string fname, IPlotterViewModel vm)
    {
        PlotterController pc = vm.Controller;

        CadData cd = new CadData(
            pc.DB,
            pc.DC.WorldScale,
            pc.PageSize);


        MpCadFile.SaveAsJson(fname, cd);
    }

    private static void LoadFromMsgPackJsonFile(string fname, IPlotterViewModel vm)
    {
        CadData? cd = MpCadFile.LoadJson(fname);

        if (cd == null) return;

        CadData rcd = cd.Value;

        vm.SetWorldScale(rcd.WorldScale);

        PlotterController pc = vm.Controller;

        pc.PageSize = rcd.PageSize;

        pc.SetDB(rcd.DB);
    }
    #endregion
}

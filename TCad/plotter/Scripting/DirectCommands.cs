//#define DEFAULT_DATA_TYPE_DOUBLE
using GLFont;
using Plotter.Controller;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;



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


namespace Plotter.Scripting;

public class DirectCommands
{
    private readonly PlotterController Controller;

    public DirectCommands(PlotterController controller)
    {
        Controller = controller;
    }

    public List<string> GetAutoCompleteForSimpleCmd()
    {
        List<string> autoComps = new()
        {
            "@clear",
            "@cls",
            "@help key",
            "@dump db",
            "@dump DC",
            "@dump fig",
            "@dump layer",
            "@dump undo",
            "@bench draw"
        };

        return autoComps;
    }

    public void BenchDraw()
    {
        ItConsole.println("BenchDraw start");

        Action draw = () =>
        {
            Controller.DC.StartDraw();
            Controller.Clear();
            Controller.DrawAll();
            Controller.DC.EndDraw();
        };

        Thread.Sleep(100);

        FontRenderer.Counter = 0;

        Stopwatch sw = new();
        sw.Start();
        int i = 0;
        int cnt = 1000;
        while (i < cnt)
        {
            ThreadUtil.RunOnMainThread(draw, true);
            i++;
        }
        sw.Stop();

        ItConsole.println("BenchDraw end");
        ItConsole.println($"BenchDraw cnt:{i} time:{sw.ElapsedMilliseconds}ms");
        ItConsole.println($"BenchDraw FPS:" + (vcompo_t)cnt / sw.ElapsedMilliseconds * 1000);
        //ItConsole.println($"FontRenderer.Counter:" + FontRenderer.Counter);
    }

    public bool ExecCommand(string s)
    {
        string[] ss = Regex.Split(s, @"[ \t]+");

        string cmd = ss[0];


        if (cmd == "@clear" || s == "@cls")
        {
            ItConsole.Clear();
        }
        else if (cmd == "@bench")
        {
            if (ss[1] == "draw")
            {
                BenchDraw();
            }
        }
        else if (cmd == "@dump")
        {
            if (ss[1] == "db")
            {
                Controller.DB.dump();
            }
            else if (ss[1] == "DC")
            {
                Controller.DC.dump();
            }
            else if (ss[1] == "fig")
            {
                if (Controller.CurrentFigure != null)
                {
                    Controller.CurrentFigure.Dump();
                }
            }
            else if (ss[1] == "layer")
            {
                if (Controller.CurrentLayer != null)
                {
                    Controller.CurrentLayer.dump();
                }
            }
            else if (ss[1] == "undo")
            {
                Controller.HistoryMan?.dumpUndoStack();
            }
        }
        else if (cmd == "@help")
        {
            if (ss.Length > 1)
            {
                if (ss[1] == "key")
                {
                    HelpOfKey(ss.Length > 2 ? ss[2] : null);
                }
            }
        }
        else if (cmd == "@clearTemp")
        {
            Controller.TempFigureList.Clear();
        }
        else
        {
            return false;
        }

        return true;
    }

    private void HelpOfKey(string keyword)
    {
        List<string> res = Controller.ViewModelIF.HelpOfKey(keyword);

        res.ForEach((s) =>
        {
            ItConsole.println(s);
        });
    }
}

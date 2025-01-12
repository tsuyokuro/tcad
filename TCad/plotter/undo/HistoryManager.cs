//#define DEFAULT_DATA_TYPE_DOUBLE
using Plotter.Controller;
using System.Collections.Generic;



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

public class HistoryManager
{
    private PlotterController mPC;

    public Stack<CadOpe> mUndoStack = new Stack<CadOpe>();
    public Stack<CadOpe> mRedoStack = new Stack<CadOpe>();

    public HistoryManager(PlotterController pc)
    {
        mPC = pc;
    }

    public void Clear()
    {
        mUndoStack.Clear();
        mRedoStack.Clear();
    }

    public void foward(CadOpe ope)
    {
        Log.plx(ope.GetType().Name);
        if (ope is null)
        {
            return;
        }

        mUndoStack.Push(ope);

        DisposeStackItems(mRedoStack);

        mRedoStack.Clear();
    }

    private void DisposeStackItems(Stack<CadOpe> stack)
    {
        foreach (CadOpe ope in stack)
        {
            ope.Dispose(mPC);
        }
    }

    public bool canUndo()
    {
        return mUndoStack.Count > 0;
    }

    public bool canRedo()
    {
        return mRedoStack.Count > 0;
    }

    public void undo()
    {
        if (mUndoStack.Count == 0) return;

        CadOpe ope = mUndoStack.Pop();

        if (ope == null)
        {
            return;
        }

        Log.plx("Undo ope:" + ope.GetType().Name);

        ope.Undo(mPC);

        mRedoStack.Push(ope);
    }

    public void redo()
    {
        if (mRedoStack.Count == 0) return;

        CadOpe ope = mRedoStack.Pop();

        if (ope == null)
        {
            return;
        }

        Log.plx("Redo ope:" + ope.GetType().Name);

        ope.Redo(mPC);
        mUndoStack.Push(ope);
    }

    public void dumpUndoStack()
    {
        Log.plx("UndoStack");
        Log.pl("{");
        Log.Indent++;
        foreach (CadOpe ope in mUndoStack)
        {
            dumpCadOpe(ope);
        }
        Log.Indent--;
        Log.pl("}");
    }

    public static void dumpCadOpe(CadOpe ope)
    {
        Log.pl(ope.GetType().Name);

        if (ope is CadOpeList)
        {
            Log.pl("{");
            Log.Indent++;
            foreach (CadOpe item in ((CadOpeList)ope).OpeList) {
                dumpCadOpe(item);
            }
            Log.Indent--;
            Log.pl("}");
        }
        else if (ope is CadOpeFigureSnapShotList)
        {
            Log.pl("{");
            Log.Indent++;
            foreach (CadOpe item in ((CadOpeFigureSnapShotList)ope).SnapShotList)
            {
                dumpCadOpe(item);
            }
            Log.Indent--;
            Log.pl("}");
        }
    }
}

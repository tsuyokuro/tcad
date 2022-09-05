using Plotter.Controller;

namespace Plotter
{
    public interface IPlotterView
    {
        DrawContext DrawContext
        {
            get;
        }

        System.Windows.Forms.Control FormsControl
        {
            get;
        }

        void SetController(PlotterController controller);

        void CursorLocked(bool locked);

        void ChangeMouseCursor(PlotterCallback.MouseCursorType cursorType);

        void SetWorldScale(double scale);

        void DrawModeUpdated(DrawTools.DrawMode mode);

        void ShowContextMenu(PlotterController sender, MenuInfo menuInfo, int x, int y);
    }

    public interface IPlotterViewForDC
    {
        void GLMakeCurrent();
        void PushToFront(DrawContext dc);
    }
}

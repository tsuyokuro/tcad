
using OpenTK.Mathematics;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Plotter
{
    public static class Color4Util
    {
        [StructLayout(LayoutKind.Explicit)]
        private struct ColorPack
        {
            [FieldOffset(0)]
            public int Argb;

            [FieldOffset(3)]
            public byte A;

            [FieldOffset(2)]
            public byte R;

            [FieldOffset(1)]
            public byte G;

            [FieldOffset(0)]
            public byte B;
        }

        public static Color4 FromArgb(int argb)
        {
            ColorPack c = default;
            c.Argb = argb;

            return new Color4(
                    c.R,
                    c.G,
                    c.B,
                    c.A
                );
        }

        public static Color ToGDIColor(Color4 c)
        {
            return Color.FromArgb(
                (int)(c.A * 255f),
                (int)(c.R * 255f),
                (int)(c.G * 255f),
                (int)(c.B * 255f));
        }
    }
}

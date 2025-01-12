//#define DEFAULT_DATA_TYPE_DOUBLE



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

public struct StackArray<T>
{
    public T v0;
    public T v1;
    public T v2;
    public T v3;

    public int Length;

    public T this[int i]
    {
        get
        {
            switch (i)
            {
                case 0: return v0;
                case 1: return v1;
                case 2: return v2;
                case 3: return v3;
            }

            return default(T);
        }

        set
        {
            switch (i)
            {
                case 0: v0 = value; break;
                case 1: v1 = value; break;
                case 2: v2 = value; break;
                case 3: v3 = value; break;
            }
        }
    }
}

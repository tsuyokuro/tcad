//#define DEFAULT_DATA_TYPE_DOUBLE
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Runtime.CompilerServices;



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


namespace GLUtil;

public class TextureProvider
{
    private static TextureProvider sInstance;

    private readonly List<int> mTextures = new();

    public static TextureProvider Instance
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get
        {
            sInstance ??= new TextureProvider();
            return sInstance;
        }
    }

    public int GetNew()
    {
        int textureID = GL.GenTexture();
        mTextures.Add(textureID);
        return textureID;
    }

    public void RemoveAll()
    {
        for (int i=0;i<mTextures.Count;i++)
        {
            GL.DeleteTexture(mTextures[i]);
        }
        mTextures.Clear();
    }

    public void Remove(int name)
    {
        if (mTextures.Contains(name))
        {
            GL.DeleteTexture(name);
            mTextures.Remove(name);
        }
    }
}

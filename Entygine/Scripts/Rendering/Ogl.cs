using Entygine.DevTools;
using Entygine.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SixLabors.ImageSharp;
using System;
using System.Diagnostics;

namespace Entygine.Rendering
{
    public static class Ogl
    {
        public static bool enableErrorCheck = false;

        private static void LogErrors()
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                DevConsole.Log(LogType.Error, error + "\n" + new StackTrace());
        }

        private static void LabelObject(ObjectLabelIdentifier id, int obj, string name)
        {
            GL.ObjectLabel(id, obj, name.Length, name);

            ErrorCode error = GL.GetError();
            //Fucking ignore the error, not sure why it triggers always
        }

        #region VAO
        public static int GenVertexArray(string name)
        {
            int vertexArray = GL.GenVertexArray();

            if (enableErrorCheck)
                LogErrors();

            LabelObject(ObjectLabelIdentifier.VertexArray, vertexArray, $"VAO: {name}");

            return vertexArray;
        }

        private static int bindedVertexArray = 0;
        public static void BindVertexArray(int handle)
        {
            GL.BindVertexArray(handle);

            bindedVertexArray = handle;

            if (enableErrorCheck)
                LogErrors();
        }

        public static void VertexArrayVertexBuffer(int obj, int index, int buffer, IntPtr offset, int stride)
        {
            GL.VertexArrayVertexBuffer(obj, index, buffer, offset, stride);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void VertexArrayElementBuffer(int obj, int buffer)
        {
            GL.VertexArrayElementBuffer(obj, buffer);

            if (enableErrorCheck)
                LogErrors();
        }
        #endregion

        public static void PixelStore(PixelStoreParameter pixelStore, int parameter)
        {
            GL.PixelStore(PixelStoreParameter.UnpackAlignment, parameter);

            if (enableErrorCheck)
                LogErrors();
        }

        /// <summary>
        /// Generates a texture in the GPU
        /// </summary>
        public static int GenTexture(string name)
        {
            int program = GL.GenTexture();

            if (enableErrorCheck)
                LogErrors();

            LabelObject(ObjectLabelIdentifier.Texture, program, $"Texture: {name}");

            return program;
        }

        /// <summary>
        /// Destroys a texture in the GPU
        /// </summary>
        public static void DeleteTexture(int programID)
        {
            GL.DeleteTexture(programID);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void AttachShader(int program, int shader)
        {
            GL.AttachShader(program, shader);

            if (enableErrorCheck)
                LogErrors();
        }

        public static string GetProgramInfoLog(int program)
        {
            string log = GL.GetProgramInfoLog(program);

            if (enableErrorCheck)
                LogErrors();

            return log;
        }

        public static int GenFramebuffer(string name)
        {
            int framebuffer = GL.GenFramebuffer();

            if (enableErrorCheck)
                LogErrors();

            LabelObject(ObjectLabelIdentifier.Framebuffer, framebuffer, $"Framebuffer: {name}");

            return framebuffer;
        }

        public static void BindFramebuffer(FramebufferTarget target, int handle)
        {
            GL.BindFramebuffer(target, handle);

            if (enableErrorCheck)
                LogErrors();
        }

        public static FramebufferErrorCode CheckFramebufferStatus(FramebufferTarget target)
        {
            return GL.CheckFramebufferStatus(target);
        }

        public static void FramebufferTexture2D(FramebufferTarget target, FramebufferAttachment attachment, TextureTarget texTarget, int handle, int level)
        {
            GL.FramebufferTexture2D(target, attachment, texTarget, handle, level);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void DrawBuffer(DrawBufferMode mode)
        {
            GL.DrawBuffer(mode);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void ReadBuffer(ReadBufferMode mode)
        {
            GL.ReadBuffer(mode);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void Viewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void DeleteShader(int shader)
        {
            GL.DeleteShader(shader);

            if (enableErrorCheck)
                LogErrors();
        }

        public static bool IsProgram(int program)
        {
            bool isProgram = GL.IsProgram(program);

            if (enableErrorCheck)
                LogErrors();

            return isProgram;
        }

        public static void DetachShader(int program, int shader)
        {
            GL.DetachShader(program, shader);

            if (enableErrorCheck)
                LogErrors();
        }

        public static int GetAttribLocation(int program, string attribName)
        {
            int attLoc = GL.GetAttribLocation(program, attribName);

            //if (attLoc == -1)
            //    DevConsole.Log("Location for " + attribName + " was not found.");

            if (enableErrorCheck)
                LogErrors();

            return attLoc;
        }

        public static int CreateProgram(string name)
        {
            int program = GL.CreateProgram();

            if (enableErrorCheck)
                LogErrors();

            LabelObject(ObjectLabelIdentifier.Program, program, $"Program {name}");

            return program;
        }

        public static int CreateShader(ShaderType type, string name)
        {
            int shader = GL.CreateShader(type);

            if (enableErrorCheck)
                LogErrors();

            LabelObject(ObjectLabelIdentifier.Shader, shader, $"Shader: {name}");

            return shader;
        }

        public static string GetShaderInfoLog(int shader)
        {
            string log = GL.GetShaderInfoLog(shader);

            if (enableErrorCheck)
                LogErrors();

            return log;
        }

        public static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void ShaderSource(int shader, string source)
        {
            GL.ShaderSource(shader, source);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void Clear(ClearBufferMask clearBufferMask)
        {
            GL.Clear(clearBufferMask);

            if (enableErrorCheck)
                LogErrors();
        }

        private static int textureBinded;
        public static void BindTexture(TextureTarget target, int handle)
        {
            GL.BindTexture(target, handle);
            textureBinded = handle;
            if (enableErrorCheck)
                LogErrors();
        }

        public static string GetActiveUniform(int program, int index, out int size, out ActiveUniformType type)
        {
            string uniform = GL.GetActiveUniform(program, index, out size, out type);

            if (enableErrorCheck)
                LogErrors();

            return uniform;
        }

        public static int GetUniformLocation(int program, string key)
        {
            int location = GL.GetUniformLocation(program, key);

            if (enableErrorCheck)
                LogErrors();

            return location;
        }

        public static int GenBuffer(string name)
        {
            int buffer = GL.GenBuffer();

            if (enableErrorCheck)
                LogErrors();

            LabelObject(ObjectLabelIdentifier.Buffer, buffer, $"Buffer {name}");

            return buffer;
        }

        public static void NamedBufferData(int obj, int size, IntPtr data, BufferUsageHint hint)
        {
            GL.NamedBufferData(obj, size, data, hint);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void NamedBufferSubData(int obj, IntPtr offset, int size, IntPtr data)
        {
            GL.NamedBufferSubData(obj, offset, size, data);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void ActiveTexture(TextureUnit texture)
        {
            GL.ActiveTexture(texture);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void GetProgram(int program, GetProgramParameterName pname, out int parameters)
        {
            GL.GetProgram(program, pname, out parameters);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void DrawArray(PrimitiveType mode, int first, int count)
        {
            GL.DrawArrays(mode, first, count);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void DrawElements(PrimitiveType mode, int count, DrawElementsType type, int indices)
        {
            GL.DrawElements(mode, count, type, indices);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void TexParameter(TextureTarget target, TextureParameterName pname, float[] param)
        {
            GL.TexParameter(target, pname, param);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void TexParameter(TextureTarget target, TextureParameterName pname, TextureWrapMode param)
        {
            GL.TexParameter(target, pname, (int)param);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void TexParameter(TextureTarget target, TextureParameterName pname, TextureMagFilter param)
        {
            GL.TexParameter(target, pname, (int)param);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void TexParameter(TextureTarget target, TextureParameterName pname, TextureMinFilter param)
        {
            GL.TexParameter(target, pname, (int)param);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr ptr)
        {
            GL.TexImage2D(target, level, internalFormat, width, height, border, format, type, ptr);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void TexImage2D(TextureTarget target, int level, PixelInternalFormat internalFormat, int width, int height, int border, PixelFormat format, PixelType type, byte[] data)
        {
            GL.TexImage2D(target, level, internalFormat, width, height, border, format, type, data);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void Disable(EnableCap cap)
        {
            GL.Disable(cap);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void GenerateMipmap(GenerateMipmapTarget target)
        {
            GL.GenerateMipmap(target);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void Enable(EnableCap cap)
        {
            GL.Enable(cap);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void PointSize(float pixelSize)
        {
            GL.PointSize(pixelSize);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void CullFace(CullFaceMode mode)
        {
            GL.CullFace(mode);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void DepthFunc(DepthFunction depth)
        {
            GL.DepthFunc(depth);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void BlendFunc(BlendingFactor source, BlendingFactor dest)
        {
            GL.BlendFunc(source, dest);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void UniformMatrix4(int location, bool transpose, ref Matrix4 matrix)
        {
            GL.UniformMatrix4(location, transpose, ref matrix);

            if (enableErrorCheck)
                LogErrors();
        }

        private static int currentProgram = 0;
        public static void UseProgram(int handle)
        {
            GL.UseProgram(handle);
            currentProgram = handle;

            if (enableErrorCheck)
                LogErrors();
        }

        public static void Uniform1(int location, int value)
        {
            GL.Uniform1(location, value);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void Uniform3(int location, Vector3 vector)
        {
            GL.Uniform3(location, vector);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void Uniform4(int location, Color01 color)
        {
            GL.Uniform4(location, (Color4)color);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void ClearColor(float r, float g, float b, float a)
        {
            GL.ClearColor(r, g, b, a);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void BindBuffer(BufferTarget target, int buffer)
        {
            GL.BindBuffer(target, buffer);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void BufferData(BufferTarget target, int size, IntPtr data, BufferUsageHint usage)
        {
            GL.BufferData(target, size, data, usage);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void BufferData<T0>(BufferTarget target, int size, T0[] data, BufferUsageHint usage) where T0 : struct
        {
            GL.BufferData(target, size, data, usage);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void BufferSubData(BufferTarget target, IntPtr offset, int size, IntPtr data)
        {
            GL.BufferSubData(target, offset, size, data);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void EnableVertexArrayAttrib(int obj, int index)
        {
            GL.EnableVertexArrayAttrib(obj, index);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void EnableVertexAttribArray(int index)
        {
            GL.EnableVertexAttribArray(index);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void VertexArrayAttribBinding(int obj, int attribIndex, int bufferIndex)
        {
            GL.VertexArrayAttribBinding(obj, attribIndex, bufferIndex);

            if (enableErrorCheck)
                LogErrors();
        }
        public static void VertexArrayAttribFormat(int obj, int attIndex, int size, VertexAttribType type, bool normalized, int relativeOffset)
        {
            GL.VertexArrayAttribFormat(obj, attIndex, size, type, normalized, relativeOffset);

            if (enableErrorCheck)
                LogErrors();
        }

        public static void VertexAttribPointer(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
        {
            GL.VertexAttribPointer(index, size, type, normalized, stride, offset);

            if (enableErrorCheck)
                LogErrors();
        }
    }
}

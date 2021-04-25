using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;

using System.IO;
using System.ComponentModel;

using System.Diagnostics;

namespace Graphics
{
    public struct Vert
    {
        public float xp, yp, zp;
        public float R, G, B;
        public float U, V;
        public float xn, yn, zn;
    }
    public struct mode
    {
        public string prem;
        public int start;
        public int count;
    }

    public struct LightVertix
    {
        public float xamb, yamb, zamb;
        public float xd, yd, zd;
        public float xs, ys, zs;
        public float xp, yp, zp;
        public float spEx;
    }

    public struct mess
    {
        public static string s;
    }
    class Renderer
    {
        Shader sh;
        uint vertexBufferID;
        uint[] vbo = { 0 };

        // For Light
        int AmbientLightID;
        int diffuseLightID;
        int specularLightID;
        int DataID;
        int flagID;
        float light_bool;

        //3D Drawing
        int MVPID;
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        mat4 MVP;

        //Loading image
        public Texture tex1;
        float texBool;
        public int texID;

        public List<mode> tempModes;
        public List<mode> originalModes;
        public List<float> Verts;
        public List<float> U_V;
        public List<float> NORM;
        public float[] verts;
        public bool selected = false;

        public float[] vaxis = {
            //selected point
                0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 0.0f, //R
                0,0, //UV
                0,0,0, //Normal
                     
            //x
                -1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, //R
                0,1, //UV
                0,1,0, //Normal
                 
		        1.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, //R
		        0,0, //UV
                0,1,0, //Normal
                
                //y
	            0.0f, -1.0f, 0.0f,
                0.0f, 1.0f, 0.0f, //G
		        1,0, //UV
                0,1,0, //Normal
                
                0.0f, 1.0f, 0.0f,
                0.0f, 1.0f, 0.0f, //G
		        0,0, //UV
                0,1,0, //Normal
                
                //z
	            0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 1.0f,  //B
		        1,1, //UV
                0,1,0, //Normal
                    
                0.0f, 0.0f, -1.0f,
                0.0f, 0.0f, 1.0f,  //B
                0,0, //UV
                0,1,0 //Normal
        };
        public void addmode(mode newmode)
        {
            tempModes.Add(newmode);
        }
        public void UpdateMode(int index, mode Umode)
        {
            tempModes.RemoveAt(index);
            tempModes.Insert(index, Umode);
        }
        public void DelteMode(int index)
        {
            tempModes.RemoveAt(index);
        }
        public void SetMode()
        {
            originalModes = new List<mode>(tempModes);
        }
        public mode getMode(int index)
        {
            return tempModes.ElementAt(index);
        }
        public void AddVertix(Vert vertix)
        {
            Verts.Add(vertix.xp);
            Verts.Add(vertix.yp);
            Verts.Add(vertix.zp);
            Verts.Add(vertix.R);
            Verts.Add(vertix.G);
            Verts.Add(vertix.B);

            U_V.Add(vertix.U);
            U_V.Add(vertix.V);

            NORM.Add(vertix.xn);
            NORM.Add(vertix.yn);
            NORM.Add(vertix.zn);
        }
        public void UpdateVertix(int index, Vert vrt)
        {
            int myindex = index * 6;

            Verts.RemoveAt(myindex);
            Verts.Insert(myindex, vrt.xp);
            myindex++;
            Verts.RemoveAt(myindex);
            Verts.Insert(myindex, vrt.yp);
            myindex++;
            Verts.RemoveAt(myindex);
            Verts.Insert(myindex, vrt.zp);
            myindex++;
            Verts.RemoveAt(myindex);
            Verts.Insert(myindex, vrt.R);
            myindex++;
            Verts.RemoveAt(myindex);
            Verts.Insert(myindex, vrt.G);
            myindex++;
            Verts.RemoveAt(myindex);
            Verts.Insert(myindex, vrt.B);

            myindex = index * 2;

            U_V.RemoveAt(myindex);
            U_V.Insert(myindex, vrt.U);
            myindex++;
            U_V.RemoveAt(myindex);
            U_V.Insert(myindex, vrt.V);

            myindex = index * 3;

            NORM.RemoveAt(myindex);
            NORM.Insert(myindex, vrt.xn);
            myindex++;
            NORM.RemoveAt(myindex);
            NORM.Insert(myindex, vrt.yn);
            myindex++;
            NORM.RemoveAt(myindex);
            NORM.Insert(myindex, vrt.zn);
        }
        public void DeleteVertix(int index)
        {
            int Myindex;
            Myindex = index * 6;
            for (int i = Myindex; i < Myindex + 6; i++)
                Verts.RemoveAt(Myindex);
            
            Myindex = index * 2;
            U_V.RemoveAt(Myindex);
            U_V.RemoveAt(Myindex);

            Myindex = index * 3;
            for (int i = Myindex; i < Myindex + 3; i++)
                NORM.RemoveAt(Myindex);
        }
        public Vert getVertixData(int index)
        {
            int Myindex;
            Myindex = index * 6;
            Vert MyPoint = new Vert();
            MyPoint.xp = Verts.ElementAt(Myindex);
            Myindex++;
            MyPoint.yp = Verts.ElementAt(Myindex);
            Myindex++;
            MyPoint.zp = Verts.ElementAt(Myindex);
            Myindex++;
            MyPoint.R = Verts.ElementAt(Myindex);
            Myindex++;
            MyPoint.G = Verts.ElementAt(Myindex);
            Myindex++;
            MyPoint.B = Verts.ElementAt(Myindex);
            Myindex = index * 2;
            MyPoint.U = U_V.ElementAt(Myindex);
            Myindex++;
            MyPoint.V = U_V.ElementAt(Myindex);
            Myindex = index * 3;
            MyPoint.xn = NORM.ElementAt(Myindex);
            Myindex++;
            MyPoint.yn = NORM.ElementAt(Myindex);
            Myindex++;
            MyPoint.zn = NORM.ElementAt(Myindex);
            return MyPoint;
        }
        public void setArrays(int[] arrofindexs)
        {
            verts = new float[(arrofindexs.Length * 11) + vaxis.Length];

            int vertsindex = 0;
            for (int i = 0; i < arrofindexs.Length; i++)
            {
                int index = arrofindexs[i];
                int myindex = index * 6;
                for (int j = myindex; j < myindex + 6; j++)
                {
                    verts[vertsindex] = Verts.ElementAt(j);
                    vertsindex++;
                }
                myindex = index * 2;
                for (int j = myindex; j < myindex + 2; j++)
                {
                    verts[vertsindex] = U_V.ElementAt(j);
                    vertsindex++;
                }
                myindex = index * 3;
                for (int j = myindex; j < myindex + 3; j++)
                {
                    verts[vertsindex] = NORM.ElementAt(j);
                    vertsindex++;
                }
            }
            vaxis.CopyTo(verts, vertsindex);
            
            Gl.glDeleteBuffers(1, vbo);
            GPU.GenerateBuffer(verts);
        }
        public void modifyvaxis(Vert vert)
        {
            vaxis[0] = vert.xp;
            vaxis[1] = vert.yp;
            vaxis[2] = vert.zp;
            vaxis[3] = 0;
            vaxis[4] = 1;
            vaxis[5] = 0;
            vaxis[6] = vert.U;
            vaxis[7] = vert.V;
            vaxis[8] = vert.xn;
            vaxis[9] = vert.yn;
            vaxis[10] = vert.zn;

            int index = verts.Length - 11 * 7;
            for (int i = 0; i < 11; i++)
            {
                verts[index] = vaxis[i];
                index++;
            }
            Gl.glDeleteBuffers(1, vbo);
            GPU.GenerateBuffer(verts);

        }
        public void setLight(LightVertix Lsrc)
        {
            //ambientLight
            //============
            vec3 ambientLight = new vec3(Lsrc.xamb, Lsrc.yamb, Lsrc.zamb);
            AmbientLightID = Gl.glGetUniformLocation(sh.ID, "aL");
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());

            //DiffuseLight
            //============
            vec3 diffuseLight = new vec3(Lsrc.xd, Lsrc.yd, Lsrc.zd);
            diffuseLightID = Gl.glGetUniformLocation(sh.ID, "diff");
            Gl.glUniform3fv(diffuseLightID, 1, diffuseLight.to_array());

            //specularLight
            //=============
            vec3 specularLight = new vec3(Lsrc.xs, Lsrc.ys, Lsrc.zs);
            specularLightID = Gl.glGetUniformLocation(sh.ID, "specularLight");
            Gl.glUniform3fv(specularLightID, 1, specularLight.to_array());

            //LightPosition
            //==============
            vec3 lightPosition = new vec3(Lsrc.xp, Lsrc.yp, Lsrc.zp);
            int LightPositionID = Gl.glGetUniformLocation(sh.ID, "LightPosition_worldspace");
            Gl.glUniform3fv(LightPositionID, 1, lightPosition.to_array());

            //attenuation & specularExponent
            //==================================
            float attenuation = 100, specularExponent = Lsrc.spEx;
            vec2 data = new vec2(attenuation, specularExponent);
            DataID = Gl.glGetUniformLocation(sh.ID, "data");
            Gl.glUniform2fv(DataID, 1, data.to_array());

            light_bool = 1.0f;
            Gl.glUniform1f(flagID, light_bool);
        }
        public void DisableLight()
        {
            light_bool = 0.0f;
            Gl.glUniform1f(flagID, light_bool);
        }
        public void Initialize()
        {
            Verts = new List<float>();
            U_V = new List<float>();
            NORM = new List<float>();
            tempModes = new List<mode>();
            originalModes = new List<mode>();
            
            verts = new float[vaxis.Length];
            vaxis.CopyTo(verts, 0);
            
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(0.5f, 0.5f, 0.5f, 0.5f);

            vertexBufferID = GPU.GenerateBuffer(verts);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);

            // View matrix 
            ViewMatrix = glm.lookAt(
                    new vec3(-0.5f, 0.7f, 1),  // Camera is at (Eye looks at)
                    new vec3(0, 0, 0),  // It looks at the Origin
                    new vec3(0, 1, 0)  // Y-axis points upwards
                );
            
            // Model matrix: apply transformations to the model
            List<mat4> Transformation = new List<mat4>();
            //Transformation.Add(glm.rota)
            Transformation.Add(glm.rotate(180.0f * 3.127f, new vec3(0, 1, 0)));
            Transformation.Add(glm.scale(new mat4(1), new vec3(1, 1, 1)));
            
            ModelMatrix = MathHelper.MultiplyMatrices(Transformation);

            // Our MVP matrix which is a multiplication of our 3 matrices 
            List<mat4> mvp = new List<mat4>();
            mvp.Add(ModelMatrix);
            mvp.Add(ViewMatrix);
            mvp.Add(ProjectionMatrix);

            MVP = MathHelper.MultiplyMatrices(mvp);
            
            sh.UseShader();
            //Texture
            texID = Gl.glGetUniformLocation(sh.ID, "texBool");
            texBool = 0.0f;
            Gl.glUniform1f(texID, texBool);
            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            MVPID = Gl.glGetUniformLocation(sh.ID, "MVP");
            //pass the value of the MVP you just filled to the vertex shader
            Gl.glUniformMatrix4fv(MVPID, 1, Gl.GL_FALSE, MVP.to_array());

            #region
            //ambientLight
            //============
            vec3 ambientLight = new vec3(1, 1, 1);
            AmbientLightID = Gl.glGetUniformLocation(sh.ID, "aL");
            Gl.glUniform3fv(AmbientLightID, 1, ambientLight.to_array());

            //DiffuseLight
            //============
            vec3 diffuseLight = new vec3(1, 1, 1);
            diffuseLightID = Gl.glGetUniformLocation(sh.ID, "diff");
            Gl.glUniform3fv(diffuseLightID, 1, diffuseLight.to_array());

            //specularLight
            //=============
            vec3 specularLight = new vec3(1, 1, 1);
            specularLightID = Gl.glGetUniformLocation(sh.ID, "specularLight");
            Gl.glUniform3fv(specularLightID, 1, specularLight.to_array());

            //LightPosition
            //==============
            vec3 lightPosition = new vec3(1, 1, 1);
            int LightPositionID = Gl.glGetUniformLocation(sh.ID, "LightPosition_worldspace");
            Gl.glUniform3fv(LightPositionID, 1, lightPosition.to_array());

            //attenuation & specularExponent
            //==================================
            float attenuation = 100, specularExponent = 50;
            vec2 data = new vec2(attenuation, specularExponent);
            DataID = Gl.glGetUniformLocation(sh.ID, "data");
            Gl.glUniform2fv(DataID, 1, data.to_array());
            #endregion

            flagID = Gl.glGetUniformLocation(sh.ID, "light_bool");
            light_bool = 0.0f;
            Gl.glUniform1f(flagID, light_bool);
        }
        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            Gl.glColorMask(1, 1, 1, 0);
            //texture
            texBool = 0.0f;
            Gl.glUniform1f(texID, texBool);

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), IntPtr.Zero);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(3 * sizeof(float)));
            //Light 
            Gl.glEnableVertexAttribArray(3);
            Gl.glVertexAttribPointer(3, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 11 * sizeof(float), (IntPtr)(8 * sizeof(float)));

            if (selected)
            {
                Gl.glPointSize(10);
                Gl.glDrawArrays(Gl.GL_POINTS, (verts.Length / 11) - 7, 1);
            }

            int startAxispt = (verts.Length / 11) - 6;
            Gl.glDrawArrays(Gl.GL_LINES, startAxispt, 6);


            //Texture
            Gl.glEnableVertexAttribArray(2);
            Gl.glVertexAttribPointer(2, 2, Gl.GL_FLOAT, Gl.GL_FALSE, 8 * sizeof(float), (IntPtr)(6 * sizeof(float)));
            if (tex1 != null)
            {
                tex1.Bind();
                texBool = 1.0f;
                Gl.glUniform1f(texID, texBool);
            }
            else
            {
                texBool = 0.0f;
                Gl.glUniform1f(texID, texBool);
            }
            Gl.glPointSize(2);
            for (int i = 0; i < originalModes.Count; i++)
            {
                switch (originalModes[i].prem)
                {
                    case "Triangles":
                        Gl.glDrawArrays(Gl.GL_TRIANGLES, originalModes[i].start, originalModes[i].count);
                        break;
                    case "Triangle_Fan":
                        Gl.glDrawArrays(Gl.GL_TRIANGLE_FAN, originalModes[i].start, originalModes[i].count);
                        break;
                    case "Triangle_Strip":
                        Gl.glDrawArrays(Gl.GL_TRIANGLE_STRIP, originalModes[i].start, originalModes[i].count);
                        break;
                    case "Points":
                        Gl.glDrawArrays(Gl.GL_POINTS, originalModes[i].start, originalModes[i].count);
                        break;
                    case "Lines":
                        Gl.glDrawArrays(Gl.GL_LINES, originalModes[i].start, originalModes[i].count);
                        break;
                    case "Line_Strip":
                        Gl.glDrawArrays(Gl.GL_LINE_STRIP, originalModes[i].start, originalModes[i].count);
                        break;
                    case "Line_Loop":
                        Gl.glDrawArrays(Gl.GL_LINE_LOOP, originalModes[i].start, originalModes[i].count);
                        break;
                    default:
                        break;
                }
            }

            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
            Gl.glDisableVertexAttribArray(2);
            Gl.glDisableVertexAttribArray(3);
        }
        public void Update()
        {
            
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
    }
}
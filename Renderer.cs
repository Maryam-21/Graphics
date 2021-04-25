using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;
using GlmNet;
//include GLM library


using System.IO;

namespace Graphics
{
    class Renderer
    {
        Shader sh;
        uint vertexBufferID;

        //3D Drawing
        int MVPID;
        mat4 ModelMatrix;
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;
        mat4 MVP;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(0, 0, 0.4f, 1);
            float[] verts = { 
		        // Polygon 1
                //p1
		        0.0f, 0.0f, 0.0f, //0
                1.0f, 0.0f, 0.0f, //R
                //p2
	            1.0f, 0.0f, 0.0f, //1
                0.0f, 1.0f, 0.0f, //G
                //p3
		        1.0f, 0.0f, -1.0f, //2
                0.0f, 0.0f, 1.0f,  //B
		        //p4
                0.0f, 0.0f, -1.0f, //3
                0.0f, 0.0f, 1.0f,  //B
		        
                //Polygon 2
                //p5
                0.0f, 1.0f, -1.0f, //4
                1.0f, 0.0f, 0.0f, //R
                //p6
                1.0f, 1.0f, -1.0f, //5
                1.0f, 0.0f, 0.0f, //R
                //p7
                1.0f, 1.0f, 0.0f, //6
                1.0f, 0.0f, 0.0f, //R
                //p8
                0.0f, 1.0f, 0.0f, //7
                1.0f, 0.0f, 0.0f, //R

                //Connecting lines
                //L1
                0.0f, 1.0f, -1.0f, //8
                1.0f, 0.0f, 0.0f, //R
                0.0f, 0.0f, -1.0f, //9
                1.0f, 0.0f, 0.0f, //R
                //L2
                1.0f, 1.0f, -1.0f, //10
                1.0f, 0.0f, 0.0f, //R
                1.0f, 0.0f, -1.0f, //11
                1.0f, 0.0f, 0.0f, //R
                //L3
                1.0f, 1.0f, 0.0f, //12
                1.0f, 0.0f, 0.0f, //R
                1.0f, 0.0f, 0.0f, //13
                1.0f, 0.0f, 0.0f, //R
                //L4
                0.0f, 1.0f, 0.0f, //14
                1.0f, 0.0f, 0.0f, //R
                0.0f, 0.0f, 0.0f, //15
                1.0f, 0.0f, 0.0f, //R

		        //Axis
		        //x
		        0.0f, 0.0f, 0.0f, //16
                1.0f, 0.0f, 0.0f, //R
		        5.0f, 0.0f, 0.0f,
                1.0f, 0.0f, 0.0f, //R
		        //y
	            0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, //G
		        0.0f, 5.0f, 0.0f,
                0.0f, 1.0f, 0.0f, //G
		        //z
	            0.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f,  //B
		        0.0f, 0.0f, -5.0f,
                0.0f, 0.0f, 1.0f,  //B
            };


            vertexBufferID = GPU.GenerateBuffer(verts);

            //ProjectionMatrix = glm.perspective(FOV, Width / Height, Near, Far);
            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);

            // View matrix 
            ViewMatrix = glm.lookAt(
                    new vec3(0,10,5),
                    new vec3(0,0,0),
                    new vec3(0,1,0)
                );

            // Model matrix: apply transformations to the model
            List<mat4> Transformations = new List<mat4>();
            Transformations.Add(glm.scale(new mat4(1), new vec3(2, 4, 1)));
            Transformations.Add(glm.rotate(-45.0f / 180.0f * 3.127f, new vec3(0, 1, 0)));
            
            ModelMatrix = MathHelper.MultiplyMatrices(Transformations);
            // Our MVP matrix which is a multiplication of our 3 matrices 
            List<mat4> mvp = new List<mat4>();
            mvp.Add(ModelMatrix);
            mvp.Add(ViewMatrix);
            mvp.Add(ProjectionMatrix);
            MVP = MathHelper.MultiplyMatrices(mvp);

            sh.UseShader();


            //Get a handle for our "MVP" uniform (the holder we created in the vertex shader)
            MVPID = Gl.glGetUniformLocation(sh.ID, "MVP");
            //pass the value of the MVP you just filled to the vertex shader
            Gl.glUniformMatrix4fv(MVPID, 1, Gl.GL_FALSE, MVP.to_array());
        }

        public void Draw()
        {
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);
            
            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6*sizeof(float), (IntPtr)0);
            Gl.glEnableVertexAttribArray(1);
            Gl.glVertexAttribPointer(1, 3, Gl.GL_FLOAT, Gl.GL_FALSE, 6 * sizeof(float), (IntPtr)(3*sizeof(float)));

            Gl.glDrawArrays(Gl.GL_POLYGON, 0, 4);
            Gl.glDrawArrays(Gl.GL_POLYGON, 4,4 );
            Gl.glDrawArrays(Gl.GL_LINES, 8, 14);



            Gl.glDisableVertexAttribArray(0);
            Gl.glDisableVertexAttribArray(1);
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

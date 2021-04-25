using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphics
{
    public partial class GraphicsForm : Form
    {
        Renderer renderer = new Renderer();
        Thread MainLoopThread;

        //Our Declarations
        int modelVerticesCounter;
        List<int> currentVertices;
        //Texture
        int textureUnits;
        bool isText;
        public GraphicsForm()
        {
            InitializeComponent();
            simpleOpenGlControl1.InitializeContexts();
            initialize();
            MainLoopThread = new Thread(MainLoop);
            MainLoopThread.Start();
            modelVerticesCounter = 0;
            currentVertices = new List<int>();
        }
        void initialize()
        {
            renderer.Initialize();   
        }
        
        void MainLoop()
        {
            while (true)
            {
                renderer.Update();
                renderer.Draw();
                simpleOpenGlControl1.Refresh();
                if (currentVerticesLB.SelectedIndex == -1)
                    renderer.selected = false;
            }
        }
        private void GraphicsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            renderer.CleanUp();
            MainLoopThread.Abort();
        }

        private void simpleOpenGlControl1_Paint(object sender, PaintEventArgs e)
        {
            renderer.Draw();
        }

        private void groupBox1_Enter(object sender, System.EventArgs e)
        {

        }

        private void GraphicsForm_Load(object sender, System.EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, System.EventArgs e)
        {

        }

        private void label3_Click(object sender, System.EventArgs e)
        {

        }

        private void CurentVertices_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, System.EventArgs e)
        {

        }

        private void label17_Click(object sender, System.EventArgs e)
        {

        }

        private void button9_Click(object sender, System.EventArgs e) //Open Image
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox15.Text = openFileDialog1.FileName;
                pictureBox1.Image = new Bitmap(openFileDialog1.FileName);
                isText = true;
            }
        }

        private void button5_Click(object sender, System.EventArgs e) //Add Mode
        {
            mode newmode = new mode();

            newmode.prem = primitiveCB.SelectedItem.ToString();
            newmode.start = int.Parse(primitiveStartTB.Text);
            newmode.count = int.Parse(primitiveCount.Text);
            renderer.addmode(newmode);

            drawingModesLB.Items.Add(primitiveCB.SelectedItem.ToString() + " " + primitiveStartTB.Text);
            //Add item to drawing modes list
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e) //Select a vertex to get values and show in opengl window
        {
            if (currentVerticesLB.SelectedIndex != -1)
            {
                string vertex = currentVerticesLB.SelectedItem.ToString();
                int vIndex = currentVerticesLB.SelectedIndex;
                Vert vert = renderer.getVertixData(vIndex);

                xpTB.Text = vert.xp.ToString();
                ypTB.Text = vert.yp.ToString();
                zpTB.Text = vert.zp.ToString();
                rTB.Text = vert.R.ToString();
                gTB.Text = vert.G.ToString();
                bTB.Text = vert.B.ToString();
                xnTB.Text = vert.xn.ToString();
                ynTB.Text = vert.yn.ToString();
                znTB.Text = vert.zn.ToString();
                uTB.Text = vert.U.ToString();
                vTB.Text = vert.V.ToString();

                renderer.modifyvaxis(vert);
                renderer.selected = true;
            }
        }

        private void button1_Click(object sender, System.EventArgs e) //Add vertex
        {
            //Add new vertex struct
            Vert new_vertex = new Vert();

            new_vertex.xp = float.Parse(xpTB.Text);
            new_vertex.yp = float.Parse(ypTB.Text);
            new_vertex.zp = float.Parse(zpTB.Text);
            new_vertex.R = float.Parse(rTB.Text);
            new_vertex.G = float.Parse(gTB.Text);
            new_vertex.B = float.Parse(bTB.Text);
            new_vertex.xn = float.Parse(xnTB.Text);
            new_vertex.yn = float.Parse(ynTB.Text);
            new_vertex.zn = float.Parse(znTB.Text);
            new_vertex.U = float.Parse(uTB.Text);
            new_vertex.V = float.Parse(vTB.Text);
            renderer.AddVertix(new_vertex);
            
            currentVerticesLB.Items.Add("V" + modelVerticesCounter.ToString());
            modelVerticesCounter++;
        }

        private void button3_Click(object sender, System.EventArgs e) //Delete vertex
        {
            //Send the index to the fn and delete it
            string vertex = currentVerticesLB.SelectedItem.ToString();
            int newvert = currentVerticesLB.SelectedIndex;
            //int vIndex = int.Parse(newvert);
            currentVerticesLB.Items.RemoveAt(newvert);
            renderer.DeleteVertix(newvert);
            xpTB.Text = " ";
            ypTB.Text = " ";
            zpTB.Text = " ";
            rTB.Text = " ";
            gTB.Text = " ";
            bTB.Text = " ";
            xnTB.Text = " ";
            ynTB.Text = " ";
            znTB.Text = " ";
            uTB.Text = " ";
            vTB.Text = " ";

        }

        private void button7_Click(object sender, System.EventArgs e) //Delete Mode
        {
            //Send the index to the fn and delete it
            int curIndex = drawingModesLB.SelectedIndex;
            renderer.DelteMode(curIndex);

            drawingModesLB.Items.RemoveAt(drawingModesLB.SelectedIndex);
        }

        private void button4_Click(object sender, EventArgs e) //Update Current Vertices
        {
            string[] curVer = vertexIndicesTB.Text.ToString().Split(',');
            currentVertices.Clear();
            for(int i=0; i<curVer.Length; i++)
            {
                currentVertices.Add(int.Parse(curVer[i]));
            }
        }

        private void button2_Click(object sender, EventArgs e) //update a certain vertex
        {
            Vert new_vertex = new Vert();

            new_vertex.xp = float.Parse(xpTB.Text);
            new_vertex.yp = float.Parse(ypTB.Text);
            new_vertex.zp = float.Parse(zpTB.Text);
            new_vertex.R = float.Parse(rTB.Text);
            new_vertex.G = float.Parse(gTB.Text);
            new_vertex.B = float.Parse(bTB.Text);
            new_vertex.xn = float.Parse(xnTB.Text);
            new_vertex.yn = float.Parse(ynTB.Text);
            new_vertex.zn = float.Parse(znTB.Text);
            new_vertex.U = float.Parse(uTB.Text);
            new_vertex.V = float.Parse(vTB.Text);
           
            int curIndex = currentVerticesLB.SelectedIndex;
            /*
            string index = currentVerticesLB.Items[currentVerticesLB.SelectedIndex].ToString();
            index.Remove(0);
            curIndex = int.Parse(index);
            */
            renderer.UpdateVertix(curIndex , new_vertex);

            renderer.modifyvaxis(new_vertex);
            renderer.selected = true;

            xpTB.Text = " ";
            ypTB.Text = " ";
            zpTB.Text = " ";
            rTB.Text = " ";
            gTB.Text = " ";
            bTB.Text = " ";
            xnTB.Text = " ";
            ynTB.Text = " ";
            znTB.Text = " ";
            uTB.Text = " ";
            vTB.Text = " ";

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void drawingModesLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (drawingModesLB.SelectedIndex != -1)
            {
                mode m = new mode();
                int selectedIndex = drawingModesLB.SelectedIndex;
                m = renderer.getMode(selectedIndex);
                primitiveStartTB.Text = m.start.ToString();
                primitiveCount.Text = m.count.ToString();
                for(int i = 0; i < primitiveCB.Items.Count; i++)
                {
                    if(m.prem == primitiveCB.Items[i].ToString())
                    {
                        primitiveCB.SelectedIndex = i;
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mode newmode = new mode();
            newmode.prem = primitiveCB.Text;
            newmode.start = int.Parse(primitiveStartTB.Text);
            newmode.count = int.Parse(primitiveCount.Text);

            int curIndex = drawingModesLB.SelectedIndex;
            renderer.UpdateMode(curIndex,newmode);
        }

        private void button8_Click(object sender, EventArgs e)
        {
        
            renderer.setArrays(currentVertices.ToArray());
            renderer.SetMode();
            if (isText == true)
                renderer.tex1 = new Texture(openFileDialog1.FileName, textureUnits++);
            //MessageBox.Show(mess.s);
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            LightVertix newLight = new LightVertix();
            newLight.xamb = float.Parse(textBox1.Text);
            newLight.yamb = float.Parse(textBox2.Text);
            newLight.zamb = float.Parse(textBox3.Text);
            newLight.xd = float.Parse(textBox4.Text);
            newLight.yd = float.Parse(textBox5.Text);
            newLight.zd = float.Parse(textBox6.Text);
            newLight.xs = float.Parse(textBox7.Text);
            newLight.ys = float.Parse(textBox8.Text);
            newLight.zs = float.Parse(textBox9.Text);
            newLight.xp = float.Parse(textBox10.Text);
            newLight.yp = float.Parse(textBox11.Text);
            newLight.zp = float.Parse(textBox12.Text);
            newLight.spEx = float.Parse(textBox13.Text);

            renderer.setLight(newLight);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            renderer.DisableLight();
        }

        private void xpTB_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

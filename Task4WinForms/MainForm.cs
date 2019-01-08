using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyDrawing;
using MyDrawing.D3;
using MyDrawing.VisualObjects;

namespace Task4WinForms
{
    public partial class MainForm : Form
    {
        private Drawer _drawer;
        private Bitmap _bmp;

        private string _modelPath = "";
        private string _texturePath = "";

        private Scene _scene;
        private Model _model;

        private readonly List<Light> _lights = new List<Light>()
        {
            new DirectionalLight(LightType.Diffuse, new Vector(0, 0, 1)),
            new DirectionalLight(LightType.Diffuse, new Vector(1, 1, 1)),
        };

        public MainForm()
        {
            InitializeComponent();
            _bmp = new Bitmap(PictureBox.Width, PictureBox.Height);
            _drawer = new Drawer(PictureBox, _bmp, DrawType.Standart);
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        private void ModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                _texturePath = "";
                _modelPath = openFileDialog.FileName;
            }
        }

        private void RenderBtn_Click(object sender, EventArgs e)
        {
            Render();
        }

        private void Render()
        {
            Vector rotation = new Vector(0, 0, 0);

            _model = new Model(_modelPath, _texturePath)
            {
                Rotation = rotation
            };

            _scene = new Scene(_bmp, _model, _lights, new Camera(new Vector(0, 0, 1)), new Point(PictureBox.Width / 2, -PictureBox.Height / 2));

            PictureBox.SizeMode = PictureBoxSizeMode.Normal;
            PictureBox.Image = _scene.RenderScene();

        }

        private Vertex _prevPoint;
        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            var curPoint = new Vertex(e.X, e.Y, 0);
            if (_prevPoint != null && _scene != null)
            {
                if ((e.Button & MouseButtons.Left) != 0)
                {
                    Vertex v1 = new Vertex(_prevPoint.Y / 100, _prevPoint.X / 100, 0);
                    Vertex v2 = new Vertex(curPoint.Y / 100, curPoint.X / 100, 0);
                    var rotateVector = new Vector(v1, v2);
                    _scene.Model.Rotation += rotateVector;
                    PictureBox.Image = _scene.RenderScene();
                }
            }

            _prevPoint = curPoint;
        }
    }
}

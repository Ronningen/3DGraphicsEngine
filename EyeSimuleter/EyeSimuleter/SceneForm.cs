using System;
using System.Windows.Forms;

namespace EyeSimuleter
{
    public partial class SceneForm : Form
    {
        private Scene scene;

        public SceneForm()
        {
            InitializeComponent();
            scene = new Scene(sceneBox);
        }

        private void graphicTimer_Tick(object sender, EventArgs e)
        {
            sceneBox.Image = scene.NextFrame();
        }

        #region eye control

        private void sceneBox_MouseMove(object sender, MouseEventArgs e)
        {
            scene.SetMouse(e);
        }

        private void sceneBox_MouseWheel(object sender, MouseEventArgs e)
        {
            scene.SetFOV(e);
        }

        private void SceneForm_KeyDown(object sender, KeyEventArgs e)
        {
            scene.KeyDown(e);
        }

        private void SceneForm_KeyUp(object sender, KeyEventArgs e)
        {
            scene.KeyUp(e);
        }

        #endregion
    }
}

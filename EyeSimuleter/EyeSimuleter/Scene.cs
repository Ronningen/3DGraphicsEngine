using System;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

using static EyeSimuleter.EyeParams;

namespace EyeSimuleter
{
    class Scene
    {
        private Bitmap canvas;
        private Graphics g;

        private Eye eye;

        private List<ConvexPolygon> decor;

        private Action evulater;

        /// <summary>
        /// Использется, чтобы считывать положения мыши не более раза за тик таймера.
        /// </summary>
        private bool ticked;

        public Scene(PictureBox box)
        {
            width = box.Width;
            height = box.Height;

            observingDistance = width / (float)(2 * Math.Tan(StartFOV / 2));

            canvas = new Bitmap(width, height);
            g = Graphics.FromImage(canvas);

            eye = new Eye();

            decor = new List<ConvexPolygon>();
            #region decoration

            GenerateCube(1000);
            decor.Add(new ConvexPolygon(Brushes.Black, 7, (800, 0, 0), (0, 0, 400), (1, 0, 0)));
            decor.Add(new ConvexPolygon(Brushes.Black, 3, (0, 800, 0), (0, 0, 400), (0, 1, 0)));
            decor.Add(new ConvexPolygon(Brushes.Black, 3, (0, 0, 800), (0, 400, 0), (0, 0, 1))); 

            #endregion

            evulater += eye.Move;
        }

        public Image NextFrame()
        {
            g.Clear(Color.White);

            evulater();

            eye.ShowLookRayPixel(g, decor);

            g.FillEllipse(Brushes.Black, width / 2 - 3, height / 2 - 3, 6, 6);

            ticked = true;
            return canvas;
        }

        #region eye control

        public void SetMouse(MouseEventArgs e)
        {
            if (ticked)
            {
                eye.Rotate(e.Button == MouseButtons.Left ? new Point?(e.Location) : null);
                ticked = false;
            }
        }

        public void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                eye.moving.forward = true;
            if (e.KeyCode == Keys.S)
                eye.moving.backward = true;
            if (e.KeyCode == Keys.D)
                eye.moving.right = true;
            if (e.KeyCode == Keys.A)
                eye.moving.left = true;
            if (e.KeyCode == Keys.Space)
                eye.moving.up = true;
            if (e.KeyCode == Keys.ShiftKey)
                eye.moving.down = true;
        }

        public void KeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
                eye.moving.forward = false;
            if (e.KeyCode == Keys.S)
                eye.moving.backward = false;
            if (e.KeyCode == Keys.D)
                eye.moving.right = false;
            if (e.KeyCode == Keys.A)
                eye.moving.left = false;
            if (e.KeyCode == Keys.Space)
                eye.moving.up = false;
            if (e.KeyCode == Keys.ShiftKey)
                eye.moving.down = false;
        }

        public void SetFOV(MouseEventArgs e)
        {
            observingDistance += (float)e.Delta / 10;
        }

        #endregion

        #region polygon generating

        private void GenetateTriangles(uint amount, DirectCoordinate boxVertex, DirectCoordinate boxSize)
        {
            Random r = new Random();
            for (uint i = 0; i < amount; i++)
                decor.Add(new ConvexPolygon(new SolidBrush(Color.FromArgb(r.Next(160, 256), r.Next(256), r.Next(256), r.Next(256))), new DirectCoordinate[]
                {
                    boxVertex + (r.Next((int)boxSize.X), r.Next((int)boxSize.Y), r.Next((int)boxSize.Z)),
                    boxVertex + (r.Next((int)boxSize.X), r.Next((int)boxSize.Y), r.Next((int)boxSize.Z)),
                    boxVertex + (r.Next((int)boxSize.X), r.Next((int)boxSize.Y), r.Next((int)boxSize.Z))
                }));
        }

        private void GenerateCube(int w)
        {
            DirectCoordinate
                fdl = (w, -w, -w), ful = (w, w, -w), fur = (w, w, w), fdr = (w, -w, w),
                bdl = (-w, -w, -w), bul = (-w, w, -w), bur = (-w, w, w), bdr = (-w, -w, w);
            decor.Add(new ConvexPolygon(Brushes.Red, new DirectCoordinate[] { fdl, ful, fur, fdr }));
            decor.Add(new ConvexPolygon(Brushes.Blue, new DirectCoordinate[] { bdl, bul, bur, bdr }));
            decor.Add(new ConvexPolygon(Brushes.Yellow, new DirectCoordinate[] { ful, fur, bur, bul }));
            decor.Add(new ConvexPolygon(Brushes.Magenta, new DirectCoordinate[] { fdl, fdr, bdr, bdl }));
            decor.Add(new ConvexPolygon(Brushes.Green, new DirectCoordinate[] { fdl, ful, bul, bdl }));
            decor.Add(new ConvexPolygon(Brushes.Gray, new DirectCoordinate[] { fdr, fur, bur, bdr }));
        }

        #endregion
    }
}

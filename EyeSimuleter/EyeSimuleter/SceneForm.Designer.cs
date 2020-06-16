namespace EyeSimuleter
{
    partial class SceneForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.sceneBox = new System.Windows.Forms.PictureBox();
            this.graphicTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sceneBox)).BeginInit();
            this.SuspendLayout();
            // 
            // sceneBox
            // 
            this.sceneBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sceneBox.Location = new System.Drawing.Point(0, 0);
            this.sceneBox.Name = "sceneBox";
            this.sceneBox.Size = new System.Drawing.Size(750, 450);
            this.sceneBox.TabIndex = 0;
            this.sceneBox.TabStop = false;
            this.sceneBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.sceneBox_MouseMove);
            this.sceneBox.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.sceneBox_MouseWheel);
            // 
            // graphicTimer
            // 
            this.graphicTimer.Enabled = true;
            this.graphicTimer.Interval = 66;
            this.graphicTimer.Tick += new System.EventHandler(this.graphicTimer_Tick);
            // 
            // SceneForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 450);
            this.Controls.Add(this.sceneBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SceneForm";
            this.Text = "Eye simulater";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SceneForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.SceneForm_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.sceneBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox sceneBox;
        private System.Windows.Forms.Timer graphicTimer;
    }
}


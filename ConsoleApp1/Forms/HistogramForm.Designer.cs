namespace ConsoleApp1.Forms
{
    partial class HistogramForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlChart;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlChart = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnlChart
            // 
            this.pnlChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChart.Location = new System.Drawing.Point(0, 0);
            this.pnlChart.Name = "pnlChart";
            this.pnlChart.Size = new System.Drawing.Size(784, 461);
            this.pnlChart.TabIndex = 0;
            this.pnlChart.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlChart_Paint);
            // 
            // HistogramForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.pnlChart);
            this.MinimumSize = new System.Drawing.Size(550, 350);
            this.Name = "HistogramForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Гистограмма распределения студентов — DecanatPRO";
            this.ResumeLayout(false);
        }
    }
}

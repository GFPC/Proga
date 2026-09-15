using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using BusinessLogic;

namespace ConsoleApp1.Forms
{
    public partial class HistogramForm : Form
    {
        private readonly Logic _logic;

        public HistogramForm(Logic logic)
        {
            _logic = logic;
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Resize += (s, e) => pnlChart.Invalidate();
        }

        private void pnlChart_Paint(object sender, PaintEventArgs e)
        {
            RenderHistogram(e.Graphics, pnlChart.ClientRectangle, _logic);
        }

        public static void RenderHistogram(Graphics g, Rectangle bounds, Logic logic)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            var dist = logic.GetSpecialityDistribution();
            int total = dist.Values.Sum();

            if (dist.Count == 0 || total == 0)
            {
                using var font = new Font("Segoe UI", 12F, FontStyle.Regular);
                using var brush = new SolidBrush(Color.Gray);
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                g.DrawString("Нет данных о студентах для построения гистограммы", font, brush, bounds, sf);
                return;
            }

            // Header
            using (var titleFont = new Font("Segoe UI", 13F, FontStyle.Bold))
            using (var titleBrush = new SolidBrush(Color.FromArgb(33, 37, 41)))
            {
                g.DrawString("Распределение студентов по направлениям подготовки", titleFont, titleBrush, bounds.X + 20, bounds.Y + 15);
            }

            using (var subFont = new Font("Segoe UI", 9.5F, FontStyle.Regular))
            using (var subBrush = new SolidBrush(Color.FromArgb(108, 117, 125)))
            {
                g.DrawString($"Всего студентов: {total} | Направлений: {dist.Count}", subFont, subBrush, bounds.X + 20, bounds.Y + 42);
            }

            Color[] barColors = new[]
            {
                Color.FromArgb(66, 133, 244),   // Blue
                Color.FromArgb(52, 168, 83),   // Green
                Color.FromArgb(251, 188, 5),   // Yellow
                Color.FromArgb(234, 67, 53),   // Red
                Color.FromArgb(142, 68, 173),  // Purple
                Color.FromArgb(230, 126, 34),  // Orange
                Color.FromArgb(26, 188, 156),  // Teal
            };

            int startY = bounds.Y + 80;
            int maxCount = dist.Values.Max();
            int labelWidth = 260;
            int rightMargin = 100;
            int chartWidth = Math.Max(100, bounds.Width - bounds.X - labelWidth - rightMargin - 40);
            int availableHeight = bounds.Height - startY - 40;
            int barHeight = Math.Clamp(availableHeight / Math.Max(dist.Count, 1) - 15, 24, 45);

            int index = 0;
            using var labelFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            using var valueFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            using var labelBrush = new SolidBrush(Color.FromArgb(40, 40, 40));

            foreach (var kvp in dist)
            {
                int y = startY + index * (barHeight + 15);
                if (y + barHeight > bounds.Bottom - 10) break;

                string specName = kvp.Key;
                int count = kvp.Value;
                double pct = (double)count / total * 100.0;
                int barPixelWidth = (int)Math.Round((double)count / maxCount * chartWidth);
                if (barPixelWidth < 6) barPixelWidth = 6;

                // Draw Spec Name (truncated if too long)
                var labelRect = new RectangleF(bounds.X + 20, y + (barHeight - 20) / 2f, labelWidth - 10, 25);
                var sfLeft = new StringFormat { Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap };
                g.DrawString(specName, labelFont, labelBrush, labelRect, sfLeft);

                // Background bar track
                int barX = bounds.X + 20 + labelWidth;
                var trackRect = new Rectangle(barX, y, chartWidth, barHeight);
                using (var trackBrush = new SolidBrush(Color.FromArgb(240, 242, 245)))
                {
                    g.FillRectangle(trackBrush, trackRect);
                }

                // Filled Bar
                Color color = barColors[index % barColors.Length];
                var barRect = new Rectangle(barX, y, barPixelWidth, barHeight);
                using (var barBrush = new SolidBrush(color))
                {
                    g.FillRectangle(barBrush, barRect);
                }

                // Value label on right
                string countText = $"{count} чел. ({pct:F1}%)";
                using (var valBrush = new SolidBrush(Color.FromArgb(50, 50, 50)))
                {
                    g.DrawString(countText, valueFont, valBrush, barX + barPixelWidth + 10, y + (barHeight - 18) / 2f);
                }

                index++;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MartManagementSystem
{
    internal class Services
    {
        public static void ShowAlert(string msg)
        {
            Form alert = new Form();
            alert.StartPosition = FormStartPosition.Manual;
            alert.FormBorderStyle = FormBorderStyle.None;
            alert.BackColor = Color.DodgerBlue;
            alert.Size = new Size(300, 80);
            alert.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - 320, 20);
            alert.Padding = new Padding(5, 5, 5, 5);

            Label title = new Label();
            title.Text = "Information";
            title.Font = new Font("Arial", 10, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.Dock = DockStyle.Top;
            title.TextAlign = ContentAlignment.TopLeft;

            Label lbl = new Label();
            lbl.Text = msg;
            lbl.ForeColor = Color.White;
            lbl.Dock = DockStyle.Fill;
            lbl.TextAlign = ContentAlignment.MiddleCenter;

            alert.Controls.Add(title);
            alert.Controls.Add(lbl);
            alert.Show();

            Task.Delay(2000).ContinueWith(t => alert.Invoke(new Action(alert.Close)));
        }
    }
}

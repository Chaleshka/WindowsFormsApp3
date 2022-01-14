using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public class ChooseBox : Form
    {
        ListBox listBox;

        public ChooseBox(List<Object> list)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Size = new Size(300, 250);
            this.Text = "Удаление задачи";

            listBox = new ListBox();
            listBox.Size = new Size(250, 125);
            listBox.Font = new Font(TextBox.DefaultFont, FontStyle.Regular);
            listBox.Location = new Point(20, 50);
            listBox.ScrollAlwaysVisible = true;

            this.Controls.Add(listBox);

            listBox.Show();

            listBox.KeyPress += new KeyPressEventHandler(textBox_KeyPress);

            Label label = new Label();
            label.AutoSize = false;
            label.Size = new Size(250, 25);
            label.Font = new Font(label.Font, FontStyle.Regular);
            label.Location = new Point(20, 25);
            label.Text = "Выберите задачу для удаления: ";

            this.Controls.Add(label);

            label.Show();

            Button buttonOK = new Button();
            buttonOK.Size = new Size(80, 25);
            buttonOK.Location = new Point(105, 175);
            buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            buttonOK.Text = "OK";

            this.Controls.Add(buttonOK);

            buttonOK.Show();

            Button buttonCancel = new Button();
            buttonCancel.Size = new Size(80, 25);
            buttonCancel.Location = new Point(190, 175);
            buttonCancel.Text = "Cancel";

            this.Controls.Add(buttonCancel);

            buttonCancel.Show();

            buttonCancel.Click += new EventHandler(buttonCancel_Click);


            for (int i = 0; i < list.Count; i += 2)
                listBox.Items.Add(list[i]);
        }

        public void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)Keys.Enter)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

                this.Close();
            }
        }

        public void buttonCancel_Click(object sander, EventArgs e)
        {
            this.Close();
        }

        public int getIndex()
        {
            if (this.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return -1;

            return listBox.SelectedIndex;
        }
    }
}

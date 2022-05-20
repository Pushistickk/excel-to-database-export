using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace excel_to_database_exporter
{
	public partial class Form1 : Form
	{
		string filename;
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				DialogResult result = openFileDialog1.ShowDialog();

				if (result != DialogResult.OK)
					throw new Exception("Файл не выбран");

				filename = openFileDialog1.FileName;
				textBox1.Text = filename;
				listBox1.Items.Add(filename);
				button1_Click(sender, e);
			}
			catch(Exception exc)
			{
				MessageBox.Show(exc.Message,"Ошибка", MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}

        private void button6_Click(object sender, EventArgs e)
        {

        }
    }
}

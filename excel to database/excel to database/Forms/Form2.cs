using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace excel_to_database
{
	public partial class Form2 : Form
	{
		public Form2()
		{
			InitializeComponent();
		}

		private void Form2_Load(object sender, EventArgs e)
		{
			dataGridView1 = Program.problematickData;
			foreach(Region region in Program.database.Regions)
			{
				comboBox1.Items.Add(region.Regionname);
			}
			foreach(City city in Program.database.Cities)
			{
				comboBox2.Items.Add(city.Cityname);
			}
			comboBox3.Items.Add("электричество");
			comboBox3.Items.Add("горячая вода");
			comboBox3.Items.Add("холодная вода");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Form form = new Form1();
			this.Hide();
			form.ShowDialog();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (textBox1.Text == "" || textBox2.Text == ""||textBox3.Text ==""||textBox5.Text == ""|| textBox6.Text == ""|| textBox7.Text == ""|| comboBox1.SelectedIndex == -1|| comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
			{
				MessageBox.Show("Есть незаполненные данные");
				return;
			}
			try
			{
				Record record = new Record();
				record.Accountnumber = Convert.ToInt32(textBox1.Text);
				record.Fnp = textBox2.Text;
				record.Regionid = comboBox1.SelectedIndex;
				record.Cityid = comboBox2.SelectedIndex;
				record.Streetadress = textBox5.Text;
				record.Streetnumber = textBox6.Text;
				record.Apartmentnumber = textBox7.Text;
				record.Metertype = comboBox3.Items[comboBox3.SelectedIndex].ToString();
				record.Meterreading = Convert.ToInt32(textBox3.Text);
				Program.database.Records.Add(record);
				Program.database.SaveChanges();
			}
			catch(Exception exc)
			{
				MessageBox.Show("Ошибка", exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}

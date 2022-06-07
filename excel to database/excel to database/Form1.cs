using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ExcelDataReader;
namespace excel_to_database
{
	public partial class Form1 : Form
	{
		string filename;
		static string[] files;
		public Form1()
		{
			InitializeComponent();
			tabControl1.TabPages.Clear();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			openFileDialog1.Filter = "Excel files|*.xls;*.xlsx;*.xlsm;*.xlsb";
			openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			try
			{
				DialogResult result = openFileDialog1.ShowDialog();

				if (result != DialogResult.OK)
					throw new Exception("Файл не выбран");

				filename = openFileDialog1.FileName;
				listBox1.Items.Add(filename);
				excelToTabControl();
			}
			catch (Exception exc)
			{
				MessageBox.Show(exc.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void listBox1_DragDrop(object sender, DragEventArgs e)
		{

			listBox1.Items.Clear();
			listBox1.BackColor = Color.FromArgb(243, 243, 243);
			files = (string[])e.Data.GetData(DataFormats.FileDrop);
			listBox1.Items.AddRange(files);
			excelToTabControl();
		}

		private void listBox1_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				bool allow = true;
				files = (string[])e.Data.GetData(DataFormats.FileDrop);
				foreach (string file in files)
				{
					string ext = Path.GetExtension(file);
					if (ext != ".xls" && ext != ".xlsx" && ext != ".xlsm" && ext != ".xlsb")
					{
						allow = false;
						break;
					}
				}
				listBox1.BackColor = (allow ? Color.FromArgb(217, 234, 211) : Color.FromArgb(244, 204, 204));
				e.Effect = (allow ? DragDropEffects.Copy : DragDropEffects.None);
			}
		}

		private void listBox1_DragLeave(object sender, EventArgs e)
		{

			listBox1.BackColor = Color.FromArgb(243, 243, 243);
		}

		public static DataGridView addDatagridview(int index) //создает динамические datagridview с наполнением
		{
			DataGridView data = new DataGridView();
			data.AutoSize = true;
			data.Dock = DockStyle.Fill;
			data.Name = "dynDGR";
			data.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
			data.DataSource = GetExcelData(files[index]);

			return data;
		}

		public static DataTable GetExcelData(string path) //получает данные из excel таблицы 
		{
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			DataTable dt = new DataTable();
			FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read);
			IExcelDataReader reader = ExcelReaderFactory.CreateReader(fileStream);
			DataSet dataSet = reader.AsDataSet();
			dt = dataSet.Tables[0];
			fileStream.Close();
			return dt;
		}
		public void excelToTabControl()
		{
			if (listBox1.Items.Count == 0)
			{
				return;
			}
			tabControl1.TabPages.Clear();
			foreach (string file in files)//добавляет новые вкладки для всех файлов.
			{
				tabControl1.TabPages.Add(Path.GetFileName(file));
				tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(addDatagridview(tabControl1.TabPages.Count - 1));
			}
		}
	}
}

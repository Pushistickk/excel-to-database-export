using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
namespace excel_to_database
{
	internal class Recorder
	{
		List<City> cities = new List<City>();
		List<Region> regions = new List<Region>();
		List<User> users = new List<User>();
		public List<string> typeList = new List<string>();
		Regex trashCleaner = new Regex("\\b(?i)(?:Область|Обл|г|город|гор)\\b");
		public string[] scheme;
		public Recorder()
		{
			cities = Program.database.Cities.ToList<City>();
			regions = Program.database.Regions.ToList<Region>();
			users = Program.database.Users.ToList<User>();
		}
		public void BuildScheme(int index)
		{
			int mistakeCounter = 1;
			Regex regex;
			Regex meterTypeFinder = new Regex("(?i)(?:хв|гв|эл|хол|гор|холодная|горячая|электричество|холодная вода|горячая вода)");
			Regex punctRemover = new Regex("[^\\w\\s\\d]");
			Regex spaceRemover = new Regex("[\\s]");
			DataGridView dgv = new DataGridView();
			DataGridView dataGrid = Program.dataGrids[index];
			dgv = CopyDataGridView(dataGrid);
		mistakeData:
			typeList.Clear();
			scheme = new string[dgv.ColumnCount];
			foreach (DataGridViewCell cell in dgv.Rows[mistakeCounter].Cells)                //составление списка типов данных
			{
				DateTime dDate;
				CultureInfo info = new CultureInfo("ru-RU");
				if (DateTime.TryParseExact(cell.Value.ToString(), "dd.MM.yyyy", info, DateTimeStyles.None, out dDate))
				{
					cell.Value = dDate.ToString("dd.MM.yyyy");
					typeList.Add("date");
				}
				else
					typeList.Add(cell.Value.GetType().ToString().ToLower());
			}		//составление списка типов данных

			foreach (Region region in regions)											//поиск адреса
			{
				regex = new Regex(region.Regionname);
				for (int i = 0; i < dgv.Rows[mistakeCounter].Cells.Count; i++)
				{
					if (regex.IsMatch(dgv.Rows[mistakeCounter].Cells[i].Value.ToString()))
					{
						dgv.Rows[mistakeCounter].Cells[i].Value = regex.Replace(dgv.Rows[mistakeCounter].Cells[i].Value.ToString(), " ");
					cityNextCell:
						scheme[i] = "adress";
						dgv.Rows[mistakeCounter].Cells[i].Value = trashCleaner.Replace(dgv.Rows[mistakeCounter].Cells[i].Value.ToString(), "");
						dgv.Rows[mistakeCounter].Cells[i].Value = punctRemover.Replace(dgv.Rows[mistakeCounter].Cells[i].Value.ToString(), " ");
						if (spaceRemover.Replace(dgv.Rows[mistakeCounter].Cells[i].Value.ToString(), "") != "")
						{
							foreach (City city in cities)
							{
							cityCheck:
								regex = new Regex(city.Cityname);
								if (regex.IsMatch(dgv.Rows[mistakeCounter].Cells[i].Value.ToString()))
								{
									dgv.Rows[mistakeCounter].Cells[i].Value = regex.Replace(dgv.Rows[mistakeCounter].Cells[i].Value.ToString(), "");
								AdressCheck:
									if (spaceRemover.Replace(dgv.Rows[mistakeCounter].Cells[i].Value.ToString(), "") != "")
									{
										scheme[i] = "adress";
										regex = new Regex("[\\d]");
										if (!regex.IsMatch(dgv.Rows[4].Cells[i].Value.ToString()))
										{
											List<string> vs = new List<string>(dgv.Rows[mistakeCounter].Cells[i + 1].Value.ToString().Split(" "));
										check:
											for (int j = 0; j < vs.Count; j++)
											{
												if (vs[j] == "" || vs[j] == " ")
												{
													vs.RemoveAt(j);
													goto check;
												}
											}
											scheme[i + 1] = "adress";
											if (vs.Count == 1)
											{
												scheme[i + 2] = "adress";
											}
											else if (vs.Count == 2)
											{
												if (!regex.IsMatch(vs[1]))
												{
													scheme[i + 2] = "adress";
												}
											}
											goto adressFound;
										}
										else
										{
											scheme[i] = "adress";
											int c = 0;
											List<string> vs = new List<string>(dgv.Rows[mistakeCounter].Cells[i].Value.ToString().Split(" "));
											foreach (string str in vs)
											{
												if (regex.IsMatch(str))
												{
													c++;
												}
											}
											if (c != 2)
											{
												scheme[i + 1] = "adress";
											}
											goto adressFound;
										}
									}
									else
									{
										i++;
										goto AdressCheck;
									}
								}
								else
								{
									i++;
									goto cityCheck;
								}
							}
						}
						else
						{
							i++;
							goto cityNextCell;
						}
					}
				}
			}                                       //поиск адреса /adress/
		adressFound:
			for (int i = 0; i < typeList.Count; i++)                                    //поиск лицевого счета
			{
				if (typeList[i] == "system.double" && scheme[i] == null)
				{
					string fullAdressToCheck = "";
					for (int j = 0; j < scheme.Count(); j++)
					{
						if (scheme[j] == "adress")
							fullAdressToCheck += dataGrid.Rows[mistakeCounter].Cells[j].Value + " ";
					}
					fullAdressToCheck = trashCleaner.Replace(fullAdressToCheck, "");
					fullAdressToCheck = punctRemover.Replace(fullAdressToCheck, "");
					foreach (User user in users)
					{
						string fullAdress;
						fullAdress = user.Region + " " + user.City + " " + user.Streetadress + " " + user.Streetnumber + " " + user.Apartmentnumber;
						if (spaceRemover.Replace(fullAdress, "") == spaceRemover.Replace(fullAdressToCheck, ""))
						{
							if (Convert.ToInt32(dataGrid.Rows[mistakeCounter].Cells[i].Value) == user.Accountnumber)
							{
								scheme[i] = "accountNumber";
								goto accountNumberFound;
							}
						}
					}
				}
			}                                 //поиск лицевого счета /accountNumber/
		accountNumberFound:
			foreach (User user in users)												//поиск фио
			{
				for (int i = 0; i < scheme.Count(); i++)
				{
					if (dataGrid.Rows[mistakeCounter].Cells[i].Value.ToString() == user.Fnp)
					{
						scheme[i] = "fullname";
						goto fullnameFound;
					}
				}
			}                                             //поиск фио /fullname/
		fullnameFound:
			for (int i = 0; i < scheme.Count(); i++)							        //поиск периодов
			{
				int c = 0;
			check:
				if (typeList[i] == "date" && c == 0)
				{
					scheme[i] = "dateStart";
					i++;
					if (i >= scheme.Count())
						goto datesFound;
					c++;
					goto check;
				}
				else if (typeList[i] == "date" && c == 1)
				{
					scheme[i] = "dateEnd";
					goto datesFound;
				}
				else
				{
					i++;
					if (i >= scheme.Count())
						goto datesFound;
					goto check;
				}
			}									//поиск периодов /dateStart/ /dateEnd/
		datesFound:
			for (int i = 0; i < scheme.Count(); i++)									//поиск показаний
			{
				if (typeList[i] == "system.double" && scheme[i] == null)
				{
					scheme[i] = "meterReading";
					goto meterReadingFound;
				}
			}                                 //поиск показаний /meterReading/
		meterReadingFound:
			for (int i = 0; i < scheme.Count(); i++)                            //поиск типа счетчика
			{
				if (meterTypeFinder.IsMatch(dataGrid.Rows[mistakeCounter].Cells[i].Value.ToString()) && scheme[i] == null)
				{
					scheme[i] = "meterType";
				}
			}                                 //поиск типа счетчика /meterType/

			for (int i = 0; i < scheme.Count(); i++)									//поиск ошибок
			{
				if (scheme[i] == null)
				{
					if (mistakeCounter == dataGrid.Rows.Count-2)
					{
						throw new Exception("Проблемный файл. Программа не может определить где какие данные расположены");
					}
					mistakeCounter++;
					goto mistakeData;
				}
			}									//поиск ошибки

		}   //Создает схему данных, проверяя каждую ячейку
		public string GetFullScheme()
		{
			string fullScheme = "";
			foreach(string str in scheme)
			{
				fullScheme += str + " ";
			}
			return fullScheme.Remove(fullScheme.Length - 1);
		}   //получить всю схему данных
		private DataGridView CopyDataGridView(DataGridView dgv_org)
		{
			DataGridView dgv_copy = new DataGridView();
			try
			{
				if (dgv_copy.Columns.Count == 0)
				{
					foreach (DataGridViewColumn dgvc in dgv_org.Columns)
					{
						dgv_copy.Columns.Add(dgvc.Clone() as DataGridViewColumn);
					}
				}

				DataGridViewRow row = new DataGridViewRow();

				for (int i = 0; i < dgv_org.Rows.Count; i++)
				{
					row = (DataGridViewRow)dgv_org.Rows[i].Clone();
					int intColIndex = 0;
					foreach (DataGridViewCell cell in dgv_org.Rows[i].Cells)
					{
						row.Cells[intColIndex].Value = cell.Value;
						intColIndex++;
					}
					dgv_copy.Rows.Add(row);
				}
				dgv_copy.AllowUserToAddRows = false;
				dgv_copy.Refresh();

			}
			catch (Exception ex)
			{
				MessageBox.Show("Copy DataGridViw", ex.Message);
			}
			return dgv_copy;
		}   //функция для копирования одной таблицы в другую. через datacource не работает. я не знаю при этом почему. только так.
		public void RecordInfo(int index)   //запись в базу данных
		{
			for (int i = 0; i < Program.dataGrids[index].Rows.Count; i++) 
			{
				Record record = new Record();
				string fullAdress = "";
				for (int j = 0; j < Program.dataGrids[index].Columns.Count; j++) 			//составление данных для новой строки
				{
					if (scheme[j] == "accountNumber")
						record.Accountnumber = Convert.ToInt32(Program.dataGrids[index].Rows[i].Cells[j].Value);
					else if (scheme[j] == "fullname")
						record.Fnp = Program.dataGrids[index].Rows[i].Cells[j].Value.ToString();
					else if (scheme[j] == "meterReading")
						record.Meterreading = Convert.ToInt32(Program.dataGrids[index].Rows[i].Cells[j].Value);
					else if (scheme[j] == "meterType")
						record.Metertype = Program.dataGrids[index].Rows[i].Cells[j].Value.ToString();
					else if (scheme[j] == "dateStart")
					{
						DateTime dt;
						if (DateTime.TryParse(Program.dataGrids[index].Rows[i].Cells[j].Value?.ToString(), out dt))
						{
							record.Datestart = DateOnly.FromDateTime(dt);
						}
					}
					else if (scheme[j] == "dateEnd")
					{
						DateTime dt;
						if (DateTime.TryParse(Program.dataGrids[index].Rows[i].Cells[j].Value?.ToString(), out dt))
						{
							record.Dateend = DateOnly.FromDateTime(dt);
						}
					}
					else if (scheme[j] == "adress")
						fullAdress += Program.dataGrids[index].Rows[i].Cells[j].Value.ToString() + " ";
				}       //составление данных для новой строки
				fullAdress = fullAdress.Remove(fullAdress.Count() - 1);
				foreach(string str in scheme)
				{
					if (str == "dateEnd")
						goto dateEndFound;
				}
				DateOnly date = record.Datestart;
				date.AddMonths(1);
				record.Dateend = date;
			dateEndFound:
				Regex regex;															//разделение адреса по правильным переменным
				Regex punctRemover = new Regex("[^\\w\\s\\d]");
				fullAdress = punctRemover.Replace(fullAdress, " ");
				foreach (Region region in regions)
				{
					regex = new Regex(region.Regionname);
					if(regex.IsMatch(fullAdress))
					{
						record.Regionid = region.Id;
						fullAdress = regex.Replace(fullAdress, "");
						foreach (City city in cities)
						{
							regex = new Regex(city.Cityname);
							if(regex.IsMatch(fullAdress))
							{
								record.Cityid = city.Id;
								fullAdress = trashCleaner.Replace(fullAdress, "");
								fullAdress = regex.Replace(fullAdress, "");
								List<string> stings = new List<string>(fullAdress.Split(" "));
							check:
								for(int c = 0; c < stings.Count;c++)
								{
									if(stings[c] ==""||stings[c]==" ")
									{
										stings.RemoveAt(c);
										goto check;
									}
								}
								string streetAdress = "";
								regex = new Regex("\\d");
							check1:
								for (int c = 0; c < stings.Count; c++)
								{
									if (regex.IsMatch(stings[c]))
									{
										break;
									}
									streetAdress += stings[c] + " ";
									stings.RemoveAt(c);
									goto check1;
								}
								streetAdress = streetAdress.Remove(streetAdress.Count() - 1);
								record.Streetadress = streetAdress;
								if (stings.Count == 2)
								{
									record.Streetnumber = stings[0];
									record.Apartmentnumber = stings[1];
									goto adressDeciphered;
								}
								else if (stings.Count == 3)
								{
									record.Streetnumber = stings[0] + stings[1];
									record.Apartmentnumber = stings[2];
									goto adressDeciphered;
								}
								else if (stings.Count == 4)
								{
									record.Streetnumber = stings[0] + stings[1];
									record.Apartmentnumber = stings[2] + stings[3];
									goto adressDeciphered;
								}
								else
									throw new Exception("меньше двух или больше четырех оставшихся сущностей в номер дома/номер квартиры");
							}
						}
					}
				}
			adressDeciphered:
				Program.database.Records.Add(record);
			}
			Program.database.SaveChanges();
		}

	}
}

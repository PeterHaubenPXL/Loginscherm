using CreateSP.Properties;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Input;
//using static System.Net.Mime.MediaTypeNames;

namespace CreateSP
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}


		string DBNaam;

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			txtProcedure.Text = "/* ****Voorbeeld**** \n\nCREATE PROCEDURE [dbo].[S_XXXX]\r\nAS\r\nSET NOCOUNT ON;\r\n\r\nSELECT Top 1 Memodatum\r\nFROM Verrichtingen\r\nORDER BY MemoDatum desc\r\n\nVraag hiervoor hulp van IT'er\r\n\r\n--******************************************************--\n*/";
			txtProcedure.Height = 500;

			strComputerName = Environment.MachineName.ToString();

			cboDatabase_Vullen();

			if (!DAC.MyConnectionString.Contains("Master;") && DAC.MyConnectionString != null && DAC.MyConnectionString != "")
			{
				DBNaam = DAC.MyConnectionString.Substring(DAC.MyConnectionString.IndexOf("Catalog=") + 8);
				DBNaam = DBNaam.Substring(0, DBNaam.IndexOf(";Data Source="));
			}
			else
			{
				DBNaam = "";
			}

		}


		string MachineNaam, ServerNaam, UserNaam;

		private void cboDatabase_Vullen()
		{
			SqlConnection con;
			SqlCommand cmd;
			SqlDataReader dr;

			MachineNaam = Environment.MachineName;
			ServerNaam = "";
			UserNaam = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

			RegistryView registryView = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

			using (RegistryKey hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView))
			{
				RegistryKey instanceKey = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL", false);

				if (instanceKey != null)
				{
					if (instanceKey.ValueCount > 1)  //Meerdere Servers op deze machiene
					{
						//ToDo Hier kunnen we de Server kiezen
						int X = 0;
					}

					foreach (var instanceName in instanceKey.GetValueNames())
					{
						ServerNaam = MachineNaam + @"\" + instanceName;

						try
						{
							//https://www.c-sharpcorner.com/blogs/get-all-installed-sql-server-instances-on-local-machine-using-c-sharp

							if (DAC.MyConnectionString == null || DAC.MyConnectionString == "")
							{
								DAC.MyConnectionString = @"Integrated Security=SSPI;Persist Security Info=False;User ID=" + UserNaam + ";Initial Catalog=Master;Data Source=" + ServerNaam;
								DBNaam = "";
							}
							else
							{
								if (DBNaam != null && DBNaam != "")
								{
									if (DBNaam.ToUpper() == "MASTER")
									{
										DBNaam = "";
									}
								}
								else
								{
									if ((DAC.MyConnectionString != null || DAC.MyConnectionString != "") && !DAC.MyConnectionString.Contains("Master"))
									{
										DBNaam = DAC.MyConnectionString.Substring(DAC.MyConnectionString.IndexOf("Catalog=") + 8);
										DBNaam = DBNaam.Substring(0, DBNaam.IndexOf(";Data Source="));
									}
									else
									{
										DBNaam = "";
									}
								}
							}

							con = new SqlConnection(DAC.MyConnectionString);  // new SqlConnection("Data Source=" + txtServer.Text + ";Database=Master;data source=.; uid=sa; pwd=Micr0s0ft;");
							con.Open();

							cmd = null;
							cmd = new SqlCommand("select distinct * from sysdatabases where dbid > 4 " +
												"Order By name", con);

							// DBID 1 tot 4 zijn standaard databases (eigen aan SQLSERVER)
							// die niet verwijdert mogen worden

							dr = cmd.ExecuteReader();

							cboDatabase.Items.Clear();

							cboDatabase.Items.Add("");

							while (dr.Read())
							{
								cboDatabase.Items.Add(dr[0]);
							}
							dr.Close();

							if (cboDatabase.Items.Count > 1)   // "> 1" want 1 = "" leeg item
							{
								if (Common.foundFilePath == null && (DBNaam == null || DBNaam == ""))
								{
									try
									{
										Task T = Task.Run(() =>
										{
											if (Common.foundFilePath == null)
											{
												Common.foundFilePath = FindFile(@"C:\Program Files\", "model_msdbdata.mdf");

												if (Common.foundFilePath == null)
												{
													Common.foundFilePath = FindFile(@"D:\Program Files\", "model_msdbdata.mdf");
												}

												if (Common.foundFilePath == null)
												{
													Common.foundFilePath = FindFile(@"D:\PF\", "model_msdbdata.mdf");
												}

												if (Common.foundFilePath == null)
												{
													Common.foundFilePath = FindFile(@"D:\", "model_msdbdata.mdf");
												}

												if (Common.foundFilePath == null)
												{
													Common.foundFilePath = FindFile(@"C:\", "model_msdbdata.mdf");
												}
											}
										});

									}
									catch (Exception ex)
									{
										MessageBox.Show(ex.Message.ToString(), "Error",
											MessageBoxButton.OK, MessageBoxImage.Error);
									}

									break;
								}
							}
							else
							{
								MessageBox.Show("Klik op 'Nieuwe Database aanmaken'\n\nOm je eerste database aan te maken" + "\n\nOf kies 'Restore database', om een backup terug te zetten", "Nieuwe database",
									MessageBoxButton.OK, MessageBoxImage.Information);
							}
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message.ToString(), "Error",
								MessageBoxButton.OK, MessageBoxImage.Error);
						}
					}
					return;
				}
				else   // InstanceKey == null // SQLSERVER nog niet geïnstalleerd
				{
					if (MessageBox.Show("SQL Server is niet geïnstalleerd op deze computer\n\n" +
						"Wil je SQL Server nu installeren?\n\nZie dat je een actieve internet verbinding hebt.", "SQL Server",
						MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
					{
						// Aanmaken SQL Server

						string fileName = "SQL2022-SSEI-Expr.exe";

						try
						{
							var process = new Process();
							process.StartInfo.UseShellExecute = true;
							process.StartInfo.FileName = fileName;
							process.Start();
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message, "Error",
								MessageBoxButton.OK, MessageBoxImage.Error);
							return;
						}
					}
				}
			}

		}


		private void cboSP_Vullen()
		{
			GenerateCollection();

		}


		private ObservableCollection<string> MijnSPCollectie;
		//private ObservableCollection <string> MijnSqlCollectie;


		private void GenerateCollection()
		{
			MijnSPCollectie = new ObservableCollection<string>();

			Q = "SELECT SCHEMA_NAME(schema_id) AS [Schema],Name\n" +
								"FROM sys.procedures\n" +
								"WHERE Name NOT LIKE 'sp_%'\n" +
								"ORDER BY Name";

			string mySQLQuery = Q;
			using (SqlConnection CN = new SqlConnection(DAC.MyConnectionString))
			{
				using (SqlCommand CMD = new SqlCommand(Q, CN))
				{
					CN.Open();
					using (SqlDataReader reader = CMD.ExecuteReader())
					{
						while (reader.Read())
						{
							MijnSPCollectie.Add((string)reader["Name"]);
						}
					}
				}
			}

			cboSP.ItemsSource = MijnSPCollectie;

		}


		private string FindFile(string directory, string fileName)
		{
			string Tijdelijk = null;
			try
			{
				Tijdelijk = Directory.GetFiles(directory, fileName).FirstOrDefault();

				if (!String.IsNullOrEmpty(Tijdelijk))
				{
					if (Tijdelijk.Contains("Binn"))
					{
						Tijdelijk = Tijdelijk.Substring(0, Tijdelijk.IndexOf("Binn"));
						Tijdelijk += @"Data\";
						Common.foundFilePath = Tijdelijk;
					}
				}
			}
			catch { } // The most likely exception is UnauthorizedAccessException
					  // and there is not much to do about that

			if (Common.foundFilePath != null && Common.foundFilePath.Length > 4)
			{
				return Common.foundFilePath;

			}
			return Tijdelijk;
		}


		private string FindFile2(string directory, string fileName)
		{
			string Tijdelijk = null;
			try
			{
				Tijdelijk = Directory.GetFiles(directory, fileName).FirstOrDefault();

				string[] allfiles = Directory.GetFiles(directory, fileName, SearchOption.AllDirectories);

				if (allfiles.Count() > 1)
				{
					MijnSPCollectie = new ObservableCollection<string>();

					cboSQL.Visibility = Visibility.Visible;
					txtZoek.Visibility = Visibility.Collapsed;
					btnReset.IsEnabled = true;

					foreach (string file in allfiles)
					{
						cboSQL.Items.Add(file);
					}
				}
				else
				{
					btnReset.IsEnabled = true;

					if (!String.IsNullOrEmpty(Tijdelijk))
					{
						return Tijdelijk;
					}
				}
			}
			catch (Exception ex)
			{
				//MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			} // The most likely exception is UnauthorizedAccessException
			  // and there is not much to do about that

			if (cboSQL.Items.Count == 0)
			{
				MessageBox.Show("Script niet gevonden", "Script niet gevonden", MessageBoxButton.OK,
					MessageBoxImage.Exclamation);
			}
			return "";

		}


		private void cboDatabase_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			//DBNaam = txtProcedure.Text;

			if (DBNaam != "")
			{
				DAC.MyConnectionString = @"Integrated Security=SSPI;Persist Security Info=False;User ID=" + UserNaam +
					";Initial Catalog=" + cboDatabase.Text + ";Data Source=" + ServerNaam;

				//DBNaam = cboDatabase.Text;

				if ((string)btnZoekScript.Content == "Herlaad script")
				{
					btnSave.IsEnabled = true;
				}

				btnBackup.IsEnabled = true;

				cboSP_Vullen();
			}
			else
			{
				//MessageBox.Show("Je hebt geen database gekozen", "Kies Database", MessageBoxButton.OK, MessageBoxImage.Error);

				btnSave.IsEnabled = false;

				return;
			}

		}

		private void cboDatabase_DropDownClosed(object sender, EventArgs e)
		{
			DBNaam = cboDatabase.Text;

			if (DBNaam != "")
			{
				DAC.MyConnectionString = @"Integrated Security=SSPI;Persist Security Info=False;User ID=" + UserNaam +
					";Initial Catalog=" + cboDatabase.Text + ";Data Source=" + ServerNaam;

				//DBNaam = cboDatabase.Text;

				if ((string)btnZoekScript.Content == "Herlaad script")
				{
					btnSave.IsEnabled = true;
				}

				btnBackup.IsEnabled = true;

				cboSP_Vullen();
			}
			else
			{
				MessageBox.Show("Je hebt geen database gekozen", "Kies Database", MessageBoxButton.OK, MessageBoxImage.Error);

				btnSave.IsEnabled = false;

				return;
			}

		}


		string ScriptGevonden = "";
		string ScriptFile = "";

		private void chkTekst_Click(object sender, RoutedEventArgs e)
		{
			if (chkTekst.IsChecked == true)
			{
				int X = 0;
			}
			else
			{
				btnZoekScript.Content = "Herlaad script";
				btnZoekScript_Click(null, null);
			}

		}

		private void cboSP_DropDownClosed(object sender, EventArgs e)
		{
			txtZoek.Text = cboSP.Text;

			if (txtZoek.Text != "")
			{
				if ((string)btnZoekScript.Content == "Zoek in script")
				{
					btnZoekScript_Click(null, null);
				}
			}

		}


		string Q = "";

		void Queries()
		{
			string Select, From, Order, Where, KortNaam;

			if (cboSP.Name == "Storedprocedures")
			{
				txtProcedure.Text = "SELECT SCHEMA_NAME(schema_id) AS [Schema],Name\n" +
								"FROM sys.procedures\n" +
								"WHERE Name NOT LIKE 'sp_%'\n" +
								"ORDER BY Name";
			}

		}

		private void cboSP_DropDownOpened(object sender, EventArgs e)
		{
			if (btnZoekScript.Content == "Herlaad script")
			{
				btnZoekScript_Click(null, null);
			}
		}

		string VerzamelMap;
		string strComputerName;

		private void btnBackup_Click(object sender, RoutedEventArgs e)
		{
			string x;

			string Map = "";
			DateTime tijdelijk = DateTime.Now;

			DirectoryInfo di;
			if (Directory.Exists(@"H:\Backup R\") == true)
			{
				VerzamelMap = @"H:\Backup R\";
			}
			else if (Directory.Exists(@"G:\Backup R\") == true)
			{
				VerzamelMap = @"G:\Backup R\";
			}
			else if (Directory.Exists(@"F:\Backup R\") == true)
			{
				VerzamelMap = @"F:\Backup R\";
			}
			else if (Directory.Exists(@"E:\Backup R\") == true)
			{
				VerzamelMap = @"E:\Backup R\";
			}
			else if (Directory.Exists(@"D:\Backup R\") == true)
			{
				VerzamelMap = @"D:\Backup R\";
			}
			else if (Directory.Exists(@"C:\Backup R\") == true)
			{
				VerzamelMap = @"C:\Backup R\";
			}
			else
			{
				VerzamelMap = "";
			}

			if (VerzamelMap == "")   // VerzamelMap is een Globale variabele in clsWinSelectieVM.cs
			{
				MessageBox.Show("Kies de verzamelmap, waar de backup moet opgeslagen worden,\n\t" +
							"de submappen worden automatisch aangemaakt\n\n" +
							@"bv E:\Backup R\ als verzamelmap", "Kies verzamelmap",
					MessageBoxButton.OK, MessageBoxImage.Information);

				// Folder picker (Using to gevoegd : using Microsoft.WindowsAPICodePack.Dialogs;

				CommonOpenFileDialog dialog = new CommonOpenFileDialog();
				dialog.IsFolderPicker = true;

				if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
				{
					VerzamelMap = dialog.FileName;

					di = Directory.CreateDirectory(VerzamelMap);
				}
				else
				{
					MessageBox.Show("Aanmaak en opslag van Backup file geannuleerd", "Geannuleerd",
						MessageBoxButton.OK, MessageBoxImage.Warning);

					Common.StatusDatabaseGewijzigd = false;
					return;
				}
			}
			else   // VerzamelMap != ""
			{
				if (MessageBox.Show("Wil je opslaan in?\n    " + VerzamelMap, "Opslaan in map",
						MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
				{
					prbScripting.Visibility = Visibility.Visible;
				}
				else
				{
					VerzamelMap = "";
					MessageBox.Show("Kies een andere verzamelmap, waar de Backup moet opgeslagen worden,\n\t" +
							"de submappen worden automatisch aangemaakt\n\n", "Verzamelmap",
						MessageBoxButton.OK, MessageBoxImage.Information);

					// Folder picker

					CommonOpenFileDialog dialog = new CommonOpenFileDialog();
					dialog.IsFolderPicker = true;

					if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
					{
						VerzamelMap = dialog.FileName;

						di = Directory.CreateDirectory(VerzamelMap);
					}
					else
					{
						MessageBox.Show("Aanmaak en opslag van Backup-file geannuleerd", "Geannuleerd",
							MessageBoxButton.OK, MessageBoxImage.Warning);

						//instSelectie.brdHighLight.Background = new SolidColorBrush(Colors.Transparent);

						if (Common.StatusBeveiliging == false)
						{
							//instSelectie.cboDatabase.SelectedIndex = 0;
							//CurrentUser.IsAuthenticated = false;
						}
						else
						{
							//CurrentUser.IsAuthenticated = true;
						}

						Common.StatusDatabaseGewijzigd = false;

						return;
					}
				}
			}

			if (VerzamelMap.Last() != '\\')
			{
				VerzamelMap = VerzamelMap + @"\";
			}
			Map = VerzamelMap;

			Directory.CreateDirectory(VerzamelMap);  // Maakt de map aan wanneer die nog niet bestaat

			prbScripting.Visibility = Visibility.Visible;

			string Backup_File = "";
			bool OK = false;

			//Dit is de Task die het werk doet
			Task T = Task.Run(() =>
			{
				Backup_File = CreateBackupAsync();

				if (query("RESTORE VERIFYONLY FROM DISK = '" + Backup_File + "'") == true)
				{
					OK = true;

					if (strComputerName.ToUpper() == "DESKTOP-F0HGDLQ")
					{
						OK = false;

						Backup_File = CreateBackupAsync(2);

						if (query("RESTORE VERIFYONLY FROM DISK = '" + Backup_File + "'") == true)
						{
							OK = true;
						}
					}
				}

			});

			do
			{
				DoEvents(); // Hier door zie je de Progress van de ProgressBar
			} while (T.IsCompleted == false);

			if (OK == true)
			{
				prbScripting.Visibility = Visibility.Collapsed;
				MessageBox.Show("Backup opgeslagen, en geverifieerd", "Backup",
					MessageBoxButton.OK, MessageBoxImage.Information);

				Common.StatusDatabaseGewijzigd = false;

				cboDatabase.Text = DBNaam;
			}
			else   // OK == false
			{
				prbScripting.Visibility = Visibility.Collapsed;
				MessageBox.Show("Backup is niet geverifieerd\n\nMaak een nieuwe Backup aan", "Backup mislukt",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}

			// Deze items maak ik hier opnieuw aan, omdat de items uit de task in een andere tread aangemaakt zijn

			if (Directory.Exists(Map)) // Als de drive niet aangesloten is, dan bestaat de Map ook niet
			{
				string con = DAC.MyConnectionString;
				string DBNaam = con.Substring(con.IndexOf("Catalog=") + 8); // database name  
				DBNaam = DBNaam.Substring(0, DBNaam.IndexOf(";Data Source="));
			}
			else
			{
				MessageBox.Show("Uw opslagmedium is niet aangesloten", "Opslagmedium",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}
		}


		SqlConnection con;
		SqlCommand cmd;


		public bool query(string que)
		{
			if (DAC.MyConnectionString != null)
			{
				con = new SqlConnection(DAC.MyConnectionString);
				con.Open();

				try
				{
					cmd = new SqlCommand(que, con);

					cmd.ExecuteNonQuery();

					return true;
				}
				catch (Exception)
				{
					if (que.Contains("RESTORE VERIFYONLY FROM DISK"))
					{
						MessageBox.Show("De gekozen Backup-file is corrupt\n\nKies een andere backup", "File is corrupt",
							MessageBoxButton.OK, MessageBoxImage.Warning);
					}
					return false;
				}
				finally
				{
					con.Close();
				}
			}
			else
			{
				return false;
			}

		}


		private string CreateBackupAsync(int Doorloop = 0)
		{
			SqlConnection con = new SqlConnection(DAC.MyConnectionString);

			string conString = DAC.MyConnectionString;
			string Map, MapOLD;

			DateTime tijdelijk = DateTime.Now;
			DirectoryInfo di;

			DBNaam = conString.Substring(conString.IndexOf("Catalog=") + 8);
			DBNaam = DBNaam.Substring(0, DBNaam.IndexOf(";Data Source="));

			if (Doorloop == 2)
			{
				Map = @"D:\Dropbox\Backup R\";
			}
			else
			{
				Map = VerzamelMap + tijdelijk.ToString(@"yyyy-MM-dd") + @"\";
			}

			MapOLD = Map + "OLD\\";

			di = Directory.CreateDirectory(Map);

			string BackupScript = @"BACKUP DATABASE [" + DBNaam + "] TO  DISK = N'" + Map + DBNaam + ".bak' WITH NOFORMAT, NOINIT,  NAME = N'" + DBNaam + "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

			if (File.Exists(Map + DBNaam + ".bak") == true)
			{
				if (Doorloop == 2)
				{
					di = Directory.CreateDirectory(MapOLD);

					if (File.Exists(MapOLD + DBNaam + "OLD.bak") == true)
					{
						// File deleten, anders wordt de file append
						File.Delete(MapOLD + DBNaam + "OLD.bak");
					}
					File.Copy(Map + DBNaam + ".bak", MapOLD + DBNaam + "OLD.bak");
				}

				// File deleten, anders wordt de file append
				File.Delete(Map + DBNaam + ".bak");
			}

			SqlCommand cmd = new SqlCommand(BackupScript, con);
			con.Open();
			int x = cmd.ExecuteNonQuery();
			con.Close();
			return Map + DBNaam + ".bak";

		}


		public static void DoEvents()
		{
			Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
															  new Action(delegate { }));
		}

		private void cboSQL_DropDownClosed(object sender, EventArgs e)
		{
			txtZoek.Text = cboSQL.Text;
			ScriptGevonden = txtZoek.Text;

			Mouse.OverrideCursor = Cursors.Wait;

			scriptTotaal = "";

			if (txtZoek.Text != "")
			{
				btnZoekScript.Content = "Open script";
				btnZoekScript_Click(null, null);

				cboSQL.Visibility = Visibility.Collapsed;
				txtZoek.Visibility = Visibility.Visible;
				txtZoek.Text = "";
			}
			chkTekst.IsEnabled = true;

		}


		string scriptTotaal = "";

		private void btnReset_Click(object sender, RoutedEventArgs e)
		{
			if (cboSQL.Items.Count > 1)
			{
				cboSQL.Visibility = Visibility.Visible;
				cboSQL.IsDropDownOpen = true;

				txtZoek.Visibility = Visibility.Collapsed;
				cboDatabase.Text = "";
			}

		}

		private void btnZoekScript_Click(object sender, RoutedEventArgs e)
		{
			if ((string)btnZoekScript.Content == "Zoek script")
			{
				ScriptGevonden = "";
				ScriptFile = txtZoek.Text;

				if (ScriptGevonden == "")
				{
					if (Debugger.IsAttached)
					{
						string temp = @"D:\Dropbox\DB_Scripts";
						ScriptGevonden = FindFile2(temp, ScriptFile);
					}
					else
					{
						string temp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
						//MessageBox.Show(temp,"Location Executable");
						ScriptGevonden = FindFile2(temp, ScriptFile);
					}
				}

				if (cboSQL.Items.Count > 1)
				{
					cboSQL.IsDropDownOpen = true;
					btnReset.IsEnabled = true;
				}
				else
				{
					btnReset.IsEnabled = false;
				}

				if (ScriptGevonden.Length > 4)
				{
					btnZoekScript.Content = "Open script";

					btnZoekScript_Click(null, null);
				}
			}
			else if ((string)btnZoekScript.Content == "Open script" || (string)btnZoekScript.Content == "Herlaad script")
			{
				bool OK = false;
				string Tekst="";

				if ((string)btnZoekScript.Content == "Herlaad script")
				{
					txtZoek.Text = "";
				}

				btnZoekScript.Content = "Script is aan het laden";

				

				Task T = Task.Run(() =>
				{
					Tekst = LaadScript();
				});

				do
				{
					//prbScripting.Visibility = Visibility.Visible;

					Mouse.OverrideCursor = Cursors.Wait;

					DoEvents(); // Hier door zie je de Progress van de ProgressBar
				} while (T.IsCompleted == false);

				Mouse.OverrideCursor = Cursors.Arrow;

				chkTekst.IsEnabled = true;

				txtProcedure.Text = Tekst;

				btnZoekScript.Content = "Open script";

				if ((string)btnZoekScript.Content == "Open script")
				{
					prbScripting.Visibility = Visibility.Collapsed;

					txtZoek.Text = "";

					if (cboDatabase.Text == "")
					{
						if (Tekst.ToUpper().Contains("CREATE DATABASE"))
						{
							Tekst = txtProcedure.Text.Substring(txtProcedure.Text.ToUpper().IndexOf("CREATE "));
							Tekst = Tekst.Substring(0, Tekst.IndexOf("]"));
							Tekst = Tekst.Substring(Tekst.IndexOf("[") + 1);

							DBNaam = Tekst;

							cboDatabase.Text = Tekst;
						}
					}
				}
				else
				{
					btnSave.IsEnabled = false;
				}

				btnZoekScript.Content = "Zoek in script";

				cboSP.IsEnabled = true;

				txtZoek.Focus();

				Mouse.OverrideCursor = Cursors.Arrow;
			}
			else if ((string)btnZoekScript.Content == "Zoek in script")
			{
				if (txtZoek.Text == "")
				{
					return;
				}

				if (chkTekst.IsChecked == false)
				{
					if (txtProcedure.Text.ToUpper().Contains("[" + txtZoek.Text.ToUpper() + "]"))
					{
						int pos = txtProcedure.Text.ToUpper().IndexOf("[" + txtZoek.Text.ToUpper() + "]");
						if (pos != -1)
						{
							string temp = txtProcedure.Text;
							temp = temp.Substring(txtProcedure.Text.ToUpper().IndexOf(txtZoek.Text.ToUpper()));
							temp = temp.Substring(temp.ToUpper().IndexOf("CREATE "));
							temp = temp.Substring(0, temp.IndexOf("*** --") + 6);

							txtProcedure.Text = temp;

							btnZoekScript.Content = "Herlaad script";

							btnSave.IsEnabled = true;
						}
					}
				}
				else   // chkTekst.IsChecked == true
				{
					if (txtProcedure.Text.ToUpper().Contains(txtZoek.Text.ToUpper()))
					{
						int pos = txtProcedure.Text.ToUpper().IndexOf(txtZoek.Text.ToUpper());
						if (pos != -1)
						{
							string temp = txtProcedure.Text;
							temp = temp.Substring(txtProcedure.Text.ToUpper().IndexOf(txtZoek.Text.ToUpper()));

							txtProcedure.Text = temp;

							txtProcedure.SelectionStart = 0;
							txtProcedure.SelectionLength = txtZoek.Text.Length;
							txtProcedure.Focus();

							btnZoekScript.Content = "Zoek volgende";
						}
					}
					else
					{
						MessageBox.Show("Geen resultaat voor opgegeven tekst", "Geen resultaat", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
					}
				}
			}
			else if ((string)btnZoekScript.Content == "Zoek volgende")
			{
				string temp = txtProcedure.Text.ToUpper().Substring(txtZoek.Text.ToUpper().Length);

				txtProcedure.Text = temp;

				if (txtProcedure.Text.ToUpper().Contains(txtZoek.Text.ToUpper()))
				{
					temp = temp.Substring(temp.ToUpper().IndexOf(txtZoek.Text.ToUpper()));

					txtProcedure.Text = temp;

					txtProcedure.SelectionStart = 0;
					txtProcedure.SelectionLength = txtZoek.Text.Length;
					txtProcedure.Focus();

					btnZoekScript.Content = "Zoek volgende";
				}
				else
				{
					MessageBox.Show("Tekst niet gevonden");

					btnZoekScript.Content = "Herlaad script";
				}
			}

			int x = 0;
		}


		private string LaadScript()
		{
			string Tekst = File.ReadAllText(ScriptGevonden);
			return Tekst;
		}



		private void btnSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				StringBuilder sbSP = new StringBuilder();

				sbSP.AppendLine(txtProcedure.Text);

				string temp = sbSP.ToString();

				if (temp != "")
				{
					if (temp.Contains("*/"))
					{
						temp = temp.Substring(temp.IndexOf("*/") + 2);
					}

					if ((!(temp.ToUpper().Contains("CREATE") || temp.ToUpper().Contains("ALTER")) || temp.Length < 15))
					{
						MessageBox.Show("Foutieve tekst", "Storedprocedure", MessageBoxButton.OK, MessageBoxImage.Error);
						return;
					}
					else
					{
						if (temp.ToUpper().Contains("CREATE"))
						{
							temp = temp.Substring(temp.ToUpper().IndexOf("CREATE"));
						}
						else if (temp.ToUpper().Contains("ALTER"))
						{
							temp = temp.Substring(temp.ToUpper().IndexOf("ALTER"));
						}

						if (temp.ToUpper().Contains("S_ADMINSECURITY") || (temp.ToUpper().Contains("LOGINS") &&
							(temp.ToUpper().Contains("DELETE") || temp.ToUpper().Contains("DROP") ||
							 temp.ToUpper().Contains("UPDATE"))))
						{
							MessageBox.Show("Verboden Query ingegeven", "Verboden Query");
							return;
						}
					}
				}
				else   // temp == ""
				{
					MessageBox.Show("Tekst is leeg", "Storedprocedure", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

				using (SqlConnection connection = new SqlConnection(DAC.MyConnectionString))
				{
					using (SqlCommand cmd = new SqlCommand(temp, connection))
					{
						connection.Open();
						cmd.CommandType = CommandType.Text;
						cmd.ExecuteNonQuery();
						connection.Close();
					}
				}
				MessageBox.Show("Storedprocedure opgeslagen", "Storedprocedure", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Fout opslag");
			}
		}

	}

}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using System.Xml.Linq;
using Microsoft.Win32;


namespace WpfOsztalyzas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fajlNev = "naplo.txt";
        //Így minden metódus fogja tudni használni.
        List<Osztalyzat> jegyek = new List<Osztalyzat>();

        public MainWindow()
        {
            InitializeComponent();
			// todo Fájlok kitallózásával tegye lehetővé a naplófájl kiválasztását!
			// Ha nem választ ki semmit, akkor "naplo.csv" legyen az állomány neve. A későbbiekben ebbe fog rögzíteni a program.

			var dialog = new Microsoft.Win32.OpenFileDialog();
			dialog.FileName = "naplo";
			dialog.DefaultExt = ".txt";
			dialog.Filter = "naplo.txt"; 

			bool? result = dialog.ShowDialog();

			if (result == true)
			{
				string filename = dialog.FileName;
			}



			// todo A kiválasztott naplót egyből töltse be és a tartalmát jelenítse meg a datagrid-ben!

			OpenFileDialog ofd = new OpenFileDialog();

			if ((bool)ofd.ShowDialog()! && ofd.FileName.EndsWith(".csv"))
			{
				fajlNev = ofd.FileName;
			}
			using (StreamReader sr = new StreamReader(fajlNev))
			{
				while (!sr.EndOfStream)
				{
					string[] currentSplit = sr.ReadLine()!.Split(";");
					jegyek.Add(new Osztalyzat(currentSplit[0],
						currentSplit[1],
						currentSplit[2],
						Convert.ToInt32(currentSplit[^1])));
				}
			}

			private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
			//todo Ne lehessen rögzíteni, ha a következők valamelyike nem teljesül!
			// a) - A név legalább két szóból álljon és szavanként minimum 3 karakterből!
			//      Szó = A szöközökkel határolt karaktersorozat.
			// b) - A beírt dátum újabb, mint a mai dátum

			string[] szetvalasztott = txtNev.Text.Split(" ");
				if (szetvalasztott.Count()==1)
				{
					MessageBox.Show("A név legalább 2 szóból álljon!", "NameError", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}

			if (DateTime.Compare(datDatum.SelectedDate!.Value,DateAndTime.Now)>0)
			{
					MessageBox.Show("A beírt dátum újabb, mint a mai dátum", "DateError", MessageBoxButton.OK, MessageBoxImage.Error);


			}
			

			//todo A rögzítés mindig az aktuálisan megnyitott naplófájlba történjen!

			string currentLogFilePath;
			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
			{
				currentLogFilePath = dialog.FileName;
				using (StreamWriter writer = new StreamWriter(currentLogFilePath, true))
				{
					writer.WriteLine();
				}
			}

			//A CSV szerkezetű fájlba kerülő sor előállítása
			string csvSor = $"{txtNev.Text};{datDatum.Text};{cboTantargy.Text};{sliJegy.Value}";
            //Megnyitás hozzáfűzéses írása (APPEND)
            StreamWriter sw = new StreamWriter(fajlNev, append: true);
            sw.WriteLine(csvSor);
            sw.Close();
			//todo Az újonnan felvitt jegy is jelenjen meg a datagrid-ben!

			jegyek.Add(new Osztalyzat(txtNev.Text, datDatum.Text, cboTantargy.Text, Convert.ToInt32(sliJegy.Value)));
				jegyek.txt.Text = $"Osztályzatok: {jegyek.Count()}, Átlag:{jegyek.Average(x => x.Jegy):.0}";

        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            jegyek.Clear();  //A lista előző tartalmát töröljük
            StreamReader sr = new StreamReader(fajlNev); //olvasásra nyitja az állományt
            while (!sr.EndOfStream) //amíg nem ér a fájl végére
            {
                string[] mezok = sr.ReadLine().Split(";"); //A beolvasott sort feltördeli mezőkre
                //A mezők értékeit felhasználva létrehoz egy objektumot
                Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3])); 
                jegyek.Add(ujJegy); //Az objektumot a lista végére helyezi
            }
            sr.Close(); //állomány lezárása

            //A Datagrid adatforrása a jegyek nevű lista lesz.
            //A lista objektumokat tartalmaz. Az objektumok lesznek a rács sorai.
            //Az objektum nyilvános tulajdonságai kerülnek be az oszlopokba.
            dgJegyek.ItemsSource = jegyek;
        }

        private void sliJegy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblJegy.Content = sliJegy.Value; //Több alternatíva van e helyett! Legjobb a Data Binding!
        }

		//todo Felület bővítése: Az XAML átszerkesztésével biztosítsa, hogy láthatóak legyenek a következők!
		// - A naplófájl neve
		// - A naplóban lévő jegyek száma
		// - Az átlag

		//todo Új elemek frissítése: Figyeljen rá, ha új jegyet rögzít, akkor frissítse a jegyek számát és az átlagot is!

		private void UpdateStats()
		{
			int numOfGrades = jegyek.Count;
			double avgGrade = jegyek.Average();

			lblJegy.Content = "Jegyek száma: " + numOfGrades;
			lblAtlag.Content = "Átlag: " + avgGrade.ToString("0.00");
		}





		//todo Helyezzen el alkalmas helyre 2 rádiónyomógombot!
		//Feliratok: [■] Vezetéknév->Keresztnév [O] Keresztnév->Vezetéknév
		//A táblázatban a név azserint szerepeljen, amit a rádiónyomógomb mutat!
		//A feladat megoldásához használja fel a ForditottNev metódust!
		//Módosíthatja az osztályban a Nev property hozzáférhetőségét!
		//Megjegyzés: Felételezzük, hogy csak 2 tagú nevek vannak

		private void rbFirstNameFirst_Checked(object sender, RoutedEventArgs e)
		{
			RefreshTable();
		}

		private void rbLastNameFirst_Checked(object sender, RoutedEventArgs e)
		{
			RefreshTable();
		}

		private void RefreshTable()
		{
			bool firstNameFirst = rbFirstNameFirst.IsChecked ?? false;
			bool lastNameFirst = rbLastNameFirst.IsChecked ?? false;

			if (firstNameFirst)
			{
				foreach (var person in Nev)
				{
					person.FullName = person.Vezeteknev + " " + person.Keresztnev;
				}
			}
			else if (lastNameFirst)
			{
				foreach (var person in Nev)
				{
					person.FullName = ForditottNev(person.Keresztnev, person.Vezeteknev);
				}
			}
		}
	}
}


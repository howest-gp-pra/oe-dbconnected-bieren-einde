using System;
using System.Collections.Generic;
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
using Pra.Bieren.CORE.Entities;
using Pra.Bieren.CORE.Services;

namespace Pra.Bieren.WPF
{

    public partial class winBieren : Window
    {
        bool isNew;
        BierService bierService;
        BierSoortService bierSoortService;
        public winBieren()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewDefault();
            PopulateCombobox();
            PopulateListbox();
        }
        private void ViewDefault()
        {
            grpBieren.IsEnabled = true;
            grpDetails.IsEnabled = false;
            btnSave.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Hidden;

        }
        private void ViewOperation()
        {
            grpBieren.IsEnabled = false;
            grpDetails.IsEnabled = true;
            btnSave.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Visible;
        }
        private void InitializeControls()
        {
            txtNaam.Text = "";
            txtAlcohol.Text = "";
            cmbBiersoort.SelectedIndex = 0;
            sldScore.Value = 1;
        }
        private void PopulateListbox()
        {
            bierService = new BierService();
            lstBieren.ItemsSource = bierService.Bieren;
        }
        private void PopulateCombobox()
        {
            bierSoortService = new BierSoortService();
            cmbBiersoort.ItemsSource = bierSoortService.BierSoorten;
        }
        private void lstBieren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeControls();
            if (lstBieren.SelectedItem == null) return;

            Bier bier = (Bier)lstBieren.SelectedItem;
            txtNaam.Text = bier.Naam;
            txtAlcohol.Text = bier.Alcohol.ToString("0.00");
            int indeks = 0;
            foreach(BierSoort bierSoort in cmbBiersoort.Items)
            {
                if(bierSoort.ID == bier.BierSoortID)
                {
                    cmbBiersoort.SelectedIndex = indeks;
                    break;
                }
                indeks++;
            }
            sldScore.Value = (double)bier.Score;

        }
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            ViewOperation();
            InitializeControls();
            txtNaam.Focus();
            isNew = true;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lstBieren.SelectedItem == null) return;
            ViewOperation();
            txtNaam.Focus();
            isNew = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lstBieren.SelectedItem == null) return;
            Bier bier = (Bier)lstBieren.SelectedItem;
            if(!bierService.DeleteBier(bier))
            {
                MessageBox.Show("Er heeft zich een fout voorgedaan", "Error");
            }
            else
            {
                lstBieren.ItemsSource = null;
                lstBieren.ItemsSource = bierService.Bieren;
                lstBieren.SelectedIndex = -1;
                InitializeControls();
            }



        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Bier bier;
            if (isNew)
            {
                bier = new Bier();
                try
                {
                    bier.Naam = txtNaam.Text.Trim();
                    bier.BierSoortID = ((BierSoort)cmbBiersoort.SelectedItem).ID;
                    double.TryParse(txtAlcohol.Text.Trim(), out double alcohol);
                    bier.Alcohol = alcohol;
                    bier.Score = (int) sldScore.Value;
                    if (!bierService.AddBier(bier))
                    {
                        MessageBox.Show("Er heeft zich een onverwachte fout voorgedaan", "ERROR");
                        return;
                    }
                }
                catch(Exception fout)
                {
                    MessageBox.Show(fout.Message, "ERROR");
                    return;
                }
            }
            else
            {
                bier = (Bier)lstBieren.SelectedItem;
                try
                {
                    bier.Naam = txtNaam.Text.Trim();
                    bier.BierSoortID = ((BierSoort)cmbBiersoort.SelectedItem).ID;
                    double.TryParse(txtAlcohol.Text.Trim(), out double alcohol);
                    bier.Alcohol = alcohol;
                    bier.Score = (int)sldScore.Value;
                    if(!bierService.EditBier(bier))
                    {
                        MessageBox.Show("Er heeft zich een onverwachte fout voorgedaan", "ERROR");
                        return;
                    }
                }
                catch (Exception fout)
                {
                    MessageBox.Show(fout.Message, "ERROR");
                    return;
                }
            }
            lstBieren.ItemsSource = null;
            lstBieren.ItemsSource = bierService.Bieren;
            lstBieren.SelectedItem = bier;
            ViewDefault();            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ViewDefault();
            lstBieren_SelectionChanged(lstBieren, null);
        }

        private void btnBiersoorten_Click(object sender, RoutedEventArgs e)
        {
            int indeks = cmbBiersoort.SelectedIndex;

            winBierSoorten winbiersoorten = new winBierSoorten();
            winbiersoorten.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            winbiersoorten.ShowDialog();

            PopulateCombobox();
            try
            {
                cmbBiersoort.SelectedIndex = indeks;
            }
            catch
            {
                cmbBiersoort.SelectedIndex = -1;
            }
        }


    }
}

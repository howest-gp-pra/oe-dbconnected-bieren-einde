using System;
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
    /// <summary>
    /// Interaction logic for winBierSoorten.xaml
    /// </summary>
    public partial class winBierSoorten : Window
    {
        public winBierSoorten()
        {
            InitializeComponent();
        }
        
        BierSoortService bierSoortService;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateListbox();
        }
        private void PopulateListbox()
        {
            bierSoortService = new BierSoortService();
            lstBiersoorten.ItemsSource = bierSoortService.BierSoorten;
        }
        private void lstBiersoorten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtNew.Text = "";
            txtEdit.Text = "";
            if (lstBiersoorten.SelectedItem == null) return;

            BierSoort bierSoort = (BierSoort)lstBiersoorten.SelectedItem;
            txtEdit.Text = bierSoort.Soort;
        }

        private void btnSaveNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BierSoort bierSoort = new BierSoort();
                bierSoort.Soort = txtNew.Text.Trim();
                if(!bierSoortService.AddBierSoort(bierSoort))
                    MessageBox.Show("Er heeft zich een onverwachte fout voorgedaan", "ERROR");
                else
                {
                    lstBiersoorten.ItemsSource = null;
                    lstBiersoorten.ItemsSource = bierSoortService.BierSoorten;
                    lstBiersoorten.SelectedItem = bierSoort;
                }
            }
            catch (Exception fout)
            {
                MessageBox.Show(fout.Message, "ERROR");
                return;
            }


        }

        private void btnSaveCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (lstBiersoorten.SelectedItem == null) return;
            try
            {
                BierSoort bierSoort = (BierSoort)lstBiersoorten.SelectedItem;
                bierSoort.Soort = txtEdit.Text.Trim();
                if (!bierSoortService.EditBierSoort(bierSoort))
                    MessageBox.Show("Er heeft zich een onverwachte fout voorgedaan", "ERROR");
                else
                {
                    lstBiersoorten.ItemsSource = null;
                    lstBiersoorten.ItemsSource = bierSoortService.BierSoorten;
                    lstBiersoorten.SelectedItem = bierSoort;
                }
            }
            catch (Exception fout)
            {
                MessageBox.Show(fout.Message, "ERROR");
                return;
            }
        }

        private void btnDeleteCurrent_Click(object sender, RoutedEventArgs e)
        {
            if (lstBiersoorten.SelectedItem == null) return;
            try
            {
                BierSoort bierSoort = (BierSoort)lstBiersoorten.SelectedItem;

                if (!bierSoortService.DeleteBierSoort(bierSoort))
                    MessageBox.Show("Er heeft zich een onverwachte fout voorgedaan", "ERROR");
                else
                {
                    lstBiersoorten.ItemsSource = null;
                    lstBiersoorten.ItemsSource = bierSoortService.BierSoorten;
                    txtEdit.Text = "";
                }
            }
            catch (Exception fout)
            {
                MessageBox.Show(fout.Message, "ERROR");
                return;
            }

        }


    }
}

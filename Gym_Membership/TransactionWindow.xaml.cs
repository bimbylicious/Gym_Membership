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
using System.Windows.Shapes;

namespace Gym_Membership
{
    /// <summary>
    /// Interaction logic for TransactionWindow.xaml
    /// </summary>
    public partial class TransactionWindow : Window
    {
        public TransactionWindow()
        {
            InitializeComponent();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void FinishTransaction_Click(object sender, RoutedEventArgs e)
        {
            CalculateTransaction();
        }

        private void daysBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            // Calculate total price and change when daysBox text changes
            CalculateTransaction();
        }
        private void customerMoneyBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CalculateTransaction();
        }
        private void CalculateTransaction()
        {
            if (int.TryParse(customerMoneyBox.Text, out int customerMoney) &&
                int.TryParse(daysBox.Text, out int daysToAdd))
            {
                int pricePerDay = 100;
                int totalPrice = daysToAdd * pricePerDay;
                int change = customerMoney - totalPrice;

                // Display calculated values
                totalPriceBox.Text = totalPrice.ToString();
                changeBox.Text = change.ToString();
            }
            //else
            //{
            //    totalPriceBox.Text = "";
            //    changeBox.Text = "";
            //    MessageBox.Show("Please enter valid numbers for Customer's money and Days to add.");
            //}
        }

        private void customerMoneyBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            CalculateTransaction();
        }

        private void daysBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            CalculateTransaction();
        }
    }
}

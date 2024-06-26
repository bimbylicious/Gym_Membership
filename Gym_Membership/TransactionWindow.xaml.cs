using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gym_Membership
{
    public partial class TransactionWindow : Window
    {
        // Connection string for your database
        private string connectionString = "Data Source=LAPTOP-9TASG5E3\\SQLEXPRESS;Initial Catalog=GymMembership;Integrated Security=True";

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
            AddTransactionToDatabase();
        }

        private void AddTransactionToDatabase()
        {
            try
            {
                // Validate input
                if (!ValidateInput())
                {
                    MessageBox.Show("Please enter valid numbers for Customer's money and Days to add.");
                    return;
                }

                // Calculate total price and change
                int pricePerDay = 100;
                int totalPrice = int.Parse(totalPriceBox.Text);
                int change = int.Parse(changeBox.Text);

                // Get customer ID and membership length
                string customerID = (Application.Current.MainWindow as Menu)?.SelectedCustomer?.Customer_ID;
                string membershipLengthStr = (Application.Current.MainWindow as Menu)?.SelectedCustomer?.Membership_Length;

                // If membershipLengthStr is null or empty, default to current DateTime
                DateTime membershipEndDate = DateTime.Now;
                if (!string.IsNullOrEmpty(membershipLengthStr) && int.TryParse(membershipLengthStr, out int membershipLengthDays))
                {
                    // Parse membership length to days (integer)
                    membershipEndDate = DateTime.Now.AddDays(membershipLengthDays);
                }

                // Calculate membership added length (days to add)
                int membershipAddedLength = int.Parse(daysBox.Text);

                // Create connection and command objects
                string connectionString = "Data Source=LAPTOP-9TASG5E3\\SQLEXPRESS;Initial Catalog=GymMembership;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a command to get the next auto-increment value for Transaction_ID
                    string getMaxTransactionIdQuery = "SELECT ISNULL(MAX(Transaction_ID), 0) + 1 FROM Transactions";
                    SqlCommand getMaxTransactionIdCommand = new SqlCommand(getMaxTransactionIdQuery, connection);
                    int transactionID = Convert.ToInt32(getMaxTransactionIdCommand.ExecuteScalar());

                    // Create the insert query
                    string insertQuery = @"INSERT INTO Transactions (Transaction_ID, Customer_ID, Date, Membership_Added_Length, Price, Membership_Length) 
                                   VALUES (@TransactionID, @CustomerID, @Date, @MembershipAddedLength, @Price, @MembershipLength);";

                    // Create the command for the insert query
                    SqlCommand insertCommand = new SqlCommand(insertQuery, connection);

                    // Add parameters to the insert command
                    insertCommand.Parameters.AddWithValue("@TransactionID", transactionID);
                    insertCommand.Parameters.AddWithValue("@CustomerID", customerID);
                    insertCommand.Parameters.AddWithValue("@Date", DateTime.Now); // Always use current DateTime for transaction date
                    insertCommand.Parameters.AddWithValue("@MembershipAddedLength", membershipAddedLength);
                    insertCommand.Parameters.AddWithValue("@Price", totalPrice);
                    insertCommand.Parameters.AddWithValue("@MembershipLength", membershipLengthStr);

                    // Execute the insert command
                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show($"Transaction successfully added with ID: {transactionID}");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add transaction.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding transaction: {ex.Message}");
            }
        }




        private bool ValidateInput()
        {
            return int.TryParse(customerMoneyBox.Text, out int customerMoney) &&
                   int.TryParse(daysBox.Text, out int daysToAdd);
        }

        private void customerMoneyBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateTransaction();
        }

        private void daysBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CalculateTransaction();
        }

        private void customerMoneyBox_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            CalculateTransaction();
        }

        private void daysBox_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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
            else
            {
                totalPriceBox.Text = "";
                changeBox.Text = "";
            }
        }
    }
}
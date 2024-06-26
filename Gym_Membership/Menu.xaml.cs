using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Drawing;

namespace Gym_Membership
{
    public partial class Menu : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Customer> customers;
        private Customer selectedCustomer;
        private ObservableCollection<Customer> originalCustomers;
        private bool isAscendingOrder = true;
        public event PropertyChangedEventHandler PropertyChanged;

        private const string FolderName = "UploadedDocuments";
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private const string PictureFolderName = "Picture";
        private const string IdentificationCardFolderName = "Identification_Card";
        private string imagesFolderPath;
        private Document document;
        public class Document
        {
            public string DocumentName { get; set; }
            public string FilePath { get; set; }

            // Additional properties and methods as needed
        }
        public Menu(Document document)
        {
            InitializeComponent();
            this.document = document; // Initialize with passed Document object
            LoadData();
            DataContext = this;
            InitializeImageFolder();
            InitializeCamera(); // Initialize camera devices
        }

        private void InitializeCamera()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                MessageBox.Show("No video devices found");
                return;
            }

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
        }

        public ObservableCollection<Customer> Customers
        {
            get { return customers; }
            set
            {
                customers = value;
                OnPropertyChanged("Customers");
            }
        }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
                UpdateTextBoxes();
            }
        }

        private void LoadData()
        {
            try
            {
                string connectionString = "Data Source=LAPTOP-VQRQBKBN\\SQLEXPRESS;Initial Catalog=GymMembership;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
                string query = "SELECT * FROM Customer;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    Customers = new ObservableCollection<Customer>();

                    while (reader.Read())
                    {
                        Customer customer = new Customer
                        {
                            Customer_ID = reader["Customer_ID"].ToString(),
                            Customer_Name = reader["Customer_Name"].ToString(),
                            Transaction_ID = reader["Transaction_ID"].ToString(),
                            Status = reader["Status"].ToString(),
                            Staff_ID = reader["Staff_ID"].ToString(),
                            Membership_Length = reader["Membership_Length"].ToString()
                        };
                        customer.Staff_ID = MainWindow.loggedInStaffID;
                        customers.Add(customer);
                    }
                    customerList.ItemsSource = customers;
                    originalCustomers = new ObservableCollection<Customer>(customers);
                }
                SortCustomersByCustomerID();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void SortCustomersByCustomerID()
        {
            if (originalCustomers == null)
            {
                return;
            }

            var sortedCustomers = isAscendingOrder
                ? originalCustomers.OrderBy(cust => int.TryParse(cust.Customer_ID, out int id) ? id : int.MaxValue).ToList()
                : originalCustomers.OrderByDescending(cust => int.TryParse(cust.Customer_ID, out int id) ? id : int.MinValue).ToList();

            isAscendingOrder = !isAscendingOrder;

            customerList.ItemsSource = null;
            customerList.ItemsSource = sortedCustomers;
        }

        private void UpdateTextBoxes()
        {
            if (SelectedCustomer != null)
            {
                idBox.Text = SelectedCustomer.Customer_ID;
                nameBox.Text = SelectedCustomer.Customer_Name;
                statusBlock.Text = SelectedCustomer.Status;
                membershipBox.Text = SelectedCustomer.Membership_Length;
                staffIDBox.Text = SelectedCustomer.Staff_ID;

                // Load images for selected customer
                LoadCustomerImages(SelectedCustomer.Customer_ID);
            }
            else
            {
                ClearTextBoxes();
            }
        }

        private void ClearTextBoxes()
        {
            idBox.Text = string.Empty;
            nameBox.Text = string.Empty;
            statusBlock.Text = string.Empty;
            membershipBox.Text = string.Empty;
            staffIDBox.Text = string.Empty;

            // Clear images
            Picture.Source = null;
            IdentificationCard.Source = null;
        }

        private void LoadCustomerImages(string customerId)
        {
            // Construct paths for Picture and Identification_Card
            string picturePath = GetDocumentPath(customerId, PictureFolderName);
            string identificationCardPath = GetDocumentPath(customerId, IdentificationCardFolderName);

            // Load images
            Picture.Source = LoadImageFromPath(picturePath);
            IdentificationCard.Source = LoadImageFromPath(identificationCardPath);
        }

        private BitmapImage LoadImageFromPath(string path)
        {
            return !string.IsNullOrEmpty(path) ? new BitmapImage(new Uri(path, UriKind.Absolute)) : null;
        }


        private string GetDocumentPath(string customerId, string folderName)
        {
            string targetFolderPath = Path.Combine(imagesFolderPath, SelectedCustomer.Customer_ID, folderName);
            try
            {
                if (!Directory.Exists(targetFolderPath))
                {
                    Directory.CreateDirectory(targetFolderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating directory: {ex.Message}");
                // Handle the exception as needed
            }
            return targetFolderPath;
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Handle the new frame event
            // For example, you could display the frame in a PictureBox
        }

        private void CapturePictureButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCameraAndCapture("Picture");
        }

        private void CaptureIdentificationCardButton_Click(object sender, RoutedEventArgs e)
        {
            OpenCameraAndCapture("Identification_Card");
        }

        private void OpenCameraAndCapture(string documentType)
        {
            try
            {
                Camera cameraWindow = new Camera();
                if (cameraWindow.ShowDialog() == true)
                {
                    string capturedImagePath = cameraWindow.CapturedImagePath;
                    if (!string.IsNullOrEmpty(capturedImagePath))
                    {
                        // Save the captured image to the respective folder
                        string targetFolderPath = GetDocumentPath(documentType);
                        string targetFilePath = Path.Combine(targetFolderPath, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg");

                        // Copy the captured image to the target folder
                        File.Copy(capturedImagePath, targetFilePath, true);

                        // Update the database with the file path (assume Customer class and SQL connection is set up)
                        UpdateCustomerDocumentPath(documentType, targetFilePath);

                        // Reload the images in UI
                        LoadCustomerImages(SelectedCustomer.Customer_ID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error capturing image: {ex.Message}");
            }
        }

        private void UpdateCustomerDocumentPath(string documentType, string filePath)
        {
            // Update the Customer table in the database with the file path
            try
            {
                string updateQuery = $"UPDATE Customer SET {documentType} = @FilePath WHERE Customer_ID = @CustomerID";
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.GymMembershipConnectionString))
                {
                    SqlCommand command = new SqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@FilePath", filePath);
                    command.Parameters.AddWithValue("@CustomerID", SelectedCustomer.Customer_ID);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating database: {ex.Message}");
            }
        }

        private string GetDocumentPath(string folderName)
        {
            string targetFolderPath = Path.Combine(imagesFolderPath, SelectedCustomer.Customer_ID, folderName);
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }
            return targetFolderPath;
        }
       
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void customerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void signoutButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void transactionHistoryButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void customerSort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UploadPictureButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
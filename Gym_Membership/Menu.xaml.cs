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

namespace Gym_Membership
{
    public partial class Menu : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Customer> customers;
        private Customer selectedCustomer;
        private ObservableCollection<Customer> originalCustomers;
        private bool isAscendingOrder = true;
        public event PropertyChangedEventHandler PropertyChanged;

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        private const string FolderName = "UploadedDocuments";
        private const string PictureFolderName = "Picture";
        private const string IdentificationCardFolderName = "Identification_Card";
        private string imagesFolderPath;
        private Document document; // Ensure Document class is properly defined or imported

        public Menu(Document document)
        {
            InitializeComponent();
            this.document = document; // Initialize with passed Document object
            LoadData();
            DataContext = this;

            InitializeCamera(); // Initialize camera devices
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
                string connectionString = "Data Source=LAPTOP-9TASG5E3\\SQLEXPRESS;Initial Catalog=GymMembership;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";
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
            string targetFolderPath = Path.Combine(imagesFolderPath, customerId, folderName);
            if (!Directory.Exists(targetFolderPath))
            {
                Directory.CreateDirectory(targetFolderPath);
            }
            return targetFolderPath;
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

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Handle the new frame event
            // For example, you could display the frame in a PictureBox
        }

        private void CapturePicture_Click(object sender, RoutedEventArgs e)
        {
            CaptureImage("Picture");
        }

        private void CaptureIdentificationCard_Click(object sender, RoutedEventArgs e)
        {
            CaptureImage("Identification_Card");
        }

        private void CaptureImage(string documentType)
        {
            if (videoSource == null || !videoSource.IsRunning)
            {
                MessageBox.Show("Video source is not running");
                return;
            }

            videoSource.NewFrame += (sender, eventArgs) =>
            {
                Bitmap image = (Bitmap)eventArgs.Frame.Clone();

                // Proceed with saving the image or displaying it
                string customerId = SelectedCustomer?.Customer_ID;
                if (!string.IsNullOrEmpty(customerId))
                {
                    string folderName = documentType == "Picture" ? PictureFolderName : IdentificationCardFolderName;
                    string targetFolderPath = GetDocumentPath(customerId, folderName);
                    string targetFilePath = Path.Combine(targetFolderPath, $"{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg");

                    try
                    {
                        image.Save(targetFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        MessageBox.Show($"Image captured and saved: {targetFilePath}");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving image: {ex.Message}");
                    }
                }

                // Stop capturing after the image is saved
                videoSource.NewFrame -= VideoSource_NewFrame;
            };
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
    }
}

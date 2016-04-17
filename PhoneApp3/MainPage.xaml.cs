using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
//using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;
using System.IO;
using System.Collections;
using System.Text;

namespace PhoneApp3
{
  //  https://msdn.microsoft.com/en-us/library/cc265154(v=vs.95).aspx
    public partial class MainPage : PhoneApplicationPage
    {
        public IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            createDir();
            refresh();
        }
        public void createDir()
        {
            // Create three directories in the root.
            store.CreateDirectory("MyApp1");
            store.CreateDirectory("MyApp2");
            store.CreateDirectory("MyApp3");

            // Create a file in the root.
            IsolatedStorageFileStream rootFile = store.CreateFile("InTheRoot");
            rootFile.Close();
        }
        public void refresh()
        {
            ComboBoxMenu.Items.Clear();
            ComboBoxMenu.SelectedIndex = -1;
            // Gather file information
            string[] directoriesInTheRoot = store.GetDirectoryNames();
            string[] filesInTheRoot = store.GetFileNames();

            // List the directories in the root.
            ComboBoxMenu.Items.Add("Directories in root:");
            foreach (string dir in directoriesInTheRoot)
            {
                ComboBoxMenu.Items.Add(dir);
            }
            ComboBoxMenu.Items.Add(" ");

            // List files in the root.
            ComboBoxMenu.Items.Add("Files in the root:");
            foreach (string fileName in filesInTheRoot)
            {
                ComboBoxMenu.Items.Add(fileName);
            }
            ComboBoxMenu.Items.Add(" ");
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        { //Obtain a virtual store for application 
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            //Create new subdirectory 
            //fileStorage.CreateDirectory("textFiles");
            //Create a new StreamWriter, to write the file to the specified location. 
            StreamWriter fileWriter = new StreamWriter(new IsolatedStorageFileStream((string)ComboBoxMenu.SelectedItem, FileMode.OpenOrCreate, fileStorage));
            //Write the contents of our TextBox to the file. 
             fileWriter.WriteLine(textBox1.Text);
            //Close the StreamWriter. 
             fileWriter.Close();
        }
        private void GetButton_Click(object sender, RoutedEventArgs e) 
        { //Obtain a virtual store for application 
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            //Create a new StreamReader 
            StreamReader fileReader = null;  
            try 
            { 
                //Read the file from the specified location. 
                fileReader = new StreamReader(new IsolatedStorageFileStream((string)ComboBoxMenu.SelectedItem, FileMode.Open, fileStorage)); 
                //Read the contents of the file (the only line we created). 
                string textFile = fileReader.ReadLine();  
                //Write the contents of the file to the TextBlock on the page. 
                textBox1.Text = textFile; 
                fileReader.Close(); 
            } 
            catch 
            { 
                //If they click the view button first, we need to handle the fact that the file hasn't been created yet. 
                textBox1.Text = "Empty or bad file";
            }
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (store.FileExists((string)ComboBoxMenu.SelectedItem))
                {
                    store.DeleteFile((string)ComboBoxMenu.SelectedItem);
                    refresh();
                }
            }
            catch (IsolatedStorageException ex)
            {
                ComboBoxMenu.Items.Add(ex.Message);
                //sb.AppendLine(ex.Message);
            }

        }
        private void DeleteDirButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete a specific directory.
            
            try
            {                
                if (store.DirectoryExists((string)ComboBoxMenu.SelectedItem))
                {
                    store.DeleteDirectory((string)ComboBoxMenu.SelectedItem);
                    refresh();
                }
            }
            catch (IsolatedStorageException ex)
            {
                ComboBoxMenu.Items.Add(ex);
            }
        }
    }
}
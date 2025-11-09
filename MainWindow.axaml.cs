using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using AndroidDebloater.Components;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AndroidDebloater
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<AndroidPackage> _items;
        
        public MainWindow()
        {
            InitializeComponent();
            DebloatBtn.IsEnabled = false;
            CDebloatBtn.IsEnabled = false;
            mSelector.IsEnabled = false;
            ScriptPanel.IsVisible = false;
            CustomPanel.IsVisible = false;
            HelpPanel.IsVisible = false;
            EasyDebloatButton.IsEnabled = false;
            ProDebloatButton.IsEnabled = false;
            ClOutputBox.IsVisible = false;
        }

        public void ListDevices(object sender, RoutedEventArgs args)
        {
            ClOutput.Text = ShellExecutor.ListADB();
            ConnectedDevicesLabel.Content = ShellExecutor.ListADB();
            // Regular expression to match the exact word "device"
            string pattern = @"\bdevice\b";

            // Match only lines with the exact word "device"
            foreach (string line in ClOutput.Text.Split('\n'))
            {
                if (Regex.IsMatch(line.Trim(), pattern))
                {
                    Console.WriteLine($"Matched: {line.Trim()}");
                    DebloatBtn.IsEnabled = true;
                    CDebloatBtn.IsEnabled = true;
                    EasyDebloatButton.IsEnabled = true;
                    ProDebloatButton.IsEnabled = true;
                    //cSelector.IsEnabled = true;
                    //sSelector.IsEnabled = true;
                    //ScriptPanel.IsVisible = true;
                }
            }
        }

        public void StartDebloater(object sender, RoutedEventArgs args)
        {
            
            if ((bool)gDebloat.IsChecked)
            {
                ClOutput.Text = ShellExecutor.StartDebloat(1);
            }else if ((bool)aDebloat.IsChecked)
            {
                ClOutput.Text = ShellExecutor.StartDebloat(2);
            }else if ((bool)tpDebloat.IsChecked)
            {
                ClOutput.Text = ShellExecutor.StartDebloat(3);
            }
            else
            {
                //Manufacturer Debloat
                int selectedIndex = mSelector.SelectedIndex;

                switch (selectedIndex)
                {
                    case 0:
                        //Google
                        ClOutput.Text = ShellExecutor.StartDebloat(4);
                        break;
                    case 1:
                        //Huawei
                        ClOutput.Text = ShellExecutor.StartDebloat(5);
                        break;
                    case 2:
                        //Oneplus
                        ClOutput.Text = ShellExecutor.StartDebloat(6);
                        break;
                    case 3:
                        //Oppo
                        ClOutput.Text = ShellExecutor.StartDebloat(7);
                        break;
                    case 4:
                        //Realme
                        ClOutput.Text = ShellExecutor.StartDebloat(8);
                        break;
                    case 5:
                        //Samsung
                        ClOutput.Text = ShellExecutor.StartDebloat(9);
                        break;
                    case 6:
                        //Vivo
                        ClOutput.Text = ShellExecutor.StartDebloat(10);
                        break;
                    case 7:
                        //Xiaomi
                        ClOutput.Text = ShellExecutor.StartDebloat(11);
                        break;
                }
            }
        }

        public void EnableSelector(object sender, RoutedEventArgs args)
        {
            mSelector.IsEnabled = true;
        }

        public void DisableSelector(object sender, RoutedEventArgs args)
        {
            mSelector.IsEnabled = false;
        }

        public void ShowScripts(object sender, RoutedEventArgs args)
        {
            ScriptPanel.IsVisible = true;
            CustomPanel.IsVisible = false;
            ListPanel.IsVisible = false;
            ClOutputBox.IsVisible = true;
            HelpPanel.IsVisible = false;
        }

        public void ShowDeviceList(object sender, RoutedEventArgs args)
        {
            ScriptPanel.IsVisible = false;
            CustomPanel.IsVisible = false;
            ListPanel.IsVisible = true;
            ClOutputBox.IsVisible = false;
            HelpPanel.IsVisible = false;
        }

        public void ShowHelpPanel(object sender, RoutedEventArgs args)
        {
            ScriptPanel.IsVisible = false;
            CustomPanel.IsVisible = false;
            ListPanel.IsVisible = false;
            ClOutputBox.IsVisible = false;
            HelpPanel.IsVisible = true;
        }

        public void ShowCustomSelector(object sender, RoutedEventArgs args)
        {
            CustomPanel.IsVisible = true;
            ScriptPanel.IsVisible = false;
            ListPanel.IsVisible = false;
            ClOutputBox.IsVisible = true;
            HelpPanel.IsVisible = false;
            
            _items = new ObservableCollection<AndroidPackage>(CreateObservableCollection(ShellExecutor.GetPackages()));

            // Get the ItemsControl by name and set its ItemsSource
            var packageControl = this.FindControl<ItemsControl>("PackageList");
            packageControl.ItemsSource = _items;
        }
        
        private void RemoveSelected(object sender, RoutedEventArgs e)
        {
            var selectedItems = new List<string>();
            foreach (var item in _items)
            {
                if (item.IsChecked)
                {
                    selectedItems.Add(item.Text);
                }
            }
            
            ClOutput.Text = "Uninstalling " + selectedItems.Count + " packages... \n";
            
            foreach (var item in selectedItems)
            {
                ClOutput.Text += item + ": " +ShellExecutor.RemovePackage(item);
            }
        }
        
        public ObservableCollection<AndroidPackage> CreateObservableCollection(string input)
        {
            var collection = new ObservableCollection<AndroidPackage>();

            // Split the input into lines
            var lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                // Remove the "package:" prefix and add to the collection
                var cleanedLine = line.Replace("package:", "").Trim();
                collection.Add(new AndroidPackage { Text = cleanedLine, IsChecked = false });
            }

            return collection;
        }
    }
}
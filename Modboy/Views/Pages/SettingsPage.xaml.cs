// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <SettingsPage.xaml.cs>
//  Created By: Alexey Golub
//  Date: 24/02/2016
// ------------------------------------------------------------------ 

using System.Windows;
using System.Windows.Controls;
using Modboy.Models.API;

namespace Modboy.Views.Pages
{
    public partial class SettingsPage
    {
        public AuthorizationInfo AuthorizationInfo { get; } = new AuthorizationInfo();

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void UsernameChanged(object sender, TextChangedEventArgs e)
        {
            AuthorizationInfo.Username = UsernameTextBox.Text;
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            AuthorizationInfo.Password = PasswordTextBox.Password;
        }

        private void btnResetCredentials_Click(object sender, RoutedEventArgs e)
        {
            PasswordTextBox.Clear();
        }

        private void AliasDataGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            var grid = (DataGrid) sender;
            grid.CommitEdit(DataGridEditingUnit.Row, true);
        }
    }
}
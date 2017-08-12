// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <UpdaterWindow.xaml.cs>
//  Created By: Alexey Golub
//  Date: 24/02/2016
// ------------------------------------------------------------------ 

using System.Windows;

namespace Modboy.Views
{
    public partial class UpdaterWindow
    {
        public UpdaterWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }
    }
}
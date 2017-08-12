// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <AboutWindow.xaml.cs>
//  Created By: Alexey Golub
//  Date: 25/02/2016
// ------------------------------------------------------------------ 

using System.Windows;

namespace Modboy.Views
{
    public partial class AboutWindow
    {
        public AboutWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }
    }
}
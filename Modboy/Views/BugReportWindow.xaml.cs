// ------------------------------------------------------------------ 
//  Solution: <GameBananaClient>
//  Project: <Modboy>
//  File: <BugReportWindow.xaml.cs>
//  Created By: Alexey Golub
//  Date: 28/06/2016
// ------------------------------------------------------------------ 

using System.Windows;

namespace Modboy.Views
{
    public partial class BugReportWindow
    {
        public BugReportWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
        }
    }
}
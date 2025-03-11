using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Diplom.ViewModels;
using Diplom.Views;


namespace Diplom
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        Navigation mainVM = new Navigation();
        protected override void OnStartup(StartupEventArgs e)
        {
            // при запуске
            base.OnStartup(e);
            new MainWindow() { DataContext = mainVM }.Show();
        }
    }
}

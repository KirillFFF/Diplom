using ProductsClient.Views;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Threading;

namespace ProductsClient
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            switch (e.Exception)
            {
                case HttpRequestException ex when ex.Equals(e.Exception):
                    {
                        switch (ex.Message)
                        {
                            case string line when line.Contains($"{HttpStatusCode.Unauthorized}"):
                                {
                                    MessageBox.Show("Произошла ошибка авторизации.\nВойдите в аккаунт ещё раз.", Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                    Process.Start(ResourceAssembly.Location);
                                    Current.Shutdown();
                                    break;
                                }
                            default:
                                {
                                    MessageBox.Show($"Возникла ошибка при выполнении запроса.\nПодробнее:\n{ex.Message}", Current.Windows.OfType<MainWindow>().First().Title, MessageBoxButton.OK, MessageBoxImage.Warning);
                                    break;
                                }
                        }

                        break;
                    }
                case Exception ex when ex.Equals(e.Exception):
                    {
                        MessageBox.Show($"А у нас тут неожиданная ошибочка вышла :(\n{ex}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
            }

            e.Handled = true;
        }
    }
}

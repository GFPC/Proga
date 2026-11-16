using System;
using System.Windows.Forms;
using ConsoleApp1.Presenters;

namespace ConsoleApp1
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var presenter = new Presenter();
            presenter.Run();
        }
    }
}

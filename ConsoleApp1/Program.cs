using System;
using System.Linq;
using System.Windows.Forms;
using BusinessLogic;
using ConsoleApp1.Cli;
using ConsoleApp1.Forms;
using DataAccessLayer.Dapper;
using DataAccessLayer.EF;
using Model;

namespace ConsoleApp1
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                // Выбор сценария работы с БД: через EF или через Dapper
                bool useDapper = args.Any(a => string.Equals(a, "--dapper", StringComparison.OrdinalIgnoreCase));

                Logic logic;
                if (useDapper)
                {
                    logic = new Logic(new DapperRepository<Student>());
                }
                else
                {
                    logic = new Logic(new EntityRepository<Student>());
                }

                logic.SeedInitialData();

                bool isCli = args.Any(a => string.Equals(a, "--cli", StringComparison.OrdinalIgnoreCase));

                if (isCli)
                    CliView.Run(logic);
                else
                    RunGuiMode(logic);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка приложения:\n{ex.Message}\n\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void RunGuiMode(Logic logic)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(logic));
        }
    }
}
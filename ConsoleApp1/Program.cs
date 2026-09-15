using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Win32.SafeHandles;
using BusinessLogic;
using ConsoleApp1.Cli;
using ConsoleApp1.Forms;

namespace ConsoleApp1
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool FreeConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        private const int ATTACH_PARENT_PROCESS = -1;
        private const int STD_INPUT_HANDLE = -10;
        private const int STD_OUTPUT_HANDLE = -11;
        private const int STD_ERROR_HANDLE = -12;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                var logic = new Logic();
                logic.SeedInitialData();

                bool isCli = args.Any(a => string.Equals(a, "--cli", StringComparison.OrdinalIgnoreCase));

                if (isCli)
                {
                    RunCliMode(logic);
                }
                else
                {
                    RunGuiMode(logic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка приложения:\n{ex.Message}\n\n{ex.StackTrace}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void RunCliMode(Logic logic)
        {
            CliView.Run(logic);
        }

        private static void RunGuiMode(Logic logic)
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                try { FreeConsole(); } catch { }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm(logic));
        }
    }
}

using System;
using System.Windows.Forms;

namespace TrelloSys
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // SỬA DÒNG NÀY:
            // Đổi từ new CardDetailForm() thành new MainForm()
            Application.Run(new MainForm());
        }
    }
}
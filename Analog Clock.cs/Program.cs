// This module's imports.
using System;
using System.Windows.Forms;

//This namespace contains this program's core class.
namespace Analog_Clock.cs
{
    //This class contains this program's core procedures.
    static class CoreClass
    {
        //This procedure is executed when this program is executed.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new InterfaceWindow());
        }
    }
}

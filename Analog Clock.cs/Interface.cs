//This module's imports.
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

//This namespace contains this program's main interface window class.
namespace Analog_Clock.cs
{
    //This class contains this program's main interface window.
    public partial class InterfaceWindow : Form
    {
        private AnalogClockControl AnalogClock = new AnalogClockControl();                                                       // Contains a reference to an instance of the analog clock control.
        private FileVersionInfo ProgramInformation = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);   // Contains this program's information.

        //This procedure initializes this windows.
        public InterfaceWindow()
        {
            InitializeComponent();

            this.InformationMenu.Click += InformationMenu_Click;
            this.QuitMenu.Click += QuitMenu_Click;

            AnalogClock.HandleError += HandleError;
            this.Text = $"{ProgramInformation.ProductName} v{ProgramInformation.FileVersion} - by: {ProgramInformation.CompanyName}";

            this.Controls.Add(AnalogClock);
            AnalogClock.Top = MenuBar.Height;

            this.ClientSize = new Size(AnalogClock.Width, AnalogClock.Height + this.MenuBar.Height);
        }

        //This procedure handles any errors that occur.
        private void HandleError(Exception ExceptionO)
        {
            try
            {
                MessageBox.Show(ExceptionO.Message, ProgramInformation.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch
            {
                Environment.Exit(0);
            }
        }


        //This procedure displays information about this program.
        private void InformationMenu_Click(Object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(ProgramInformation.Comments, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }
        }

        //This procedure closes this window.
        private void QuitMenu_Click(Object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ExceptionO)
            {
                HandleError(ExceptionO);
            }
        }
    }
}
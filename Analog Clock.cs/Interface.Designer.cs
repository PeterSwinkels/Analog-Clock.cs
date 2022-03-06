
namespace Analog_Clock.cs
{
    partial class InterfaceWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InterfaceWindow));
            this.MenuBar = new System.Windows.Forms.MenuStrip();
            this.ProgramMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.InformationMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.QuitMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuBar
            // 
            this.MenuBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ProgramMainMenu});
            this.MenuBar.Location = new System.Drawing.Point(0, 0);
            this.MenuBar.Name = "MenuBar";
            this.MenuBar.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.MenuBar.Size = new System.Drawing.Size(227, 24);
            this.MenuBar.TabIndex = 1;
            this.MenuBar.Text = "MenuBar";
            // 
            // ProgramMainMenu
            // 
            this.ProgramMainMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InformationMenu,
            this.QuitMenu});
            this.ProgramMainMenu.Name = "ProgramMainMenu";
            this.ProgramMainMenu.Size = new System.Drawing.Size(65, 20);
            this.ProgramMainMenu.Text = "&Program";
            // 
            // InformationMenu
            // 
            this.InformationMenu.Name = "InformationMenu";
            this.InformationMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.InformationMenu.Size = new System.Drawing.Size(174, 22);
            this.InformationMenu.Text = "&Information";
            // 
            // QuitMenu
            // 
            this.QuitMenu.Name = "QuitMenu";
            this.QuitMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.QuitMenu.Size = new System.Drawing.Size(174, 22);
            this.QuitMenu.Text = "&Quit";
            // 
            // InterfaceWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(227, 197);
            this.Controls.Add(this.MenuBar);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "InterfaceWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MenuBar.ResumeLayout(false);
            this.MenuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.MenuStrip MenuBar;
        internal System.Windows.Forms.ToolStripMenuItem ProgramMainMenu;
        internal System.Windows.Forms.ToolStripMenuItem InformationMenu;
        internal System.Windows.Forms.ToolStripMenuItem QuitMenu;
    }
}


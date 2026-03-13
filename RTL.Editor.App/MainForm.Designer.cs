namespace RTL.Editor.App;

partial class MainForm
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        menuStrip1 = new MenuStrip();
        fileToolStripMenuItem = new ToolStripMenuItem();
        loadSampleToolStripMenuItem = new ToolStripMenuItem();
        getHtmlToolStripMenuItem = new ToolStripMenuItem();
        openFileToolStripMenuItem = new ToolStripMenuItem();
        saveFileToolStripMenuItem = new ToolStripMenuItem();
        webView = new Microsoft.Web.WebView2.WinForms.WebView2();
        menuStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)webView).BeginInit();
        SuspendLayout();
        // 
        // menuStrip1
        // 
        menuStrip1.ImageScalingSize = new Size(24, 24);
        menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
        menuStrip1.Location = new Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Padding = new Padding(5, 2, 0, 2);
        menuStrip1.Size = new Size(765, 24);
        menuStrip1.TabIndex = 0;
        menuStrip1.Text = "menuStrip1";
        // 
        // fileToolStripMenuItem
        // 
        fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { loadSampleToolStripMenuItem, getHtmlToolStripMenuItem, openFileToolStripMenuItem, saveFileToolStripMenuItem });
        fileToolStripMenuItem.Name = "fileToolStripMenuItem";
        fileToolStripMenuItem.Size = new Size(12, 20);
        fileToolStripMenuItem.Text = "File";
        // 
        // loadSampleToolStripMenuItem
        // 
        loadSampleToolStripMenuItem.Name = "loadSampleToolStripMenuItem";
        loadSampleToolStripMenuItem.Size = new Size(180, 22);
        loadSampleToolStripMenuItem.Text = "Load sample";
        loadSampleToolStripMenuItem.Click += loadSampleToolStripMenuItem_Click;
        // 
        // getHtmlToolStripMenuItem
        // 
        getHtmlToolStripMenuItem.Name = "getHtmlToolStripMenuItem";
        getHtmlToolStripMenuItem.Size = new Size(180, 22);
        getHtmlToolStripMenuItem.Text = "Get HTML";
        getHtmlToolStripMenuItem.Click += getHtmlToolStripMenuItem_Click;
        // 
        // openFileToolStripMenuItem
        // 
        openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
        openFileToolStripMenuItem.Size = new Size(180, 22);
        openFileToolStripMenuItem.Text = "Open...";
        openFileToolStripMenuItem.Click += openFileToolStripMenuItem_Click;
        // 
        // saveFileToolStripMenuItem
        // 
        saveFileToolStripMenuItem.Name = "saveFileToolStripMenuItem";
        saveFileToolStripMenuItem.Size = new Size(180, 22);
        saveFileToolStripMenuItem.Text = "Save...";
        saveFileToolStripMenuItem.Click += saveFileToolStripMenuItem_Click;
        // 
        // webView
        // 
        webView.AllowExternalDrop = true;
        webView.CreationProperties = null;
        webView.DefaultBackgroundColor = Color.White;
        webView.Dock = DockStyle.Fill;
        webView.Location = new Point(0, 24);
        webView.Margin = new Padding(2, 2, 2, 2);
        webView.Name = "webView";
        webView.Size = new Size(765, 472);
        webView.TabIndex = 1;
        webView.ZoomFactor = 1D;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(765, 496);
        Controls.Add(webView);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Margin = new Padding(2, 2, 2, 2);
        Name = "MainForm";
        Text = "RTL WebView2 Editor";
        Load += MainForm_Load;
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)webView).EndInit();
        ResumeLayout(false);
        PerformLayout();

    }

    #endregion

    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loadSampleToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem getHtmlToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem saveFileToolStripMenuItem;
    private Microsoft.Web.WebView2.WinForms.WebView2 webView;
}
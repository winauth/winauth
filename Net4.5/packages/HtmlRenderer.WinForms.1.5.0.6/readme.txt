
********** Welcome to the HTML Renderer WinForms library! *****************************************

This library provides the rich formatting power of HTML in your WinForms .NET applications using
simple controls or static rendering code.
For more info see HTML Renderer on CodePlex: http://htmlrenderer.codeplex.com

********** DEMO APPLICATION ***********************************************************************

HTML Renderer Demo application showcases HTML Renderer capabilities, use it to explore and learn
on the library: http://htmlrenderer.codeplex.com/wikipage?title=Demo%20application

********** FEEDBACK / RELEASE NOTES ***************************************************************

If you have problems, wish to report a bug, or have a suggestion please start a thread on 
HTML Renderer discussions page: http://htmlrenderer.codeplex.com/discussions

For full release notes and all versions see: http://htmlrenderer.codeplex.com/releases

********** QUICK START ****************************************************************************

For more Quick Start see: https://htmlrenderer.codeplex.com/wikipage?title=Quick%20start

***************************************************************************************************
********** Quick Start: Use HTML panel control on WinForms form

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();

        TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel htmlPanel = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
        htmlPanel.Text = "<p><h1>Hello World</h1>This is html rendered text</p>";
        htmlPanel.Dock = DockStyle.Fill;
        Controls.Add(htmlPanel);
    }
}

***************************************************************************************************
********** Quick Start: Create image from HTML snippet

class Program
{
    private static void Main(string[] args)
    {
        Image image = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage("<p><h1>Hello World</h1>This is html rendered text</p>");
        image.Save("image.png", ImageFormat.Png);
    }
}

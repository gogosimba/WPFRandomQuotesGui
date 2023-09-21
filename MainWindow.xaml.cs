using ErrorReportsGui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ErrorReportsGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RandomQuoteGenerator r1 = new RandomQuoteGenerator();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            
            
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var randomquote = await Services.RandomQuoteGenerator.GetRandomQuote();
                quote_textbox.Text = randomquote.content;
                author_textbox.Text = randomquote.originator.name;
                var hyperlink = new Hyperlink(new Run(randomquote.originator.url));
                hyperlink.NavigateUri = new Uri(randomquote.originator.url);
                hyperlink.RequestNavigate += Hyperlink_RequestNavigate;
                url_textblock.Inlines.Clear();
                url_textblock.Inlines.Add(hyperlink);

            }
            catch (Exception ex)
            {
                quote_textbox.Text = ex.Message;
            }        
            
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            string navigateUri = hyperlink.NavigateUri.AbsoluteUri;

            try
            {
                // Use the correct command based on the operating system
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // For Windows
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = navigateUri,
                        UseShellExecute = true
                    });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    // For macOS
                    Process.Start("open", navigateUri);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    // For Linux
                    Process.Start("xdg-open", navigateUri);
                }
                else
                {
                    // Unsupported OS
                    throw new PlatformNotSupportedException("Unsupported operating system.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions here
                MessageBox.Show($"Error: {ex.Message}");
            }

            e.Handled = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

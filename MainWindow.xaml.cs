using ErrorReportsGui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

namespace ErrorReportsGui;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    private Dictionary<string, List<Models.RandomQuoteModel>> authorQuotes = new Dictionary<string, List<Models.RandomQuoteModel>>();
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
        await UpdateUIWithQuotes();
        var fadeInStoryboard = FindResource("FadeInStoryboard") as Storyboard;
        if (fadeInStoryboard != null)
        {
            fadeInStoryboard.Begin();
        }

    }

    private async Task UpdateUIWithQuotes()
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
            ComboBox1.Items.Add(randomquote.originator.name);
            // Add the quote to the author's list in the dictionary
            if (!authorQuotes.ContainsKey(randomquote.originator.name))
            {
                authorQuotes[randomquote.originator.name] = new List<Models.RandomQuoteModel>();
            }
            authorQuotes[randomquote.originator.name].Add(randomquote);
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
            
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = navigateUri,
                    UseShellExecute = true
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                
                Process.Start("open", navigateUri);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", navigateUri);
            }
            else
            {
                throw new PlatformNotSupportedException("Unsupported operating system.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }

        e.Handled = true;
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {

    }


    private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {
        // Get the selected author from the ComboBox
        string selectedAuthor = ComboBox1.SelectedItem as string;

        if (selectedAuthor != null && authorQuotes.ContainsKey(selectedAuthor))
        {
            // Display quotes for the selected author
            List<Models.RandomQuoteModel> quotes = authorQuotes[selectedAuthor];
            if (quotes.Count > 0)
            {
                int randomIndex = new Random().Next(0, quotes.Count);
                quote_textbox.Text = quotes[randomIndex].content;
            }
        }
        else
        {
            // Handle the case where no quotes are available for the selected author
            quote_textbox.Text = "No quotes available for this author.";
        }
    }

   
}

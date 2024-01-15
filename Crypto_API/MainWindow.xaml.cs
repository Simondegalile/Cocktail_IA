using Crypto_API.View;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Crypto_API
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Userdata user;
        public MainWindow()
        {
            InitializeComponent();
            user = JsonSerializer.Deserialize<Userdata>(File.ReadAllText("../../../Ressources/Userdata.json"));
        }

        private void cocktails_Click(object sender, RoutedEventArgs e)
        {
            //Page Cocktails
            Window_containers.Children.Clear();
            User_cocktail user_Cocktail = new();
            Window_containers.Children.Add(user_Cocktail);
        }

        private void Logine_Click(object sender, RoutedEventArgs e)
        {
            //Page Ingredient 
            Window_containers.Children.Clear();
            User_ingredient user_Ingredients = new();
            Window_containers.Children.Add(user_Ingredients);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            if (Tb_Info_UserName.Text == user.UserName && PWB_Info_Password.Password.ToString() == user.PassWord)
            {
                BP_Login.IsEnabled = true;
                BP_cocktails.IsEnabled = true;
            }
        }
    }
}

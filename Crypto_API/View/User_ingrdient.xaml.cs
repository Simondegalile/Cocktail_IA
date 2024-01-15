using System.Windows;
using System.Windows.Controls;
using System.IO;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Crypto_API.View
{
    public class Userdata
    {
        //Afin de recupérer les getteurs et les setteurs, sans ca nous ne pouvons pas utiliser les valeurs de notre fichié json
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Drink { get; set; }
        public string Rating { get; set; }
    }
    
    public partial class User_ingredient : UserControl

    {
        Userdata user;
        public User_ingredient()
        {
            InitializeComponent();
            //Lecture de notre fichier Json
             user = JsonSerializer.Deserialize<Userdata>(File.ReadAllText("../../../Ressources/Userdata.json"));

            TB_UserEmail.Text = user.Email.ToString();
            TB_Name.Text = user.UserName.ToString();
        }

        public void Bp_Save_Click(object sender, RoutedEventArgs e)
        {
            user.UserName = TB_UserName.Text;
            user.Email = TB_Email.Text;
            user.PassWord = TB_PassWord.Password;

            string jsonData = JsonSerializer.Serialize(user);
            File.WriteAllText("../../../Ressources/Userdata.json", jsonData);
            MessageBox.Show("Data Save");

        }
    }
}

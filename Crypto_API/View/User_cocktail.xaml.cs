using System.Windows;
using System.Windows.Controls;
using System.IO;
using System;
using System.Diagnostics;
using System.Windows.Media.Media3D;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Xaml;

using Crypto_API.Service;
using System.Windows.Media;
using System.Xml.Linq;

using iTextSharp.text;
using iTextSharp.text.pdf;
using Paragraph = iTextSharp.text.Paragraph;


namespace Crypto_API.View
{
    /// <summary>
    /// Logique d'interaction pour User_cocktail.xaml
    /// </summary>
    public partial class User_cocktail : UserControl
    {
        Userdata user;
        ClasseCocktail classeApi;


        public User_cocktail()
        {
            InitializeComponent();

            classeApi = new ClasseCocktail();

            //Lecture de notre fichier Json
            user = JsonSerializer.Deserialize<Userdata>(File.ReadAllText("../../../Ressources/Userdata.json"));
            TBK_Ingredients.Text = classeApi.ingredient.ToString();
            TBK_Instruction.Text = classeApi.instruction.ToString();
            Name.Text = classeApi.name.ToString();

           
           
        }
    private async void Recherche_Click(object sender, RoutedEventArgs e)
{
    if (TB_Ingredients == null)
    {
        MessageBox.Show("Vous devez entrer un nom valide");
    }
    else
    {
        // Écrit dans le fichier JSON à l'emplacement de "Drink"
        user.Drink = TB_Ingredients.Text;
        string jsonData = JsonSerializer.Serialize(user);
        File.WriteAllText("../../../Ressources/Userdata.json", jsonData);

        // Appel de notre fonction principale (getMain) de notre classeApi
        ClasseCocktail classeApi = new ClasseCocktail();
        classeApi.getMain();
        TBK_Ingredients.Text = classeApi.ingredient.ToString();
        TBK_Instruction.Text = classeApi.instruction.ToString();
        Name.Text = classeApi.name.ToString();

        // Utilisation d'Amazon Polly pour lire les instructions à haute voix
        var instructionsText = classeApi.instruction;
        await classeApi.SynthesizeSpeechAsync(instructionsText);
    }
}


        private void NoteValide_Click(object sender, RoutedEventArgs e)
        {

            //Edition de "Rating" en lui donnant la valeur de NoteCocktail
            user.Rating = NoteCocktail.Value.ToString();

            string jsonData = JsonSerializer.Serialize(user);
            File.WriteAllText("../../../Ressources/Userdata.json", jsonData);     
        }

        private void BTN_imprimante_Click(object sender, RoutedEventArgs e)
        {
            GeneratePdf();
        }

        private void GeneratePdf()
        {
            string outFile = Environment.CurrentDirectory + "/recette.pdf";

            //créations du doc

            Document doc = new Document();
            PdfWriter.GetInstance(doc, new FileStream(outFile, FileMode.Create));
            doc.Open();
            


            //Palette de couleur

            BaseColor blue = new BaseColor(0, 75, 153);
            BaseColor violet = new BaseColor(112, 43, 158);
            BaseColor blanc = new BaseColor(255, 255, 255);

            // Police d'écriture

            Font policetext = new Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20f, iTextSharp.text.Font.BOLD, blue);
            Font policetitre = new Font(iTextSharp.text.Font.FontFamily.HELVETICA, 20f, iTextSharp.text.Font.BOLD, violet);


            //création du paragraphe
            
            
            Paragraph p1 = new Paragraph(Name.Text + "\n\n", policetitre);
            p1.Alignment = Element.ALIGN_CENTER;
            doc.Add(p1);
            
            Paragraph p2 = new Paragraph("recette: " +TBK_Instruction.Text + "\n\n", policetext);
            p2.Alignment = Element.ALIGN_LEFT;
            doc.Add(p2);

            
            Paragraph p3 = new Paragraph("Ingredients: " + TBK_Ingredients.Text + "\n\n", policetext);
            p3.Alignment = Element.ALIGN_LEFT;
            doc.Add(p3);

            //Fermer le doc

            doc.Close();
            Process.Start(@"cmd.exe ", @"/c " + outFile);
            OpenPDF(outFile);


        }
        private void OpenPDF(string pdfPath)
        {
            using (Process p = new Process())
            {
                p.StartInfo = new ProcessStartInfo()
                {
                    CreateNoWindow = true,
                    UseShellExecute = true,
                    FileName = pdfPath
                };

                p.Start();
            };
        }
    }
}

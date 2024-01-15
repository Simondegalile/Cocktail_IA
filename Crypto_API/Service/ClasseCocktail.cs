using System;

using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.IO;

using Amazon;
using Amazon.Polly; 
using Amazon.Polly.Model;
using Amazon.Runtime;

using JsonSerializer = System.Text.Json.JsonSerializer;
using Newtonsoft.Json;




namespace Crypto_API.Service
{
    internal class ClasseCocktail
    {
        public string instruction { get; set; }
        public string name { get; set; }
        public string ingredient { get; set; }

        //Pour la connexion AWS
        private readonly IAmazonPolly _pollyClient;

        ListCocktails coktail;
        private readonly HttpClient HttpClient;
        private const string key = "1z08C7uVCun1LaC0bn6lRpfQSL0NQEpXrbFTPJyF";



        public ClasseCocktail()
        {
            // Configurez les clés d'accès AWS ici
            string awsAccessKeyId = "AKIA4MTWIJO2ZJJWWU7Q";
            string awsSecretAccessKey = "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";

            var awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
            var awsRegionEndpoint = RegionEndpoint.GetBySystemName("eu-west-3");


            _pollyClient = new AmazonPollyClient(awsCredentials, awsRegionEndpoint);



            Userdata user = JsonSerializer.Deserialize<Userdata>(File.ReadAllText("../../../Ressources/Userdata.json"));

            string Drink = user.Drink;

            if (Drink == "")
            {
                Drink = "Mojito";
            }

            HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Add("X-Api-Key", key);

            //Attente de la fontion Getcoktail
            var task = Task.Run(async () => await Getcocktail(Drink));
            task.Wait();
            var asyncResult = task.Result;


            getMain();
        }


        public async Task SynthesizeSpeechAsync(string text)
        {
            // Utilisation du client Polly configuré dans le constructeur
            var synthesizeSpeechRequest = new SynthesizeSpeechRequest
            {
                OutputFormat = OutputFormat.Mp3,
                VoiceId = VoiceId.Joanna,
                Text = text
            };

            try
            {
                var response = await _pollyClient.SynthesizeSpeechAsync(synthesizeSpeechRequest);

                string outputPath = @"C:\Users\SLAB81\Desktop\Cockatil\speech.mp3";
                using (var fileStream = File.Create(outputPath))
                {
                    response.AudioStream.CopyTo(fileStream);
                    fileStream.Flush();
                    fileStream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calling Amazon Polly: " + ex.Message);
            }
        }


       


        private async Task<string> Getcocktail(string Drink)   
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage reponse = await HttpClient.GetAsync("https://api.api-ninjas.com/v1/cocktail?name=" + Drink);

            if (reponse.IsSuccessStatusCode)
            {
                //"{\"cocktails\":" les antis slash permettent de prendre en compte les " " afin que strBuilder retourne "cocktails" a la place de cocktails
                string strBuilder = "{\"cocktails\":";
                var content = await reponse.Content.ReadAsStringAsync();

                //Ajout de la valeur que contient "content" sois le json de l'api
                strBuilder += content.ToString();

                //Ajout de } a la fin de strBuilder voir screen 
                strBuilder += "}";
                coktail = JsonConvert.DeserializeObject<ListCocktails>(strBuilder);
            }
            else
            {
                MessageBox.Show("error");
            }
            return "ok";
        }
        public void getMain()
        {


            try
            {
                //Fonction qui donne une valeur a mes variables en fontion de l'API 
                instruction = coktail.cocktails[0].instructions;
                name = coktail.cocktails[1].name.ToUpper();
                ingredient = string.Join(",", coktail.cocktails[1].ingredients);
            }
            catch { MessageBox.Show("Champ invalide"); }

        }
    }
    #region CoktailApi

}
#endregion


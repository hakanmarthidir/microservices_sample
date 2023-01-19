using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;

namespace sharedconfiguration
{

    //I would like to test HashiCorp.Vault. 
    // Note : dev mode will be working on in-memory mode. When you re-start your vault container, all your secrets will be deleted. 

    //Nuget > VaultSharp 
    // or you can get your secrets by using HttpClientFactory

    public static class VaultDependencyInjection
    {
        public static async Task<IConfigurationBuilder> AddVault(this IConfigurationBuilder builder)
        {
            try
            {
                var vaultAddress = Environment.GetEnvironmentVariable("Vault_Address");
                var vaultToken = Environment.GetEnvironmentVariable("Vault_Token");
                var vaultPath = Environment.GetEnvironmentVariable("Vault_Path");
                var vaultMainFolder = Environment.GetEnvironmentVariable("Vault_Folder") ?? "secret";

                if (string.IsNullOrWhiteSpace(vaultAddress))
                    throw new ArgumentNullException("Vault Address could not be empty.");

                if (string.IsNullOrWhiteSpace(vaultToken) )
                    throw new ArgumentNullException("Vault Token could not be empty.");

                if (string.IsNullOrWhiteSpace(vaultPath))
                    throw new ArgumentNullException("Vault Path could not be empty.");

                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.EntryPoint != null);

                builder
                    .AddUserSecrets(assembly:assembly, optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();

                var buildConfig = builder.Build();

                IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken);

                var vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);

                VaultSharp.IVaultClient vaultClient = new VaultSharp.VaultClient(vaultClientSettings);

                Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2
                    .ReadSecretAsync(path: vaultPath, mountPoint: vaultMainFolder);

                var result = kv2Secret.Data.Data.ToDictionary(kv => kv.Key, kv => kv.Value.ToString());

                foreach (var item in result)
                {                    
                    System.Environment.SetEnvironmentVariable(item.Key, item.Value);
                }

                builder.Build();
                return builder;
            }
            catch (Exception ex)
            {
                throw new Exception("Vault configuration failed: " + ex.Message);
            }
        }


        //With HttpClient Sample
        //public static async Task<Dictionary<string, string>> GetVaultByHttp(string vaultServerAddress, string vaultToken)
        //{
        //    //vaultServerAddress > "http://localhost:8200"
        //    HttpClient httpClient = new HttpClient();           
        //    httpClient.BaseAddress = new Uri(vaultServerAddress);
        //    httpClient.DefaultRequestHeaders.Add("X-Vault-Token", vaultToken);
        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    var vaultPath = "Development";
        //    JObject json = JObject.Parse(await httpClient.GetStringAsync($"/v1/secret/data/{vaultPath}"));
        //    JToken secrets = json["data"]["data"];

        //    Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(secrets.ToString());
        //    foreach (var item in values)
        //    {
        //        Console.WriteLine($"Key: {item.Key} Value: {item.Value}");
        //    }
        //    return values;
            
        //}

    }




}
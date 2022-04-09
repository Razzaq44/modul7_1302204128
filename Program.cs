using System;
using System.Text.Json;
using System.IO;

namespace Jurnal7
{
    
    public class program
    {        
        public static void Main(string[] args)
        {
            int uang;
            int biayaTF = 0;
            int methodsTF = 0;
            int totalTF;
            string confirm;

            BankTransferConfig bankConfig = new BankTransferConfig();

            int threshold = bankConfig.config.transfer.threshold;
            int high_fee = bankConfig.config.transfer.high_fee;
            int low_fee = bankConfig.config.transfer.low_fee;

            Console.WriteLine("Language: (en/id)");
            string pilihBahasa = Console.ReadLine();
            Console.WriteLine();

            if (pilihBahasa == "en")
            {
                Console.Write("Please insert the amount of money to transfer: ");
                uang = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                if (uang <= threshold)
                {
                    biayaTF = low_fee;
                }
                else if (uang >= threshold)
                {
                    biayaTF = high_fee;
                }

                totalTF = uang + biayaTF;
                Console.WriteLine("Transfer fee = " + biayaTF);
                Console.WriteLine("Total amount = " + totalTF);
                Console.WriteLine();
                Console.WriteLine("Transfer method:");

                for (int i = 0; i < bankConfig.config.methods.Count; i++)
                {                 
                    Console.WriteLine((i+1) + ". " + bankConfig.config.methods[i]);
                }

                Console.Write("Select transfer method: ");
                methodsTF = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                Console.WriteLine("Please type " + bankConfig.config.confirmation.en + " to confirm the transaction:");
                confirm = Console.ReadLine();
                Console.WriteLine();

                if (confirm == bankConfig.config.confirmation.en)
                {
                    Console.WriteLine("The transfer is completed");
                }
                else
                {
                    Console.WriteLine("Transfer is cancelled");
                }
            }
            else if (pilihBahasa == "id")
            {

                Console.Write("Masukkan jumlah uang yang akan di-transfer: ");
                uang = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                if (uang <= bankConfig.config.transfer.threshold)
                {
                    biayaTF = bankConfig.config.transfer.low_fee;
                }
                else if (uang >= bankConfig.config.transfer.threshold)
                {
                    biayaTF = bankConfig.config.transfer.high_fee;
                }

                Console.WriteLine("Biaya transfer = " + biayaTF);
                Console.WriteLine("Total biaya = " + (biayaTF + uang));
                Console.WriteLine();
                Console.WriteLine("metode transfer:");

                for (int i = 0; i < bankConfig.config.methods.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + bankConfig.config.methods[i]);
                }

                Console.Write("Pilih metode transfer: ");
                methodsTF = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                Console.WriteLine("Ketik " + bankConfig.config.confirmation.id + " untuk mengkonfirmasi transaksi:");
                confirm = Console.ReadLine();
                Console.WriteLine();

                if (confirm == bankConfig.config.confirmation.id)
                {
                    Console.WriteLine("Proses transfer berhasil");
                }
                else
                {
                    Console.WriteLine("Transfer dibatalkan");
                }
            }            
        }
    }
    
    public class BankTransferConfig
    {
        public Config config;
        public string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        public string fileConfigName = "bank_transfer_config.json";

        public BankTransferConfig()
        {
            try
            {
                ReadConfigFile();
            }
            catch (Exception)
            {
                SetDefault();
                WriteNewConfigFile();
            }
        }

        private Config ReadConfigFile()
        {
            string jsonStringFromFile = File.ReadAllText(path + "/" + fileConfigName);
            config = JsonSerializer.Deserialize<Config>(jsonStringFromFile);
            return config;
        }

        private void SetDefault()
        {         
            string lang = "en";

            Transfer transfer = new Transfer();

            transfer.threshold = 25000000;
            transfer.low_fee = 6500;
            transfer.high_fee = 15000;

            List<string> method = new List<string>();
            method.Add("RTO (real-time)");
            method.Add("SKN");
            method.Add("RTGS");
            method.Add("BI FAST");

            Confirm confirm = new Confirm();

            confirm.en = "yes";
            confirm.id = "ya";

            config = new Config(lang, transfer, method, confirm);
        }

        private void WriteNewConfigFile()
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            string jsonString = JsonSerializer.Serialize(config, options);
            string fullFilePath = path + "/" + fileConfigName;
            File.WriteAllText(fullFilePath, jsonString);
        }
    }

    public class Config
    {
        public string lang { get; set; }
        public Transfer transfer { get; set; }
        public List<string> methods { get; set; }
        public Confirm confirmation { get; set; }
        public Config() { }
        public Config(string lang, Transfer transfer, List<string> method, Confirm confirmation)
        {
            this.lang = lang;
            this.transfer = transfer;
            this.methods = method;
            this.confirmation = confirmation;
        }
    }

    public class Transfer
    {
        public int threshold { get; set; }
        public int low_fee { get; set; }
        public int high_fee { get; set; }
    }

    public class Confirm
    {
        public string en { get; set; }
        public string id { get; set; }
    }
}

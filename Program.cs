using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Newtonsoft.Json; // You'll need to install this NuGet package

class Program
{
    private static HashSet<string> PopularDomains;

    [STAThread]
    static void Main()
    {
        Console.Title = "ComboScrapper | v0.1  By @SaidosHits";

        // Load domains from JSON file
        try
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "domains.json");
            if (!File.Exists(jsonPath))
            {
                // Create default JSON file if it doesn't exist
                var defaultDomains = new List<string>
                {
                    "gmail.com", "hotmail.com", "yahoo.com", "outlook.com", "aol.com",
                    "icloud.com", "msn.com", "live.com", "protonmail.com", "mail.com",
                    "yandex.com", "zoho.com", "gmx.com", "comcast.net", "verizon.net",
                    "att.net", "sbcglobal.net", "bellsouth.net", "cox.net", "orange.fr"
                };
                File.WriteAllText(jsonPath, JsonConvert.SerializeObject(defaultDomains, Formatting.Indented));
            }

            string jsonContent = File.ReadAllText(jsonPath);
            PopularDomains = new HashSet<string>(JsonConvert.DeserializeObject<List<string>>(jsonContent), StringComparer.OrdinalIgnoreCase);
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error loading domains from JSON: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        logo();

        MessageBox.Show(
          "Join my Channel: https://t.me/SaidosHits_Tools",
          "Information",
          MessageBoxButtons.OK,
          MessageBoxIcon.Information
      );

        Console.ResetColor();
        Console.WriteLine("");
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(" [*] Warning this tool work ony with Url logs that contian this form URL:EMAIL:PASSWORD ");
        Console.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(" [>] Press any key to select txt Log file ");
        Console.ReadKey();

        using (OpenFileDialog openFileDialog = new OpenFileDialog())
        {
            openFileDialog.Title = "Select a File";
            openFileDialog.Filter = "Text files |*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Console.Clear();
                string filePath = openFileDialog.FileName;
                int fileLength = File.ReadAllLines(filePath).Length;

                if (fileLength == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine(" Sorry, the txt file selected is empty :)");
                    Console.ReadKey();
                    Console.Clear();
                    Main();
                }
                else
                {
                    Dictionary<string, List<string>> domainEmails = new Dictionary<string, List<string>>();
                    List<string> Usernames = new List<string>();
                    int totalEmails = 0;

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;

                            string[] parts = line.Split(':');
                            if (parts.Length >= 3)
                            {
                                string emailOrUser = parts[parts.Length - 2];
                                string password = parts[parts.Length - 1];
                                string combinedValue = $"{emailOrUser}:{password}";

                                if (emailOrUser.Contains("@"))
                                {
                                    string domain = GetDomainFromEmail(emailOrUser);

                                    if (!string.IsNullOrEmpty(domain) && PopularDomains.Contains(domain))
                                    {
                                        if (!domainEmails.ContainsKey(domain))
                                        {
                                            domainEmails[domain] = new List<string>();
                                        }
                                        domainEmails[domain].Add(combinedValue);
                                        totalEmails++;
                                    }
                                }
                                else
                                {
                                    Usernames.Add(combinedValue);
                                }
                            }
                        }
                    }

                    string Current_location = AppDomain.CurrentDomain.BaseDirectory;
                    var Current_time = DateTime.Now;
                    string folder_name = string.Format("Result {0:[dd.MM.yyyy] [HH.mm.ss]}", Current_time);
                    Directory.CreateDirectory(folder_name);

                    string usernamesFolder = Path.Combine(folder_name, "Usernames");
                    Directory.CreateDirectory(usernamesFolder);
                    string Usernames_combo = Path.Combine(usernamesFolder, "Usernames.txt");
                    using (StreamWriter write = new StreamWriter(Usernames_combo))
                    {
                        foreach (var user in Usernames)
                        {
                            write.WriteLine(user);
                        }
                    }

                    foreach (var domain in domainEmails.Keys)
                    {
                        string domainFile = Path.Combine(folder_name, $"{domain}.txt");
                        using (StreamWriter write = new StreamWriter(domainFile))
                        {
                            foreach (var email in domainEmails[domain])
                            {
                                write.WriteLine(email);
                            }
                        }
                    }

                    Console.Clear();
                    logo();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine("");
              
                    Console.WriteLine($" [>] Processed domains: {domainEmails.Count}");
                    Console.WriteLine($" [>] Total emails processed: {totalEmails}");
                    Console.WriteLine($" [>] Usernames found: {Usernames.Count}");

                    foreach (var domain in domainEmails.Keys)
                    {
                        int domainCount = domainEmails[domain].Count;
                        double percentage = (double)domainCount / totalEmails * 100;
                        Console.WriteLine($" [>] {domain}: {domainCount} entries ({percentage:F2}%)");
                    }
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Console.WriteLine(" [>] Press any key to exit");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("No file selected.");
            }
        }
    }

    static string GetDomainFromEmail(string email)
    {
        try
        {
            var match = Regex.Match(email, @"@([\w-]+\.)+[\w-]{2,4}");
            if (match.Success)
            {
                return match.Value.Substring(1).ToLower();
            }
            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    static void logo()
    {
        Console.Write("   ____                _          ____                                          \r\n  / ___|___  _ __ ___ | |__   ___/ ___|  ___ _ __ __ _ _ __  _ __   ___ _ __    \r\n | |   / _ \\| '_  _ \\| '_ \\ / _ \\___ \\ / __| '__/ _ | '_ \\| '_ \\ / _ \\ '__|   \r\n | |__| (_) | | | | | | |_) | (_) |__) | (__| | | (_| | |_) | |_) |  __/ |      \r\n  \\____\\___/|_| |_| |_|_.__/ \\___/____/ \\___|_|  \\__,_| .__/| .__/ \\___|_|   \r\n                                                      |_|   |_|              ");
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordManager
{
    class Program
    {
        static string connectionString = "Data Source=passwords.db;Version=3;";
        static byte[] key;
        static byte[] iv;
        static Dictionary<string, string> passwordStore = new Dictionary<string, string>();
        static string storedUsername = "admin"; // Change as needed
        static string storedPassword = "password"; // Change as needed
        static string storedUsername2 = "admin2"; // Change as needed
        static string storedPassword2 = "password2"; // Change as needed
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            // Display ASCII Art
            DisplayTitle();
            DrawAsterisks();
            DrawAsterisks();
            DisplayCenteredText("NO UNAUTHORIZED ACCESS");
            DrawAsterisks();
            DrawAsterisks();
            int maxAttempts = 3;
            for(int attempts = 0; attempts<maxAttempts; attempts++ )
            {
                if(attempts > maxAttempts)
                {
                    Console.WriteLine("Login failed.");
                    return;
                }
                if (Login())
                { 
                    if (Login2())
                    {
                        manage();
                    }
                    else {
                        Console.WriteLine("Login failed. Please try again");
                        attempts++;
                    }
                    
                }
                else {
                    Console.WriteLine("Login failed. Please try again");
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Clear();
                }
            }
        }

        static void DisplayTitle()
        {
            Console.Title = "Password Manager"; // Set title
            Console.BackgroundColor = ConsoleColor.Black; // Background color
            Console.ForegroundColor = ConsoleColor.Green; // Text color
            Console.Clear(); // Apply background color
            Console.WriteLine(@"
 ______                                     _    ______                                      
(_____ \                                   | |  |  ___ \                                     
 _____) ____  ___  ___ _ _ _  ___   ____ _ | |  | | _ | | ____ ____   ____  ____  ____  ____ 
|  ____/ _  |/___)/___| | | |/ _ \ / ___/ || |  | || || |/ _  |  _ \ / _  |/ _  |/ _  )/ ___)
| |   ( ( | |___ |___ | | | | |_| | |  ( (_| |  | || || ( ( | | | | ( ( | ( ( | ( (/ /| |    
|_|    \_||_(___/(___/ \____|\___/|_|   \____|  |_||_||_|\_||_|_| |_|\_||_|\_|| |\____|_|    
                                                                          (_____|                
 
");

        }
        static void manage() {
            {
                Console.BackgroundColor = ConsoleColor.Black;
                DrawLetters();
                DrawLetters();
                DrawLetters();
                DrawLetters();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                DrawLetters();
                DrawLetters();
                Console.Clear();
                DrawAsterisks();
                DrawAsterisks();
                DisplayCenteredText("Welcome");
                DrawAsterisks();
                DrawAsterisks();
                // Generate a random key and IV for AES
                using (var aes = Aes.Create())
                {
                    aes.GenerateKey();
                    aes.GenerateIV();
                    key = aes.Key;
                    iv = aes.IV;
                }
                while (true)
                {

                    Console.Write("PasswordManager> ");
                    string command = Console.ReadLine();

                    switch (command.ToLower())
                    {
                        case "add":
                            AddPassword();
                            break;
                            Console.Clear();
                        case "view":
                            ViewPasswords();
                            break;
                        case "delete":
                            DeletePassword();
                            Console.Clear();
                            break;
                        case "exit":
                            Console.WriteLine("Exiting Password Manager...");
                            return;
                        default:
                            Console.WriteLine("Invalid command. Available commands: add, view, delete, exit.");
                            break;
                    }
                }
            }
        }
        static void DrawAsterisks()
        {
            // Get the width of the console window
            int width = Console.WindowWidth;

            // Print asterisks across the width
            for (int i = 0; i < width; i++)
            {
                Console.Write("*");
            }

            Console.WriteLine(); // Move to the next line
        }
        static string GenerateRandomString(int length)
        {
            Random random = new Random();
            char[] letters = new char[length];

            for (int i = 0; i < length; i++)
            {
                // Generate a random letter between A-Z
                letters[i] = (char)('A' + random.Next(0, 26));
            }

            return new string(letters);
        }
        static void DrawLetters()
        {
            // Get the width of the console window
            int width = Console.WindowWidth;
            Random random = new Random();
            // Print asterisks across the width
            for (int i = 0; i < width; i++)
            {
                Console.Write(GenerateRandomString(width));
            }

            Console.WriteLine(); // Move to the next line
        }
        static void DisplayCenteredText(string text)
        {
            int width = Console.WindowWidth;
            int spaces = (width - text.Length) / 2;

            // Print spaces before the text
            Console.WriteLine(new string(' ', spaces) + text);
        }
        static void ClearScreen()
        {
            Console.Clear();
            DisplayTitle(); // Display title again after clearing
        }
        private static string ReadPassword()
        {
            String password = String.Empty;
            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password[0..^1];
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
            }
            return password;


        }
        static bool Login()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
           String password= ReadPassword();
            Console.WriteLine();
            return username == storedUsername && password == storedPassword;
        }
        static bool Login2()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = ReadPassword();
            Console.WriteLine();
            return username == storedUsername2 && password == storedPassword2;
        }
        static void AddPassword()
        {
            Console.Write("Enter a name for the password (e.g., website name): ");
            string name = Console.ReadLine();
            Console.Write("Enter the password: ");
            string password = Console.ReadLine();

            if (passwordStore.ContainsKey(name))
            {
                Console.WriteLine("Password for this name already exists. Updating the password.");
            }

            passwordStore[name] = password;
            Console.WriteLine("Password added/updated successfully!");
        }
        static void ViewPasswords()
        {
            if (passwordStore.Count == 0)
            {
                Console.WriteLine("No passwords stored.");
                return;
            }

            Console.WriteLine("Stored Passwords:");
            foreach (var entry in passwordStore)
            {
                DrawAsterisks();
                Console.WriteLine($"Name: {entry.Key}");
                Console.WriteLine($"Password:{entry.Value}");
                DrawAsterisks();
            }
        }
        static void DeletePassword()
        {
            Console.Write("Enter the name of the password to delete: ");
            string name = Console.ReadLine();

            if (passwordStore.Remove(name))
            {
                Console.WriteLine("Password deleted successfully.");
            }
            else
            {
                Console.WriteLine("No password found with that name.");
            }
        }
        static void DisplayMatrix(char[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int topPosition = Console.WindowHeight;

            while (true) // Infinite loop for continuous movement
            {
                Console.Clear(); // Clear the console

                // Display the letters
                for (int i = 0; i < rows; i++)
                {
                    // Calculate the current display position
                    int currentRow = (topPosition + i) % (Console.WindowHeight + 1);
                    Console.SetCursorPosition(0, currentRow); // Set the cursor position

                    for (int j = 0; j < cols; j++)
                    {
                        Console.Write(matrix[i, j] + " ");
                    }
                }

                // Move the top position upwards
                topPosition--;

                // Reset the topPosition if it goes out of bounds
                if (topPosition < -rows)
                    topPosition = Console.WindowHeight;

                // Pause briefly for effect
                Thread.Sleep(10); // Adjust for speed
            }
        }

    }
}


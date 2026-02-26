using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBank
{
    class Program
    {
        // Static list to hold users while the program is running
        static List<User> users = new List<User>();
        static User currentUser = null;

        static void Main(string[] args)
        {
            // Seed some initial data for testing
            users.Add(new User("101", "Admin", "1234", 1000.0));

            while (true)
            {
                if (currentUser == null)
                    ShowWelcomeMenu();
                else
                    ShowDashboard();
            }
        }

        // ===============================
        // Welcome Menu
        // ===============================

        static void ShowWelcomeMenu()
        {
            Console.WriteLine("\n=== Welcome to Simple Bank (In-Memory) ===");
            Console.WriteLine("1- Login");
            Console.WriteLine("2- Register");
            Console.WriteLine("3- Exit");
            Console.Write("Select option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Login(); break;
                case "2": Register(); break;
                case "3": Environment.Exit(0); break;
                default: Console.WriteLine("Invalid selection."); break;
            }
        }

        static void Login()
        {
            Console.Write("Enter your ID: ");
            string id = Console.ReadLine();

            Console.Write("Enter Password: ");
            string pass = Console.ReadLine();

            // Search the local list for the user
            currentUser = users.Find(u => u.GetId() == id && u.GetPassword() == pass);

            if (currentUser != null)
                Console.WriteLine($"Welcome back, {currentUser.GetUsername()}!");
            else
                Console.WriteLine("Invalid ID or password.");
        }

        static void Register()
        {
            Console.Write("Enter unique ID: ");
            string id = Console.ReadLine();

            if (users.Any(u => u.GetId() == id))
            {
                Console.WriteLine("Error: ID already exists.");
                return;
            }

            Console.Write("Username: ");
            string name = Console.ReadLine();

            Console.Write("Password: ");
            string pass = Console.ReadLine();

            Console.Write("Initial Balance: ");
            if (double.TryParse(Console.ReadLine(), out double balance))
            {
                users.Add(new User(id, name, pass, balance));
                Console.WriteLine("Account created successfully.");
            }
            else
            {
                Console.WriteLine("Invalid balance format.");
            }
        }

        // ===============================
        // Dashboard
        // ===============================

        static void ShowDashboard()
        {
            Console.WriteLine($"\n--- {currentUser.GetUsername()}'s Dashboard ---");
            Console.WriteLine($"Current Balance: ${currentUser.GetBalance()}");
            Console.WriteLine("1- Deposit");
            Console.WriteLine("2- Withdraw");
            Console.WriteLine("3- Transfer");
            Console.WriteLine("4- Delete Account");
            Console.WriteLine("5- Logout");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": Deposit(); break;
                case "2": Withdraw(); break;
                case "3": Transfer(); break;
                case "4": DeleteAccount(); break;
                case "5": Logout(); break;
                default: Console.WriteLine("Invalid selection."); break;
            }
        }

        static void Deposit()
        {
            Console.Write("Enter Amount to Deposit: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                currentUser.SetBalance(currentUser.GetBalance() + amount);
                Console.WriteLine($"Successfully deposited ${amount}. New balance: ${currentUser.GetBalance()}");
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void Withdraw()
        {
            Console.Write("Enter Amount to Withdraw: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                if (currentUser.GetBalance() >= amount)
                {
                    currentUser.SetBalance(currentUser.GetBalance() - amount);
                    Console.WriteLine($"Successfully withdrew ${amount}. New balance: ${currentUser.GetBalance()}");
                }
                else
                {
                    Console.WriteLine("Insufficient balance.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void Transfer()
        {
            Console.Write("Recipient ID: ");
            string targetId = Console.ReadLine();

            var targetUser = users.Find(u => u.GetId() == targetId);

            if (targetUser == null || targetId == currentUser.GetId())
            {
                Console.WriteLine("Recipient not found or invalid.");
                return;
            }

            Console.Write("Amount to Transfer: ");
            if (double.TryParse(Console.ReadLine(), out double amount) && amount > 0)
            {
                if (currentUser.GetBalance() >= amount)
                {
                    Console.Write("Confirm your password: ");
                    string pass = Console.ReadLine();

                    if (pass == currentUser.GetPassword())
                    {
                        // Perform the in-memory transfer
                        currentUser.SetBalance(currentUser.GetBalance() - amount);
                        targetUser.SetBalance(targetUser.GetBalance() + amount);
                        Console.WriteLine($"Transfer of ${amount} to {targetUser.GetUsername()} successful.");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect password.");
                    }
                }
                else
                {
                    Console.WriteLine("Insufficient balance for this transfer.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount.");
            }
        }

        static void DeleteAccount()
        {
            Console.Write("Are you sure you want to delete your account? (y/n): ");
            if (Console.ReadLine().ToLower() == "y")
            {
                users.Remove(currentUser);
                currentUser = null;
                Console.WriteLine("Account permanently deleted from current session.");
            }
        }

        static void Logout()
        {
            currentUser = null;
            Console.WriteLine("Logged out successfully.");
        }
    }
}
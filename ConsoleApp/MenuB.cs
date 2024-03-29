﻿//**************************************************
// File: MenuB.cs
//
// Purpose: Displays the menu for businesses.
//
// Written By: Ivan Williams
//
// Compiler: Visual Studio 2019
//**************************************************
using System;
using System.Collections.ObjectModel;
using System.Reflection;

namespace ConsoleApp
{
    class MenuB : Menu
    {
        #region Properties
        public Business Business { get; set; }

        public ObservableCollection<Business> Others { get; set; }
        #endregion

        #region Member Methods
        //**************************************************
        // Method: Constructor
        //
        // Purpose: Initializing the Business and Others
        //          properties.
        //**************************************************
        public MenuB(Business b, ObservableCollection<Business> o)
        {
            Business = b;
            Others = o;
            mainMenu();
        }

        //**************************************************
        // Method: mainMenu
        //
        // Purpose: Displaying the main menu.
        //**************************************************
        public override void mainMenu()
        {
            Console.Clear();
            int choice;
            Console.WriteLine("MAIN MENU");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Add Item");
            Console.WriteLine("2. View Inventory");
            Console.WriteLine("3. View Dispensaries");
            Console.WriteLine("4. View Account Information");
            Console.WriteLine("5. Log Out");
            Console.WriteLine("====================================");
            choice = getChoice(1, 5);
            switch (choice) {
                case 1:
                    addItem();
                    break;
                case 2:
                    viewInv(Business);
                    break;
                case 3:
                    viewDispos();
                    break;
                case 4:
                    viewAccount();
                    break;
            }
            if (choice != 5) {
                mainMenu();
            }
        }

        //**************************************************
        // Method: check
        //
        // Purpose: Checking if an item exists based on the
        //          name of the item.
        //**************************************************
        private bool check(string name) {
            foreach (MenuItem item in Business.Items)
            {
                if (item.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        //**************************************************
        // Method: addItem
        //
        // Purpose: Adding an item to a Business's
        //          inventory.
        //**************************************************
        private void addItem()
        {
            Console.Clear();
            Console.WriteLine("Please enter the following information as it appears");
            MenuItem item = new MenuItem();
            PropertyInfo[] properties = typeof(MenuItem).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                string input;
                Console.Write(property.Name + ": ");
                input = Console.ReadLine();
                property.SetValue(item, input);
                if (property.Name == "Price") {
                    double tryDouble;
                    bool success = double.TryParse(input, out tryDouble);
                    while (!success || tryDouble < 0.01)
                    {
                        Console.Write(property.Name + ": ");
                        input = Console.ReadLine();
                        property.SetValue(item, input);
                        success = double.TryParse(input, out tryDouble);
                    }
                }
            }
            if (!check(item.Name))
            {
                Business.Items.Add(item);
                Console.WriteLine("The item (" + item.Name + ") was successfully added.");
            }
            else
            {
                Console.WriteLine("This item was already added.");
            }
            string wait = Console.ReadLine();
        }

        //**************************************************
        // Method: viewDispos
        //
        // Purpose: Viewing other businesses and their
        //          inventories.
        //**************************************************
        private void viewDispos()
        {
            ObservableCollection<Business[]> list = new ObservableCollection<Business[]>();
            int count = 0;
            Business[] arr = new Business[6];

            foreach (Business b in Others)
            {
                if (count > 5)
                {
                    list.Add(arr);
                    count = 0;
                    arr = null;
                }
                if (count == 0 && arr == null)
                {
                    arr = new Business[6];
                }
                arr[count] = b;
                count++;
            }
            if (arr != null)
            {
                list.Add(arr);
            }
            count = 0;

            string choice;
            do
            {
                Console.Clear();
                string[] choices = new string[9];
                choices[0] = "<";
                choices[1] = ">";
                choices[2] = "0";

                Console.WriteLine("Browse Dispensaries");
                Console.WriteLine("\t\t\t\tPage " + (count + 1));
                Console.WriteLine("====================================");
                int counter = 1;
                foreach (Business b in list[count])
                {
                    if (b != null)
                    {
                        Console.WriteLine(counter + ". " + b.Name);
                        choices[counter + 2] = counter.ToString();
                        counter++;
                    }
                }
                Console.WriteLine("0. Exit");
                Console.WriteLine("====================================");
                Console.Write("Enter selection (type '<'/'>' to switch pages): ");
                choice = Console.ReadLine();
                while (Array.Find(choices, c => c == choice) == null)
                {
                    Console.Write("Invalid. Re-enter selection: ");
                    choice = Console.ReadLine();
                }

                if (choice == "<")
                {
                    if (count > 0)
                    {
                        count--;
                    }
                    else
                    {
                        count = list.Count - 1;
                    }
                }
                else if (choice == ">")
                {
                    if (count < list.Count - 1)
                    {
                        count++;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                else
                {
                    int n = Convert.ToInt32(choice);
                    if (n < 7 && n > 0)
                    {
                        if (list[count][n - 1].Items.Count == 0)
                        {
                            Console.WriteLine("This dispensary doesn't have products yet.");
                            string wait = Console.ReadLine();
                        }
                        else
                        {
                            viewInv(list[count][n - 1]);
                        }
                    }
                }
            } while (choice != "0");
        }

        //**************************************************
        // Method: getInv
        //
        // Purpose: Getting the inventory of a Business.
        //**************************************************
        private ObservableCollection<MenuItem[]> getInv(Business dispo)
        {
            int count = 0;
            ObservableCollection<MenuItem[]> inv = new ObservableCollection<MenuItem[]>();
            MenuItem[] arr = new MenuItem[6];

            foreach (MenuItem item in dispo.Items)
            {
                if (count > 5)
                {
                    inv.Add(arr);
                    count = 0;
                    arr = null;
                }
                if (count == 0 && arr == null)
                {
                    arr = new MenuItem[6];
                }
                arr[count] = item;
                count++;
            }
            if (arr != null)
            {
                inv.Add(arr);
            }
            return inv;
        }

        //**************************************************
        // Method: viewInv
        //
        // Purpose: Viewing the inventory of a Business.
        //**************************************************
        private void viewInv(Business dispo)
        {
            int count = 0;
            ObservableCollection<MenuItem[]> inv = getInv(dispo);

            string choice;
            do
            {
                Console.Clear();
                string[] choices = new string[9];
                choices[0] = "<";
                choices[1] = ">";
                choices[2] = "0";

                Console.WriteLine(dispo.Name + "'s Inventory");
                Console.WriteLine("\t\t\t\tPage " + (count + 1));
                Console.WriteLine("====================================");
                int counter = 1;
                foreach (MenuItem item in inv[count])
                {
                    if (item != null)
                    {
                        Console.WriteLine(counter + ". " + item.Name);
                        choices[counter + 2] = counter.ToString();
                        counter++;
                    }
                }
                Console.WriteLine("0. Exit");
                Console.WriteLine("====================================");
                Console.Write("Enter selection (type '<'/'>' to switch pages): ");
                choice = Console.ReadLine();
                while (Array.Find(choices, c => c == choice) == null)
                {
                    Console.Write("Invalid. Re-enter selection: ");
                    choice = Console.ReadLine();
                }

                if (choice == "<")
                {
                    if (count > 0)
                    {
                        count--;
                    }
                    else
                    {
                        count = inv.Count - 1;
                    }
                }
                else if (choice == ">")
                {
                    if (count < inv.Count - 1)
                    {
                        count++;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                else
                {
                    int n = Convert.ToInt32(choice);
                    if (n < 7 && n > 0)
                    {
                        viewItem(inv[count][n - 1], dispo == Business);
                        inv = getInv(dispo);
                    }
                }
            } while (choice != "0");
        }

        //**************************************************
        // Method: viewItem
        //
        // Purpose: Displaying an item's information.
        //**************************************************
        private void viewItem(MenuItem item, bool main)
        {
            Console.Clear();
            int choice;
            Console.WriteLine("====================================");
            PropertyInfo[] properties = typeof(MenuItem).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != "Information" && property.Name != "PrescriptionLength")
                {
                    Console.WriteLine(property.Name + ": " + ((property.Name == "Price") ? "$" : "") + property.GetValue(item));
                }
            }
            if (main) {
                Console.WriteLine("------------------------------------");
                Console.WriteLine("1. Remove Item");
                Console.WriteLine("2. Cancel");
            }
            Console.WriteLine("====================================");
            if (main)
            {
                choice = getChoice(1, 2);
                if (choice == 1)
                {
                    Business.Items.Remove(item);
                }
            }
            else
            {
                string wait = Console.ReadLine();
            }
        }

        //**************************************************
        // Method: viewAccount
        //
        // Purpose: Displaying the Business's account info.
        //**************************************************
        protected override void viewAccount()
        {
            Console.Clear();
            Console.WriteLine("Your Account\n");
            PropertyInfo[] properties = typeof(Business).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.Name != "Items")
                {
                    Console.WriteLine(property.Name + ": " + property.GetValue(Business));
                }
            }
            string wait = Console.ReadLine();
        }
        #endregion
    }
}

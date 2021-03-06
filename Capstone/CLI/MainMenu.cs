﻿using Capstone.Models;
using MenuFramework;
using System;
using Figgle;
using System.Collections.Generic;
using System.Text;

namespace Capstone.CLI
{
    /// <summary>
    /// The Main Menu for the vending machine
    /// </summary>
    /// <seealso cref="MenuFramework.ConsoleMenu" />
    public class MainMenu : ConsoleMenu
    {
        /*a few global color constants that should apply to all menus and sub-menus. 
         * This will keep the look of our program consistent
         */
        /// <summary>
        /// The menu item text foreground color
        /// </summary>
        public static ConsoleColor GlobalItemForegroundColor = ConsoleColor.Gray;
        /// <summary>
        /// The global selected item foreground color
        /// </summary>
        public static ConsoleColor GlobalSelectedItemForegroundColor = ConsoleColor.Yellow;

        /// <summary>
        /// The Vending machine we will be working with.
        /// </summary>
        private VendingMachine machine;
        /*******************************************************************************
         * Private data:
         * Usually, a menu has to hold a reference to some type of "business objects",
         * on which all of the actions requested by the user are performed. A common 
         * technique would be to declare those private fields here, and then pass them
         * in through the constructor of the menu.
         * ****************************************************************************/

        // NOTE: This constructor could be changed to accept arguments needed by the menu
        /// <summary>
        /// Initializes a new instance of the Main Menu
        /// </summary>
        public MainMenu(VendingMachine machine)
        {
            this.machine = machine;//initialize the vending machine
            Configure(cfg =>
           {
               cfg.ItemForegroundColor = GlobalItemForegroundColor;
               cfg.SelectedItemForegroundColor = GlobalSelectedItemForegroundColor;
               cfg.MenuSelectionMode = MenuSelectionMode.Arrow; // KeyString: User types a key, Arrow: User selects with arrow
               cfg.KeyStringTextSeparator = ": ";
               cfg.Title = "Main Menu";
           });
        }

        /// <summary>
        /// Basically just re-drawing the menu by overriding the <see cref="ConsoleMenu.RebuildMenuOptions" /> method.
        /// </summary>
        protected override void RebuildMenuOptions()
        {
            base.ClearOptions();
            AddOption("Display Vending Machine Items", DisplayItems, "D");
            AddOption("Purchase Items", PurchaseMenu, "P");
            AddOption("Quit", Close, "Q");
            if (this.machine.SalesreportsUnlocked)
            {
                AddOption("Generate Sales Report", WriteSalesReport, "W");
            }
        }
        /// <summary>
        /// We want to show the big pretty logo before each menu.
        /// </summary>
        protected override void OnBeforeShow()
        {
            DisplayLogo();
            base.OnBeforeShow();
        }

        /// <summary>
        /// Prints a big "VENDO-MATIC 800" logo to the screen. Should usually be called in the OnBeforeShow method
        /// </summary>
        public static void DisplayLogo()
        {
            //get the current color so we can restore it when we're done
            ConsoleColor oldForegroundColor = Console.ForegroundColor;
            ConsoleColor oldBackgroundColor = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(FiggleFonts.SlantSmall.Render(" VENDO-MATIC 800 "));
            Console.ForegroundColor = oldForegroundColor;
            Console.BackgroundColor = oldBackgroundColor;
        }

        /// <summary>
        /// Gives a small little message to the user at the end of the menu.
        /// </summary>
        protected override void OnAfterShow()
        {
            Console.WriteLine();
            Console.WriteLine("Paul VandenBroeck and Jamie Uhl");
            Console.WriteLine("Making Robust, Hand-Crafted Vending Machines since February 2021");
        }

        /// <summary>
        /// Prints the VendingMachine's menu to the console.
        /// </summary>
        /// <returns></returns>
        private MenuOptionResult DisplayItems()
        {
            MainMenu.DisplayLogo();
            machine.PrintInventory();
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        /// <summary>
        /// Opens the Purchase sub-menu
        /// </summary>
        /// <returns></returns>
        private MenuOptionResult PurchaseMenu()
        {
            PurchaseMenu purchaseMenu = new PurchaseMenu(this.machine);
            purchaseMenu.Show();
            return MenuOptionResult.DoNotWaitAfterMenuSelection;
        }

        /// <summary>
        /// Writes a new sales report to a file.
        /// </summary>
        /// <returns></returns>
        private MenuOptionResult WriteSalesReport()
        {
            this.machine.WriteSalesReportToFile(this.machine.GenerateSalesReport());
            Console.WriteLine("Sales Report creation successful.");
            return MenuOptionResult.WaitAfterMenuSelection;
        }
    }
}

﻿using System;
using System.Xml.Serialization;
using EntityMtwServer.Entities;

namespace InstallerMTW.Managers {
    /// <summary>
    /// Manage the dialog with the user, get the user input and creates a CommandsManager instance to carry out commands.
    /// </summary>
    public sealed class DialogManager {
        private ProcessManager cmdManager { get; set; }
        private bool runDialog;

        //range variable holds the range of RTSPs selected by the user.
        public static List<string> range = new List<string>();

        public DialogManager() {
            cmdManager = new ProcessManager();
            runDialog = true;
        }

        /// <summary>
        /// Initialize the dialog with the user and get the user's input as paramater to CommandsManager methods.
        /// </summary>
        public void StartTerminalDialog() {
            Console.WriteLine("Welcome to MTW Installer! \n"
                          + "Make sure that you're running this app on Linux Ubuntu 18.04. \n"
                          + "Your Current Linux distribution is: ");
            System.Console.WriteLine();
            cmdManager.ExecuteCmd("lsb_release -a \n");


            try {
                while (runDialog == true) {
                    Console.WriteLine("Select the desired package to install: \n[1] " +
                     "MQTT \n[2] Nginx \n[3] SQL Server 2017 \n[4] Mssql-Tools \n[5] Restore MasterServer "
                     + "\n[6] Restore TmHub \n[7] Git \n[8] Node Js 16x \n[9] ApiMtwServer \n[10] Record");
                    System.Console.WriteLine("type 'exit' to exit");

                    string input = Console.ReadLine().ToString();

                    if (input == "exit") {
                        runDialog = false;
                    }
                    else {
                        cmdManager.RedirectCommand(input);
                    }
                }
            }
            catch (ProcessException e) {
                string error = e.Message;
                Console.WriteLine(error);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }

        }

        public static void RangeDialog(List<Equipment> list) {
            System.Console.WriteLine("Type the  start of the range: ");
            int start = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Type the end of the range: ");
            int end = int.Parse(Console.ReadLine());

            SelectedRange(start, end, list);
        }

        public static void SelectedRange(int start, int end, List<Equipment> equipments) {
            for (int cont = start; cont <= end; cont++) {
                range.Add(equipments[cont].PrimaryRtsp);
            }
        }

        public static bool CreateRangeOfCameras() {
            Console.WriteLine("Do you wish to create a new service for each rtsp in the selected range?[y/n]  ");
            string response = Console.ReadLine().ToString();
            if (response == "y") {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool RemoveRangeOfCameras() {
            Console.WriteLine("Do you wish to remove each camera in the selected range?[y/n]  ");
            string response = Console.ReadLine().ToString();
            if (response == "y") {
                return true;
            }
            else {
                return false;
            }
        }

        public static string RecordOption() {
            Console.Clear();
            Console.WriteLine("[1]List \n[2]Create \n[3]Remove \n[4]Change \n[5]Exit");
            string selectedOption = Console.ReadLine().ToString();
            return selectedOption;
        }
    }
}
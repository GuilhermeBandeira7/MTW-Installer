
namespace InstallerMTW.Managers {
    public static class ServiceManager {

        public static void CreateShAndService(string cameraIP, string recordString) {
            ChangeAndCreateDirectory("/home", "records");
            IEnumerable<string> filesList = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.sh", SearchOption.AllDirectories);
            string fileName = $"record_{cameraIP}.sh";
            if (!filesList.Contains(fileName)) {
                using (FileStream newFile = new FileStream(fileName, FileMode.CreateNew)) {
                    using (StreamWriter writer = new StreamWriter(newFile)) {
                        writer.WriteLine(recordString);
                        writer.Flush(); //Despeja o buffer para o stream
                    };
                };
                CreateService($"record_{cameraIP}");
            }
            else {
                File.WriteAllText("/home/record/" + fileName, recordString);
            }
        }

        public static int ManageFileEnumeration() {
            if (Directory.Exists("/home/records")) {
                int totalFiles = 0;
                IEnumerable<string> qntFile = Directory.EnumerateFiles(Directory.GetCurrentDirectory()); //current file has to be records
                foreach (var file in qntFile) {
                    totalFiles++;
                }
                return totalFiles;
            }
            else {
                throw new ProcessException("could not access /home/records");
            }
        }

        public static void CreateService(string fileName) {
            ChangeDirectory("/etc/systemd/system");
            string nameService = $"{fileName}.service";
            Console.WriteLine(Directory.GetCurrentDirectory());
            EditServiceScript(fileName);
        }

        /// <summary>
        /// Change the linux directory to home and creates a record directory to save records of a camera.
        /// </summary>
        /// <param name="dirToChange"></param>
        /// <param name="dirNameToCreate"></param>
        /// <exception cref="ProcessException"></exception>
        public static void ChangeAndCreateDirectory(string dir, string dirNameToCreate) {
            if (Directory.Exists(dir)) {
                Directory.SetCurrentDirectory(dir);  //home
                if (Directory.Exists(dirNameToCreate)) //if records already exists
                {
                    Directory.SetCurrentDirectory(dirNameToCreate); //set records as current directory
                }
                else {
                    Directory.CreateDirectory("/home/" + dirNameToCreate); //create records
                    Directory.SetCurrentDirectory(dirNameToCreate);
                }
                Console.WriteLine("Current Directory: " + Directory.GetCurrentDirectory());
            }
            else {
                throw new ProcessException("This directory does not exist.");
            }
        }

        public static void ChangeDirectory(string filePath) {
            if (Directory.Exists(filePath)) {
                Directory.SetCurrentDirectory(filePath);
            }
        }

        public static void EditServiceScript(string serviceName) //Refazer esse método de maneira inteligente
        {
            using (FileStream file = new FileStream(Directory.GetCurrentDirectory() + $"/{serviceName}.service", FileMode.CreateNew)) {
                using (StreamWriter write = new StreamWriter(file)) {
                    write.WriteLine("[Unit]");
                    write.WriteLine("Description=CameraStream");
                    write.WriteLine("StartLimitBurst=0");
                    write.WriteLine(" ");
                    write.WriteLine("[Service]");
                    write.WriteLine("WorkingDirectory=/home/records/");
                    write.WriteLine($"ExecStart=/bin/bash /home/records/{serviceName}" + ".sh");
                    write.WriteLine("Restart=always ");
                    write.WriteLine("RestartSec=10  ");
                    write.WriteLine(" ");
                    write.WriteLine("[Install] ");
                    write.WriteLine("WantedBy=multi-user.target");

                    write.Flush();
                }
            }
        }

        public static void CreateRangeOfRecordService(List<string> SelectedRange) {
            for (int cont = 0; cont <= SelectedRange.Count; cont++) {
                string recordString = $"transport tcp - allowed_media_types video - i \"rtsp://admin:admin@{SelectedRange.ElementAt(cont)}:8554\" -vcodec" +
                         "copy -map 0 -f segment -segment_time 20 -strftime 1 /home/records/camera/%Y%m%d%H%M%S.mkv";
                CreateShAndService(SelectedRange.ElementAt(cont), recordString);
            }

        }
    }
}

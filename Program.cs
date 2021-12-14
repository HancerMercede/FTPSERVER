
WriteLine("Welcome SFTP SERVER APP..");

UpLoadFiles();
DownLoadFiles();

void UpLoadFiles()
{
    var _Logger = new LogsHistory(@"F:\FTPSERVER\LogsHistory");
    try
    {
        WriteLine("Uploading files to Server.....");

        var remoteDirectory = @"";
        var PrivateKey = new PrivateKeyFile(@"F:\LlavesFTP\id_rsa", "12345678");
        var KeyFiles = new[] { PrivateKey };
        var Methods = new List<AuthenticationMethod>();
        Methods.Add(new PrivateKeyAuthenticationMethod(Credentials.User, KeyFiles));

        using (SftpClient CLient = new SftpClient(new ConnectionInfo(Credentials.Host, Credentials.User, Methods.ToArray())))
        {
            CLient.Connect();
            _Logger.AddLogs($"Connecting to the server.");

            var sourceFile = @"Y:\Test\dev.txt";
            using (var stream = File.OpenRead(sourceFile))
            {
                CLient.UploadFile(stream, remoteDirectory + Path.GetFileName(sourceFile), x => { WriteLine(x); });
                _Logger.AddLogs($"Loading the File {sourceFile} to the server.");
            }

            CLient.Disconnect();
            _Logger.AddLogs($"Closing the connection to the server.");
        }
    }
    catch (Exception ex)
    {
        _Logger.AddLogs($"Something happens Uploading the file: {ex.Message}");
    }
   
}

void DownLoadFiles()
{
    var _Logger = new LogsHistory(@"F:\FTPSERVER\LogsHistory");

    try
    {
        WriteLine("Downloading files from Server.....");

        var remoteDirectory = @"/home/sftp/";
        var localDirectory = @"Y:\Test\DownLoadFiles";

        var PrivateKey = new PrivateKeyFile(@"F:\LlavesFTP\id_rsa", "12345678");
        var KeyFiles = new[] { PrivateKey };

        var Methods = new List<AuthenticationMethod>();
        Methods.Add(new PrivateKeyAuthenticationMethod(Credentials.User, KeyFiles));

        using (SftpClient Client1 = new SftpClient(new ConnectionInfo(Credentials.Host, Credentials.User, Methods.ToArray())))
        {
            Client1.Connect();
            _Logger.AddLogs($"Connecting to the server: {remoteDirectory}.");

            var Files = Client1.ListDirectory(remoteDirectory);

            foreach (var file in Files)
            {
                var remoteFileName = file.Name;
                if ((!file.Name.StartsWith(".")) && (file.LastWriteTime.Date == DateTime.Today) && file.Name.EndsWith("txt"))
                    using (Stream fileDownLoaded = File.OpenWrite(Path.Combine(localDirectory, remoteFileName)))
                    {
                        Client1.DownloadFile(remoteDirectory + remoteFileName, fileDownLoaded);
                        _Logger.AddLogs($"Dawnloading the file {remoteFileName}.");

                        Client1.DeleteFile(remoteDirectory + remoteFileName);
                        _Logger.AddLogs($"Deleting the file {remoteFileName} from server {remoteDirectory}.");

                        _Logger.AddLogs($"Cheking the file {remoteFileName} it was deleted from server {remoteDirectory}.");
                        if (File.Exists(remoteDirectory))
                            WriteLine($"File:{remoteFileName} exists.");
                        else
                            WriteLine($"File:{remoteFileName} doesnt exist.");

                        WriteLine($"File {file.Name} was downloaded from {remoteDirectory} successfully!");
                    }
            }

            WriteLine($"{Files.Where(x => x.Name.EndsWith("txt")).Count()} was downloaded successfully!");
            Client1.Disconnect();
            _Logger.AddLogs($"{Files.Where(x => x.Name.EndsWith("txt")).Count()} was downloaded successfully!");
            _Logger.AddLogs($"Closing the connection to the server {remoteDirectory}.");
        }
    }
    catch (Exception ex)
    {
        _Logger.AddLogs($"Something happens Downloading the file: {ex.Message}");
    }

}






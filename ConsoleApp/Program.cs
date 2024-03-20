using GoogleSync;
using System.Timers;

namespace ConsoleApp
{
    class Program
    {
        static readonly GoogleDrive drive = new GoogleDrive("C:\\Users\\vojta\\source\\repos\\GoogleSync\\GoogleSync\\client_secret.json");
        static readonly string folderPath = @"C:\Users\vojta\Documents\GoogleSync";
        static readonly string folderId = drive.GetFolderId("Test");
        static void Main(string[] args)
        {
            var timer = new System.Timers.Timer(30000); //30 seconds

            timer.Elapsed += OnTimedEvent;

            OnTimedEvent(timer, null);

            timer.Enabled = true;

            Console.ReadKey();
        }

        private static async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Started Synchronization at {0}", e?.SignalTime ?? DateTime.Now);

            await drive.Synchronize(folderId, folderPath);

            Console.WriteLine("Synchronization finished at {0}", DateTime.Now);
        }

    }
}
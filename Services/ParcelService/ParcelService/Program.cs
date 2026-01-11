using SharedCore;
using SharedCore.Utilities;

namespace ParcelService
{ 
    internal class Program
    {
        static void Main(string[] args)
        {
            Overrides.AppName = "ParcelService";
            Overrides.LogToConsole = true;
            Overrides.Environment = Overrides.EnvironmentType.Local;
            Logger.Info("Parcel Service is starting up...");

            var listener = new ServiceListener();
            listener.StartListening();
            int counter = 1;
            while (Helper.IsRunning)
            {
                counter++;
                Thread.Sleep(1000);
                if (counter % 60 == 0)
                {
                    counter = 1;
                }
            }
        }
    }
}
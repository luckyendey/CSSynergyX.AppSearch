namespace CSSynergyX.AppSearch.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var apps = Core.Tools.GetApplications();
            string inputSearch = "New Request";
            var result = Core.Tools.GetFilteredApplication(inputSearch, apps);

            Console.ReadKey();
        }
    }
}
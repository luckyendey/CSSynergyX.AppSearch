using System.Collections.Generic;

namespace CSSynergyX.AppSearch.Core.Interfaces
{
    internal interface IFileService
    {
        string GetXmlDirectory();

        List<string> GetFilesStartingWith(string prefix);
    }
}
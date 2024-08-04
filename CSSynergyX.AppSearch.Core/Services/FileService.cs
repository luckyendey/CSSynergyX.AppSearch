using CSSynergyX.AppSearch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSSynergyX.AppSearch.Core.Services
{
    internal class FileService : IFileService
    {
        public string GetXmlDirectory()
        {
            string currentDirectory = "";
#if DEBUG
            currentDirectory = "C:\\Exact Synergy Enterprise 501\\xml";
#else
            currentDirectory = AppContext.BaseDirectory;
#endif
            return Directory.GetParent(currentDirectory).FullName + "\\xml";
        }

        public List<string> GetFilesStartingWith(string prefix)
        {
            string xmlDirectory = GetXmlDirectory();
            return Directory.GetFiles(xmlDirectory, $"{prefix}*.xml").ToList();
        }
    }
}
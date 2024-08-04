using CSSynergyX.AppSearch.Core.Entities;
using CSSynergyX.AppSearch.Core.Interfaces;
using CSSynergyX.AppSearch.Core.Services;
using FuzzySharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace CSSynergyX.AppSearch.Core
{
    public class Tools
    {
        private readonly IXmlService _xmlService;
        private readonly IFileService _fileService;

        public Tools()
        {
            _xmlService = new XmlService();
            _fileService = new FileService();
        }

        public List<string> GetMenuFiles()
        {
            return _fileService.GetFilesStartingWith("Menu");
        }

        public List<string> GetApplicationFiles()
        {
            return _fileService.GetFilesStartingWith("Application");
        }

        public static List<Application> GetApplications()
        {
            var tools = new Tools();
            List<string> applicationFiles = tools.GetApplicationFiles();

            List<Application> applications = new List<Application>();

            foreach (var file in applicationFiles)
            {
                var apps = tools.ReadApplicationFile(file);
                applications.AddRange(apps);
            }

            return applications;
        }

        /// <summary>
        /// Filters a list of applications based on fuzzy string matching against a combined title string.
        /// </summary>
        /// <param name="input">The input string to search for.</param>
        /// <param name="applications">The list of applications to filter.</param>
        /// <param name="threshold">
        /// (Optional) The minimum score required for a match (default is 55).
        /// </param>
        /// <param name="recordCount">
        /// (Optional) The maximum number of matches to return (default is 20).
        /// </param>
        /// <returns>
        /// A list of matching applications, ordered by their match score in descending order, up to the specified record count.
        /// Each application will have a 'Score' property indicating its fuzzy match score.
        /// </returns>
        /// <remarks>
        /// - The combined title for each application is created by concatenating its Title, ModuleCaption, CategoryLevel1Caption, and CategoryLevel2Caption properties.
        /// - The fuzzy matching algorithm uses the FuzzySharp library.
        /// </remarks>
        public static List<Application> GetFilteredApplication(string input, List<Application> applications, int threshold = 55, int recordCount = 20)
        {
            var combinedTitles = applications
                .Select(app => new
                {
                    Application = app,
                    CombinedTitle = $"{app.Title}-{app.ModuleCaption}-{app.CategoryLevel1Caption}-{app.CategoryLevel2Caption}"
                })
                .ToList();

            var matches = combinedTitles.Select(title => new
            {
                CombinedTitle = title.CombinedTitle,
                Ratio = Fuzz.TokenSortRatio(input, title.CombinedTitle)
            })
                .OrderByDescending(x => x.Ratio)
                .ToList();

            var matchedApplications = matches
                .Select(match =>
                {
                    var combinedTitle = combinedTitles.FirstOrDefault(ct => ct.CombinedTitle == match.CombinedTitle);
                    if (combinedTitle != null)
                    {
                        var application = combinedTitle.Application;
                        application.Score = match.Ratio; // Assign the score
                        return application;
                    }
                    return null;
                })
                .OrderByDescending(x => x.Score)
                .Take(recordCount)
                .ToList();

            return matchedApplications;
        }

        public List<Application> ReadApplicationFile(string filePath)
        {
            List<Application> applications = new List<Application>();
            XmlDocument doc = _xmlService.LoadXmlDocument(filePath);
            XmlNodeList applicationNodes = _xmlService.SelectNodes(doc, "/applications/application");

            foreach (XmlNode applicationNode in applicationNodes)
            {
                Application application = new Application
                {
                    Id = applicationNode.Attributes["id"].Value,
                    Link = _xmlService.SelectSingleNode(applicationNode, "link").InnerText,
                    TermId1 = Convert.ToInt32(_xmlService.SelectSingleNode(applicationNode, "title/caption").Attributes["id"].Value)
                };

                XmlNodeList appCaptions = _xmlService.SelectNodes(applicationNode, "title/caption");
                int index = 1;
                foreach (XmlNode caption in appCaptions)
                {
                    switch (index)
                    {
                        case 1:
                            application.TermId1 = Convert.ToInt32(caption.Attributes["id"].Value);
                            application.Caption1 = caption.InnerText;
                            break;

                        case 2:
                            application.TermId2 = Convert.ToInt32(caption.Attributes["id"].Value);
                            application.Caption2 = caption.InnerText;
                            break;

                        case 3:
                            application.TermId3 = Convert.ToInt32(caption.Attributes["id"].Value);
                            application.Caption3 = caption.InnerText;
                            break;
                    }
                    index++;
                }

                if (string.IsNullOrEmpty(application.Caption1)) continue;

                GetCategoryLevel(ref application, filePath);

                if (string.IsNullOrEmpty(application.ModuleCaption)) continue;

                applications.Add(application);
            }

            return applications;
        }

        private void GetCategoryLevel(ref Application application, string appFilePath)
        {
            string fileName = Path.GetFileName(appFilePath);
            string module = fileName.Replace("Applications.", "").Replace(".xml", "");
            application.Module = module;

            string menuFilePath = Path.Combine(_fileService.GetXmlDirectory(), $"Menu.{module}.xml");
            if (File.Exists(menuFilePath))
            {
                XmlDocument doc = _xmlService.LoadXmlDocument(menuFilePath);
                application.ModuleCaption = _xmlService.SelectSingleNode(doc, "/menu/category/title/caption").InnerText;

                XmlNode element = _xmlService.SelectSingleNode(doc, $"//*[@id='{application.Id}']");
                if (element != null)
                {
                    application.CategoryLevel1 = element.ParentNode.ParentNode.ParentNode.ParentNode.Attributes["id"].Value;
                    application.CategoryLevel1Caption = _xmlService.GetInnerText(element.ParentNode.ParentNode.ParentNode.ParentNode, "title/caption");
                    application.CategoryLevel1TermId = Convert.ToInt32(_xmlService.SelectSingleNode(element.ParentNode.ParentNode.ParentNode.ParentNode, "title/caption").Attributes["id"].Value);

                    application.CategoryLevel2 = element.ParentNode.ParentNode.Attributes["id"].Value;
                    application.CategoryLevel2Caption = element.ParentNode.ParentNode.InnerText;
                    application.CategoryLevel2TermId = Convert.ToInt32(_xmlService.SelectSingleNode(element.ParentNode.ParentNode, "title/caption").Attributes["id"].Value);
                }
            }
        }
    }
}
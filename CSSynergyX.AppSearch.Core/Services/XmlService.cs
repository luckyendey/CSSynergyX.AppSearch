using CSSynergyX.AppSearch.Core.Interfaces;
using System.Xml;

namespace CSSynergyX.AppSearch.Core.Services
{
    public class XmlService : IXmlService
    {
        public XmlDocument LoadXmlDocument(string filePath)
        {
            var doc = new XmlDocument();
            doc.Load(filePath);
            return doc;
        }

        public XmlNode SelectSingleNode(XmlDocument doc, string xPath)
        {
            return doc.SelectSingleNode(xPath);
        }

        public XmlNode SelectSingleNode(XmlNode node, string xPath)
        {
            return node.SelectSingleNode(xPath);
        }

        public XmlNodeList SelectNodes(XmlNode node, string xPath)
        {
            return node.SelectNodes(xPath);
        }

        public string GetInnerText(XmlNode node, string xPath)
        {
            return node.SelectSingleNode(xPath).InnerText;
        }
    }
}
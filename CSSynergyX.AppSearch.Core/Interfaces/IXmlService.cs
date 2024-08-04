using System.Xml;

namespace CSSynergyX.AppSearch.Core.Interfaces
{
    internal interface IXmlService
    {
        XmlDocument LoadXmlDocument(string filePath);

        XmlNode SelectSingleNode(XmlDocument doc, string xPath);

        XmlNode SelectSingleNode(XmlNode node, string xPath);

        XmlNodeList SelectNodes(XmlNode node, string xPath);

        string GetInnerText(XmlNode node, string xPath);
    }
}
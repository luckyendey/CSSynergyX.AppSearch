using System;
using System.Diagnostics;

namespace CSSynergyX.AppSearch.Core.Entities
{
    [DebuggerDisplay("{ModuleCaption}-{CategoryLevel1Caption}-{CategoryLevel2Caption} | {Title}:{Link}", Name = "{Title}")]
    public class Application
    {
        public string Module { get; set; }
        public string ModuleCaption { get; set; }
        public string CategoryLevel1 { get; set; }
        public string CategoryLevel1Caption { get; set; }
        public int CategoryLevel1TermId { get; set; }
        public string CategoryLevel2 { get; set; }
        public string CategoryLevel2Caption { get; set; }
        public int CategoryLevel2TermId { get; set; }
        public string Id { get; set; }
        public int TermId1 { get; set; }
        public int TermId2 { get; set; }
        public int TermId3 { get; set; }
        public string Caption1 { get; set; }
        public string Caption2 { get; set; }
        public string Caption3 { get; set; }

        public string Title
        { get { return string.Join(" ", new[] { Caption1, Caption2, Caption3 }).Trim().Replace(":", ""); } }

        public string Link { get; set; }
        public bool HasAccess { get; set; }
        public string CompositeString
        {
            get { return $"{Title} {ModuleCaption} {CategoryLevel1Caption} {CategoryLevel2Caption}"; }
        }
        public int Score { get; set; }
    }
}
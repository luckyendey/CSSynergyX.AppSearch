using System;
using System.Web.UI.HtmlControls;

namespace CSSynergyX.AppSearch.PageExtension
{
    internal class Portal : Exact.Web.UI.Page.ApplicationExtensionBase
    {
        public override void AfterInit()
        {
            base.AfterInit();
            System.Web.UI.HtmlControls.HtmlGenericControl dMain = (System.Web.UI.HtmlControls.HtmlGenericControl)page.FindControl("dMain");
            if (dMain != null)
            {
                string cacheName = "CSSynergyXApplications";

                if (env.Cache[cacheName] == null)
                {
                    env.Cache.Add(cacheName, CSSynergyX.AppSearch.Core.Tools.GetApplications(), (int)TimeSpan.FromDays(7).TotalMinutes);
                }

                // Create the <script> element
                HtmlGenericControl scriptTag = new HtmlGenericControl("script");
                scriptTag.Attributes.Add("type", "text/javascript");
                scriptTag.Attributes.Add("src", "CSSynergyXAppSearch.js");

                // Add the <script> to the dMain div
                dMain.Controls.Add(scriptTag);
            }
        }
    }
}
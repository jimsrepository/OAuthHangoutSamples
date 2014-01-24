﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomWebApp
{
    public partial class ManageConnection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated || System.Web.HttpContext.Current.User.Identity.Name != "admin")
            {
                Response.Redirect("~/Default.aspx");
            }
            loadVisibleSections();
        }

        protected void disconnect_Click(object sender, EventArgs e)
        {
            try
            {
                RestHelper.disconnectRealm(IppRealmOAuthProfile.Read());
            }
            catch
            {

            }
            finally { loadVisibleSections(); }
        }

        private void loadVisibleSections()
        {
            notLoggedIn.Visible = false;
            notConnected.Visible = false;
            connected.Visible = false;
            appTokensNotFound.Visible = false;
            if (!RestHelper.appTokensSet())
            {
                appTokensNotFound.Visible = true;
            }
            else if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                notLoggedIn.Visible = true;
            }
            else
            {
                notConnected.Visible = IppRealmOAuthProfile.Read().accessToken == "";
                connected.Visible = IppRealmOAuthProfile.Read().accessToken != "";
            }
            (this.Master as SiteMaster).ToggleBlueDotMenuVisibility();
        }
    }
}
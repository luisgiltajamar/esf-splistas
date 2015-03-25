using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace ManejoListas.VisualWebPart1
{
    [ToolboxItemAttribute(false)]
    public partial class VisualWebPart1 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public VisualWebPart1()
        {
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            using (var web=SPContext.Current.Web)
            {
               lstListas.Items.Clear();
                foreach (SPList lista in web.Lists)
                {
                    lstListas.Items.Add(new ListItem(lista.Title,
                        lista.ID.ToString()));
                }

            }
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void lstItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstItems.SelectedIndex != -1)
            {
               using (SPWeb web=SPContext.Current.Web)
               {
                   var lista = ViewState["lista"] as string;

                   var list = web.Lists[lista];
                   var item = list.Items[new Guid(lstItems.SelectedValue)];

                   informacion.Text = "";
                   foreach (SPField field in item.Fields)
                   {
                       informacion.Text += field.Title + "-->" + 
                           field.ToString();
                   }
               }

            }


        }

        protected void lstListas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstListas.SelectedIndex != -1)
            {
               
                using (SPWeb web=SPContext.Current.Web)
                {
                    String select = lstListas.SelectedItem.Text;
                    ViewState["lista"] = select;
                    SPList lista = web.Lists[select];
                    lstItems.Items.Clear();
                    foreach (SPListItem item in lista.Items)
                    {
                        lstItems.Items.Add(new ListItem(item.Name,
                            item.UniqueId.ToString()));
                    }
                }
            }
        }
    }
}

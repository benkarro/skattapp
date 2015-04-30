using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using App3.PhoneApp.Resources;
using Microsoft.Phone.Tasks;
using App3;

namespace App3.PhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        public void CreateAppBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton callButton = new ApplicationBarIconButton(new Uri("/Toolkit.Content/ApplicationBar.Check.png", UriKind.RelativeOrAbsolute));
            /*cancelbutton.Text = AppResources.AppBarButtonText2;
            cancelbutton.Click += CancelNewServerInstance_Click;*/
            callButton.Text = "SOL";
            callButton.Click += callButton_Click;

            ApplicationBarIconButton confirmbutton = new ApplicationBarIconButton(new Uri("/Resources/AppBarIcons/share.png", UriKind.RelativeOrAbsolute));
            /*confirmbutton.Text = AppResources.AppBarButtonText3;
            confirmbutton.Click += AddNewServerInstance_Click;*/

            ApplicationBar.Buttons.Add(callButton);
            ApplicationBar.Buttons.Add(confirmbutton);
        }

        void callButton_Click(object sender, EventArgs e)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();

            phoneCallTask.PhoneNumber = App3.Resources.Strings.SkatteopplysningenTLF;
            phoneCallTask.DisplayName = "Gage";

            phoneCallTask.Show();
        }
       
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
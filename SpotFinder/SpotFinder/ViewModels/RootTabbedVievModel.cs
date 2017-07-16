using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SpotFinder.ViewModels
{
    public class RootTabbedVievModel
    {
        private INavigation Navigation { get; }
        private Page Page { get; }

        public RootTabbedVievModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async void CheckPermissionAsync()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
                if(status != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Location))
                        status = results[Permission.Location];
                }
                int index = Navigation.NavigationStack.Count - 1;
                var currPage = Navigation.NavigationStack[index];
                await currPage.DisplayAlert("Location needed!", "You have to turn on location permission fot this app!", "Ok");
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception from CorssPermissions");
            }
        }
    }
}

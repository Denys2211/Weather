using Xamarin.Essentials;

namespace Weather.Services
{
    public static class PermissionsService
    {
        public static async void CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return;

            if (status == PermissionStatus.Denied)
            {
                await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }
        }
    }
}

﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Weather.Services
{
    public class ApiCallerGeocoding
    {
        internal async Task<Location> GetLocationGPS()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);
                return location;

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Coordinates Info", ex.Message, "OK");
                return null;
            }
        }

        internal async Task<string> GetCity(Location location)
        {
            var places = await Geocoding.GetPlacemarksAsync(location);
            var currentPlace = places?.FirstOrDefault();
            if (currentPlace != null)
                return $"{currentPlace.Locality}";
            return null;
        }

        internal async Task GetCoordinatesFromCityName(string address)
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(address);

                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Coordinates Info", ex.Message, "OK");
            }
        }
    }
}

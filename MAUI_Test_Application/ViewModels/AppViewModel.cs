using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductPriceNotificationApp.ViewModels;
using Xamarin.Essentials;

namespace ProductPriceNotificationApp.ViewModels
{
    // Class to represent the view model for the app
    public class AppViewModel : INotifyPropertyChanged
    {
        // Fields
        private decimal _price;
        private decimal _newPrice;

        // Properties
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public decimal NewPrice
        {
            get => _newPrice;
            set
            {
                _newPrice = value;
                OnPropertyChanged();
            }
        }
        public string ProductId { get; set; }

        // Commands
        public ICommand GetPriceCommand { get; }
        public ICommand SetPriceCommand { get; }

        // Constructor
        public AppViewModel()
        {
            // Initialize commands
            GetPriceCommand = new Command(async () => await GetPrice());
            SetPriceCommand = new Command(async () => await SetPrice());
        }

        // Method to get the current price of the product
        private async Task GetPrice()
        {
            // TODO: Retrieve the current price of the product
            // using the ProductId property

            // Update the Price property with the current price
            Price = 123.45m;
        }

        // Method to set the new price for the product
        private async Task SetPrice()
        {
            // TODO: Set the new price for the product
            // using the ProductId and NewPrice properties

            // Notify the user that the price has been updated
            await Application.Current.MainPage.DisplayAlert("Success", "The price has been updated.", "OK");
        }

        // Implementation of INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
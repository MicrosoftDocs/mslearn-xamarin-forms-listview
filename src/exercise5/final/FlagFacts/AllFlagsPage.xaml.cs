using FlagData;
using System;
using Xamarin.Forms;

namespace FlagFacts
{
    public partial class AllFlagsPage : ContentPage
    {
        bool isEditing;
        ToolbarItem cancelEditButton;

        public AllFlagsPage()
        {
            BindingContext = DependencyService.Get<FlagDetailsViewModel>();
            InitializeComponent();

            cancelEditButton = (ToolbarItem)Resources[nameof(cancelEditButton)];

            DataTemplate dt = new DataTemplate(typeof(ImageCell));
            dt.SetBinding(TextCell.TextProperty, new Binding(nameof(Flag.DateAdopted), 
                stringFormat: "Adopted on {0:d}"));

            dt.SetBinding(ImageCell.ImageSourceProperty,
               new Binding(nameof(Flag.ImageUrl), BindingMode.Default,
                  converter: new Converters.EmbeddedImageConverter { ResolvingAssemblyType = typeof(Flag) }));
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!isEditing)
            {
                await this.Navigation.PushAsync(new FlagDetailsPage());
            }
            else
            {
                var flag = (Flag)e.Item;
                if (flag != null && await this.DisplayAlert("Confirm",
                        $"Are you sure you want to delete {flag.Country}?", "Yes", "No"))
                {
                    DependencyService.Get<FlagDetailsViewModel>()
                        .Flags.Remove(flag);
                }
                // Reset the edit button
                OnEdit(cancelEditButton, EventArgs.Empty);
            }
        }

        private void OnEdit(object sender, EventArgs e)
        {
            var tbItem = sender as ToolbarItem;
            isEditing = (tbItem == editButton);

            ToolbarItems.Remove(tbItem);
            ToolbarItems.Add(isEditing ? cancelEditButton : editButton);
        }

        private async void OnRefreshing(object sender, EventArgs e)
        {
            try
            {
                var collection = DependencyService.Get<FlagDetailsViewModel>().Flags;
                int i = collection.Count - 1, j = 0;
                while (i > j)
                {
                    var temp = collection[i];
                    collection[i] = collection[j];
                    collection[j] = temp;
                    i--; j++;
                    await System.Threading.Tasks.Task.Delay(200); // make it take some time.
                }
            }
            finally
            {
                // Turn off the refresh.
                ((ListView)sender).IsRefreshing = false;
            }
        }
    }
}
using FlagData;
using Xamarin.Forms;

namespace FlagFacts
{
    public partial class AllFlagsPage : ContentPage
    {
        public AllFlagsPage()
        {
            BindingContext = DependencyService.Get<FlagDetailsViewModel>();
            InitializeComponent();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //DependencyService.Get<FunFlactsViewModel>().CurrentFlag = (Flag)e.Item;
            await this.Navigation.PushAsync(new FlagDetailsPage());
        }
    }
}
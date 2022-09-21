using Microsoft.AspNetCore.Components;

namespace CryptoMarket.Hub.Client.Shared
{
    public partial class LoginDisplay
    {
        [Inject] public NavigationManager Navigation { get; set; }
        [Parameter] public string ReturnUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ReturnUrl = Navigation.ToBaseRelativePath(Navigation.Uri);
        }
    }
}

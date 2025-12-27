namespace DoomMapGuessr.Services
{

    public interface INavigationService
    {

        void NavigateTo(string pageName);

        void NavigateTo(string pageName, object? parameter);

    }

}

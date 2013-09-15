using Terminal.Interface;
using Terminal.Interface.GUI;

namespace HyperToken.WinFormsGUI.Menus
{
    public class AboutMenu : IMainMenuExtension
    {
        private readonly IAboutBox _aboutBox;
        private LightMenu _menu;

        public AboutMenu(IAboutBox aboutBox)
        {
            _aboutBox = aboutBox;
        }

        public LightMenu Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new LightMenu("About");
                    _menu.Clicked += (sender, args) => _aboutBox.Open();
                }

                return _menu;
            }
        }
    }
}

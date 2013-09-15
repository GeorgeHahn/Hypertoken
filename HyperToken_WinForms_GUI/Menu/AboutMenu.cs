using Terminal.Interface;
using Terminal.Interface.GUI;

namespace HyperToken.WinFormsGUI.Menu
{
    public class AboutMenu : IMainMenuExtension
    {
        private readonly IAboutBox _aboutBox;
        private Terminal.Interface.GUI.Menu _menu;

        public AboutMenu(IAboutBox aboutBox)
        {
            _aboutBox = aboutBox;
        }

        public Terminal.Interface.GUI.Menu Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new Terminal.Interface.GUI.Menu("About");
                    _menu.Clicked += (sender, args) => _aboutBox.Open();
                }

                return _menu;
            }
        }
    }
}

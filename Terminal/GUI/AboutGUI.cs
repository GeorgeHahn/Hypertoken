using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terminal.Interface.GUI;
using Terminal.Interface;

namespace Terminal.GUI
{
    public class AboutGUI : IMainMenuExtension
    {
        private readonly IAboutBox _aboutBox;
        private Menu _menu;

        public AboutGUI(IAboutBox aboutBox)
        {
            _aboutBox = aboutBox;
        }

        public Menu Menu
        {
            get
            {
                if (_menu == null)
                {
                    _menu = new Menu("About");
                    _menu.Clicked += (sender, args) => _aboutBox.Open();
                }

                return _menu;
            }
        }
    }
}

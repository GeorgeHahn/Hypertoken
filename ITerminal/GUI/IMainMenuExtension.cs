using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Terminal.Interface.GUI
{
    public interface IMainMenuExtension
    {
        LightMenu Menu { get; }
    }

    public class LightMenu : INotifyPropertyChanged
    {
        public string Text { get; set; }

        public ObservableCollection<LightMenu> Items { get; private set; }

        public bool Checked { get; set; }

        public bool Highlight { get; set; }

        public event ItemsListOpeningEventHandler ItemsListOpening;

        public event ItemClickedEventHandler ItemClicked;

        public event MenuClickedEventHandler Clicked;

        public LightMenu(string text)
        {
            Text = text;
            Items = new ObservableCollection<LightMenu>();
        }

        public LightMenu()
            : this(string.Empty)
        { }

        public LightMenu(string text, ItemClickedEventHandler clicked)
        {
            ItemClicked += clicked;
            Text = text;
        }

        public void FireItemClicked(object sender, ItemClickedEventArgs args)
        {
            if (ItemClicked != null)
                ItemClicked(sender, args);
        }

        public void FireItemClicked(object sender, string itemName)
        {
            if (ItemClicked != null)
                ItemClicked(sender, new ItemClickedEventArgs(itemName));
        }

        public void FireItemListOpening(object sender, EventArgs args)
        {
            if (ItemsListOpening != null)
                ItemsListOpening(sender, args);
        }

        public void FirePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(sender, args);
        }

        public void FireClicked(object sender, EventArgs e)
        {
            if (Clicked != null)
                Clicked(sender, e);
        }

        public static int GetIndex(ObservableCollection<LightMenu> list, string menu)
        {
            for (int index = 0; index < list.Count; index++)
            {
                var item = list[index];
                if (item.Text == menu)
                    return index;
            }

            return -1;
        }

        public void AddRange(IEnumerable<LightMenu> menus)
        {
            foreach (var menu in menus)
                Items.Add(menu);

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs("Items"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public delegate void ItemClickedEventHandler(object sender, ItemClickedEventArgs args);

    public class ItemClickedEventArgs
    {
        public string ClickedItem { get; set; }

        public ItemClickedEventArgs()
        { }

        public ItemClickedEventArgs(string clickedItem)
        {
            ClickedItem = clickedItem;
        }
    }

    public delegate void ItemsListOpeningEventHandler(object sender, EventArgs args);

    public delegate void MenuClickedEventHandler(object sender, EventArgs args);
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace Terminal_GUI_Interface
{
	public interface IMainMenuExtension
	{
		Menu Menu { get; }
	}

	public class Menu
	{
		public string Text { get; set; }

		public ObservableCollection<Menu> Items { get; private set; }

		private bool _checked;

		public bool Checked
		{
			get { return _checked; }
			set
			{
				_checked = value;
				if (CheckStateChanged != null)
					CheckStateChanged(this, new EventArgs());
			}
		}

		public event ItemsListOpeningEventHandler ItemsListOpening;

		public event ItemClickedEventHandler ItemClicked;

		public event CheckStateChangedEventHandler CheckStateChanged;

		public Menu(string text)
		{
			Text = text;
			Items = new ObservableCollection<Menu>();
		}

		public Menu()
			: this(string.Empty)
		{ }

		public Menu(string text, ItemClickedEventHandler clicked)
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

		internal static int GetIndex(ObservableCollection<Menu> list, string menu)
		{
			for (int index = 0; index < list.Count; index++)
			{
				var item = list[index];
				if (item.Text == menu)
					return index;
			}

			return -1;
		}
	}

	public delegate void CheckStateChangedEventHandler(object sender, EventArgs args);

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
}
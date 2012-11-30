using System;
using System.Reflection;
using System.Windows.Forms;
using Terminal_Interface;

namespace HyperToken_WinForms_GUI
{
	public partial class AboutBox : Form, IAboutBox
	{
		public AboutBox()
		{
			InitializeComponent();
			Text = String.Format("About {0}", AssemblyTitle);
			labelProductName.Text = AssemblyProduct;
			labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
			labelCopyright.Text = AssemblyCopyright;
			labelCompanyName.Text = AssemblyCompany;
			textBoxDescription.AppendText("\n");

			var referencedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assem in referencedAssemblies)
			{
				var name = assem.GetName();
				textBoxDescription.AppendText(string.Format("{0}: {1}\n", name.Name, name.Version));
			}

			//textBoxDescription.Text = Assembly.GetExecutingAssembly().GetName().Version;

			//this.textBoxDescription.Text = AssemblyDescription;
		}

		public void Open()
		{
			this.ShowDialog();
		}

		#region Assembly Attribute Accessors

		public static string AssemblyTitle
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
				if (attributes.Length > 0)
				{
					AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
					if (!string.IsNullOrEmpty(titleAttribute.Title))
					{
						return titleAttribute.Title;
					}
				}
				return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
			}
		}

		public static string AssemblyVersion
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}
		}

		public static string AssemblyDescription
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyDescriptionAttribute)attributes[0]).Description;
			}
		}

		public static string AssemblyProduct
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyProductAttribute)attributes[0]).Product;
			}
		}

		public static string AssemblyCopyright
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
		}

		public static string AssemblyCompany
		{
			get
			{
				object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
				if (attributes.Length == 0)
				{
					return "";
				}
				return ((AssemblyCompanyAttribute)attributes[0]).Company;
			}
		}

		#endregion Assembly Attribute Accessors

		private void CloseForm(object sender, EventArgs e)
		{
			//LogLine("About form closing");
			Close();
		}

		private void ChangeTransparency(object sender, EventArgs e)
		{
			//this.Opacity = Math.Abs((Math.Sin(transloc) / 2)) + .45;

			//transloc += .1;
			//if (transloc > 3.14)
			//    transloc = 0;
		}
	}
}
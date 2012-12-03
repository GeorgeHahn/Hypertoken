using System.Windows.Forms;

namespace HyperToken_WinForms_GUI
{
	public partial class SettingsForm : Form
	{
		public SettingsForm(object[] bauds, object[] dataBits, object[] stopBits, string[] COMPorts, string[] dataParities, string[] flowControls)
		{
			InitializeComponent();
			comboBoxBaud.Items.AddRange(bauds);
			comboBoxDataBits.Items.AddRange(dataBits);
			comboBoxStopBits.Items.AddRange(stopBits);
			comboBoxCOMPort.Items.AddRange(COMPorts);
			comboBoxParity.Items.AddRange(dataParities);
			comboBoxFlowControl.Items.AddRange(flowControls);

			Visible = false;
		}

		public enum Settings
		{
			None = 0,
			COMPort = 1,
			BaudRate = 2,
			FlowControl = 3,
			DataBits = 4,
			DataParity = 5,
			StopBits = 6,
			SettingsHandler = 7
		}

		public delegate void OnSettingChangeEvent(object sender, System.EventArgs e);

		public event OnSettingChangeEvent OnSettingChange;

		public string SettingToString(Settings setting)
		{
			switch (setting)
			{
				case Settings.DataParity:
					return "Parity";
				case Settings.StopBits:
					return "StopBits";
				case Settings.DataBits:
					return "DataBits";
				case Settings.FlowControl:
					return "FlowControl";
				case Settings.COMPort:
					return "currentDevice";
				case Settings.BaudRate:
					return "BaudRate";
				case Settings.SettingsHandler:
					return "SettingsHandler";
				default:
					return "No setting with this value exists";
			}
		}

		private void SettingChanged(object sender, System.EventArgs e)
		{
			//TODO  Give each combobox a correct name, then just pass the sender to the eventhandler rather than go through the ugly if chain below
			int whichSetting = 0;
			string settingValue = null;
			if (sender.Equals(comboBoxCOMPort))
			{
				whichSetting = (int)Settings.COMPort;
				settingValue = comboBoxCOMPort.Text;
			}
			else
				if (sender.Equals(comboBoxBaud))
				{
					whichSetting = (int)Settings.BaudRate;
					settingValue = comboBoxBaud.Text;
				}
				else
					if (sender.Equals(comboBoxFlowControl))
					{
						whichSetting = (int)Settings.FlowControl;
						settingValue = comboBoxFlowControl.Text;
					}
					else
						if (sender.Equals(comboBoxDataBits))
						{
							whichSetting = (int)Settings.DataBits;
							settingValue = comboBoxDataBits.Text;
						}
						else
							if (sender.Equals(comboBoxParity))
							{
								whichSetting = (int)Settings.DataParity;
								settingValue = comboBoxParity.Text;
							}
							else
								if (sender.Equals(comboBoxStopBits))
								{
									whichSetting = (int)Settings.StopBits;
									settingValue = comboBoxStopBits.Text;
								}
								else
									if (sender.Equals(radioButtonForm) || sender.Equals(radioButtonMenu))
									{
										whichSetting = (int)Settings.SettingsHandler;
										if (radioButtonForm.Checked)
											settingValue = "form";
										else
											settingValue = "menu";
									}
									else
										return;

			string[] settings = new string[2];
			settings[0] = SettingToString((Settings)whichSetting);
			settings[1] = settingValue;
			if (OnSettingChange != null)
				OnSettingChange(settings, e);
		}

		public void ChangeSetting(Settings setting, string settingValue)
		{
			switch (setting)
			{
				case Settings.BaudRate:
					comboBoxBaud.Text = settingValue;
					break;

				case Settings.COMPort:
					comboBoxCOMPort.Text = settingValue;
					break;

				case Settings.DataBits:
					comboBoxDataBits.Text = settingValue;
					break;

				case Settings.DataParity:
					comboBoxParity.Text = settingValue;
					break;

				case Settings.FlowControl:
					comboBoxFlowControl.Text = settingValue;
					break;

				case Settings.StopBits:
					comboBoxStopBits.Text = settingValue;
					break;

				case Settings.SettingsHandler:
					if (settingValue == "menu")
					{
						radioButtonMenu.Checked = true;
						radioButtonForm.Checked = false;
					}
					else
						if (settingValue == "form")
						{
							radioButtonMenu.Checked = false;
							radioButtonForm.Checked = true;
						}
					break;
				default:
					break;
			}
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			//TODO Change this name!!
			Hide();
			tabControl1.SelectedIndex = 0;
		}
	}
}
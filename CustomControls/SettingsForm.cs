using System.Windows.Forms;

namespace HyperToken
{
    public partial class SettingsForm : Form
    {
        SettingsHandler settings;

        public SettingsForm(SettingsHandler settings)
        {
            InitializeComponent();

            this.settings = settings;

            /*CreateComboBoxFrom(ref comboBoxParity, settings.dataParityValues, "Parity", SettingsHandler.MenuType.comboBox, ChangeCOMParam);
            CreateComboBoxFrom(ref comboBoxStopBits, settings.stopBitsValues, "StopBits", MenuType.menu, ChangeCOMParam);
            CreateComboBoxFrom(ref comboBoxDataBits, settings.dataBitsValues, "DataBits", MenuType.menu, ChangeCOMParam);
            CreateComboBoxFrom(ref comboBoxFlowControl, settings.flowControlValues, "FlowControl", MenuType.menu, ChangeCOMParam);
            CreateComboBoxFrom(ref comboBoxCOMPort, System.IO.Ports.SerialPort.GetPortNames(), "COMPort", MenuType.menu, ChangeCOMParam);
            CreateComboBoxFrom(ref comboBoxBaud, settings.baudRateValues, "BaudRate", MenuType.menu, ChangeCOMParam);

            CreateComboBoxFrom(ref numericUpDownCharacterDelay, settings.characterDelayValues, menuItemCharacterDelay, "CharacterDelay", MenuType.menu, ChangeCOMParam);
            CreateComboBoxFrom(ref numericUpDownLineDelay, settings.lineDelayValues, menuItemLineDelay, "LineDelay", MenuType.menu, ChangeCOMParam);
            */
        }

        private bool CreateComboBoxFrom(ComboBox menu, object[] values, string name, SettingsHandler.MenuType type, System.EventHandler clickHandler)
        {
            //bool hr = SettingsHandler.PublicCreateMenuFrom(null, values, menu, name, type, clickHandler);
            //settings.addMenu(ref items);
            //return hr;
            return false;
        }
    }
}

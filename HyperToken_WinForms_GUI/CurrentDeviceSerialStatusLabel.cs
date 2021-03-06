﻿using System.Windows.Forms;
using Terminal.Interface;
using Terminal.Interface.GUI;

namespace HyperToken.WinForms
{
    public class CurrentDeviceSerialStatusLabel : IStatusbarExtension
    {
        private readonly CurrentDataDevice _terminal;

        private ToolStripStatusLabel _item;

        public CurrentDeviceSerialStatusLabel(CurrentDataDevice terminal)
        {
            _terminal = terminal;
            _terminal.PropertyChanged += (sender, args) =>
                                             {
                                                 if (args.PropertyName == "CurrentDevice")
                                                 {
                                                     SetDataDevicePropertyChanged();
                                                     UpdateLabel();
                                                 }
                                             };

            SetDataDevicePropertyChanged();
        }

        private void SetDataDevicePropertyChanged()
        {
            if (_terminal.CurrentDevice == null)
                return;

            _terminal.CurrentDevice.PropertyChanged +=
                (sender, args) => { if (args.PropertyName == "DeviceName") UpdateLabel(); };
        }

        private void UpdateLabel()
        {
            _item.Text = _terminal.CurrentDevice.FriendlyName;
        }

        public ToolStripItem StatusBarItem
        {
            get
            {
                if (_item == null)
                {
                    _item = new ToolStripStatusLabel("Current Device");
                }
                return _item;
            }
        }
    }
}
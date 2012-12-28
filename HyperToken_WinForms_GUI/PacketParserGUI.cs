using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Anotar;
using Terminal_GUI_Interface;
using Terminal_Interface;
using NLog;

namespace HyperToken_WinForms_GUI
{
    public class PacketParserGUI : GenericSettingsMenu, IMainMenuExtension
    {
        private readonly IEnumerable<IPacketInterpreter> _parsers;
        private readonly CurrentPacketParser _handler;

        public PacketParserGUI(IEnumerable<IPacketInterpreter> parsers, CurrentPacketParser handler)
        {
            _parsers = parsers;
            _handler = handler;
            handler.PropertyChanged += (sender, args) => UpdateCheckedStates(args.PropertyName);
        }

        protected override dynamic Values
        {
            get
            {
                return (from parser in _parsers
                       select parser.Name).ToArray();
            }
        }

        protected override string MenuName
        {
            get { return "Parser"; }
        }

        protected override dynamic ItemValue
        {
            get { return _handler.CurrentParser.Name; }
            set
            {
                var currentParser = from parser in _parsers
                                    where parser.Name == value
                                    select parser;

                if (currentParser == null)
                    return;

                _handler.CurrentParser = currentParser.First();
                Log.Debug("Current parser: {0}", _handler.CurrentParser.Name);
            }
        }

        protected override string PropertyName
        {
            get { return "CurrentParser"; }
        }
    }
}

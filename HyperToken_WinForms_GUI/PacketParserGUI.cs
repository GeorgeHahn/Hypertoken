using System.Collections.Generic;
using System.Linq;
using Anotar.NLog;
using Terminal.Interface;
using Terminal.Interface.GUI;

namespace HyperToken.WinFormsGUI
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
                var oldParser = _handler.CurrentParser;
                var currentParser = _parsers.FirstOrDefault(parser => parser.Name == value);

                if (currentParser == null)
                {
                    LogTo.Error("Parser {0} not found", (string)value);
                    return;
                }

                _handler.CurrentParser = currentParser;
                _handler.CurrentParser.Create();
                oldParser.Release();
                LogTo.Debug("Current parser: {0}", _handler.CurrentParser.Name);
            }
        }

        protected override string PropertyName
        {
            get { return "CurrentParser"; }
        }
    }
}

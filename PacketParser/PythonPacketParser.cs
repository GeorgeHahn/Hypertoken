using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anotar;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using NLog;
using Terminal_Interface;

namespace PacketParser
{
    public class PythonPacketParser : IPacketInterpreter
    {
        private readonly ScriptRuntime _runtime;
        private dynamic _script;

        public PythonPacketParser()
        {
            Log.Debug("PythonPacketParser created");
            _runtime = Python.CreateRuntime();

            var scriptDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (scriptDirectory == null)
                throw new NullReferenceException("ScriptDirectory should not be null");

            Log.Debug("Looking for python decoder in {0}", scriptDirectory);
            var watcher = new FileSystemWatcher(scriptDirectory, "*.py");

            watcher.Changed += (sender, args) => UpdateScript();
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.EnableRaisingEvents = true;
            UpdateScript();

            try
            {
                Log.Debug(InterpretPacket((new UTF8Encoding()).GetBytes("Hello from Python")));
            }
            catch (Exception e)
            {
                Log.WarnException("Interpreting python script threw an error", e);
            }
        }

        private void UpdateScript()
        {
            Log.Debug("Reloaded interpreter.py");

            //scriptStr = File.ReadAllText("interpreter.py");
            //source = engine.CreateScriptSourceFromString(scriptStr, "py");

            string path = "interpreter.py";
            if (File.Exists(path))
                _script = _runtime.UseFile(path);
        }

        public string InterpretPacket(byte[] packet)
        {
            // UpdateScript();
            try
            {
                return _script.Parse(packet);

                //return engine.Operations.InvokeMember(scope.GetVariable("parse"), "parse", new object[] { packet });
                //scope.SetVariable("packet", packet);
                //return engine.Execute<string>(scriptStr, scope);
            }
            catch (Exception e)
            {
                return string.Format("Script error: {0}\r\n", e.Message);
            }
        }

        public string Name { get { return "Scriptable parser"; }}
    }
}
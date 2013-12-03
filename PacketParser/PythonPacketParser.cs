using System;
using System.IO;
using System.Text;
using Anotar.NLog;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Terminal.Interface;

namespace PacketParser
{
    public class PythonPacketParser : IPacketInterpreter
    {
        private ScriptRuntime _runtime;
        private dynamic _script;
        private FileSystemWatcher watcher;
        private string name;
        private string version;
        private string author;

        public void Create()
        {
            LogTo.Debug("PythonPacketParser created");
            _runtime = Python.CreateRuntime();

            var scriptDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (scriptDirectory == null)
                throw new NullReferenceException("ScriptDirectory should not be null");

            LogTo.Debug("Looking for python decoder in {0}", scriptDirectory);
            watcher = new FileSystemWatcher(scriptDirectory, "*.py");

            watcher.Changed += (sender, args) => UpdateScript(args.FullPath);
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.EnableRaisingEvents = true;
            UpdateScript("interpreter.py");

            try
            {
                LogTo.Debug(InterpretPacket((new UTF8Encoding()).GetBytes("Hello from Python")));
            }
            catch (Exception e)
            {
                LogTo.WarnException("Interpreting python script threw an error", e);
            }
        }

        public void Release()
        {
            LogTo.Debug("Releasing PythonPacketParser");
            _runtime = null;
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
            watcher = null;
        }

        private void UpdateScript(string script)
        {
            LogTo.Debug("A script was changed - {0}", script);

            if (File.Exists(script))
            {
                _runtime.Shutdown();
                _runtime = Python.CreateRuntime();
                try
                {
                    _script = _runtime.UseFile(script);
                    LogTo.Info("Script loaded");
                    ScriptScope scope = _script;
                    dynamic parser = scope.GetVariable("ParserScript");
                    name = parser.Name;
                    version = parser.Version;
                    author = parser.Author;
                    LogTo.Info("Name: {0}, Version: {1}, Author: {2}", name, version, author);
                }
                catch (SyntaxErrorException e)
                {
                    LogTo.ErrorException("Syntax error", e);
                }
                catch (Exception e)
                {
                    LogTo.WarnException("Script error", e);
                }
            }
            else
            {
                LogTo.Error("Script not loaded (file doesn't exist)");
            }
        }

        public string InterpretPacket(byte[] packet)
        {
            try
            {
                return _script.Parse(packet);
            }
            catch (NullReferenceException e)
            {
                return string.Format("Invalid script");
            }
            catch (Exception e)
            {
                return string.Format("Script error: {0}\r\n", e.Message);
            }
        }

        public string InterpretPacket(byte[] packet, byte[] unparsedPacket)
        {
            return InterpretPacket(packet);
        }

        public string Name { get { return name ?? "Scriptable parser"; }}
    }
}
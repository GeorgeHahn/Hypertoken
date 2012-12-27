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
        private ScriptRuntime runtime;
        private dynamic script;

        public PythonPacketParser()
        {
            Log.Debug("PythonPacketParser created");
            runtime = Python.CreateRuntime();

            //engine = Python.CreateEngine();
            //scope = engine.CreateScope();

            var watcher = new FileSystemWatcher(@"C:\Users\ghahn\Documents\GitHub\Hypertoken\Terminal\bin\Debug\", "*.py");

            watcher.Changed += (sender, args) => UpdateScript();
            watcher.EnableRaisingEvents = true;
            UpdateScript();

            Log.Debug(InterpretPacket((new UTF8Encoding()).GetBytes("Hello world")));
        }

        private void UpdateScript()
        {
            Log.Debug("Reloaded interpreter.py");

            //scriptStr = File.ReadAllText("interpreter.py");
            //source = engine.CreateScriptSourceFromString(scriptStr, "py");
            script = runtime.UseFile("interpreter.py");
        }

        public string InterpretPacket(byte[] packet)
        {
            // UpdateScript();
            try
            {
                return script.Parse(packet);

                //return engine.Operations.InvokeMember(scope.GetVariable("parse"), "parse", new object[] { packet });
                //scope.SetVariable("packet", packet);
                //return engine.Execute<string>(scriptStr, scope);
            }
            catch (Exception e)
            {
                return string.Format("Script error: {0}\r\n", e.Message);
            }
        }
    }
}
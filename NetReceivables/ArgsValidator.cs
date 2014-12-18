using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetReceivables
{
    class ArgsValidator
    {
        public ArgsValidator(IEnumerable<string> args)
        {
            var argsList = args.ToList();
            if (argsList.Count != ArgsCount) throw new ArgumentException("Incorrect number of arguments supplied.", "args");
            var currentDir = Directory.GetCurrentDirectory();
            if (argsList.Any(path => !File.Exists(path) && !File.Exists(currentDir + path)))
            {
                throw new ArgumentException("File does not exist.", "path");
            }
            XRefPath = argsList.ElementAt(0);
            ReceivablesPath = argsList.ElementAt(1);
            PayablesPath = argsList.ElementAt(2);
        }

        #region Fields

        private const int ArgsCount = 3;

        #endregion

        #region Properties

        public string XRefPath { get; private set; }

        public string ReceivablesPath { get; private set; }

        public string PayablesPath { get; private set; }

        #endregion
    }
}

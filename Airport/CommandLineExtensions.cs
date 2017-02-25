using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport
{
    public static class CommandLineExtensions
    {
        public static int ReadInt(this ICommandLine commandLine, string message, bool allowEmpty = false)
        {
            int value = 0;
            string input;
            do
            {
                commandLine.Write(message);
                input = commandLine.Read();

                if (allowEmpty && string.IsNullOrWhiteSpace(input))
                {
                    break;
                }
            }
            while (!int.TryParse(input, out value) || value == 0);

            return value;
        }
    }
}

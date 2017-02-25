using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Airport
{
    public class CommandLineInterface
    {
        private readonly ICommandLine commandLine;
        private readonly Dictionary<string, Action> handlers = new Dictionary<string, Action>(StringComparer.OrdinalIgnoreCase);

        public CommandLineInterface(ICommandLine commandLine)
        {
            this.commandLine = commandLine;
        }

        public void DisplayApplicationName()
        {
            commandLine.Write("Airport.");
        }

        public void DisplayHelp(string text)
        {
            var availableCommands = string.Join(" ", handlers.Keys.OrderBy(key => key));
            //commandLine.Write($"Available commands: {availableCommands}");
            string availableCommandsByWord = null;

            foreach (var item in handlers)
            {
                if (item.Key.Contains(text))
                {
                    availableCommandsByWord += item.Key + " ";
                }
            }

            if (availableCommandsByWord == null)
            {
                availableCommandsByWord = "Command not found. Press enter to continue.";
            }

            commandLine.Write($"Available commands: {availableCommandsByWord}");
        }

        public bool ReturnCommand(string command)
        {
            foreach (var item in handlers)
            {
                if (item.Key.Equals(command, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public void SayGoodBye()
        {
            commandLine.Write("Good bye.");
        }

        public void AddCommand(string name, Action handler)
        {
            handlers.Add(name, handler);
        }

        public void HandleCommand()
        {
            string commandName = commandLine.Read();
            Action handler;

            if (handlers.TryGetValue(commandName, out handler))
            {
                handler();
            }
            else
            {
                DisplayInvalidCommandMessage(commandName);
            }
        }

        public void HandleCommand(string commandName)
        {
            Action handler;

            if (handlers.TryGetValue(commandName, out handler))
            {
                handler();
            }
            else
            {
                DisplayInvalidCommandMessage(commandName);
            }
        }

        private void DisplayInvalidCommandMessage(string commandName)
        {
            commandLine.Write($"Command {commandName} is unknown.");
        }
    }
}
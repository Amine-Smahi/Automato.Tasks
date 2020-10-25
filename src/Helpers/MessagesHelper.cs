using System;

namespace Automato.Helpers
{
    public static class MessagesHelper
    {
        public static void DisplayMessageSameLine(string message)
        {
            Console.Write(message);
        }

        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}

using System;

namespace Automato.Tasks.Helpers
{
    public static class NotificationsHelper
    {
        public static void DisplayDynamicMessage(string message)
        {
            Console.Write(message);
        }

        public static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
using System;
using System.Collections;

using infogrips.util;

namespace ricsc
{
   public class RicsCommander
   {
      public static void Main(string[] args)
      {
         int status = 0;

         try
         {
            Hashtable a = ArgumentUtil.getArguments(args);

            string command = (string)a["command"];

            if (command == null)
            {
               CommandController.command_help();
               status = 1;
            }
            else if (command.Equals("send", StringComparison.InvariantCultureIgnoreCase))
            {
               status = CommandController.command_send(a);
            }
            else if (command.Equals("check_level", StringComparison.InvariantCultureIgnoreCase))
            {
               status = CommandController.command_check_level(a);
            }
            else if (command.Equals("get_log", StringComparison.InvariantCultureIgnoreCase))
            {
               status = CommandController.command_get_log(a);
            }
            else
            {
               CommandController.command_help();
               status = 1;
            }
         }
         catch (Exception e)
         {
            Console.WriteLine(e.ToString());
            status = 1;
         }

         Environment.Exit(status);
      }
   }
}
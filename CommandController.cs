using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using infogrips.service.rics.client;
using infogrips.util;

namespace ricsc
{
   public class CommandController
   {
      private static String RICS_SERVER = "http://www.infogrips.ch/servlet/redirector/checkservice";
      private static char fileSeparator = Path.DirectorySeparatorChar;

      private static void displayTitle()
      {
         Console.WriteLine("RICS Commander Version (.NET) 1.0, (c) infoGrips GmbH, 1994-2020");
         Console.WriteLine("");
      }

      /*-------------------------------------------------------------------------------*/

      public static void command_help()
      {
         displayTitle();
         Console.WriteLine("usage: ricsc.exe -service <service> -user <e-mail>");
         Console.WriteLine("   -command send -file \"<file-pattern>\" -param1 <value1> -param2 <value2> ...");
         Console.WriteLine("   -command check_level -file <file>");
         Console.WriteLine("   -command get_log -file <file>");
      }

      private static bool sendFile(String server, String service, String user, String fname, Hashtable parameters)
      {
         ClientSession s = new ClientSession();
         bool connected = false;
         int buffer_size = 2000000;

         try
         {
            s.connect(server, service, user);
            connected = true;

            using (FileStream fs = File.OpenRead(fname))
            {
               while (true)
               {
                  byte[] b = new byte[buffer_size];
                  int len = fs.Read(b, 0, b.Length);

                  if (len == buffer_size)
                  {
                     // send full chunk
                     s.sendFile(fname, b, parameters);
                  }
                  else if (len > 0)
                  {
                     // send last chunk
                     byte[] bl = new byte[len];
                     for (int i = 0; i < len; i++)
                     {
                        bl[i] = b[i];
                     }
                     s.sendFile(fname, bl, parameters);
                  }
                  else
                  {
                     // close file by sending null
                     s.sendFile(fname, null, parameters);
                     break;
                  }
               }
            }

            s.disconnect();
            connected = false;
            return true;
         }
         catch (Exception e)
         {
            String error = e.Message;
            //error = error.Substring(error.LastIndexOf(":") + 1);
            Console.WriteLine("   error: " + error);
            Console.WriteLine(e.StackTrace);
            if (connected)
            {
               try
               {
                  s.disconnect();
               }
               catch (Exception ee)
               {
                  Console.WriteLine(ee.ToString());
               }
            }
            return false;
         }

      }

      public static int command_send(Hashtable parameters)
      {
         displayTitle();

         parameters.Remove("command");

         String server = (String)parameters["server"];
         parameters.Remove("server");
         if (server == null)
         {
            server = RICS_SERVER;
         }

         String service = (String)parameters["service"];
         parameters.Remove("service");
         if (service == null)
         {
            Console.WriteLine("parameter -service is missing");
            Environment.Exit(1);
         }
         service = service.ToUpper();
         String user = (String)parameters["user"];
         parameters.Remove("user");
         if (user == null)
         {
            Console.WriteLine("parameter -user is missing");
            Environment.Exit(1);
         }
         String file_pattern = (String)parameters["file"];

         parameters.Remove("file");
         if (file_pattern == null)
         {
            Console.WriteLine("parameter -file is missing");
            Environment.Exit(1);
         }

         // extract directory
         String directory;
         int ind = file_pattern.LastIndexOf(fileSeparator);
         if (ind >= 0)
         {
            // extract directory from pattern
            directory = file_pattern.Substring(0, ind);
            file_pattern = file_pattern.Substring(ind + 1);
         }
         else
         {
            // active directory
            directory = ".";
         }
         string[] files = Directory.GetFiles(directory).Select(file => Path.GetFileName(file)).ToArray();

         if (files.Length == 0)
         {
            Console.WriteLine("no files in directory: " + directory + " found.");
            Environment.Exit(1);
         }

         // prepare wildcard matcher
         WildCardMatcher wm = new WildCardMatcher(file_pattern);

         int status = 0;

         for (int i = 0; i < files.Length; i++)
         {

            String fname = files[i];
            if (!wm.matches(fname))
            {
               continue;
            }

            Console.WriteLine("sending " + fname + " to service " + service + " ...");

            string fullFilePath = Path.Combine(directory,fname);
            if (sendFile(server, service, user, fullFilePath, parameters))
            {
               Console.WriteLine("done.");
            }
            else
            {
               Console.WriteLine("not done.");
               status = 1;
            }
            Console.WriteLine("");

         }

         return status;
      }

      public static int command_check_level(Hashtable parameters)
      {
         displayTitle();

         String server = (String)parameters["server"];
         parameters.Remove("server");
         if (server == null)
         {
            server = RICS_SERVER;
         }

         String service = (String)parameters["service"];
         parameters.Remove("service");
         if (service == null)
         {
            Console.WriteLine("parameter -service is missing");
            Environment.Exit(-1);
         }
         String user = (String)parameters["user"];
         parameters.Remove("user");
         if (user == null)
         {
            Console.WriteLine("parameter -user is missing");
            Environment.Exit(-1);
         }
         String file = (String)parameters["file"];
         parameters.Remove("file");
         if (file == null)
         {
            Console.WriteLine("parameter -file is missing");
            Environment.Exit(-1);
         }

         if (!File.Exists(file))
         {
            Console.WriteLine("file " + file + " not found.");
            return -1;
         }

         try
         {
            ClientSession s = new ClientSession();
            s.connect(server, service, user);
            List<object> results = s.getInfo(service, MD5.getFileDigest(file));

            var filtered_results =
               ((IEnumerable)results).Cast<Dictionary<object, object>>().
               Where(rec => rec["check_level"] != null).
               Select((rec) =>
                  new
                  {
                     service = rec["service"],
                     end_time = rec["end_time"],
                     check_level = rec["check_level"],
                  }).
               Distinct().
               ToList();

            foreach (var entry in filtered_results)
            {
               String check_level = null;
               if (entry.end_time.Equals(""))
               {
                  continue;
               }
               if (entry.check_level == null)
               {
                  check_level = "unknown";
               }
               else
               {
                  check_level = "" + entry.check_level;
               }
               Console.WriteLine(
                  file + ":" +
                  " checked with service " + entry.service +
                  " at " + entry.end_time +
                  ", check_level=" + check_level
               );
            }

            s.disconnect();
            return 0; // to do !!!
         }
         catch (Exception e)
         {
            Console.WriteLine("info: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return -1;
         }
      }

      public static int command_get_log(Hashtable parameters)
      {
         displayTitle();

         String server = (String)parameters["server"];
         parameters.Remove("server");
         if (server == null)
         {
            server = RICS_SERVER;
         }

         String service = (String)parameters["service"];
         parameters.Remove("service");
         if (service == null)
         {
            Console.WriteLine("parameter -service is missing");
            Environment.Exit(-1);
         }
         String user = (String)parameters["user"];
         parameters.Remove("user");
         if (user == null)
         {
            Console.WriteLine("parameter -user is missing");
            Environment.Exit(-1);
         }
         String file = (String)parameters["file"];
         parameters.Remove("file");
         if (file == null)
         {
            Console.WriteLine("parameter -file is missing");
            Environment.Exit(-1);
         }

         if (!File.Exists(file))
         {
            Console.WriteLine("file " + file + " not found.");
            return -1;
         }

         try
         {
            ClientSession s = new ClientSession();
            s.connect(server, service, user);
            List<object> results = s.getInfo(service, MD5.getFileDigest(file));
            List<Dictionary<object, object>> filtered_results = new List<Dictionary<object, object>>(results.Count);
            String start_time = "";
            String output_dir_http = "";

            for (int i = 0; i < results.Count; i++)
            {
               Dictionary<object, object> rec = (Dictionary<object, object>)results.ElementAt(i);

               if ((String)rec["output_dir_http"] == null)
               {
                  continue;
               }
               String output_dir_http2 = (String)rec["output_dir_http"];

               if ((String)rec["start_time"] == null)
               {
                  continue;
               }
               String start_time2 = (String)rec["start_time"];

               if (start_time2.CompareTo(start_time) > 0)
               {
                  start_time = start_time2;
                  if (rec["end_time"].Equals(""))
                  {
                     output_dir_http = "";
                     continue;
                  }
                  if (rec["output_file"] != null)
                  {
                     output_dir_http = output_dir_http2 + "/" + rec["output_file"];
                  }
                  else
                  {
                     output_dir_http = output_dir_http2 + "/output.zip";
                  }
               }

            }

            s.disconnect();

            if (!output_dir_http.Trim().Equals(""))
            {
               Console.WriteLine(output_dir_http);
               return 0;
            }
            else
            {
               return 1;
            }
         }
         catch (Exception e)
         {
            Console.WriteLine("info: " + e.Message);
            Console.WriteLine(e.StackTrace);
            return -1;
         }
      }
   }
}
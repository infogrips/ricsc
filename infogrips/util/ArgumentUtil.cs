using System;
using System.Collections;

namespace infogrips.util
{
   public class ArgumentUtil
   {
      public static Hashtable getArguments(string[] argv)
      {
         if (argv == null)
         {
            Console.WriteLine("argv is null");
         }
         bool nameisnext = true;
         String name = "";
         String value = "";
         int argc = 0;

         Hashtable arguments = new Hashtable(10);
         for (int i = 0; i < argv.Length; i++)
         {

            if (nameisnext)
            {
               name = argv[i].Trim();
               if (name.StartsWith("-"))
               {
                  name = name.Substring(1);
                  nameisnext = false;
               }
               else
               {
                  argc++;
                  arguments.Add("arg" + argc, name);
               }
            }
            else
            {
               value = argv[i].Trim();
               if (value.StartsWith("-"))
               {

                  bool doublevalue = false;

                  // check is a minus int or double value
                  try
                  {
                     Double? d = Convert.ToDouble(value);
                     if (d != null) doublevalue = true;
                  }
                  catch (Exception e)
                  {
                     string ex = e.ToString();
                     doublevalue = false;
                  }

                  if (doublevalue)
                  {
                     arguments.Add(name, value);
                  }
                  else
                  {
                     arguments.Add(name, "");
                     i--;
                  }
               }
               else
               {
                  arguments.Add(name, value);
               }
               nameisnext = true;
            }
         }
         if (!nameisnext)
         {
            arguments.Add(name, "");
         }
         return arguments;
      }
   }
}
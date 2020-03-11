using System;
using System.Collections.Generic;
using System.Text;

namespace infogrips.util
{
   public class WildCardMatcher
   {
      String wildPattern = null;
      List<object> pattern = new List<object>();

      const String FIND = "find";
      const String EXPECT = "expect";
      const String ANYTHING = "anything";
      const String NOTHING = "nothing";

      public WildCardMatcher(String wildString)
      {

         wildPattern = wildString;
         wildString = wildString.ToLower();

         // remove duplicate asterisks

         int i = wildString.IndexOf("**");
         while (i >= 0)
         {
            wildString = wildString.Substring(0, i + 1) + wildString.Substring(i + 2);
            i = wildString.IndexOf("**");
         }

         // parse the input string
         char[] seps = { '*' };
         String[] values = wildString.Split(seps);

         String token = null;
         for (int j = 0; j < values.Length; j++)
         {
            token = values[j];

            if (token.Equals("*"))
            {
               pattern.Add(FIND);

               j++;
               if (i < values.Length)
               {
                  token = values[j];
                  pattern.Add(token);
               }
               else
               {
                  pattern.Add(ANYTHING);
               }
            }
            else
            {
               pattern.Add(EXPECT);
               pattern.Add(token);
            }
         }

         if (!token.Equals("*"))
         {
            pattern.Add(EXPECT);
            pattern.Add(NOTHING);
         }
      }

      public bool matches(String name)
      {
         name = name.ToLower();

         // start processing the pattern vector

         bool acceptName = true;

         String command = null;
         String param = null;

         int currPos = 0;
         int cmdPos = 0;

         while (cmdPos < pattern.Count)
         {

            command = (String)pattern[cmdPos];
            param = (String)pattern[cmdPos + 1];

            if (command.Equals(FIND))
            {

               // if we are to find 'anything'
               // then we are done

               if (param.Equals(ANYTHING))
               {
                  break;
               }

               // otherwise search for the param
               // from the curr pos

               int nextPos = name.IndexOf(param, currPos);
               if (nextPos >= 0)
               {
                  // found it
                  currPos = nextPos + param.Length;
               }
               else
               {
                  acceptName = false;
                  break;
               }

            }
            else
            {
               if (command.Equals(EXPECT))
               {

                  // if we are to expect 'nothing'
                  // then we MUST be at the end of the string

                  if (param.Equals(NOTHING))
                  {
                     if (currPos != name.Length)
                     {
                        acceptName = false;
                     }
                     // since we expect nothing else,
                     // we must finish here
                     break;
                  }
                  else
                  {
                     // otherwise, check if the expected string
                     // is at our current position
                     int nextPos = name.IndexOf(param, currPos);
                     if (nextPos != currPos)
                     {
                        acceptName = false;
                        break;
                     }
                     // if we've made it this far, then we've
                     // found what we're looking for
                     currPos += param.Length;
                  }
               }
            }

            cmdPos += 2;
         }

         return acceptName;
      }

      public String toString()
      {
         return wildPattern;
      }

      public String toPattern()
      {
         StringBuilder sb = new StringBuilder("");

         int i = 0;
         while (i < pattern.Count)
         {
            sb.Append("(");
            sb.Append((String)pattern[i]);
            sb.Append(" ");
            sb.Append((String)pattern[i + 1]);
            sb.Append(") ");
            i += 2;
         }

         return sb.ToString();
      }
   }
}
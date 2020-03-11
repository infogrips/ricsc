using System;

namespace infogrips.util
{
   public class Log
   {

      // wrapper class to call a Logger by static methods, i.e. Log.message().

      private static Logger log = null;

      public static void setLogger(Logger log)
      {
         Log.log = log;
      }

      public static Logger getLogger()
      {
         return log;
      }

      public static void setNestLevel(int level)
      {
         log.setNestLevel(level);
      }

      public static int getNestLevel()
      {
         return log.getNestLevel();
      }

      public static void incNestLevel()
      {
         log.incNestLevel();
      }

      public static void decNestLevel()
      {
         log.decNestLevel();
      }

      public static void debug(String message)
      {
         log.debug(message);
      }

      public static void message(String message)
      {
         log.message(message);
      }

      public static void warning(String message)
      {
         log.warning(message);
      }

      public static void error(String message)
      {
         log.error(message);
      }

      public static void error(String message, Exception e)
      {
         log.error(message, e);
      }

      public static void syserr(String message)
      {
         log.syserr(message);
      }

      public static void security(String message)
      {
         log.security(message);
      }
   }
}
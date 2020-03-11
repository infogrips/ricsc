using System;

namespace infogrips.util
{
   public interface Logger
   {
      public void setNestLevel(int level);
      public int getNestLevel();
      public void incNestLevel();
      public void decNestLevel();
      public void debug(String message);
      public void message(String message);
      public void warning(String message);
      public void error(String message);
      public void error(String message, Exception e);
      public void syserr(String message);
      public void security(String message);
   }
}
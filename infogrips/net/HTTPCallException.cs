using System;

namespace infogrips.net
{
   public class HTTPCallException : Exception
   {

      /*----------------------------------------------------------------------------------------*/

      public const int UNKNOWN_ERROR = 1;
      public const int NETWORK_ERROR = 2;
      public const int INTERFACE_ERROR = 3;
      public const int SERVER_ERROR = 4;

      /*----------------------------------------------------------------------------------------*/

      protected int errorcode;
      protected String message;

      public HTTPCallException(int error, String msg = null)
      {
         errorcode = error;
         switch (errorcode)
         {
            case NETWORK_ERROR:
               message = "network error";
               break;
            case INTERFACE_ERROR:
               message = "interface error";
               break;
            case SERVER_ERROR:
               message = "server error";
               break;
            default:
               message = "unknown error";
               break;
         }

         if (msg != null)
         {
            message = msg;
         }
      }

      public String getMessage()
      {
         return message;
      }

      public int getErrorCode()
      {
         return errorcode;
      }
   }

}
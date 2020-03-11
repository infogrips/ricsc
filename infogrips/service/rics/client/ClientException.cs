using System;

namespace infogrips.service.rics.client
{

   public class ClientException : Exception
   {
      public const int UNKNOWN_ERROR = 1;
      public const int NETWORK_ERROR = 2;
      public const int INTERFACE_ERROR = 3;

      public const int INVALID_USER_PASSWORD = 1000;
      public const int INSUFFICIENT_PRIVILEGE = 1001;
      public const int NO_ACTIVE_SESSION = 1002;
      public const int INVALID_USER = 1005;

      private int errorcode;
      private String errormessage = null;

      private ClientException()
      {
      }

      public ClientException(int errorcode)
      {
         if (errorcode >= 100 && errorcode <= 999)
         {
            this.errorcode = INTERFACE_ERROR;
         }
         else
         {
            this.errorcode = errorcode;
         }
         this.errormessage = null;
      }

      public ClientException(int errorcode, String message)
      {
         if (errorcode >= 100 && errorcode <= 999)
         {
            this.errorcode = INTERFACE_ERROR;
         }
         else
         {
            this.errorcode = errorcode;
         }
         this.errormessage = message;
      }

      public String getMessage()
      {
         if (errormessage != null)
         {
            return errormessage;
         }
         String message;
         switch (errorcode)
         {
            case NETWORK_ERROR:
               message = "network error";
               break;
            case INTERFACE_ERROR:
               message = "interface error";
               break;
            case INVALID_USER_PASSWORD:
               message = "invalid user / password";
               break;
            case NO_ACTIVE_SESSION:
               message = "no active session";
               break;
            case INVALID_USER:
               message = "invalid user";
               break;
            default:
               message = "unknown error";
               break;
         }
         return message;
      }

      public int getErrorCode()
      {
         return errorcode;
      }
   }
}
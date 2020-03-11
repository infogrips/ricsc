using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using infogrips.net;

namespace infogrips.service.rics.client
{

   public class ClientSession
   {
      private const string service = "RICS";
      private const string port = "Client";

      private int sessionid;
      private HTTPCallClient connectCall;
      private HTTPCallClient disconnectCall;
      private HTTPCallClient sendFileCall;
      private HTTPCallClient getInfoCall;
      private bool connected = false;

      private List<object> arguments;
      private List<object> results;

      public long getID()
      {
         return (long)sessionid;
      }

      public bool isConnected()
      {
         return connected;
      }

      private void assertConnected()
      {
         if (!connected)
         {
            throw new ClientException
               (ClientException.NO_ACTIVE_SESSION);
         }
      }

      public void connect(String server, String usr, String passwd)
      {
         connectCall = new HTTPCallClient(server, service, port, 0, 0);
         arguments = new List<object>(2);
         arguments.Add(usr);
         arguments.Add(passwd);

         try
         {

            results = connectCall.execute(arguments);
            this.sessionid = (int)results[0];
            int sessionid = (int)results[0];

            disconnectCall = new HTTPCallClient(server, service, port, 1, sessionid);
            sendFileCall = new HTTPCallClient(server, service, port, 2, sessionid);
            getInfoCall = new HTTPCallClient(server, service, port, 3, sessionid);

            connected = true;

         }
         catch (HTTPCallException e)
         {
            throw new ClientException(e.getErrorCode(), e.Message);
         }
         catch (Exception e)
         {
            throw new ClientException
               (ClientException.UNKNOWN_ERROR, e.Message);
         }

      }

      public void disconnect()
      {
         try
         {
            assertConnected();
            disconnectCall.execute(null);
            connected = false;
         }
         catch (HTTPCallException e)
         {
            throw new ClientException(e.getErrorCode());
         }
         catch (Exception e)
         {
            throw new ClientException
               (ClientException.UNKNOWN_ERROR, e.Message);
         }
      }

      public void sendFile(String fname, byte[] buffer, Hashtable options)
      {
         string fileName = Path.GetFileName(fname);
         try
         {
            arguments = new List<object>(3);
            arguments.Add(fileName);
            arguments.Add(buffer);
            arguments.Add(options);
            sendFileCall.execute(arguments);
         }
         catch (HTTPCallException e)
         {
            throw new ClientException(e.getErrorCode(), e.Message);
         }
         catch (Exception e)
         {
            throw new ClientException(HTTPCallException.UNKNOWN_ERROR, e.Message);
         }
      }
      public List<object> getInfo(String service, String md5)
      {
         try
         {
            arguments = new List<object>(2);
            arguments.Add(service);
            arguments.Add(md5);
            results = getInfoCall.execute(arguments);
            return results;
         }
         catch (HTTPCallException e)
         {
            throw new ClientException(e.getErrorCode());
         }
         catch (Exception e)
         {
            throw new ClientException(HTTPCallException.UNKNOWN_ERROR, e.Message);
         }
      }
   }
}
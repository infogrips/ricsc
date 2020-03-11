using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using infogrips.IO;
using infogrips.util;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace infogrips.net
{
   public class HTTPCallClient
   {
      private string url;

      public HTTPCallClient(String url, String service, String port, int requestid, int sessionid)
      {
         if (!url.EndsWith("/" + service))
         {
            this.url = url + "/" + service + "?" + port + ":" + sessionid + ":" + requestid;
         }
         else
         {
            this.url = url + "?" + port + ":" + sessionid + ":" + requestid;
         }
      }

      private void sendArguments(WebRequest uc, List<object> arguments)
      {

         uc.Method = "POST";
         uc.ContentType = "text/x-zip-compressed";
         uc.Headers["Pragma"] = "no-cache";

         // for testing of HTTP Athenification only (project NISZH/OIZ)
         // uc.setRequestProperty("Authorization","Basic d2lraTpwZWRpYQ==");

         DeflaterOutputStream zip = new DeflaterOutputStream(uc.GetRequestStream());
         SerialOutputStream so = new SerialOutputStream(zip);
         so.write(arguments);
         so.flush();
         zip.Close();

      }

      private List<object> receiveResults(WebRequest uc)
      {
         using (var responseStream = uc.GetResponse().GetResponseStream())
         {
            SerialInputStream si =
               new SerialInputStream(new InflaterInputStream(responseStream));

            if (si == null)
            {
               throw new HTTPCallException(HTTPCallException.NETWORK_ERROR, "no content (3)");
            }

            int? error = (int?)si.read();
            if (error == null)
            {
               throw new HTTPCallException(HTTPCallException.NETWORK_ERROR, "no content (4)");
            }

            Object results = si.read();

            if (error > 0)
            {
               throw new HTTPCallException(error.Value, (String)results);
            }

            return (List<object>)results;
         }
      }

      public List<object> executeWithLogger(List<object> arguments, Logger logger)
      {
         try
         {
            long timestamp = DateTime.Now.Ticks;
            WebRequest uc = WebRequest.Create(url + ":" + timestamp);
            sendArguments(uc, arguments);
            return receiveResults(uc);
         }
         catch (HTTPCallException e)
         {
            if (logger != null)
            {
               logger.error("HTTPCallException", e);
            }
            throw e;
         }
         catch (IOException e)
         {
            if (logger != null)
            {
               logger.error("HTTPCallException", e);
            }
            throw new HTTPCallException(HTTPCallException.NETWORK_ERROR, e.Message);
         }
         catch (Exception e)
         {
            if (logger != null)
            {
               logger.error("HTTPCallException", e);
            }
            throw new HTTPCallException(HTTPCallException.NETWORK_ERROR, e.Message);
         }

      }

      public List<object> execute(List<object> arguments)
      {
         return executeWithLogger(arguments, null);
      }
   }
}
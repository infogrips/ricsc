using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace infogrips.IO
{
   public class SerialInputStream
   {
      private Stream i = null;
      private StreamTokenizer st = null;
      private void initTokenizer()
      {
         st = new StreamTokenizer(new StreamReader(i, Encoding.GetEncoding("ISO-8859-1")));
         st.ResetSyntax();
         st.WhitespaceChars(0, 32);
         st.WordChars(33, 255);
         st.CommentChar('!');
         st.QuoteChar('\'');
         st.EolIsSignificant(false);
      }

      private SerialInputStream()
      {
      }

      public SerialInputStream(Stream i)
      {
         this.i = i;
         initTokenizer();
      }

      public String readNextToken()
      {
         try
         {
            if (st.NextToken() == StreamTokenizer.TT_EOF)
            {
               return null;
            }
            else
            {
               return st.StringValue;
            }
         }
         catch (Exception e)
         {
            string ex = e.ToString();
            return null;
         }
      }

      public void reReadNextToken()
      {
         st.PushBack();
      }

      public byte[] readBytes()
      {
         int length = Convert.ToInt32(readNextToken());
         if (length == 0)
         {
            return null;
         }
         try
         {
            byte[] b = new byte[length];
            int index = 0;
            while (length > 0)
            {
               int len = i.Read(b, index, length);
               index += len;
               length -= len;
            }
            return b;
         }
         catch (IOException e)
         {
            Console.Out.WriteLine("invalid byte array:" + e.ToString());
            return null;
         }
      }

      public String readString()
      {
         String value = readNextToken();
         if (value.Equals("NULL"))
         {
            return null;
         }
         else
         {
            return value;
         }
      }

      public int? readInt()
      {
         String value = readNextToken();
         if (value == null)
         {
            return null;
         }
         else if (value.Equals("NULL"))
         {
            return null;
         }
         else
         {
            return Convert.ToInt32(value);
         }
      }

      public long? readLong()
      {
         String value = readNextToken();
         if (value == null)
         {
            return null;
         }
         else if (value.Equals("NULL"))
         {
            return null;
         }
         else
         {
            return Convert.ToInt64(value);
         }
      }

      public Boolean? readBoolean()
      {
         String value = readNextToken();
         if (value == null)
         {
            return null;
         }
         else if (value.Equals("NULL"))
         {
            return null;
         }
         else
         {
            return Convert.ToBoolean(value);
         }
      }

      public Double? readReal()
      {
         String value = readNextToken();
         try
         {
            if (value == null)
            {
               return null;
            }
            else if (value.Equals("NULL"))
            {
               return null;
            }
            else
            {
               return Convert.ToDouble(value);
            }
         }
         catch (Exception e)
         {
            string ex = e.ToString();
            Console.Out.WriteLine("invalid real value " + value);
            return null;
         }
      }

      public List<Object> readList()
      {

         List<Object> value = new List<Object>();
         String t;

         while (true)
         {

            t = readNextToken();
            if (t == null)
            {
               break;
            }
            else if (t.Equals("}"))
            {
               break;
            }

            reReadNextToken();
            value.Add(read());

         }

         return value;
      }

      public Dictionary<Object, Object> readMap()
      {

         Dictionary<Object, Object> value = new Dictionary<Object, Object>();
         String t;

         while (true)
         {

            t = readNextToken();
            if (t == null)
            {
               break;
            }
            else if (t.Equals("}"))
            {
               break;
            }

            Object o = read();
            if (o != null)
            {
               // 2001.08.03 mg
               value.Add(t, o);
            }

         }

         return value;
      }

      public Object readObject(String type)
      {

         try
         {
            Type t = Type.GetType("Assets.infogrips." + type);
            Serial obj = (Serial)Activator.CreateInstance(t);
            return obj.ReadFrom(this);
         }
         catch (Exception ex)
         {
            Console.Out.WriteLine(ex.InnerException);
         }

         return null;
      }

      public Object read()
      {

         String type = readNextToken();
         Object value = null;

         try
         {

            if (type == null)
            {
            }
            else if (type.Equals("NULL"))
            {
            }
            else if (type.Equals("L"))
            {
               value = readObject("ch.infogrips.geoshop.server.IClientLine");
            }
            else if (type.Equals("A"))
            {
               value = readObject("ch.infogrips.geoshop.server.IClientArc");
            }
            else if (type.Equals("T"))
            {
               value = readObject("ch.infogrips.geoshop.server.IClientText");
            }
            else if (type.Equals("STRING"))
            {
               value = readString();
            }
            else if (type.Equals("INT"))
            {
               value = readInt();
            }
            else if (type.Equals("LONG"))
            {
               value = readLong();
            }
            else if (type.Equals("REAL"))
            {
               value = readReal();
            }
            else if (type.Equals("BOOLEAN"))
            {
               value = readBoolean();
            }
            else if (type.Equals("LIST"))
            {
               value = readList();
            }
            else if (type.Equals("MAP"))
            {
               value = readMap();
            }
            else if (type.Equals("BYTES"))
            {
               value = readBytes();
            }
            else
            {
               if (type.Equals("MODEL"))
               {
                  type = "IServerModel";
               }
               else if (type.Equals("PRODUCT"))
               {
                  type = "IServerProduct";
               }
               else if (type.Equals("SELECTION"))
               {
                  type = "IServerSelection";
               }
               else if (type.Equals("ORDER"))
               {
                  type = "IServerOrder";
               }
               else if (type.Equals("USER"))
               {
                  type = "IServerUser";
               }
               else if (type.Equals("SESSION"))
               {
                  type = "IServerSession";
               }
               else if (type.Equals("JOB"))
               {
                  type = "IServerJob";
               }
               else if (type.Equals("EVENT"))
               {
                  type = "IServerEvent";
               }
               else if (type.Equals("SERVICE"))
               {
                  type = "IServerService";
               }
               try
               {
                  value = readObject(type);
               }
               catch (Exception e)
               {
                  string ex = e.ToString();
                  Console.Out.WriteLine("unknown object type <" + type + ">");
               }
            }

         }
         catch (Exception e)
         {
            string ex = e.ToString();
         }

         return value;
      }

      public static Object loadObject(String fname)
      {
         FileStream fi;
         SerialInputStream si;
         try
         {
            fi = new FileStream(fname, FileMode.Open, FileAccess.Read);

            if (!fi.CanRead)
            {
               return null;
            }

            si = new SerialInputStream(fi);
            Object obj = si.read();
            fi.Close();
            return obj;
         }
         catch (Exception e)
         {
            string ex = e.ToString();
            return null;
         }
      }
   }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace infogrips.IO
{
   public class SerialOutputStream
   {
      private Stream o = null;
      private int level = 0;
      private String buffer = "";
      private bool pretty = false;
      private bool ignoreFirstPrefix = false;

      private SerialOutputStream()
      {
      }

      public SerialOutputStream(Stream o)
      {
         this.o = o;
      }

      public void setPretty(bool pretty)
      {
         this.pretty = pretty;
      }

      public void flush()
      {
         try
         {
            o.Flush();
         }
         catch (Exception e)
         {
            Console.Out.WriteLine("SerialOutputStream::flush() " + e.Message);
         }
      }

      public void writeBuffer(String b)
      {
         buffer += b;
      }

      public void openComplexObject(String label)
      {
         writeBuffer(label); writeln(); level++;
      }

      public void closeComplexObject()
      {
         level--; writeBuffer("}"); writeln();
      }

      public void write2(String t, Object obj)
      {
         if (obj != null)
         {
            buffer = buffer + t + " ";
            write(obj);
         }
      }

      public void writeLevelPrefix()
      {
         if (ignoreFirstPrefix)
         {
            ignoreFirstPrefix = false;
         }
         else if (pretty)
         {
            for (int i = 0; i < level; i++)
            {
               byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes("   ");
               o.Write(bytes, 0, bytes.Length);
            }
         }
      }

      public void writeln()
      {

         try
         {
            writeLevelPrefix();
            //20100920/mg: in android default getBytes() encoding is UTF-8
            //             under windows (ie. geoshop server) default getBytes() encoding is ISO-8859-1
            byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(buffer);
            byte[] bytes2 = Encoding.GetEncoding("ISO-8859-1").GetBytes("\r\n");
            o.Write(bytes, 0, bytes.Length);
            o.Write(bytes2, 0, bytes2.Length);
            //Console.Out.WriteLine("buffer=" + buffer);
            buffer = "";
         }
         catch (IOException e)
         {
            Console.Out.WriteLine("SerialOutputStream::writeln() " + e.Message);
         }

         buffer = "";

      }

      private void writeNull()
      {
         writeBuffer("NULL");
         writeln();
      }

      private void writeBytes(byte[] b)
      {
         if (b == null)
         {
            writeBuffer("BYTES 0");
            writeln();
         }
         else
         {
            try
            {
               byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(("BYTES " + b.Length + " "));
               o.Write(bytes, 0, bytes.Length);
               o.Write(b, 0, b.Length);

               byte[] bytes2 = Encoding.GetEncoding("ISO-8859-1").GetBytes("\r\n");
               o.Write(bytes2, 0, bytes2.Length);
            }
            catch (Exception e)
            {
               Console.WriteLine(e.ToString());
            }
         }
      }

      private void writeBoolean(Boolean b)
      {
         writeBuffer("BOOLEAN " + b.ToString());
         writeln();
      }

      private void writeInt(int i)
      {
         writeBuffer("INT " + i.ToString());
         writeln();
      }

      private void writeLong(long l)
      {
         writeBuffer("LONG " + l.ToString());
         writeln();
      }

      private void writeReal(Double r)
      {
         writeBuffer("REAL " + r.ToString());
         writeln();
      }

      public void writeStringValue(String s)
      {
         // mask ' with \' for StringTokenizer

         if (s.IndexOf("'") != -1)
         {
            String sn = "";
            for (int i = 0; i < s.Length; i++)
            {
               if (s[i] == '\'')
               {
                  sn = sn + "\\";
               }
               sn = sn + s[i];

            }
            s = sn;
         }

         if ((s.IndexOf(" ") != -1) ||
             (s.IndexOf("'") != -1) ||
             (s.IndexOf("!") != -1))
         {
            writeBuffer("'");
            writeBuffer(s);
            writeBuffer("'");
         }
         else if (s.CompareTo("") == 0)
         {
            writeBuffer("''");
         }
         else
         {
            writeBuffer(s);
         }

      }

      private void writeString(String s)
      {
         writeBuffer("STRING ");
         writeStringValue(s);
         writeln();
      }

      private void writeList(List<Object> v)
      {
         openComplexObject("LIST");
         foreach (Object obj in v)
         {
            write(obj);
         }
         closeComplexObject();
      }

      private void writeMap(Dictionary<Object, Object> h)
      {
         openComplexObject("MAP");

         foreach (KeyValuePair<Object, Object> s in h)
         {
            String key = s.Key as String;
            try
            {
               writeLevelPrefix();

               byte[] bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(key + " ");
               o.Write(bytes, 0, bytes.Length);
            }
            catch (IOException ex)
            {
               Console.Out.WriteLine("SerialOutputStream::writeMap(): " + ex.Message);
            }
            ignoreFirstPrefix = true;
            write(s.Value);
         }

         closeComplexObject();
      }

      public void write(Object obj)
      {
         if (obj == null)
         {
            writeNull();
            return;
         }

         if (obj is string)
         {
            writeString((String)obj);
         }
         else if (obj is int)
         {
            writeInt((int)obj);
         }
         else if (obj is long)
         {
            writeLong((long)obj);
         }
         else if (obj is double)
         {
            writeReal((double)obj);
         }
         else if (obj is bool)
         {
            writeBoolean((Boolean)obj);
         }
         else if (obj is List<Object>)
         {
            writeList((List<Object>)obj);
         }
         else if (obj is Hashtable)
         {
            writeMap(((Hashtable)obj).Cast<DictionaryEntry>().ToDictionary(d => d.Key, d => d.Value));
         }
         else if (obj is Dictionary<Object, Object>)
         {
            writeMap((Dictionary<Object, Object>)obj);
         }
         else if (obj is byte[])
         {
            writeBytes((byte[])obj);
         }
         else
         {
            try
            {
               //((Serial)obj).writeTo(this);
            }
            catch (Exception e)
            {
               string ex = e.ToString();
               Console.Out.WriteLine("unknown object type <" + obj.GetType() + ">");
            }
         }

      }

      public static bool saveObject(String fname, Object obj)
      {
         FileStream fo;
         SerialOutputStream so;

         try
         {
            fo = new FileStream(fname, FileMode.Open, FileAccess.Write);
            so = new SerialOutputStream(fo);
            so.pretty = true;
            so.ignoreFirstPrefix = false;
            so.write(obj);
            fo.Close();
            return true;
         }
         catch (Exception e)
         {
            Console.WriteLine(e.Message);
         }

         return false;
      }
   }
}
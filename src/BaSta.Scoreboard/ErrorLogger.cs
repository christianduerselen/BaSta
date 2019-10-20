using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Microsoft.VisualBasic;

namespace BaSta.Scoreboard
{
  internal class ErrorLogger
  {
    private static string xmlFilePath = "c:\\errorlogs\\ScoreboardRegieErrorlog.xml";
    private static string xmlFolder = "c:\\errorlogs";
    private static string strStartDateTime = string.Empty;
    private const int xmlBackupSize = 500000;

    public static void createErrorLogger()
    {
      XmlTextWriter xmlTextWriter = (XmlTextWriter) null;
      try
      {
        if (!Directory.Exists(ErrorLogger.xmlFolder))
          Directory.CreateDirectory(ErrorLogger.xmlFolder);
        if (File.Exists(ErrorLogger.xmlFilePath))
          return;
        xmlTextWriter = new XmlTextWriter(ErrorLogger.xmlFilePath, (Encoding) null);
        xmlTextWriter.Formatting = Formatting.Indented;
        xmlTextWriter.WriteStartDocument(false);
        xmlTextWriter.WriteComment("Scoreboardregie application error logger");
        xmlTextWriter.WriteStartElement(nameof (ErrorLogger));
        xmlTextWriter.WriteEndElement();
        xmlTextWriter.Flush();
        xmlTextWriter.Close();
      }
      catch (Exception ex)
      {
        ErrorLogger.displayErrorMessage(ex);
      }
      finally
      {
        xmlTextWriter?.Close();
      }
    }

    public static void startSession()
    {
      bool flag = false;
      try
      {
        ErrorLogger.createErrorLogger();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(ErrorLogger.xmlFilePath);
        XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Session");
        if (elementsByTagName.Count == 0)
        {
          flag = true;
        }
        else
        {
          IEnumerator enumerator = elementsByTagName.GetEnumerator();
          int num = 0;
          while (enumerator.MoveNext())
          {
            if (string.Compare(((XmlNode) enumerator.Current).Attributes["startdatetime"].Value.ToString(), ErrorLogger.strStartDateTime) == 0)
              ++num;
          }
          if (num == 0)
            flag = true;
        }
        if (!flag)
          return;
        if ((int) new FileInfo(ErrorLogger.xmlFilePath).Length > 500000)
        {
          string str = FileSystem.Dir(ErrorLogger.xmlFilePath + "*", FileAttribute.Normal);
          int num1 = 0;
          do
          {
            int num2 = str.IndexOf(".xml.");
            if (num2 == -1)
              num1 = 1;
            else if (ErrorLogger.IsNumeric(str.Substring(num2 + 5)))
            {
              if (Convert.ToInt32(str.Substring(num2 + 5)) + 1 > num1)
                num1 = Convert.ToInt32(str.Substring(num2 + 5)) + 1;
            }
            else
              num1 = 1;
            str = FileSystem.Dir() ?? string.Empty;
          }
          while (str != string.Empty);
          FileSystem.Rename(ErrorLogger.xmlFilePath, ErrorLogger.xmlFilePath + "." + (object) num1);
          ErrorLogger.createErrorLogger();
          xmlDocument.Load(ErrorLogger.xmlFilePath);
        }
        ErrorLogger.strStartDateTime = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();
        XmlElement xmlElement = (XmlElement) xmlDocument.GetElementsByTagName(nameof (ErrorLogger)).Item(0);
        XmlNode element = (XmlNode) xmlDocument.CreateElement("Session");
        xmlElement.AppendChild(element);
        ((XmlElement) element).SetAttribute("startdatetime", ErrorLogger.strStartDateTime);
        xmlDocument.Save(ErrorLogger.xmlFilePath);
      }
      catch (Exception ex)
      {
        ErrorLogger.displayErrorMessage(ex);
      }
    }

    public static void addToLog(string strException, string strMessage)
    {
      try
      {
        ErrorLogger.startSession();
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(ErrorLogger.xmlFilePath);
        foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("Session"))
        {
          if (string.Compare(xmlNode.Attributes["startdatetime"].Value, ErrorLogger.strStartDateTime) == 0)
          {
            XmlNode element1 = (XmlNode) xmlDocument.CreateElement("Error");
            xmlNode.AppendChild(element1);
            XmlElement xmlElement = (XmlElement) element1;
            xmlElement.SetAttribute("time", DateTime.Now.ToShortTimeString());
            XmlNode element2 = (XmlNode) xmlDocument.CreateElement("Exception");
            element2.AppendChild((XmlNode) xmlDocument.CreateTextNode(strException));
            xmlElement.AppendChild(element2);
            XmlNode element3 = (XmlNode) xmlDocument.CreateElement("Message");
            element3.AppendChild((XmlNode) xmlDocument.CreateTextNode(strMessage));
            xmlElement.AppendChild(element3);
            xmlDocument.Save(ErrorLogger.xmlFilePath);
          }
        }
      }
      catch (Exception ex)
      {
        ErrorLogger.displayErrorMessage(ex);
      }
    }

    public static void endSession()
    {
      try
      {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(ErrorLogger.xmlFilePath);
        foreach (XmlNode xmlNode in xmlDocument.GetElementsByTagName("Session"))
        {
          if (string.Compare(xmlNode.Attributes["startdatetime"].Value, ErrorLogger.strStartDateTime) == 0)
          {
            ((XmlElement) xmlNode).SetAttribute("enddatetime", DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString());
            xmlDocument.Save(ErrorLogger.xmlFilePath);
          }
        }
      }
      catch (Exception ex)
      {
        ErrorLogger.displayErrorMessage(ex);
      }
    }

    public static void displayErrorMessage(Exception err)
    {
      int num = (int) MessageBox.Show("An error has occurred. The error is: " + err.Message + " " + err.StackTrace.ToString(), "XML ErrorLogger | Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
    }

    public static bool IsNumeric(string value)
    {
      try
      {
        Convert.ToInt32(value);
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}

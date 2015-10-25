using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace AMDES_WEB
{
    public class Alert
    {
        private static Hashtable m_executingPages = new Hashtable();

        private Alert()
        {
        }

        private static bool redirect;
        private static string redirectAddress;
        //    * static method to call 
        public static void Show(string message, bool redir = false, string redirAddr = "default.aspx")
        {
            redirect = redir;
            redirectAddress = redirAddr;
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "'");
            // If this is the first time a page has called this method then
            if (!m_executingPages.Contains(HttpContext.Current.Handler))
            {
                // Attempt to cast HttpHandler as a Page.
                Page executingPage = HttpContext.Current.Handler as Page;
                if (executingPage != null)
                {
                    // Create a Queue to hold one or more messages.
                    Queue messageQueue = new Queue();
                    // Add our message to the Queue
                    messageQueue.Enqueue(cleanMessage);
                    // Add our message queue to the hash table. Use our page reference
                    // (IHttpHandler) as the key.
                    m_executingPages.Add(HttpContext.Current.Handler, messageQueue);
                    // Wire up Unload event so that we can inject 
                    // some JavaScript for the alerts.
                    executingPage.Unload += ExecutingPage_Unload;
                }
            }
            else
            {
                // If were here then the method has already been 
                // called from the executing Page.
                // We have already created a message queue and stored a
                // reference to it in our hashtable. 
                Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
                // Add our message to the Queue
                queue.Enqueue(cleanMessage);
            }
        }

        // Our page has finished rendering so lets output the
        // JavaScript to produce the alert's
        private static void ExecutingPage_Unload(object sender, EventArgs e)
        {
            // Get our message queue from the hashtable
            Queue queue = (Queue)m_executingPages[HttpContext.Current.Handler];
            if (queue != null)
            {
                StringBuilder sb = new StringBuilder();
                // How many messages have been registered?
                int iMsgCount = queue.Count;
                // Use StringBuilder to build up our client slide JavaScript.
                sb.Append("<script language='javascript'>");
                // Loop round registered messages
                string sMsg = null;
                while (System.Math.Max(System.Threading.Interlocked.Decrement(ref iMsgCount), iMsgCount + 1) > 0)
                {
                    sMsg = (string)queue.Dequeue();
                    sMsg = sMsg.Replace("\r\n", "\\n");
                    sMsg = sMsg.Replace("\"", "'");
                    sb.Append("alert( \"" + sMsg + "\" );");
                    if (redirect)
                    {
                        sb.Append("document.location = " + "'" + redirectAddress + "'");
                    }
                }
                // Close our JS
                sb.Append("</script>");
                // Were done, so remove our page reference from the hashtable
                m_executingPages.Remove(HttpContext.Current.Handler);
                // Write the JavaScript to the end of the response stream.
                HttpContext.Current.Response.Write(sb.ToString());
            }
        }
    }
}
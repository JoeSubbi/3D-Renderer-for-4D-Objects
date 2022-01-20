using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using UnityEngine.UI;

public class SendData : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        SendResults("joe.subbiani@gmail.com", "4D Shape Results" + StateController.Filename, StateController.Datapath);
    }

    public void SendResults(string toAddress, string subjectText, string bodyText)
    {
        try
        {
            MailMessage mail = new MailMessage();

            string fromAddress = "joe.subbiani@gmail.com";
            mail.From = new MailAddress(fromAddress);
            mail.To.Add(toAddress);
            mail.Subject = subjectText;
            mail.Body = bodyText;
            mail.Attachments.Add(new Attachment(Path.Combine(StateController.Datapath, StateController.Filename)));

            SmtpClient smtpServer = new SmtpClient("smtp.elasticemail.com");
            smtpServer.Port = 2525;
            smtpServer.Credentials = new System.Net.NetworkCredential(fromAddress, "BFCAFF9594B5EB3990094F53A260D8F013DC") as ICredentialsByHost;
            smtpServer.EnableSsl = true;

            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

            smtpServer.Send(mail);
            Debug.Log("Success");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void CopyToClipboard()
    {
        string path = Path.Combine(StateController.Datapath, StateController.Filename);
     
        //Read the text from directly from the file
        StreamReader reader = new StreamReader(path);
        GUIUtility.systemCopyBuffer = reader.ReadToEnd();
    }
}

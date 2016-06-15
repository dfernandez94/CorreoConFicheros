using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CorreoConFichero
{
    class Mail
    {

        /*Email*/
        public String de { set; get; }
        public String para { set; get; }
        public String bcc { set; get; }
        public String mensaje { set; get; }
        public String asunto { set; get; }
        public List<string> archivos { set; get; }

        /*SMTP*/
        public String usuario { set; get; }
        public String contra { set; get; }
        public bool habilitarSsl { set; get; }
        public String smtp { set; get; }
        public int puerto { set; get; }

        public System.Net.Mail.MailMessage email { set; get; }

        public string error { set; get; }

        public Mail(string de, string para, string asunto, string mensaje, string bcc="", List<string> archivos = null)
        {
            this.de = de;
            this.para = para;
            this.mensaje = mensaje;
            this.asunto = asunto;
            this.archivos = archivos;
            this.bcc = bcc;

            usuario = ConfigurationManager.AppSettings["correo.usuario"];
            contra = ConfigurationManager.AppSettings["correo.contra"];
            habilitarSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["correo.ssl"]);
            smtp = ConfigurationManager.AppSettings["correo.smtp"];
            puerto = Convert.ToInt32(ConfigurationManager.AppSettings["correo.puerto"]);
        }

        public bool enviar()
        {

            if (de.Trim().Equals("") || para.Trim().Equals("") || asunto.Trim().Equals(""))
            {
                error = "El mail, el asunto y el mensaje son obligatorios";
                return false;
            }

            try
            {
                email = new System.Net.Mail.MailMessage(de, para, asunto, mensaje);
                

                if (archivos != null)
                {
                    foreach (string archivo in archivos)
                    {
                        //comprobamos si existe el archivo y lo agregamos a los adjuntos
                        if (System.IO.File.Exists(@archivo))
                            email.Attachments.Add(new Attachment(@archivo));

                    }
                }

                email.IsBodyHtml = true;
                email.From = new MailAddress(de);

                if (!bcc.Equals(""))
                {
                    string[] emails = bcc.Split(',');
                    foreach (string e in emails)
                    {
                        email.Bcc.Add(e);
                    }
                }
                

                System.Net.Mail.SmtpClient smtpMail = new System.Net.Mail.SmtpClient(smtp);

                smtpMail.EnableSsl = habilitarSsl;
                smtpMail.UseDefaultCredentials = false; 
                smtpMail.Host = smtp;
                smtpMail.Port = puerto; 
                smtpMail.Credentials = new System.Net.NetworkCredential(usuario, contra); 

                smtpMail.Send(email);
                smtpMail.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                error = "Ocurrio un error: " + ex.Message;
                return false;
            }

            

        }
    }
}
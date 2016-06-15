using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CorreoConFichero
{
    /// <summary>
    /// Descripción breve de CorreoConFichero
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class CorreoConFichero : System.Web.Services.WebService
    {

        [WebMethod]
        public string generarFichero()
        {
            string[] albaranes = { "144292#220#220#0#542595#10/05/16#09:15#13/05/16#1#60#100",
                "144292#220#220#0#300079#10/05/16#09:15#13/05/16#1#150#20",
                "144292#220#220#0#300109#10/05/16#09:15#13/05/16#1#10#30000" };

            try {
                System.IO.File.WriteAllLines(@"C:\Albaranes\WriteLines.txt", albaranes);
                return "Fichero generado";
            }
            catch(Exception e)
            {
                return "No se ha generado el fichero "+e.Message;
            }

           
        }

        [WebMethod]
        public Byte[] obtenerFichero()
        {
            string strdocPath;
            strdocPath = @"C:\Albaranes\WriteLines.txt";

            FileStream objfilestream = new FileStream(strdocPath, FileMode.Open, FileAccess.Read);
            int len = (int)objfilestream.Length;
            Byte[] documentcontents = new Byte[len];
            objfilestream.Read(documentcontents, 0, len);
            objfilestream.Close();

            return documentcontents;
        }

        /*Leer el albaran en el cliente*/

        /*
        FileStream objfilestream = new FileStream(sFile, FileMode.Open, FileAccess.Read);
        int len = (int)objfilestream.Length;
        Byte[] mybytearray = new Byte[len];
        objfilestream.Read(mybytearray, 0, len);
        localhost.Service1 myservice = new localhost.Service1();
        myservice.SaveDocument(mybytearray, sFile.Remove(0, sFile.LastIndexOf("\\") + 1));
        objfilestream.Close();
        */
        
        [WebMethod]
        public string enviarEmail()
        {
            
            List<string> lstArchivos = new List<string>();
            lstArchivos.Add(@"C:\Albaranes\WriteLines.txt");
            
            Mail oMail = new Mail("pruebaa <davidtigrex@gmail.com>", "dfernandez@afirmatica.com","asunto", "mensaje", lstArchivos);
            
            if (oMail.enviar())
            {
                return "se envio el mail";
            }
            else
            {
                return "no se envio el mail: " + oMail.error;
            }


        }
    }
}

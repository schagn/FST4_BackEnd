using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public class FTPHelper
    {
        private string host = null;
        private string user = null;
        private string pass = null;

        public FTPHelper(string hostIP, string userName, string password) { host = hostIP; user = userName; pass = password; }
        public void upload(string remoteFile, string localFile)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(host + "/" + remoteFile);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(user, pass);
            Stream requestStream = request.GetRequestStream();
            FileStream fileStream = new FileStream(localFile, FileMode.Open);
            fileStream.CopyTo(requestStream);
            fileStream.Close();
            requestStream.Close();
        }
    }
}

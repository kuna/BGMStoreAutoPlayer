using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.IO;
using System.Web;
using System.Web.Util;

namespace BGMStoreAutoPlayer
{
    class BGMStore
    {
        public string musicurl;
        public string filename;
        public string title;

        public void parse()
        {

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://bgmstore.net/random");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("utf-8"));

            String html = sr.ReadToEnd().ToString();
            int m = html.IndexOf("mp3 파일로 다운로드");
            html = html.Substring(0, m);
            int m2 = html.LastIndexOf("<a");
            musicurl = html.Substring(m2 + 9, m - m2 - 9 - 2);
            filename = Uri.UnescapeDataString(musicurl.Substring(musicurl.LastIndexOf("/") + 1)) + ".mp3";

            int t1 = html.LastIndexOf("<h3>")+4;
            int t2 = html.LastIndexOf("</h3>");
            title = html.Substring(t1, t2-t1);
        }

        public void download()
        {
            if (musicurl == "") return;
            WebClient wc = new WebClient();
            wc.DownloadFile(musicurl, "tempfile");
        }

        public void savefile()
        {
            System.IO.File.Move("tempfile", filename);
        }
    }
}

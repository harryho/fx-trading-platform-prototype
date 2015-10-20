using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.ComponentModel;

namespace fxpa
{
    class DSWebClient
    {
        private WebClient webClient;
        private DataProvider provider;
        
        public DSWebClient( DataProvider provider)
        {
            webClient = new WebClient(  );
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            this.provider = provider;
            //webClient.DownloadDataCompleted += new DownloadDataCompletedEventHandler(webClient_DownloadDataCompleted);
        }

        //  H :  His file ,  R : Realtime Data
        string loadingMark;

        public string LoadingMark
        {
            get { return loadingMark; }
            set { loadingMark = value; }
        }

        void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            webClient.DownloadFileCompleted -= new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);
            webClient.Dispose();
            webClient = null;
            if( e.UserState !=null)
                Console.WriteLine(" complete  UserState~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + e.UserState);
            if(e.Error != null )
                Console.WriteLine(" complete Error ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~" + e.Error.StackTrace);
          

                if (e.Error == null)
                    provider.CdlLoadingStatus = DownloadStatus.Finished;
                else
                {
                    provider.CdlLoadingStatus = DownloadStatus.Failed;
                }
                provider.InitCdlFile();
     
        }

        public void DownloadFile(string uri, string fpath)
        {
            try          
            {
                webClient.DownloadFileAsync(new Uri(uri), fpath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

    }
}

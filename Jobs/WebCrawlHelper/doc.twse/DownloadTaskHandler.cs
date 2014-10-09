using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Concurrent;
using System.Diagnostics;


namespace doc.twse
{
    public class DownloadTaskHandler
    {
        string _downloadPath;
        public string DownloadPath
        {
            get { return _downloadPath; }
            set { _downloadPath = value; }
        }

        const int DownloadPoolSize = 4;
        const int MaxRetryDownload = 3;

        protected ConcurrentQueue<DocDownloadTask> documentQueue = null;

        DocDownloadWnd ownerForm;

        List<DocDownloadTask> aryDocuments = null;

        public List<DocDownloadTask> DocumentsList
        {
            get { return aryDocuments; }
            //set { aryDocuments = value; }
        }

        public DownloadTaskHandler(DocDownloadWnd formOwner)
        {
            documentQueue = new ConcurrentQueue<DocDownloadTask>();
            aryDocuments = new List<DocDownloadTask>();
            ownerForm = formOwner;
        }

        public void AddDownloadTask(DocDownloadTask Task)
        {
            documentQueue.Enqueue(Task);
            aryDocuments.Add(Task);
            Debug.WriteLine(string.Format("Doc Enqueue({0}) : {1}", documentQueue.Count, Task.DocId));
        }

        public void Clear()
        {

            while (!documentQueue.IsEmpty)
            {
                DocDownloadTask task = null;
                documentQueue.TryDequeue(out task);
            }
            aryDocuments.Clear();
        }

        public void TryStart()
        {
            if (!documentQueue.IsEmpty)
            {
                ConcurrentBag<string> dataContainer = new ConcurrentBag<string>();
                IList<Task> taskList = new List<Task>();

                for (int i = 0; i < DownloadPoolSize; i++)
                {
                    Task task = Task.Run(() =>
                    {
                        ThreadDownloadDocument();
                    });

                    taskList.Add(task);
                }
            }
        }

        public bool IsAllDone()
        {
            bool allDone = true;
            if (aryDocuments.Count > 0)
            {
                foreach (DocDownloadTask doc in aryDocuments)
                {
                    if (doc.Status != DocDownloadStatus.Downloaded)
                    {
                        allDone = false;
                        break;
                    }
                }
            }
            else
            {
                allDone = false;
            }

            return allDone;
        }

        private void ThreadDownloadDocument()
        {
            try
            {
                DocDownloadTask task = null;

                while (documentQueue.TryDequeue(out task))
                {
                    Debug.WriteLine("Task {0} started...", task.DocId);

                    
                    if (task.Status == DocDownloadStatus.New ||
                        task.Status == DocDownloadStatus.ServerBusy ||
                        task.Status == DocDownloadStatus.RefreshUrlFailed ||
                        task.Status == DocDownloadStatus.DownloadFailed)
                    {
                        if (task.RefreshUrl() != DocDownloadStatus.UrlReady)
                        {
                            documentQueue.Enqueue(task);
                            continue;
                        }
                            //_docCountUrlReady++;
                    }

                    if (task.Status == DocDownloadStatus.UrlReady)
                    {
                        if (task.Download() == DocDownloadStatus.Downloaded)
                        {
                            //ownerForm.Refresh();
                        }
                        else
                        {
                            documentQueue.Enqueue(task);
                        }
                    }

                    Debug.WriteLine("Task {0} ended.", task.DocId);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}

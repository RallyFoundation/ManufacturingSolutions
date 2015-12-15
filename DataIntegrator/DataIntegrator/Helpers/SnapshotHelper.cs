using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataIntegrator.Helpers.Snapshot
{
    class SnapshotHelper
    {
        protected IList<IDictionary<string, object>> lastSnapShot;
        protected IList<IDictionary<string, object>> currentSnapShot;
        protected IList<IDictionary<string, object>> snapShotDeleted;
        protected IList<IDictionary<string, object>> snapShotNew;
        protected IList<IDictionary<string, object>> snapShotUpdated;

        public string[] UniqueIdentifierNames { get; set; }

        public virtual void SetLast(IList<IDictionary<string, object>> last)
        {
            this.lastSnapShot = (last != null) ? new List<IDictionary<string, object>>(last) : last;
        }

        public virtual void SetCurrent(IList<IDictionary<string, object>> current)
        {
            this.currentSnapShot = (current != null) ? new List<IDictionary<string, object>>(current) : current;
        }

        public virtual IList<IDictionary<string, object>> GetDeleted()
        {
            return this.snapShotDeleted;
        }

        public virtual IList<IDictionary<string, object>> GetNew()
        {
            return this.snapShotNew;
        }

        public virtual IList<IDictionary<string, object>> GetUpdated()
        {
            return this.snapShotUpdated;
        }

        public string GetMostRecentSnapshot(string snapshotPath, string snapshotEncodingName) 
        {
            string returnValue = null;

            if (Directory.Exists(snapshotPath))
            {
                string[] snapshotFileNames = Directory.GetFiles(snapshotPath);

                DateTime creationTime = DateTime.MinValue;

                FileInfo fileInfo;

                string mostRecentSnapshotName = "";

                foreach (string snapshotFileName in snapshotFileNames)
                {
                    fileInfo = new FileInfo(snapshotFileName);

                    if (creationTime < fileInfo.CreationTimeUtc)
                    {
                        mostRecentSnapshotName = snapshotFileName;
                        creationTime = fileInfo.CreationTimeUtc;
                    }
                }

                if (!String.IsNullOrEmpty(mostRecentSnapshotName))
                {
                    Encoding snapshotEncoding = String.IsNullOrEmpty(snapshotEncodingName) ? Encoding.Default : Encoding.GetEncoding(snapshotEncodingName);

                    using (FileStream stream = new FileStream(mostRecentSnapshotName, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (StreamReader reader = new StreamReader(stream, snapshotEncoding))
                        {
                            returnValue = reader.ReadToEnd();
                        };
                    };
                }

            }

            return returnValue;
        }

        public void SetSnapshot(string snapshotPath, string snapshotContent, string snapshotEncodingName) 
        {
            Encoding snapshotEncoding = String.IsNullOrEmpty(snapshotEncodingName) ? Encoding.Default : Encoding.GetEncoding(snapshotEncodingName);

            if (!Directory.Exists(snapshotPath))
            {
                Directory.CreateDirectory(snapshotPath);
            }
            
            string pattern = "yyyy-MM-dd-hh-mm-ss-ffffzzz";

            string fileName = DateTime.Now.ToString(pattern);

            fileName = fileName.Replace("+", "A");

            fileName = fileName.Replace(":", "-");

            fileName += ".xml";

            string fileFullName = snapshotPath.EndsWith("\\") ? (snapshotPath + fileName) : (snapshotPath + "\\" + fileName);

            byte[] fileBytes = snapshotEncoding.GetBytes(snapshotContent);

            using (FileStream stream = new FileStream(fileFullName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                //using (StreamWriter writer = new StreamWriter(stream, snapshotEncoding))
                //{
                //    writer.Write(snapshotContent);
                //    writer.Flush();
                //    stream.Flush();
                //};

                stream.Write(fileBytes, 0, fileBytes.Length);
                stream.Flush();
            };
        }

        public virtual bool Compare()
        {
            bool returnValue = false;

            if ((this.lastSnapShot != null) && (this.lastSnapShot.Count > 0) && (this.currentSnapShot != null) && (this.currentSnapShot.Count > 0))
            {
                int matchCount = 0;

                //string key = this.UniqueIdentifierName;

                for (int i = 0; i < this.currentSnapShot.Count; i++)
                {
                    for (int j = 0; j < this.lastSnapShot.Count; j++)
                    {
                        foreach (string keyCurrent in this.currentSnapShot[i].Keys)
                        {
                            if (this.lastSnapShot[j][keyCurrent].ToString().ToLower() == this.currentSnapShot[i][keyCurrent].ToString().ToLower())
                            {
                                matchCount++;
                            }
                            else if(this.UniqueIdentifierNames.Contains(keyCurrent)) //else if (keyCurrent == key)
                            {
                                matchCount = 0;//Meaning that there is new or deleted since the key of unique identifier is different.
                                break;
                            }
                        }

                        if (matchCount > 0)
                        {
                            if ((matchCount < this.currentSnapShot[i].Keys.Count) && (matchCount < this.lastSnapShot[j].Keys.Count))
                            {
                                if (this.snapShotUpdated == null)
                                {
                                    this.snapShotUpdated = new List<IDictionary<string, object>>();
                                }

                                this.snapShotUpdated.Add(this.currentSnapShot[i]);
                            }

                            this.lastSnapShot.RemoveAt(j);
                            j--;

                            this.currentSnapShot.RemoveAt(i);
                            i--;

                            matchCount = 0;

                            break;
                        }
                    }
                }
            }

            if ((this.currentSnapShot != null) && (this.currentSnapShot.Count > 0))
            {
                this.snapShotNew = new List<IDictionary<string, object>>();

                foreach (IDictionary<string, object> newData in this.currentSnapShot)
                {
                    this.snapShotNew.Add(newData);
                }
            }

            if ((this.lastSnapShot != null) && (this.lastSnapShot.Count > 0))
            {
                this.snapShotDeleted = new List<IDictionary<string, object>>();

                foreach (IDictionary<string, object> deletedData in this.lastSnapShot)
                {
                    this.snapShotDeleted.Add(deletedData);
                }
            }

            returnValue = true;

            return returnValue;
        }
    }
}

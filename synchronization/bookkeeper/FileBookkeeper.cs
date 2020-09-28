using System.IO;
using System.Collections.Generic;
using System;
namespace notizen_web_api.synchronization.bookkeeper
{
    public class FileBookkeeper: IBookkeeper
    {
        public FileBookkeeper(string baseFilePath) {
            if (string.IsNullOrEmpty(baseFilePath)) {
                throw new ArgumentException(nameof(baseFilePath));
            }
            _baseFilePath = baseFilePath;
        }
        public void Persist(IList<BookkeeperChange> bookkeeperChanges) {
            if (bookkeeperChanges == null) {
                throw new ArgumentNullException(nameof(bookkeeperChanges));
            }

            DateTime date = DateTime.UtcNow;
            long timeStamp = date.Ticks;

            var filePath = Path.Combine(_baseFilePath, _fileName);
            
            var streamWriter = new StreamWriter(_baseFilePath, true);
            using(streamWriter) {
                foreach(BookkeeperChange bookkeeperChange in bookkeeperChanges) {
                    var contentElements = new string[] {timeStamp.ToString(), bookkeeperChange.Type, 
                            bookkeeperChange.Id, bookkeeperChange.Operation.ToString()};
                    //Input checken, dass keine leerzeichen im Input vorhanden sind. Dann aber schreiben was geht
                    string content = string.Join(" ", contentElements);
                    streamWriter.WriteLine(content);
                }

                streamWriter.Flush();
            }
        }
        public  BookkeeperResult GetBookkeeperInfo(long revision) {
            throw new NotImplementedException();
        }

        private readonly string _baseFilePath;
        private readonly string _fileName = "changes.bookkeeper";
    }
}
using System.Collections.Generic;

namespace notizen_web_api.synchronization.bookkeeper
{
    public enum BookkeeperOperation {Create, Delete, Update}

    public class BookkeeperChange {
        public BookkeeperChange(string id, string type, Operation operation) {
            Id = id;
            Type = type;
            Operation = operation;
        }
        public string Id {get;}
        public string Type {get;}
        public Operation Operation {get;}

        public static BookkeeperChange GenerateCreate(string id, string type) {
            return new BookkeeperChange(id, type, Operation.Create);
        }

        public static BookkeeperChange GenerateUpdate(string id, string type) {
            return new BookkeeperChange(id, type, Operation.Update);
        }

        public static BookkeeperChange GenerateDelete(string id, string type) {
            return new BookkeeperChange(id, type, Operation.Delete);
        }
    }

    public class BookkeeperResult {
        public long NewestRevision {get; set;}
        public IList<BookkeeperChange> Changes {get; set;}
    }

    public interface IBookkeeper
    {
         void Persist(IList<BookkeeperChange> bookkeeperChanges);
         BookkeeperResult GetBookkeeperInfo(long revision);
    }
}
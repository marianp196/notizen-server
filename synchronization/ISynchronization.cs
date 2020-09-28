using System.Collections.Generic;

namespace notizen_web_api.synchronization
{
    public enum Operation {Create, Update, Delete}

    public class DataChangeMeta {
        public string Id {get; set;}
        public Operation Type {get; set;}
        public string Operation {get; set; }
    }

    public class DataChange<TValue>: DataChangeMeta {
        public TValue Value {get; set;}
    }

    public abstract class PersistResult {
        public string Id {get; set;}
        public Operation Type {get; set;}
        public string Operation {get; set; }
        public bool Successful{get; set;}
        public string Message {get; set;}
        public string Code{get; set;}
    }

    public interface ISynchronizationResult {
        long Revision {get; set;}
        DataChange<object> ServerChanges {get;}
        IList<DataChange<TValue>> GetServerChangesForType<TValue>();
        IList<DataChange<object>> GetGetServerChangesForType(string type);
        IList<PersistResult> PersistedObjects {get; set;}
        IList<PersistResult> PersistedErrors {get; set;}
    }

    public interface ISynchronization
    {
        ISynchronizationResult Sync(IList<DataChange<object>> changes);
    }
}
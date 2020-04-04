using System;
using System.Collections.Generic;
using System.Linq;

namespace notizen_web_api.notes
{
    public class CashedNoteService : INotesService
    {
        public CashedNoteService(INotesService internalNoteService, FilterNoteService filterNoteService, TimeSpan cashingTime)
        {
            _internalNoteService = internalNoteService ?? throw new ArgumentNullException(nameof(internalNoteService));
            _filterNoteService = filterNoteService ?? throw new ArgumentNullException(nameof(filterNoteService));
            _cashingTime = cashingTime;

            _internalCache = new Dictionary<Guid, Note>();
            _lastRead = null;
        }

        public ManipulateResult Create(NoteContent noteContent)
        {
            lock (_lock)
            {
                var cache = getInternalCache();
                var result = _internalNoteService.Create(noteContent);
                cache.Add(result.Result.Id, result.Result);
                return result;
            }
        }

        public ManipulateResult Update(Guid id, NoteContent noteContent)
        {
            lock (_lock)
            {
                var cache = getInternalCache();
                var result = _internalNoteService.Update(id, noteContent);
                cache.Add(result.Result.Id, result.Result);
                return result;
            }
        }

        public void DeleteById(Guid id)
        {
            lock (_lock)
            {
                var cache = getInternalCache();
                _internalNoteService.DeleteById(id);
                cache.Remove(id);
            }
        }

        public GetResult GetByID(Guid id)
        {
            lock (_lock)
            {
                var cache = getInternalCache();

                if (cache.TryGetValue(id, out var value))
                {
                    return GetResult.CreateFound(value.Clone());
                }
                else
                {
                    return GetResult.CreateNotFound();
                }
            }
        }

        public IEnumerable<Note> GetAll()
        {
            lock(_lock) 
            {
                return getAll();
            }
        }

        public IEnumerable<Note> GetFilterd(IList<string> categoryIds, string text)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetFilterd(IList<string> categoryIds)
        {
            lock(_lock)
            {                
                var notes = getAll();
                return _filterNoteService.GetFilterdByCategories(notes, categoryIds);
            }
        }

        public IEnumerable<Note> GetFilterd(string text)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Note> getAll() {
            return getInternalCache().Values.Select(n => n.Clone()).ToList();
        }

        private IDictionary<Guid, Note> getInternalCache()
        {
            var now = DateTime.UtcNow;
            
            if (_lastRead == null || now.Subtract(_lastRead.Value).TotalMilliseconds > _cashingTime.TotalMilliseconds)
            {
                var notes = _internalNoteService.GetAll();
                var keyValuePairs = notes.Select(n => new KeyValuePair<Guid, Note>(n.Id, n));
                _internalCache = new Dictionary<Guid, Note>(keyValuePairs);
                _lastRead = now;
            }

            return _internalCache;
        }

        private readonly TimeSpan _cashingTime;
        private readonly INotesService _internalNoteService;
        private readonly FilterNoteService _filterNoteService;
        private readonly object _lock = new object();
        private IDictionary<Guid, Note> _internalCache;
        private DateTime? _lastRead;

    }
}
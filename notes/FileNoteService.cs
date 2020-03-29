using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace notizen_web_api.notes
{
    public class FileNoteService : INotesService
    {
        public FileNoteService(string basePath, string fileExtension)
        {
            _basePath = basePath;
            _fileExtension = fileExtension;
        }

        public ManipulateResult Create(NoteContent noteContent)
        {
            if (noteContent == null)
            {
                throw new ArgumentNullException(nameof(noteContent));
            }

            var note = Note.CreateDefault(Guid.NewGuid());
            note.copyNoteContentFrom(noteContent);

            var violations = note.Validate();
            if (violations.Any())
            {
                return ManipulateResult.CreateInvalid(violations);
            }

            string path = buildPath(note.Id);

            writeToStorage(note);

            return ManipulateResult.CreateValid(note);
        }

        public ManipulateResult Update(Guid id, NoteContent noteContent)
        {
            var getResult = GetByID(id);
            if (!getResult.Found)
            {
                throw new KeyNotFoundException(id.ToString());
            }

            var note = getResult.Result;
            note.UpdateDate = DateTime.Now;
            note.copyNoteContentFrom(noteContent);

            var violations = note.Validate();
            if (violations.Any())
            {
                return ManipulateResult.CreateInvalid(violations);
            }

            writeToStorage(note);

            return ManipulateResult.CreateValid(note);
        }

        public void DeleteById(Guid id) {
            var getResult = GetByID(id);
            if (!getResult.Found)
            {
                throw new KeyNotFoundException(id.ToString());
            }

            var path = buildPath(id);
            try {
                File.Delete(path);
            } catch(KeyNotFoundException) {
                throw new KeyNotFoundException(id.ToString());
            }
        }

        public GetResult GetByID(Guid id)
        {
            var path = buildPath(id);
            Note note = null;

            try
            {
                note = readFromFile(path);
            }
            catch (FileNotFoundException)
            {
                return GetResult.CreateNotFound();
            }

            return GetResult.CreateFound(note);
        }

        public IEnumerable<Note> GetAll()
        {
            var notesResult = new List<Note>();
            var files = Directory.EnumerateFiles(_basePath);
            var noteFiles = files.Where(file => file.ToLower().EndsWith(_fileExtension));

            foreach (var noteFile in noteFiles)
            {
                notesResult.Add(readFromFile(noteFile));
            }
            return notesResult;
        }

        public IEnumerable<Note> GetFilterd(IList<string> categoryIds, string text)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Note> GetFilterd(IList<string> categoryIds)
        {
            if (categoryIds == null) {
                throw new ArgumentNullException(nameof(categoryIds));
            }
            var allNotes = GetAll().ToList();

            return allNotes.Where(note => {
                return categoryIds.Any(categorySearchedFor => 
                     categorySearchedFor != null
                     && note?.Content?.CategoryIds != null
                     && note.Content.CategoryIds.Any(
                         categid => categorySearchedFor.ToLower() == categid.ToLower()
                     )
                ); // ToDo hier vlt in map konvertieren
            });
        }

        public IEnumerable<Note> GetFilterd(string text)
        {
            throw new NotImplementedException();
        }

        private Note readFromFile(string path)
        {
            using (StreamReader file = File.OpenText(path))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    return serializer.Deserialize<Note>(reader);
                }
            }
        }

        private void writeToStorage(Note note)
        {
            string path = buildPath(note.Id);

            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, note);
            }
        }

        private string buildPath(Guid id)
        {
            return Path.Combine(_basePath, id.ToString("D") + _fileExtension);
        }

        private readonly string _basePath;
        private readonly string _fileExtension;
    }
}
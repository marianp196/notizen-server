using System;
using System.Collections.Generic;

namespace notizen_web_api.notes
{
    public interface INotesService
    {
        ManipulateResult Create(NoteContent noteContent);
        ManipulateResult Update(Guid guid, NoteContent noteContent);

        GetResult GetByID(Guid id);
        IEnumerable<Note> GetAll();       
        IEnumerable<Note> GetFilterd(IList<string> categoryIds, string text);
        IEnumerable<Note> GetFilterd(IList<string> categoryIds);
        IEnumerable<Note> GetFilterd(string text);
    }

    public class ManipulateResult {
        public bool Valid {get; set;}
        public Note Result {get; set;}
        public IEnumerable<IViolation> Violations {get; set;}

        public static ManipulateResult CreateValid(Note note) {
            return new ManipulateResult() {
                Valid = true,
                Result = note,
                Violations = new List<IViolation>()
            };
        }

       public static ManipulateResult CreateInValid(IEnumerable<IViolation> violations) {
            return new ManipulateResult() {
                Valid = false,
                Result = null,
                Violations = violations
            };
        }
    }

    public class GetResult {
        public bool Found {get; set;}
        public Note Result {get; set;}

        public static GetResult CreateFound(Note note) {
            return new GetResult() {
                Found = true,
                Result = note
            };
        }

       public static GetResult CreateNotFound() {
            return new GetResult() {
                Found = false,
                Result = null
            };
        }
    }

    public interface IViolation {
        string Text {get;}
    }
}
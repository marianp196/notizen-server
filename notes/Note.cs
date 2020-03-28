using System;
using System.Collections.Generic;

namespace notizen_web_api.notes
{
    public class Note
    {
        public Guid Id { get; set; }
        public DateTime? CreationDate {get; set;}
        public DateTime? UpdateDate {get; set;}
        public NoteContent Content {get; set;}

        public void copyNoteContentFrom(NoteContent content) {
            if (Content == null) {
                Content = new NoteContent();
            }
            Content.CategoryIds = content.CategoryIds ?? new List<string>();
            Content.FreeTags = content.FreeTags;
            Content.Header = content.Header;
            Content.Text = content.Text;
        }

        public IEnumerable<IViolation> Validate() {
            return new List<IViolation>();
        }

        public static Note CreateDefault(Guid id) {
            var note = new Note();
            note.Id = id;
            note.CreationDate = DateTime.Now;
            note.UpdateDate = DateTime.Now;
            note.Content = new NoteContent();
            return note;
        }
    }

    public class NoteContent {
        public IList<string> CategoryIds{get; set;}
        public string FreeTags {get; set;}
        public string Header {get; set;}
        public string Text {get; set;}
    }
}
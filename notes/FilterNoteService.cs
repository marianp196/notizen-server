using System;
using System.Collections.Generic;
using System.Linq;

namespace notizen_web_api.notes
{
    public class FilterNoteService
    {
        public IEnumerable<Note> GetFilterdByCategories(IEnumerable<Note> notes, IList<string> categoryIds)
        {
            if (categoryIds == null) {
                throw new ArgumentNullException(nameof(categoryIds));
            }

            if (notes == null) {
                throw new ArgumentNullException(nameof(notes));
            }

            return notes.Where(note => {
                return categoryIds.Any(categorySearchedFor => 
                     categorySearchedFor != null
                     && note?.Content?.CategoryIds != null
                     && note.Content.CategoryIds.Any(
                         categid => categorySearchedFor.ToLower() == categid.ToLower()
                     )
                ); // ToDo hier vlt in map konvertieren
            });
        } 

        public IEnumerable<Note> GetFilterdByText(IEnumerable<Note> notes, string text)
        {
            if (notes == null) {
                throw new ArgumentNullException(nameof(notes));
            }

            if (string.IsNullOrEmpty(text)) {
                return notes;
            }

            return notes;
        } 
    }
}
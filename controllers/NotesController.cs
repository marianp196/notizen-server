using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using notizen_web_api.notes;

namespace notizen_web_api.Controllers
{
    [ApiController]
    public class NotesController : ControllerBase
    {
        public NotesController(ILogger<NotesController> logger, INotesService notesService)
        {
            _logger = logger;
            _notesService = notesService;
        }   

        [HttpPost("[controller]")]
        public IActionResult Create([FromBody] NoteContent content) {
            var result = _notesService.Create(content);
            
            if (result.Valid) {
                return Ok(result.Result);
            } else {
                return BadRequest(result.Violations);
            }
        }   

        [HttpPut("[controller]/{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] NoteContent content) {
            var result = _notesService.Update(id, content);
            
            if (result.Valid) {
                return Ok(result.Result);
            } else {
                return BadRequest(result.Violations);
            }
        }

        [HttpDelete("[controller]/{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            // ToDo rückmeldung
            _notesService.DeleteById(id);
            return Ok();
        }

        [HttpGet("[controller]/{id}")]
        public IActionResult Get([FromRoute] Guid id)
        {
            var result = _notesService.GetByID(id);
            if (result.Found) {
                return Ok(result.Result);
            } else {
                return NotFound();
            }
        }

        [HttpGet("[controller]")]
        public IActionResult Get([FromQuery] string categoryId)
        {
            if (categoryId == null) {
                return Ok(_notesService.GetAll());
            }

            var catgoryIds = new string[] {categoryId};
            var notes = _notesService.GetFilterd(catgoryIds);
            return Ok(notes);           
        }

        private readonly INotesService _notesService;
        private readonly ILogger<NotesController> _logger;
    }
}

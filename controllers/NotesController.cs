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

        [HttpDelete("[controller]/{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            // ToDo rückmeldung
            _notesService.DeleteById(id);
            return Ok();
        }

        [HttpGet("[controller]")]
        public IActionResult Get()
        {
            var notes = _notesService.GetAll();
            return Ok(notes);           
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

        private readonly INotesService _notesService;
        private readonly ILogger<NotesController> _logger;
    }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import {map} from 'rxjs/operators';
import { NoteService } from '../services/note.service';
import { Note } from '../services/note';

@Component({
  selector: 'app-note-list',
  templateUrl: './note-list.component.html',
  styleUrls: ['./note-list.component.scss'],
})
export class NoteListComponent implements OnInit {

  constructor(private activated: ActivatedRoute, private noteService: NoteService) { }

  public category: Observable<string>;
  public notes: Observable<Note[]>;

  ngOnInit() {
    this.category = this.activated.params.pipe(map(obj => obj.category));
    this.notes = this.noteService.getAll();
  }

}

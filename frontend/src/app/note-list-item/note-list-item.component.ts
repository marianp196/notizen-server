import { Component, OnInit, Input } from '@angular/core';
import { Note } from '../services/note';
import * as moment from 'moment';

@Component({
  selector: 'app-note-list-item',
  templateUrl: './note-list-item.component.html',
  styleUrls: ['./note-list-item.component.scss'],
})
export class NoteListItemComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    this.date = moment(this.note.creationDate).format('dd DD.MM.YY');
  }

  @Input() public note: Note;
  public date: string;

}

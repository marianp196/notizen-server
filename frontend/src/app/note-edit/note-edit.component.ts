import { ModalController } from '@ionic/angular';
import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import * as _ from 'lodash';
import { NoteContent } from '../services/note';

@Component({
  selector: 'app-note-edit',
  templateUrl: './note-edit.component.html',
  styleUrls: ['./note-edit.component.scss'],
})
export class NoteEditComponent implements OnInit {

  constructor(private modal: ModalController) { }

  @Input() note: NoteContent;
  public noteGroup: FormGroup;

  ngOnInit() {
    this.noteGroup = new FormGroup({
      header: new FormControl(this.note.header),
      freeTags: new FormControl(this.note.freeTags),
      text: new FormControl(this.note.text),
      categoryIds: new FormControl(this.note.categoryIds)
    });
  }

  public close() {
    this.modal.dismiss();
  }

  public save() {
    const data = this.noteGroup.getRawValue();
    this.modal.dismiss(data, 'save');
  }
}

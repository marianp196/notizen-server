import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { NoteService } from '../services/note.service';
import { Note, NoteContent } from '../services/note';
import { ModalController, ToastController } from '@ionic/angular';
import { NoteEditComponent } from '../note-edit/note-edit.component';

@Component({
  selector: 'app-note-list',
  templateUrl: './note-list.component.html',
  styleUrls: ['./note-list.component.scss'],
})
export class NoteListComponent implements OnInit {

  constructor(
    private activated: ActivatedRoute,
    private noteService: NoteService,
    private modalController: ModalController,
    private toastController: ToastController) { }

  public category: Observable<string>;
  public notes: Observable<Note[]>;

  ngOnInit() {
    this.category = this.activated.params.pipe(map(obj => obj.category));
    this.reload();
  }

  public async delete(note: Note) {
    this.noteService.delete(note).subscribe(
      () => { 
        this.reload();
        this.showTost('Gelöscht: ' + note.content.header);
      }
    );
  }

  public async update(note: Note) {
    const modal = await this.modalController.create({component: NoteEditComponent,
      componentProps: {note: note.content}});

    await modal.present();
    const result = await modal.onDidDismiss();

    if (result.role === 'save') {
    }
  }

  public async create() {
    const noteContent = NoteContent.createDefault();
    const modal = await this.modalController.create({component: NoteEditComponent, 
      componentProps: {note: noteContent}});

    await modal.present();
    const result = await modal.onDidDismiss();

    if (result.role === 'save') {
      this.noteService.create(result.data).subscribe(
        note => {
          this.showTost('Notiz erstellt: ' + note.content.header);
          this.reload();
        }
      );
    }
  }

  private reload() {
    this.notes = this.noteService.getAll();
  }

  private async showTost(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 2000
    });
    toast.present();
  }
}

import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable, ReplaySubject } from 'rxjs';
import { map, switchMap, first, shareReplay } from 'rxjs/operators';
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

  private searchSubject = new ReplaySubject<string>(1);

  ngOnInit() {
    this.category = this.activated.params.pipe(
      map(obj => obj.category),
      shareReplay(1)
    );
    this.searchSubject.next(null);
    this.reload();
  }

  public async delete(note: Note) {
    this.noteService.delete(note).subscribe(
      () => {
        this.reload();
        this.showToast('Gelöscht: ' + note.content.header);
      }
    );
  }

  public async update(note: Note) {
    const modal = await this.modalController.create({component: NoteEditComponent,
      componentProps: {note: note.content}});

    await modal.present();
    const result = await modal.onDidDismiss();

    if (result.role === 'save') {
      this.noteService.update(note.id, result.data).subscribe(
        noter => {
          this.showToast('Aktualisiert:' + noter.content.header);
          this.reload();
        }
      );
    }
  }

  public async create() {
    const noteContent = NoteContent.createDefault();
    const catgoryId = await this.category.pipe(first()).toPromise();
    noteContent.categoryIds = [catgoryId];
    const modal = await this.modalController.create({component: NoteEditComponent,
      componentProps: {note: noteContent}});

    await modal.present();
    const result = await modal.onDidDismiss();

    if (result.role === 'save') {
      this.noteService.create(result.data).subscribe(
        note => {
          this.showToast('Notiz erstellt: ' + note.content.header);
          this.reload();
        }
      );
    }
  }

  public search(obj) {
    if (obj && obj.detail) {
      this.searchSubject.next(obj.detail.value);
    }
  }

  private reload() {
    this.notes = this.category.pipe(
      switchMap(category => this.noteService.getAllByID(category)),
      switchMap(notes =>
        //Todo hier vlt. nen debounce
        this.searchSubject.pipe(map(
          str => {
            if (!notes) {
              return notes;
            }

            if (notes.length === 0 || !str) {
              return notes;
            }

            return notes.filter(
              note => note && note.content
              && (
                note.content.header && note.content.header.toLowerCase().includes(str.toLowerCase())
                || note.content.freeTags && note.content.freeTags.toLowerCase().includes(str.toLowerCase())
                || note.content.text && note.content.text.toLowerCase().includes(str.toLowerCase())
              )
            );
          }
        ))
      )
    );
  }

  private async showToast(message: string) {
    const toast = await this.toastController.create({
      message,
      duration: 2000
    });
    toast.present();
  }
}

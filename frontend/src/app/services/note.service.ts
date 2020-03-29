import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Note, NoteContent } from './note';
import { HttpClient } from '@angular/common/http';
import { not } from '@angular/compiler/src/output/output_ast';

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  constructor(private httpClient: HttpClient) { }

  public getAll(): Observable<Note[]> {
    return this.httpClient.get<Note[]>('http://localhost:5000/notes');
  }

  public create(noteContent: NoteContent): Observable<Note> {
    return this.httpClient.post<Note>('http://localhost:5000/notes', noteContent);
  }

  public delete(note: Note): Observable<any> {
    const uri = 'http://localhost:5000/notes/' + note.id;
    return this.httpClient.delete<any>(uri);
  }
}

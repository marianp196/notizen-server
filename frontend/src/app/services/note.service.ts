import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Note } from './note';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  constructor(private httpClient: HttpClient) { }

  public getAll(): Observable<Note[]> {
    return this.httpClient.get<Note[]>('http://localhost:5000/notes');
  }
}

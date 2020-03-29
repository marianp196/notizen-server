import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor() { }

  public getCategories(): Observable<string[]> {
    const categories = [
      'Serien',
      'Filme',
      'TodoSerienFilme',
      'ToDo',
      'Erlebnisse',
      'Themen'
    ];

    return of(categories);
  }
}

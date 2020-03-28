import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { NoteListComponent } from './note-list/note-list.component';

const routes: Routes = [
  {
    path: 'notes/:category',
    component: NoteListComponent
  },
  {
    path: '',
    redirectTo: 'notes/serien',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [
    RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
  ],
  exports: [RouterModule]
})
export class AppRoutingModule {}

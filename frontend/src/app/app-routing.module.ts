import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { NoteListComponent } from './note-list/note-list.component';
import { SearchMachineComponent } from './search-machine/search-machine.component';

const routes: Routes = [
  {
    path: 'notes/category/:category',
    component: NoteListComponent
  },
  {
    path: 'notes/search',
    component: SearchMachineComponent
  },
  {
    path: '',
    redirectTo: 'notes/search',
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

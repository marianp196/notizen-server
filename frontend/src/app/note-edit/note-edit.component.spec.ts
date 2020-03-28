import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { NoteEditComponent } from './note-edit.component';

describe('NoteEditComponent', () => {
  let component: NoteEditComponent;
  let fixture: ComponentFixture<NoteEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoteEditComponent ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(NoteEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

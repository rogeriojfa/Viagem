import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RotaListaComponent } from './rota-lista.component';

describe('RotaListaComponent', () => {
  let component: RotaListaComponent;
  let fixture: ComponentFixture<RotaListaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RotaListaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RotaListaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

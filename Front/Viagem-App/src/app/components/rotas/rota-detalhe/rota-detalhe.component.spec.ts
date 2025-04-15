import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RotaDetalheComponent } from './rota-detalhe.component';

describe('RotaDetalheComponent', () => {
  let component: RotaDetalheComponent;
  let fixture: ComponentFixture<RotaDetalheComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RotaDetalheComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RotaDetalheComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

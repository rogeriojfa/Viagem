/* tslint:disable:no-unused-variable */

import { TestBed, inject, waitForAsync } from '@angular/core/testing';
import { RotaService } from './rota.service';

describe('Service: Rota', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [RotaService]
    });
  });

  it('should ...', inject([RotaService], (service: RotaService) => {
    expect(service).toBeTruthy();
  }));
});

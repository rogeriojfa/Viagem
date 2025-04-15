import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NgxSpinnerService } from 'ngx-spinner';
import { RotaService } from '@app/services/rota.service';

@Component({
  selector: 'app-buscar-rota',
  templateUrl: './buscar-rota.component.html',
  styleUrls: ['./buscar-rota.component.scss']
})

export class BuscarRotaComponent {
  origem: string = '';
  destino: string = '';
  resultado: any;
  mostrarResultado: boolean = false;
  carregando: boolean = false;
  hasError: boolean;


  constructor(private http: HttpClient, private spinner: NgxSpinnerService, private rotaService: RotaService) {}

  buscarRota(): void {
    this.spinner.show();
    this.hasError = false;

    this.rotaService.buscarMelhorRota(this.origem, this.destino).subscribe({
      next: data => {
        this.resultado = data;
        this.hasError = false;
        this.mostrarResultado = true;
        this.spinner.hide();
      },
      error: err => {
        this.resultado = err.error || 'Erro ao buscar a melhor rota.';
        this.hasError = true;
        this.mostrarResultado = true;
        this.spinner.hide();
      }
    });
  }
}

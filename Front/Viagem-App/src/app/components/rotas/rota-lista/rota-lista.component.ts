import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Rota } from '@app/models/Rota';
import { RotaService } from '@app/services/rota.service';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-rota-lista',
  templateUrl: './rota-lista.component.html',
  styleUrls: ['./rota-lista.component.scss'],
})
export class RotaListaComponent implements OnInit {
  modalRef: BsModalRef;
  public rotas: Rota[] = [];
  public rotaId = 0;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  public filtrarRotas(evt: any): void {
    if (this.termoBuscaChanged.observers.length === 0) {
      this.termoBuscaChanged
        .pipe(debounceTime(1000))
        .subscribe((filtrarPor) => {
          this.spinner.show();
          this.rotaService
            .getRotas(filtrarPor)
            .subscribe(
              (paginatedResult: PaginatedResult<Rota[]>) => {
                this.rotas = paginatedResult.result;

              },
              (error: any) => {
                this.spinner.hide();
                this.toastr.error('Erro ao Carregar as Rotas', 'Erro!');
              }
            )
            .add(() => this.spinner.hide());
        });
    }
    this.termoBuscaChanged.next(evt.value);
  }

  constructor(
    private rotaService: RotaService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  public ngOnInit(): void {
    this.carregarRotas();
  }

  public carregarRotas(): void {
    this.spinner.show();

    this.rotaService
      .getRotas()
      .subscribe(
        (paginatedResult: PaginatedResult<Rota[]>) => {
          this.rotas = paginatedResult.result;
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao Carregar as Rotas', 'Erro!');
        }
      )
      .add(() => this.spinner.hide());
  }

  openModal(event: any, template: TemplateRef<any>, rotaId: number): void {
    event.stopPropagation();
    this.rotaId = rotaId;
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.modalRef.hide();
    this.spinner.show();

    this.rotaService
      .deleteRota(this.rotaId)
      .subscribe(
        (result: any) => {
          if (result.message === 'Deletado') {
            this.toastr.success(
              'A Rota foi deletada com Sucesso.',
              'Deletada!'
            );
            this.carregarRotas();
          }
        },
        (error: any) => {
          console.error(error);
          this.toastr.error(
            `Erro ao tentar deletar a rota ${this.rotaId}`,
            'Erro'
          );
        }
      )
      .add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef.hide();
  }

  detalheRota(id: number): void {
    this.router.navigate([`rotas/detalhe/${id}`]);
  }
}

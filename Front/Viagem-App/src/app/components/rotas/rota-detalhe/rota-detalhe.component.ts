import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl,
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

import { RotaService } from '@app/services/rota.service';
import { Rota } from '@app/models/Rota';
import { DatePipe } from '@angular/common';
import { environment } from '@environments/environment';

@Component({
  selector: 'app-rota-detalhe',
  templateUrl: './rota-detalhe.component.html',
  styleUrls: ['./rota-detalhe.component.scss'],
  providers: [DatePipe],
})
export class RotaDetalheComponent implements OnInit {
  modalRef: BsModalRef;
  rotaId: number;
  rota = {} as Rota;
  form: FormGroup;
  estadoSalvar = 'post';
  loteAtual = { id: 0, nome: '', indice: 0 };
  imagemURL = 'assets/img/upload.png';
  file: File;

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }

  get f(): any {
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      containerClass: 'theme-default',
      showWeekNumbers: false,
    };
  }

  constructor(
    private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRouter: ActivatedRoute,
    private rotaService: RotaService,
    private spinner: NgxSpinnerService,
    private toastr: ToastrService,
    private modalService: BsModalService,
    private router: Router,
    private datePipe: DatePipe
  ) {
    this.localeService.use('pt-br');
  }

  public carregarRota(): void {
    this.rotaId = +this.activatedRouter.snapshot.paramMap.get('id');

    if (this.rotaId !== null && this.rotaId !== 0) {
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.rotaService
        .getRotaById(this.rotaId)
        .subscribe(
          (rota: Rota) => {
            this.rota = { ...rota };
            this.form.patchValue(this.rota);
          },
          (error: any) => {
            this.toastr.error('Erro ao tentar carregar a Rota.', 'Erro!');
            console.error(error);
          }
        )
        .add(() => this.spinner.hide());
    }
  }

  ngOnInit(): void {
    this.carregarRota();
    this.validation();
  }

  public validation(): void {
    this.form = this.fb.group({
      origem: ['', Validators.required],
      destino: ['', Validators.required],
      valor: ['', Validators.required],
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

   public cssValidator(campoForm: FormControl | AbstractControl): any {
     return { 'is-invalid': campoForm.errors && campoForm.touched };
   }

  public salvarRota(): void {
    this.spinner.show();
    if (this.form.valid) {
      this.rota =
        this.estadoSalvar === 'post'
          ? { ...this.form.value }
          : { id: this.rota.id, ...this.form.value };

      this.rotaService[this.estadoSalvar](this.rota).subscribe(
        (rotaRetorno: Rota) => {
          this.toastr.success('Rota salva com Sucesso!', 'Sucesso');
          this.router.navigate([`rotas/lista`]);
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toastr.error('Error ao salvar a rota', 'Erro');
        },
        () => this.spinner.hide()
      );
    }
  }
}

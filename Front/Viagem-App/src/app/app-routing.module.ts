import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RotasComponent } from './components/rotas/rotas.component';
import { RotaDetalheComponent } from './components/rotas/rota-detalhe/rota-detalhe.component';
import { RotaListaComponent } from './components/rotas/rota-lista/rota-lista.component';


import { HomeComponent } from './components/home/home.component';
import { BuscarRotaComponent } from './components/rotas/buscar-rota/buscar-rota.component';


const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: '',
    runGuardsAndResolvers: 'always',

    children: [

      { path: 'rotas', redirectTo: 'rotas/lista' },
      {
        path: 'rotas',
        component: RotasComponent,
        children: [
          { path: 'detalhe/:id', component: RotaDetalheComponent },
          { path: 'detalhe', component: RotaDetalheComponent },
          { path: 'lista', component: RotaListaComponent },
          { path: 'buscar', component: BuscarRotaComponent },

        ],
      },
    ],
  },
  { path: '**', redirectTo: 'rotas/lista', pathMatch: 'full' },
];
@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { Rota } from './../models/Rota';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { take, map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { PaginatedResult } from '@app/models/Pagination';

@Injectable(
// { providedIn: 'root'}
)
export class RotaService {
  baseURL = environment.apiURL + 'api/rotas';

  constructor(private http: HttpClient) { }

  public getRotas(term?: string): Observable<PaginatedResult<Rota[]>> {
    const paginatedResult: PaginatedResult<Rota[]> = new PaginatedResult<Rota[]>();

    let params = new HttpParams;

    if (term != null && term != '')
      params = params.append('term', term)

    return this.http
      .get<Rota[]>(this.baseURL, {observe: 'response', params })
      .pipe(
        take(1),
        map((response) => {
          paginatedResult.result = response.body;
          if(response.headers.has('Pagination')) {
            paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
          }
          return paginatedResult;
        }));
  }

  public getRotaById(id: number): Observable<Rota> {
    return this.http
      .get<Rota>(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

  public post(rota: Rota): Observable<Rota> {
    return this.http
      .post<Rota>(this.baseURL, rota)
      .pipe(take(1));
  }

  public put(rota: Rota): Observable<Rota> {
    return this.http
      .put<Rota>(`${this.baseURL}/${rota.id}`, rota)
      .pipe(take(1));
  }

  public deleteRota(id: number): Observable<any> {
    return this.http
      .delete(`${this.baseURL}/${id}`)
      .pipe(take(1));
  }

  buscarMelhorRota(origem: string, destino: string): Observable<string> {
    const params = new HttpParams()
      .set('origem', origem)
      .set('destino', destino);

    return this.http.get(`${this.baseURL}/BuscarMelhorRota`, {
      params,
      responseType: 'text'
    });
  }
}

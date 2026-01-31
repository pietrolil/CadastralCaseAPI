import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { NaturalPerson } from '../models/natural-person.model';

@Injectable({
  providedIn: 'root'
})
export class NaturalPersonService {
  private apiUrl = `${environment.apiUrl}/naturalPerson`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<NaturalPerson[]> {
    return this.http.get<NaturalPerson[]>(this.apiUrl);
  }

  getById(id: string): Observable<NaturalPerson> {
    return this.http.get<NaturalPerson>(`${this.apiUrl}/${id}`);
  }

  create(person: NaturalPerson): Observable<NaturalPerson> {
    return this.http.post<NaturalPerson>(this.apiUrl, person);
  }

  update(id: string, person: NaturalPerson): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, person);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

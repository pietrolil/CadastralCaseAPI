import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LegalPerson } from '../models/legal-person.model';

@Injectable({
  providedIn: 'root'
})
export class LegalPersonService {
  private apiUrl = `${environment.apiUrl}/legalPerson`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<LegalPerson[]> {
    return this.http.get<LegalPerson[]>(this.apiUrl);
  }

  getById(id: string): Observable<LegalPerson> {
    return this.http.get<LegalPerson>(`${this.apiUrl}/${id}`);
  }

  create(person: LegalPerson): Observable<LegalPerson> {
    return this.http.post<LegalPerson>(this.apiUrl, person);
  }

  update(id: string, person: LegalPerson): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, person);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

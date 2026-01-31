import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { NaturalPersonService } from './natural-person.service';
import { NaturalPerson } from '../models/natural-person.model';
import { environment } from '../../environments/environment';

describe('NaturalPersonService', () => {
  let service: NaturalPersonService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.apiUrl}/naturalPerson`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        NaturalPersonService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(NaturalPersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all natural persons', () => {
    const mockPersons: NaturalPerson[] = [
      { id: '1', name: 'João Silva', taxId: '123.456.789-09', birthDate: '1990-01-01' }
    ];

    service.getAll().subscribe(persons => {
      expect(persons.length).toBe(1);
      expect(persons).toEqual(mockPersons);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockPersons);
  });

  it('should create natural person', () => {
    const newPerson: NaturalPerson = { 
      name: 'João Silva', 
      taxId: '123.456.789-09', 
      birthDate: '1990-01-01' 
    };

    service.create(newPerson).subscribe(person => {
      expect(person).toEqual(newPerson);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(newPerson);
  });

  it('should delete natural person', () => {
    service.delete('1').subscribe();

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});

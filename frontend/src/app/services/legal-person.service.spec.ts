import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { LegalPersonService } from './legal-person.service';
import { LegalPerson } from '../models/legal-person.model';
import { environment } from '../../environments/environment';

describe('LegalPersonService', () => {
  let service: LegalPersonService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.apiUrl}/legalPerson`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        LegalPersonService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(LegalPersonService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all legal persons', () => {
    const mockPersons: LegalPerson[] = [
      { 
        id: '1', 
        companyName: 'Empresa Teste LTDA',
        tradeName: 'Teste',
        taxId: '12.345.678/0001-95', 
        foundingDate: '2020-01-01' 
      }
    ];

    service.getAll().subscribe(persons => {
      expect(persons.length).toBe(1);
      expect(persons).toEqual(mockPersons);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockPersons);
  });

  it('should create legal person', () => {
    const newPerson: LegalPerson = { 
      companyName: 'Empresa Teste LTDA',
      tradeName: 'Teste', 
      taxId: '12.345.678/0001-95', 
      foundingDate: '2020-01-01' 
    };

    service.create(newPerson).subscribe(person => {
      expect(person).toEqual(newPerson);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    req.flush(newPerson);
  });
});

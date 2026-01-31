import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting, HttpTestingController } from '@angular/common/http/testing';
import { AddressService } from './address.service';
import { Address } from '../models/address.model';
import { environment } from '../../environments/environment';

describe('AddressService', () => {
  let service: AddressService;
  let httpMock: HttpTestingController;
  const apiUrl = `${environment.apiUrl}/address`;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        AddressService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(AddressService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all addresses', () => {
    const mockAddresses: Address[] = [
      { id: '1', postalCode: '12345-678', number: '100', street: 'Rua Teste', city: 'São Paulo', state: 'SP' }
    ];

    service.getAll().subscribe(addresses => {
      expect(addresses.length).toBe(1);
      expect(addresses).toEqual(mockAddresses);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('GET');
    req.flush(mockAddresses);
  });

  it('should get address by id', () => {
    const mockAddress: Address = { 
      id: '1', 
      postalCode: '12345-678',
      number: '100', 
      street: 'Rua Teste', 
      city: 'São Paulo', 
      state: 'SP' 
    };

    service.getById('1').subscribe(address => {
      expect(address).toEqual(mockAddress);
    });

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('GET');
    req.flush(mockAddress);
  });

  it('should create address', () => {
    const newAddress: Address = { 
      postalCode: '12345-678',
      number: '100', 
      street: 'Rua Teste', 
      city: 'São Paulo', 
      state: 'SP' 
    };

    service.create(newAddress).subscribe(address => {
      expect(address).toEqual(newAddress);
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newAddress);
    req.flush(newAddress);
  });

  it('should update address', () => {
    const updatedAddress: Address = { 
      id: '1', 
      postalCode: '12345-678',
      number: '100', 
      street: 'Rua Atualizada', 
      city: 'São Paulo', 
      state: 'SP' 
    };

    service.update('1', updatedAddress).subscribe();

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedAddress);
    req.flush(null);
  });

  it('should delete address', () => {
    service.delete('1').subscribe();

    const req = httpMock.expectOne(`${apiUrl}/1`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});

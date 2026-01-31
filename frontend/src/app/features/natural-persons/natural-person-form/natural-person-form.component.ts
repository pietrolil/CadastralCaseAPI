import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { NaturalPersonService } from '../../../services/natural-person.service';
import { AddressService } from '../../../services/address.service';
import { NaturalPerson } from '../../../models/natural-person.model';
import { Address } from '../../../models/address.model';

@Component({
  selector: 'app-natural-person-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    ButtonModule,
    CalendarModule,
    DropdownModule,
    ToastModule
  ],
  providers: [MessageService],
  template: `
    <p-toast></p-toast>
    
    <div class="card">
      <div class="header-section">
        <h2>{{ isEditMode ? 'Editar' : 'Nova' }} Pessoa Física</h2>
      </div>

      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <div class="form-grid">
          <div class="form-field">
            <label for="name">Nome *</label>
            <input 
              pInputText 
              id="name" 
              formControlName="name"
              [class.p-invalid]="form.get('name')?.invalid && form.get('name')?.touched"
              class="w-full" />
            <small class="p-error" *ngIf="form.get('name')?.invalid && form.get('name')?.touched">
              <i class="pi pi-exclamation-circle"></i> Nome é obrigatório
            </small>
          </div>

          <div class="form-field">
            <label for="taxId">CPF *</label>
            <input 
              pInputText 
              id="taxId" 
              formControlName="taxId"
              placeholder="000.000.000-00"
              [class.p-invalid]="form.get('taxId')?.invalid && form.get('taxId')?.touched"
              class="w-full" />
            <small class="p-error" *ngIf="form.get('taxId')?.invalid && form.get('taxId')?.touched">
              <i class="pi pi-exclamation-circle"></i> CPF é obrigatório
            </small>
          </div>

          <div class="form-field">
            <label for="birthDate">Data de Nascimento *</label>
            <p-calendar 
              id="birthDate" 
              formControlName="birthDate"
              dateFormat="dd/mm/yy"
              [showIcon]="true"
              [styleClass]="form.get('birthDate')?.invalid && form.get('birthDate')?.touched ? 'w-full p-invalid' : 'w-full'">
            </p-calendar>
            <small class="p-error" *ngIf="form.get('birthDate')?.invalid && form.get('birthDate')?.touched">
              <i class="pi pi-exclamation-circle"></i> Data de nascimento é obrigatória
            </small>
          </div>

          <div class="form-field">
            <label for="email">Email</label>
            <input 
              pInputText 
              id="email" 
              formControlName="email"
              type="email"
              [class.p-invalid]="form.get('email')?.invalid && form.get('email')?.touched"
              class="w-full" />
            <small class="p-error" *ngIf="form.get('email')?.invalid && form.get('email')?.touched">
              <i class="pi pi-exclamation-circle"></i> Email inválido
            </small>
          </div>

          <div class="form-field">
            <label for="phone">Telefone</label>
            <input 
              pInputText 
              id="phone" 
              formControlName="phone"
              placeholder="(00) 00000-0000"
              class="w-full" />
          </div>

          <div class="form-field">
            <label for="addressId">Endereço</label>
            <p-dropdown 
              id="addressId"
              formControlName="addressId"
              [options]="addresses"
              optionLabel="displayLabel"
              optionValue="id"
              placeholder="Selecione um endereço"
              [filter]="true"
              filterBy="displayLabel"
              filterPlaceholder="Buscar endereço..."
              [showClear]="true"
              emptyMessage="Nenhum endereço cadastrado"
              emptyFilterMessage="Nenhum endereço encontrado"
              [styleClass]="'w-full address-dropdown'">
              <ng-template pTemplate="selectedItem" let-address>
                <div class="selected-address" *ngIf="address">
                  <i class="pi pi-map-marker" style="color: #3b82f6; margin-right: 0.5rem;"></i>
                  <span>{{ address.street }}, {{ address.number }} - {{ address.city }}/{{ address.state }}</span>
                </div>
              </ng-template>
              <ng-template pTemplate="item" let-address>
                <div class="address-item">
                  <div class="address-main">
                    <i class="pi pi-map-marker" style="color: #3b82f6;"></i>
                    <span class="address-street">{{ address.street }}, {{ address.number }}</span>
                  </div>
                  <div class="address-details">
                    <span class="address-district">{{ address.district }} - {{ address.city }}/{{ address.state }}</span>
                  </div>
                  <div class="address-cep">
                    <i class="pi pi-inbox" style="font-size: 0.7rem;"></i>
                    <span>CEP: {{ address.postalCode }}</span>
                  </div>
                </div>
              </ng-template>
            </p-dropdown>
            <small class="p-info">
              <i class="pi pi-info-circle"></i> Opcional - Primeiro cadastre endereços em <a [routerLink]="['/addresses']" class="info-link">Endereços</a>
            </small>
          </div>
        </div>

        <div class="form-actions">
          <p-button 
            label="Cancelar" 
            severity="secondary"
            [outlined]="true"
            [routerLink]="['/natural-persons']">
          </p-button>
          <p-button 
            label="Salvar" 
            type="submit"
            [disabled]="form.invalid || loading"
            [loading]="loading">
          </p-button>
        </div>
      </form>
    </div>
  `,
  styles: [`
    .card {
      background: white;
      padding: 2.5rem;
      border-radius: 12px;
      box-shadow: 0 4px 12px rgba(0,0,0,0.08);
      border: 1px solid #e5e7eb;
    }

    .header-section {
      margin-bottom: 2rem;
      padding-bottom: 1rem;
      border-bottom: 2px solid #f3f4f6;
    }

    h2 {
      margin: 0;
      color: #333;
      font-size: 1.75rem;
      font-weight: 600;
    }

    .form-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 1.5rem;
      margin-bottom: 2rem;
    }

    .form-field {
      display: flex;
      flex-direction: column;
    }

    label {
      margin-bottom: 0.5rem;
      font-weight: 600;
      color: #374151;
      font-size: 0.875rem;
    }

    .w-full {
      width: 100%;
    }

    ::ng-deep .w-full .p-calendar {
      width: 100%;
    }

    ::ng-deep .p-dropdown,
    ::ng-deep .p-calendar .p-inputtext {
      background: #ffffff !important;
    }

    ::ng-deep .p-dropdown-panel,
    ::ng-deep .p-datepicker {
      background: #ffffff !important;
    }

    ::ng-deep .p-invalid.p-inputtext,
    ::ng-deep .p-invalid .p-inputtext,
    ::ng-deep .p-invalid.p-dropdown {
      border-color: #ef4444 !important;
    }

    ::ng-deep .p-invalid.p-inputtext:focus,
    ::ng-deep .p-invalid .p-inputtext:focus,
    ::ng-deep .p-invalid.p-dropdown:focus {
      box-shadow: 0 0 0 0.2rem rgba(239, 68, 68, 0.25) !important;
    }

    .p-error {
      color: #ef4444;
      margin-top: 0.5rem;
      font-size: 0.875rem;
      display: flex;
      align-items: center;
      gap: 0.25rem;
      animation: slideDown 0.3s ease;
    }

    .p-info {
      color: #6b7280;
      margin-top: 0.5rem;
      font-size: 0.875rem;
      display: flex;
      align-items: center;
      gap: 0.25rem;
    }

    .info-link {
      color: #3b82f6;
      text-decoration: underline;
      font-weight: 500;
      transition: color 0.2s ease;
    }

    .info-link:hover {
      color: #2563eb;
    }

    ::ng-deep .address-dropdown {
      background: #ffffff !important;
    }

    ::ng-deep .address-dropdown .p-dropdown-label {
      padding: 0.75rem !important;
      display: flex;
      align-items: center;
    }

    .selected-address {
      display: flex;
      align-items: center;
      font-size: 0.95rem;
      color: #1f2937;
    }

    ::ng-deep .address-item {
      padding: 0.5rem 0;
      display: flex;
      flex-direction: column;
      gap: 0.375rem;
    }

    ::ng-deep .address-main {
      display: flex;
      align-items: center;
      gap: 0.5rem;
      font-weight: 600;
      color: #1f2937;
      font-size: 0.95rem;
    }

    ::ng-deep .address-street {
      flex: 1;
    }

    ::ng-deep .address-details {
      display: flex;
      align-items: center;
      padding-left: 1.75rem;
    }

    ::ng-deep .address-district {
      font-size: 0.875rem;
      color: #6b7280;
    }

    ::ng-deep .address-cep {
      display: flex;
      align-items: center;
      gap: 0.375rem;
      padding-left: 1.75rem;
      font-size: 0.8rem;
      color: #9ca3af;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-items .p-dropdown-item {
      padding: 0.875rem 1rem !important;
      transition: background-color 0.2s ease;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-items .p-dropdown-item:hover {
      background: #f3f4f6 !important;
    }

    ::ng-deep .p-dropdown-panel .p-dropdown-items .p-dropdown-item.p-highlight {
      background: #dbeafe !important;
      color: #1e40af !important;
    }

    ::ng-deep .p-dropdown-filter-container {
      padding: 0.75rem !important;
    }

    ::ng-deep .p-dropdown-filter {
      padding: 0.625rem 0.75rem !important;
      width: 100% !important;
    }

    @keyframes slideDown {
      from {
        opacity: 0;
        transform: translateY(-10px);
      }
      to {
        opacity: 1;
        transform: translateY(0);
      }
    }

    .form-actions {
      display: flex;
      gap: 1rem;
      justify-content: flex-end;
      padding-top: 1.5rem;
      border-top: 1px solid #e5e7eb;
    }

    @media (max-width: 768px) {
      .card {
        padding: 1.5rem;
      }

      .form-grid {
        grid-template-columns: 1fr;
      }

      .form-actions {
        flex-direction: column-reverse;
      }

      .form-actions ::ng-deep .p-button {
        width: 100%;
      }
    }
  `]
})
export class NaturalPersonFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  loading = false;
  personId?: string;
  addresses: Address[] = [];

  constructor(
    private fb: FormBuilder,
    private personService: NaturalPersonService,
    private addressService: AddressService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      taxId: ['', Validators.required],
      birthDate: ['', Validators.required],
      email: ['', Validators.email],
      phone: [''],
      addressId: [null]
    });
  }

  ngOnInit(): void {
    this.loadAddresses();
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.personId = id;
      this.loadPerson(this.personId);
    }
  }

  loadAddresses(): void {
    this.addressService.getAll().subscribe({
      next: (addresses) => {
        this.addresses = addresses.map(addr => ({
          ...addr,
          displayLabel: `${addr.street}, ${addr.number} - ${addr.city}/${addr.state} (CEP: ${addr.postalCode})`
        }));
      },
      error: (error) => {
        this.messageService.add({
          severity: 'warn',
          summary: 'Aviso',
          detail: this.getErrorMessage(error, 'Não foi possível carregar os endereços')
        });
      }
    });
  }

  loadPerson(id: string): void {
    this.loading = true;
    this.personService.getById(id).subscribe({
      next: (person) => {
        this.form.patchValue({
          ...person,
          birthDate: new Date(person.birthDate)
        });
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(error, 'Erro ao carregar pessoa física')
        });
        this.loading = false;
      }
    });
  }

  private getErrorMessage(error: any, defaultMessage: string): string {
    if (error?.error?.message) {
      return error.error.message;
    }
    if (error?.error?.errors) {
      const errors = error.error.errors;
      const firstError = Object.values(errors)[0];
      if (Array.isArray(firstError) && firstError.length > 0) {
        return firstError[0] as string;
      }
    }
    if (error?.message) {
      return error.message;
    }
    return defaultMessage;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    const person: NaturalPerson = this.form.value;

    const operation = this.isEditMode
      ? this.personService.update(this.personId!, person)
      : this.personService.create(person) as any;

    operation.subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: `Pessoa física ${this.isEditMode ? 'atualizada' : 'criada'} com sucesso`
        });
        setTimeout(() => {
          this.router.navigate(['/natural-persons']);
        }, 1000);
      },
      error: (err: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(err, `Erro ao ${this.isEditMode ? 'atualizar' : 'criar'} pessoa física`)
        });
        this.loading = false;
      }
    });
  }
}

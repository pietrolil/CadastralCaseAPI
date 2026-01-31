import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { AddressService } from '../../../services/address.service';
import { Address } from '../../../models/address.model';

@Component({
  selector: 'app-address-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    ButtonModule,
    ToastModule
  ],
  providers: [MessageService],
  template: `
    <p-toast></p-toast>
    
    <div class="card">
      <div class="header-section">
        <h2>{{ isEditMode ? 'Editar' : 'Novo' }} Endereço</h2>
      </div>

      <form [formGroup]="form" (ngSubmit)="onSubmit()">
        <div class="form-grid">
          <div class="form-field">
            <label for="postalCode">CEP *</label>
            <input 
              pInputText 
              id="postalCode" 
              formControlName="postalCode"
              placeholder="00000-000"
              [class.p-invalid]="form.get('postalCode')?.invalid && form.get('postalCode')?.touched"
              class="w-full" />
            <small class="p-error" *ngIf="form.get('postalCode')?.invalid && form.get('postalCode')?.touched">
              <i class="pi pi-exclamation-circle"></i> CEP é obrigatório
            </small>
          </div>

          <div class="form-field">
            <label for="street">Logradouro</label>
            <input 
              pInputText 
              id="street" 
              formControlName="street"
              class="w-full" />
          </div>

          <div class="form-field">
            <label for="number">Número *</label>
            <input 
              pInputText 
              id="number" 
              formControlName="number"
              [class.p-invalid]="form.get('number')?.invalid && form.get('number')?.touched"
              class="w-full" />
            <small class="p-error" *ngIf="form.get('number')?.invalid && form.get('number')?.touched">
              <i class="pi pi-exclamation-circle"></i> Número é obrigatório
            </small>
          </div>

          <div class="form-field">
            <label for="complement">Complemento</label>
            <input 
              pInputText 
              id="complement" 
              formControlName="complement"
              class="w-full" />
          </div>

          <div class="form-field">
            <label for="district">Bairro</label>
            <input 
              pInputText 
              id="district" 
              formControlName="district"
              class="w-full" />
          </div>

          <div class="form-field">
            <label for="city">Cidade</label>
            <input 
              pInputText 
              id="city" 
              formControlName="city"
              class="w-full" />
          </div>

          <div class="form-field">
            <label for="state">Estado</label>
            <input 
              pInputText 
              id="state" 
              formControlName="state"
              placeholder="UF"
              maxlength="2"
              class="w-full" />
          </div>

          <div class="form-field">
            <label for="stateName">Nome do Estado</label>
            <input 
              pInputText 
              id="stateName" 
              formControlName="stateName"
              class="w-full" />
          </div>
        </div>

        <div class="form-actions">
          <p-button 
            label="Cancelar" 
            severity="secondary"
            [outlined]="true"
            [routerLink]="['/addresses']">
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
      font-weight: 500;
      color: #333;
    }

    .w-full {
      width: 100%;
    }

    ::ng-deep .p-invalid {
      border-color: #ef4444 !important;
    }

    ::ng-deep .p-invalid:focus {
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
export class AddressFormComponent implements OnInit {
  form: FormGroup;
  isEditMode = false;
  loading = false;
  addressId?: string;

  constructor(
    private fb: FormBuilder,
    private addressService: AddressService,
    private messageService: MessageService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.form = this.fb.group({
      postalCode: ['', Validators.required],
      street: [''],
      number: ['', Validators.required],
      complement: [''],
      district: [''],
      city: [''],
      state: [''],
      stateName: [''],
      ibgeCode: [''],
      areaCode: [''],
      queryViaCep: [true]
    });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.addressId = id;
      this.loadAddress(this.addressId);
    }
  }

  loadAddress(id: string): void {
    this.loading = true;
    this.addressService.getById(id).subscribe({
      next: (address) => {
        this.form.patchValue(address);
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(error, 'Erro ao carregar endereço')
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
    const address: Address = this.form.value;

    const operation = this.isEditMode
      ? this.addressService.update(this.addressId!, address)
      : this.addressService.create(address) as any;

    operation.subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: `Endereço ${this.isEditMode ? 'atualizado' : 'criado'} com sucesso`
        });
        setTimeout(() => {
          this.router.navigate(['/addresses']);
        }, 1000);
      },
      error: (err: any) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(err, `Erro ao ${this.isEditMode ? 'atualizar' : 'criar'} endereço`)
        });
        this.loading = false;
      }
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { AddressService } from '../../../services/address.service';
import { Address } from '../../../models/address.model';

@Component({
  selector: 'app-address-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    TableModule,
    ButtonModule,
    ToastModule,
    ConfirmDialogModule
  ],
  providers: [MessageService, ConfirmationService],
  template: `
    <p-toast></p-toast>
    <p-confirmDialog></p-confirmDialog>
    
    <div class="card">
      <div class="header-section">
        <h2>Endereços</h2>
        <p-button 
          label="Novo Endereço" 
          icon="pi pi-plus" 
          [routerLink]="['/addresses/new']">
        </p-button>
      </div>

      <p-table 
        [value]="addresses" 
        [loading]="loading"
        [paginator]="true" 
        [rows]="10"
        [showCurrentPageReport]="true"
        currentPageReportTemplate="Exibindo {first} a {last} de {totalRecords} registros"
        [rowsPerPageOptions]="[10, 25, 50]">
        
        <ng-template pTemplate="header">
          <tr>
            <th>CEP</th>
            <th>Logradouro</th>
            <th>Número</th>
            <th>Cidade</th>
            <th>Estado</th>
            <th style="width: 150px">Ações</th>
          </tr>
        </ng-template>
        
        <ng-template pTemplate="body" let-address>
          <tr>
            <td>{{address.postalCode}}</td>
            <td>{{address.street}}</td>
            <td>{{address.number || '-'}}</td>
            <td>{{address.city}}</td>
            <td>{{address.state}}</td>
            <td>
              <p-button 
                icon="pi pi-pencil" 
                [rounded]="true"
                [text]="true"
                severity="info"
                [routerLink]="['/addresses/edit', address.id]">
              </p-button>
              <p-button 
                icon="pi pi-trash" 
                [rounded]="true"
                [text]="true"
                severity="danger"
                (onClick)="confirmDelete(address)">
              </p-button>
            </td>
          </tr>
        </ng-template>
        
        <ng-template pTemplate="emptymessage">
          <tr>
            <td colspan="6" class="text-center">Nenhum endereço encontrado.</td>
          </tr>
        </ng-template>
      </p-table>
    </div>
  `,
  styles: [`
    .card {
      background: white;
      padding: 2rem;
      border-radius: 8px;
      box-shadow: 0 2px 8px rgba(0,0,0,0.1);
    }

    .header-section {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 1.5rem;
    }

    h2 {
      margin: 0;
      color: #333;
    }

    .text-center {
      text-align: center;
    }
  `]
})
export class AddressListComponent implements OnInit {
  addresses: Address[] = [];
  loading = false;

  constructor(
    private addressService: AddressService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.loadAddresses();
  }

  loadAddresses(): void {
    this.loading = true;
    this.addressService.getAll().subscribe({
      next: (data) => {
        this.addresses = data;
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(error, 'Erro ao carregar endereços')
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

  confirmDelete(address: Address): void {
    this.confirmationService.confirm({
      message: `Deseja realmente excluir o endereço ${address.street}?`,
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.deleteAddress(address.id!);
      }
    });
  }

  deleteAddress(id: string): void {
    this.addressService.delete(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: 'Endereço excluído com sucesso'
        });
        this.loadAddresses();
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(error, 'Erro ao excluir endereço')
        });
      }
    });
  }
}

import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { MessageService, ConfirmationService } from 'primeng/api';
import { NaturalPersonService } from '../../../services/natural-person.service';
import { NaturalPerson } from '../../../models/natural-person.model';

@Component({
  selector: 'app-natural-person-list',
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
        <h2>Pessoas Físicas</h2>
        <p-button 
          label="Nova Pessoa Física" 
          icon="pi pi-plus" 
          [routerLink]="['/natural-persons/new']">
        </p-button>
      </div>

      <p-table 
        [value]="persons" 
        [loading]="loading"
        [paginator]="true" 
        [rows]="10"
        [showCurrentPageReport]="true"
        currentPageReportTemplate="Exibindo {first} a {last} de {totalRecords} registros"
        [rowsPerPageOptions]="[10, 25, 50]">
        
        <ng-template pTemplate="header">
          <tr>
            <th>Nome</th>
            <th>CPF</th>
            <th>Data de Nascimento</th>
            <th>Email</th>
            <th>Telefone</th>
            <th style="width: 150px">Ações</th>
          </tr>
        </ng-template>
        
        <ng-template pTemplate="body" let-person>
          <tr>
            <td>{{person.name}}</td>
            <td>{{person.taxId}}</td>
            <td>{{person.birthDate | date:'dd/MM/yyyy'}}</td>
            <td>{{person.email || '-'}}</td>
            <td>{{person.phone || '-'}}</td>
            <td>
              <p-button 
                icon="pi pi-pencil" 
                [rounded]="true"
                [text]="true"
                severity="info"
                [routerLink]="['/natural-persons/edit', person.id]">
              </p-button>
              <p-button 
                icon="pi pi-trash" 
                [rounded]="true"
                [text]="true"
                severity="danger"
                (onClick)="confirmDelete(person)">
              </p-button>
            </td>
          </tr>
        </ng-template>
        
        <ng-template pTemplate="emptymessage">
          <tr>
            <td colspan="6" class="text-center">Nenhuma pessoa física encontrada.</td>
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
export class NaturalPersonListComponent implements OnInit {
  persons: NaturalPerson[] = [];
  loading = false;

  constructor(
    private personService: NaturalPersonService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.loadPersons();
  }

  loadPersons(): void {
    this.loading = true;
    this.personService.getAll().subscribe({
      next: (data) => {
        this.persons = data;
        this.loading = false;
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(error, 'Erro ao carregar pessoas físicas')
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

  confirmDelete(person: NaturalPerson): void {
    this.confirmationService.confirm({
      message: `Deseja realmente excluir ${person.name}?`,
      header: 'Confirmar Exclusão',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.deletePerson(person.id!);
      }
    });
  }

  deletePerson(id: string): void {
    this.personService.delete(id).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Sucesso',
          detail: 'Pessoa física excluída com sucesso'
        });
        this.loadPersons();
      },
      error: (error) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Erro',
          detail: this.getErrorMessage(error, 'Erro ao excluir pessoa física')
        });
      }
    });
  }
}

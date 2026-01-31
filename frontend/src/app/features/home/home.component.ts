import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ButtonModule } from 'primeng/button';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterModule, ButtonModule],
  template: `
    <div class="home-container">
      <div class="content-wrapper">
        <div class="header-section">
          <h1>Sistema de Cadastro</h1>
          <div class="divider"></div>
        </div>

        <div class="description-section">
          <p class="intro">
            Bem-vindo ao <strong>Sistema de Cadastro</strong>, uma aplicação completa para gerenciamento 
            de pessoas físicas, jurídicas e endereços.
          </p>

          <div class="features">
            <h2>Funcionalidades</h2>
            <ul>
              <li>
                <i class="pi pi-check-circle"></i>
                <span><strong>Pessoas Físicas:</strong> Cadastro completo com CPF, data de nascimento, email e telefone</span>
              </li>
              <li>
                <i class="pi pi-check-circle"></i>
                <span><strong>Pessoas Jurídicas:</strong> Gestão de empresas com CNPJ, razão social e nome fantasia</span>
              </li>
              <li>
                <i class="pi pi-check-circle"></i>
                <span><strong>Endereços:</strong> Integração com ViaCEP para consulta automática de CEP</span>
              </li>
            </ul>
          </div>

          <div class="cta-section">
            <p>Utilize o menu lateral para navegar entre as funcionalidades do sistema.</p>
            <div class="button-group">
              <p-button 
                label="Pessoas Físicas" 
                icon="pi pi-user"
                [routerLink]="['/natural-persons']"
                styleClass="p-button-raised">
              </p-button>
              <p-button 
                label="Pessoas Jurídicas" 
                icon="pi pi-building"
                [routerLink]="['/legal-persons']"
                styleClass="p-button-raised">
              </p-button>
              <p-button 
                label="Endereços" 
                icon="pi pi-map-marker"
                [routerLink]="['/addresses']"
                styleClass="p-button-raised">
              </p-button>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .home-container {
      min-height: calc(100vh - 120px);
      padding: 2rem;
      background: linear-gradient(135deg, #f5f7fa 0%, #c3cfe2 100%);
    }

    .content-wrapper {
      max-width: 900px;
      margin: 0 auto;
      background: white;
      border-radius: 16px;
      box-shadow: 0 8px 24px rgba(0,0,0,0.12);
      padding: 3rem;
    }

    .header-section {
      text-align: center;
      margin-bottom: 2.5rem;
    }

    .header-section h1 {
      font-size: 2.5rem;
      color: #1976d2;
      margin: 0 0 1rem 0;
      font-weight: 700;
    }

    .divider {
      width: 100px;
      height: 4px;
      background: linear-gradient(90deg, #1976d2, #2196f3);
      margin: 0 auto;
      border-radius: 2px;
    }

    .description-section {
      color: #333;
    }

    .intro {
      font-size: 1.1rem;
      line-height: 1.8;
      color: #555;
      margin-bottom: 2.5rem;
      text-align: center;
    }

    .features, .tech-stack, .cta-section {
      margin-bottom: 2.5rem;
    }

    h2 {
      font-size: 1.5rem;
      color: #1976d2;
      margin-bottom: 1.5rem;
      font-weight: 600;
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .features ul {
      list-style: none;
      padding: 0;
      margin: 0;
    }

    .features li {
      display: flex;
      align-items: flex-start;
      gap: 1rem;
      margin-bottom: 1rem;
      padding: 1rem;
      background: #f8f9fa;
      border-radius: 8px;
      transition: transform 0.2s;
    }

    .features li:hover {
      transform: translateX(8px);
      background: #e3f2fd;
    }

    .features li i {
      color: #4caf50;
      font-size: 1.25rem;
      margin-top: 0.25rem;
    }

    .features li span {
      flex: 1;
      line-height: 1.6;
    }

    .cta-section {
      text-align: center;
      padding: 2rem;
      background: linear-gradient(135deg, #e3f2fd 0%, #bbdefb 100%);
      border-radius: 12px;
      margin-bottom: 0;
    }

    .cta-section p {
      font-size: 1.1rem;
      color: #555;
      margin-bottom: 1.5rem;
    }

    .button-group {
      display: flex;
      gap: 1rem;
      justify-content: center;
      flex-wrap: wrap;
    }

    @media (max-width: 768px) {
      .home-container {
        padding: 1rem;
      }

      .content-wrapper {
        padding: 2rem 1.5rem;
      }

      .header-section h1 {
        font-size: 2rem;
      }

      .tech-grid {
        grid-template-columns: 1fr;
      }

      .button-group {
        flex-direction: column;
      }

      .button-group ::ng-deep .p-button {
        width: 100%;
      }
    }
  `]
})
export class HomeComponent {}

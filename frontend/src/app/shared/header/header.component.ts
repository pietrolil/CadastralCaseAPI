import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  template: `
    <header class="header">
      <div class="container">
        <div class="logo">
          <i class="pi pi-home"></i>
          <h1>Sistema Cadastral</h1>
        </div>
      </div>
    </header>
  `,
  styles: [`
    .header {
      background-color: #1976d2;
      color: white;
      padding: 1rem 0;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .container {
      max-width: 1200px;
      margin: 0 auto;
      padding: 0 1rem;
      display: flex;
      justify-content: space-between;
      align-items: center;
    }

    .logo {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    .logo h1 {
      margin: 0;
      font-size: 1.5rem;
      font-weight: 500;
    }

    .user-info {
      display: flex;
      align-items: center;
      gap: 0.5rem;
    }

    i {
      font-size: 1.5rem;
    }
  `]
})
export class HeaderComponent {}

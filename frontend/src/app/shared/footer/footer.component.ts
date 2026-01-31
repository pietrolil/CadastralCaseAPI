import { Component } from '@angular/core';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [],
  template: `
    <footer class="footer">
      <div class="container">
        <p>&copy; 2026 Sistema Cadastral. Todos os direitos reservados.</p>
      </div>
    </footer>
  `,
  styles: [`
    .footer {
      background-color: #f5f5f5;
      padding: 1rem 0;
      text-align: center;
      border-top: 1px solid #ddd;
      margin-top: auto;
    }

    .container {
      max-width: 1200px;
      margin: 0 auto;
      padding: 0 1rem;
    }

    p {
      margin: 0;
      color: #666;
    }
  `]
})
export class FooterComponent {}

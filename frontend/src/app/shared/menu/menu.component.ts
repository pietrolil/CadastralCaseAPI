import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="menu">
      <ul>
        <li>
          <a routerLink="/home" routerLinkActive="active" [routerLinkActiveOptions]="{exact: true}">
            <i class="pi pi-home"></i>
            <span>Início</span>
          </a>
        </li>
        <li>
          <a routerLink="/natural-persons" routerLinkActive="active">
            <i class="pi pi-user"></i>
            <span>Pessoas Físicas</span>
          </a>
        </li>
        <li>
          <a routerLink="/legal-persons" routerLinkActive="active">
            <i class="pi pi-building"></i>
            <span>Pessoas Jurídicas</span>
          </a>
        </li>
        <li>
          <a routerLink="/addresses" routerLinkActive="active">
            <i class="pi pi-map-marker"></i>
            <span>Endereços</span>
          </a>
        </li>
      </ul>
    </nav>
  `,
  styles: [`
    .menu {
      background-color: #f5f5f5;
      min-height: calc(100vh - 120px);
      padding: 1rem 0;
      box-shadow: 2px 0 4px rgba(0,0,0,0.1);
    }

    ul {
      list-style: none;
      padding: 0;
      margin: 0;
    }

    li {
      margin-bottom: 0.5rem;
    }

    a {
      display: flex;
      align-items: center;
      gap: 0.75rem;
      padding: 0.75rem 1.5rem;
      color: #333;
      text-decoration: none;
      transition: all 0.3s;
    }

    a:hover {
      background-color: #e0e0e0;
    }

    a.active {
      background-color: #1976d2;
      color: white;
    }

    i {
      font-size: 1.25rem;
    }
  `]
})
export class MenuComponent {}

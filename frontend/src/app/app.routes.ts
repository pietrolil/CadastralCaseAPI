import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    loadComponent: () => import('./features/home/home.component')
      .then(m => m.HomeComponent)
  },
  {
    path: 'addresses',
    loadChildren: () => import('./features/addresses/addresses.routes')
      .then(m => m.addressRoutes)
  },
  {
    path: 'natural-persons',
    loadChildren: () => import('./features/natural-persons/natural-persons.routes')
      .then(m => m.naturalPersonRoutes)
  },
  {
    path: 'legal-persons',
    loadChildren: () => import('./features/legal-persons/legal-persons.routes')
      .then(m => m.legalPersonRoutes)
  },
  {
    path: '**',
    redirectTo: '/home'
  }
];

import { Routes } from '@angular/router';

export const legalPersonRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./legal-person-list/legal-person-list.component')
      .then(m => m.LegalPersonListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./legal-person-form/legal-person-form.component')
      .then(m => m.LegalPersonFormComponent)
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./legal-person-form/legal-person-form.component')
      .then(m => m.LegalPersonFormComponent)
  }
];

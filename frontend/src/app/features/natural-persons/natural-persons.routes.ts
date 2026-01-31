import { Routes } from '@angular/router';

export const naturalPersonRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./natural-person-list/natural-person-list.component')
      .then(m => m.NaturalPersonListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./natural-person-form/natural-person-form.component')
      .then(m => m.NaturalPersonFormComponent)
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./natural-person-form/natural-person-form.component')
      .then(m => m.NaturalPersonFormComponent)
  }
];

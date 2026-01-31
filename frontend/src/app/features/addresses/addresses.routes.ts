import { Routes } from '@angular/router';

export const addressRoutes: Routes = [
  {
    path: '',
    loadComponent: () => import('./address-list/address-list.component')
      .then(m => m.AddressListComponent)
  },
  {
    path: 'new',
    loadComponent: () => import('./address-form/address-form.component')
      .then(m => m.AddressFormComponent)
  },
  {
    path: 'edit/:id',
    loadComponent: () => import('./address-form/address-form.component')
      .then(m => m.AddressFormComponent)
  }
];

export interface LegalPerson {
  id?: string;
  companyName: string;
  tradeName: string;
  taxId: string;
  foundingDate: Date | string;
  email?: string;
  phone?: string;
  addressId?: string;
  address?: any;
}

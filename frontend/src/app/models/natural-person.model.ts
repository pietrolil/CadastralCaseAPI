export interface NaturalPerson {
  id?: string;
  name: string;
  taxId: string;
  birthDate: Date | string;
  email?: string;
  phone?: string;
  addressId?: string;
  address?: any;
}

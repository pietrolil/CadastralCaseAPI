export interface Address {
  id?: string;
  postalCode: string;
  street?: string;
  number: string;
  complement?: string;
  district?: string;
  city?: string;
  state?: string;
  stateName?: string;
  ibgeCode?: string;
  areaCode?: string;
  queryViaCep?: boolean;
}

export interface Contribuyente {
  rncCedula: string;
  nombre: string;
  tipo: string;
  estatus: 'activo' | 'inactivo' | string;
}

export interface ComprobanteFiscal {
  rncCedula: string;
  NCF: string;
  monto: string;
  itbis18: string;
}

export interface ContribuyenteResumen {
  rncCedula: string;
  nombre: string;
  tipo: string;
  estatus: string;
  totalItbis: number;
  totalItbisFormateado: string;
  comprobantes: ComprobanteFiscal[];
}

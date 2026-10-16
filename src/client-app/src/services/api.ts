import type { Contribuyente, ContribuyenteResumen } from '../types/dgii.types';

const API_BASE_URL = import.meta.env.VITE_API_URL || '/api';

export async function fetchContribuyentes(signal?: AbortSignal): Promise<Contribuyente[]> {
  const response = await fetch(`${API_BASE_URL}/contribuyentes`, { signal });
  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    const detail = errorData?.detail || `Error ${response.status}: No se pudo obtener la lista de contribuyentes.`;
    throw new Error(detail);
  }
  return response.json();
}

export async function fetchContribuyenteResumen(rncCedula: string, signal?: AbortSignal): Promise<ContribuyenteResumen> {
  const response = await fetch(`${API_BASE_URL}/contribuyentes/${encodeURIComponent(rncCedula)}/resumen`, { signal });
  if (!response.ok) {
    const errorData = await response.json().catch(() => null);
    const detail = errorData?.detail || `Error ${response.status}: No se pudo obtener el resumen del contribuyente.`;
    throw new Error(detail);
  }
  return response.json();
}

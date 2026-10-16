import React from 'react';
import { Search, ChevronRight } from 'lucide-react';
import type { Contribuyente } from '../types/dgii.types';

interface Props {
  contribuyentes: Contribuyente[];
  selectedRnc: string | null;
  onSelect: (rncCedula: string) => void;
  searchQuery: string;
  onSearchChange: (query: string) => void;
  loading: boolean;
}

export const ContribuyentesList: React.FC<Props> = ({
  contribuyentes,
  selectedRnc,
  onSelect,
  searchQuery,
  onSearchChange,
  loading
}) => {
  const filtered = contribuyentes.filter(c =>
    c.nombre.toLowerCase().includes(searchQuery.toLowerCase()) ||
    c.rncCedula.includes(searchQuery)
  );

  return (
    <div className="bg-white rounded-lg border border-zinc-200 overflow-hidden flex flex-col h-full shadow-xs">
      <div className="p-3.5 border-b border-zinc-200 bg-zinc-50/70">
        <div className="flex items-center justify-between mb-2.5">
          <h2 className="text-sm font-semibold text-zinc-900 tracking-tight">
            Padrón de Contribuyentes
          </h2>
          <span className="text-[11px] font-mono font-medium px-2 py-0.5 rounded bg-zinc-200/80 text-zinc-700">
            {filtered.length} {filtered.length === 1 ? 'registro' : 'registros'}
          </span>
        </div>

        <div className="relative">
          <Search className="w-3.5 h-3.5 absolute left-3 top-1/2 -translate-y-1/2 text-zinc-400 pointer-events-none" />
          <input
            type="text"
            placeholder="Filtrar por RNC o Razón Social..."
            value={searchQuery}
            onChange={(e) => onSearchChange(e.target.value)}
            className="w-full pl-8 pr-3 py-1.5 text-xs bg-white border border-zinc-300 rounded text-zinc-900 placeholder:text-zinc-400 focus:outline-none focus:border-zinc-500 focus:ring-1 focus:ring-zinc-500 transition-colors"
          />
        </div>
      </div>

      <div className="divide-y divide-zinc-100 overflow-y-auto flex-1">
        {loading ? (
          <div className="p-8 text-center text-zinc-400">
            <div className="inline-block animate-spin w-5 h-5 border-2 border-zinc-900 border-t-transparent rounded-full mb-2"></div>
            <p className="text-xs">Consultando base de datos...</p>
          </div>
        ) : filtered.length === 0 ? (
          <div className="p-8 text-center text-zinc-500 text-xs">
            No se encontraron contribuyentes para el término ingresado.
          </div>
        ) : (
          filtered.map((contribuyente) => {
            const isSelected = selectedRnc === contribuyente.rncCedula;
            const isActive = contribuyente.estatus.toLowerCase() === 'activo';

            return (
              <button
                key={contribuyente.rncCedula}
                onClick={() => onSelect(contribuyente.rncCedula)}
                className={`w-full text-left px-3.5 py-3 transition-colors flex items-center justify-between ${
                  isSelected
                    ? 'bg-zinc-100/90 text-zinc-950 font-medium'
                    : 'hover:bg-zinc-50 text-zinc-800'
                }`}
              >
                <div className="min-w-0 pr-3">
                  <div className="flex items-center gap-2">
                    <p className={`text-xs truncate ${isSelected ? 'font-semibold text-zinc-950' : 'font-medium text-zinc-800'}`}>
                      {contribuyente.nombre}
                    </p>
                  </div>
                  <div className="flex items-center gap-2 mt-1">
                    <span className="font-mono text-[11px] text-zinc-500">
                      {contribuyente.rncCedula}
                    </span>
                    <span className="text-zinc-300">&bull;</span>
                    <span className="text-[10px] uppercase tracking-wider text-zinc-500">
                      {contribuyente.tipo}
                    </span>
                  </div>
                </div>

                <div className="flex items-center gap-2.5 shrink-0">
                  <span
                    className={`inline-block w-2 h-2 rounded-full ${
                      isActive ? 'bg-emerald-500' : 'bg-zinc-300'
                    }`}
                    title={contribuyente.estatus}
                  />
                  <ChevronRight
                    className={`w-3.5 h-3.5 text-zinc-400 transition-transform ${
                      isSelected ? 'text-zinc-900 translate-x-0.5' : ''
                    }`}
                  />
                </div>
              </button>
            );
          })
        )}
      </div>
    </div>
  );
};

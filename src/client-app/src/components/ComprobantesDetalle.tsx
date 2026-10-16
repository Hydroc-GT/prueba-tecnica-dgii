import React from 'react';
import type { ContribuyenteResumen } from '../types/dgii.types';
import { AlertCircle, RefreshCw } from 'lucide-react';

interface Props {
  resumen: ContribuyenteResumen | null;
  loading: boolean;
  error?: string | null;
  onRetry?: () => void;
}

export const ComprobantesDetalle: React.FC<Props> = ({ resumen, loading, error, onRetry }) => {
  if (loading) {
    return (
      <div className="bg-white rounded-lg border border-zinc-200 p-12 text-center text-zinc-400">
        <div className="inline-block animate-spin w-6 h-6 border-2 border-zinc-800 border-t-transparent rounded-full mb-2"></div>
        <p className="text-xs text-zinc-600">Calculando ITBIS y cargando comprobantes...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="bg-white rounded-lg border border-rose-200 p-8 text-center text-rose-900 shadow-xs">
        <div className="inline-flex items-center justify-center w-10 h-10 rounded-full bg-rose-100 text-rose-600 mb-3">
          <AlertCircle className="w-5 h-5" />
        </div>
        <p className="text-sm font-semibold text-rose-950 mb-1">No se pudo cargar el detalle del contribuyente</p>
        <p className="text-xs text-rose-600 max-w-md mx-auto mb-4">{error}</p>
        {onRetry && (
          <button
            onClick={onRetry}
            className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded bg-rose-600 hover:bg-rose-700 text-white text-xs font-medium transition-colors shadow-xs"
          >
            <RefreshCw className="w-3.5 h-3.5" />
            <span>Reintentar consulta</span>
          </button>
        )}
      </div>
    );
  }

  if (!resumen) {
    return (
      <div className="bg-white rounded-lg border border-dashed border-zinc-300 p-12 text-center text-zinc-400">
        <p className="text-sm font-medium text-zinc-700 mb-1">Ningún contribuyente seleccionado</p>
        <p className="text-xs text-zinc-500 max-w-sm mx-auto">
          Selecciona un registro del padrón para consultar el desglose de comprobantes fiscales y la liquidación acumulada de ITBIS.
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-5">
      {/* Resumen Oficial del Contribuyente y Total Liquidado */}
      <div className="bg-white rounded-lg border border-zinc-200 p-5 shadow-xs">
        <div className="flex flex-col sm:flex-row sm:items-start justify-between gap-4 pb-4 border-b border-zinc-100">
          <div>
            <h1 className="text-lg font-bold text-zinc-950 tracking-tight">
              {resumen.nombre}
            </h1>
            <div className="flex flex-wrap items-center gap-x-3 gap-y-1 mt-1 text-xs text-zinc-600">
              <span className="font-mono text-zinc-800 font-medium">
                RNC/Cédula: {resumen.rncCedula}
              </span>
              <span className="text-zinc-300">&bull;</span>
              <span className="uppercase text-[11px] text-zinc-500 font-medium">
                {resumen.tipo}
              </span>
              <span className="text-zinc-300">&bull;</span>
              <span className={`inline-flex items-center px-1.5 py-0.2 rounded text-[10px] font-medium ${
                resumen.estatus.toLowerCase() === 'activo'
                  ? 'bg-emerald-50 text-emerald-700 border border-emerald-200'
                  : 'bg-zinc-100 text-zinc-600 border border-zinc-200'
              }`}>
                {resumen.estatus}
              </span>
            </div>
          </div>

          <div className="sm:text-right bg-zinc-50 sm:bg-transparent p-3 sm:p-0 rounded border sm:border-0 border-zinc-100">
            <span className="text-[11px] font-medium text-zinc-500 uppercase tracking-wider block">
              Total ITBIS Reportado (18%)
            </span>
            <span className="text-2xl font-bold font-mono text-zinc-900 tracking-tight tabular-nums">
              {resumen.totalItbisFormateado}
            </span>
          </div>
        </div>

        <div className="grid grid-cols-2 sm:grid-cols-3 gap-3 pt-4 text-xs">
          <div>
            <span className="text-zinc-500 block text-[11px]">Comprobantes Declarados</span>
            <span className="font-semibold text-zinc-900 font-mono text-sm">
              {resumen.comprobantes.length}
            </span>
          </div>
          <div>
            <span className="text-zinc-500 block text-[11px]">Tasa Impositiva</span>
            <span className="font-semibold text-zinc-900 font-mono text-sm">
              18.00%
            </span>
          </div>
          <div className="col-span-2 sm:col-span-1">
            <span className="text-zinc-500 block text-[11px]">Tipo de Consulta</span>
            <span className="font-medium text-zinc-700 text-xs">
              Fiscal Oficial
            </span>
          </div>
        </div>
      </div>

      {/* Relación de Comprobantes Fiscales */}
      <div className="bg-white rounded-lg border border-zinc-200 overflow-hidden shadow-xs">
        <div className="px-4 py-3 border-b border-zinc-200 bg-zinc-50/70 flex items-center justify-between">
          <h2 className="text-xs font-semibold text-zinc-900 uppercase tracking-wider">
            Detalle de Comprobantes Fiscales (NCF)
          </h2>
          <span className="text-[11px] font-mono text-zinc-500">
            {resumen.comprobantes.length} transacciones
          </span>
        </div>

        {resumen.comprobantes.length === 0 ? (
          <div className="p-8 text-center text-zinc-500">
            <p className="text-xs font-medium text-zinc-700">Sin comprobantes fiscales registrados</p>
            <p className="text-[11px] text-zinc-400 mt-1">
              Este contribuyente no presenta comprobantes reportados en este período fiscal.
            </p>
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full text-left text-xs">
              <thead className="bg-zinc-50/80 text-zinc-600 uppercase font-semibold border-b border-zinc-200 text-[10px] tracking-wider">
                <tr>
                  <th className="py-2.5 px-4">NCF</th>
                  <th className="py-2.5 px-4">RNC/Cédula</th>
                  <th className="py-2.5 px-4 text-right">Monto Base</th>
                  <th className="py-2.5 px-4 text-right">ITBIS (18%)</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-zinc-100">
                {resumen.comprobantes.map((c) => (
                  <tr key={c.NCF} className="hover:bg-zinc-50/70 transition-colors">
                    <td className="py-2.5 px-4 font-mono font-medium text-zinc-900">
                      {c.NCF}
                    </td>
                    <td className="py-2.5 px-4 font-mono text-zinc-500">
                      {c.rncCedula}
                    </td>
                    <td className="py-2.5 px-4 text-right font-mono text-zinc-800 tabular-nums">
                      ${c.monto}
                    </td>
                    <td className="py-2.5 px-4 text-right font-mono font-semibold text-zinc-950 tabular-nums">
                      ${c.itbis18}
                    </td>
                  </tr>
                ))}
              </tbody>
              <tfoot className="bg-zinc-50 font-semibold border-t-2 border-zinc-200 text-zinc-950">
                <tr>
                  <td colSpan={3} className="py-3 px-4 text-right uppercase tracking-wider text-[11px] text-zinc-600">
                    Total ITBIS Liquidado:
                  </td>
                  <td className="py-3 px-4 text-right font-mono text-sm font-bold text-zinc-950 tabular-nums">
                    {resumen.totalItbisFormateado}
                  </td>
                </tr>
              </tfoot>
            </table>
          </div>
        )}
      </div>
    </div>
  );
};

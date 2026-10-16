import React, { useEffect, useState } from 'react';
import { Navbar } from './components/Navbar';
import { ContribuyentesList } from './components/ContribuyentesList';
import { ComprobantesDetalle } from './components/ComprobantesDetalle';
import { fetchContribuyentes, fetchContribuyenteResumen } from './services/api';
import type { Contribuyente, ContribuyenteResumen } from './types/dgii.types';
import { AlertCircle, RefreshCw } from 'lucide-react';

export const App: React.FC = () => {
  const [contribuyentes, setContribuyentes] = useState<Contribuyente[]>([]);
  const [selectedRnc, setSelectedRnc] = useState<string | null>(null);
  const [resumen, setResumen] = useState<ContribuyenteResumen | null>(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [loadingList, setLoadingList] = useState(true);
  const [errorList, setErrorList] = useState<string | null>(null);
  const [loadingResumen, setLoadingResumen] = useState(false);
  const [errorResumen, setErrorResumen] = useState<string | null>(null);
  const [retryListKey, setRetryListKey] = useState(0);
  const [retryResumenKey, setRetryResumenKey] = useState(0);

  // Carga inicial y recarga del padrón de contribuyentes
  useEffect(() => {
    const controller = new AbortController();

    async function executeLoad() {
      try {
        setErrorList(null);
        const data = await fetchContribuyentes(controller.signal);
        setContribuyentes(data);
        if (data.length > 0) {
          setSelectedRnc((prev) => prev ?? data[0].rncCedula);
        }
      } catch (err: unknown) {
        if (controller.signal.aborted) return;
        const message = err instanceof Error ? err.message : 'Error al conectar con la API de la DGII';
        setErrorList(message);
      } finally {
        if (!controller.signal.aborted) {
          setLoadingList(false);
        }
      }
    }

    executeLoad();

    return () => {
      controller.abort();
    };
  }, [retryListKey]);

  // Carga y consulta de comprobantes con cancelación para evitar condiciones de carrera
  useEffect(() => {
    if (!selectedRnc) return;
    const currentRnc = selectedRnc;
    const controller = new AbortController();

    async function executeFetchResumen() {
      try {
        setErrorResumen(null);
        const data = await fetchContribuyenteResumen(currentRnc, controller.signal);
        setResumen(data);
      } catch (err: unknown) {
        if (controller.signal.aborted) return;
        const message = err instanceof Error ? err.message : 'Error al obtener comprobantes del contribuyente';
        setErrorResumen(message);
        setResumen(null);
      } finally {
        if (!controller.signal.aborted) {
          setLoadingResumen(false);
        }
      }
    }

    executeFetchResumen();

    return () => {
      controller.abort();
    };
  }, [selectedRnc, retryResumenKey]);

  const handleSelectContribuyente = (rncCedula: string) => {
    if (rncCedula !== selectedRnc) {
      setSelectedRnc(rncCedula);
      setLoadingResumen(true);
      setErrorResumen(null);
    }
  };

  const handleRetryList = () => {
    setLoadingList(true);
    setRetryListKey((k) => k + 1);
  };

  const handleRetryResumen = () => {
    setLoadingResumen(true);
    setRetryResumenKey((k) => k + 1);
  };

  return (
    <div className="min-h-screen bg-zinc-100 flex flex-col font-sans text-zinc-900 selection:bg-zinc-800 selection:text-white">
      <Navbar />

      <main className="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-6">
        {errorList && (
          <div className="mb-4 p-3.5 rounded border border-rose-300 bg-rose-50 text-rose-900 flex items-center justify-between text-xs shadow-xs">
            <div className="flex items-center gap-2.5">
              <AlertCircle className="w-4 h-4 text-rose-600 shrink-0" />
              <span>{errorList}</span>
            </div>
            <button
              onClick={handleRetryList}
              className="inline-flex items-center gap-1 px-2.5 py-1 rounded bg-rose-100 hover:bg-rose-200 text-rose-800 font-medium transition-colors"
            >
              <RefreshCw className="w-3 h-3" />
              <span>Reintentar</span>
            </button>
          </div>
        )}

        <div className="grid grid-cols-1 lg:grid-cols-12 gap-6 items-start">
          <div className="lg:col-span-5 h-[calc(100vh-8.5rem)] sticky top-20">
            <ContribuyentesList
              contribuyentes={contribuyentes}
              selectedRnc={selectedRnc}
              onSelect={handleSelectContribuyente}
              searchQuery={searchQuery}
              onSearchChange={setSearchQuery}
              loading={loadingList}
            />
          </div>

          <div className="lg:col-span-7">
            <ComprobantesDetalle
              resumen={resumen}
              loading={loadingResumen}
              error={errorResumen}
              onRetry={handleRetryResumen}
            />
          </div>
        </div>
      </main>

      <footer className="border-t border-zinc-200 py-3 text-center text-[11px] text-zinc-500 bg-white">
        Dirección General de Impuestos Internos (DGII) &bull; Consulta Tributaria en Línea
      </footer>
    </div>
  );
};

export default App;

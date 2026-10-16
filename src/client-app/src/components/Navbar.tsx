import React from 'react';

export const Navbar: React.FC = () => {
  return (
    <header className="bg-white border-b border-zinc-200 text-zinc-900 sticky top-0 z-40">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16">
          <div className="flex items-center gap-3.5">
            <div className="w-9 h-9 rounded bg-blue-950 flex items-center justify-center text-white font-bold tracking-tight text-sm shadow-xs">
              DGII
            </div>
            <div>
              <div className="flex items-center gap-2">
                <span className="font-semibold text-sm tracking-tight text-zinc-900">
                  Dirección General de Impuestos Internos
                </span>
                
              </div>
              <p className="text-xs text-zinc-500">
                Consulta y Liquidación de Comprobantes Fiscales (ITBIS)
              </p>
            </div>
          </div>

          
        </div>
      </div>
    </header>
  );
};

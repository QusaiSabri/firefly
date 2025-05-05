import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router';
import './index.css';
import App from './App.tsx';
// import { StoreProvider } from '@/store';

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      {/* <StoreProvider> */}
      <App />
      {/* </StoreProvider> */}
    </BrowserRouter>
  </StrictMode>
);

import { BrowserRouter, Routes, Route } from 'react-router-dom';
import { SWRConfig } from 'swr';
import { Layout } from './components/layout/Layout';
import { HomePage } from './pages/HomePage';
import { PropertyDetailPage } from './pages/PropertyDetailPage';
import { NotFoundPage } from './pages/NotFoundPage';

const swrConfig = {
  errorRetryCount: 3,
  errorRetryInterval: 5000,
  revalidateOnFocus: false,
  revalidateOnReconnect: true,
  keepPreviousData: true,
  dedupingInterval: 2000,
};

function App() {
  return (
    <SWRConfig value={swrConfig}>
      <BrowserRouter>
        <Layout>
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/properties/:id" element={<PropertyDetailPage />} />
            <Route path="*" element={<NotFoundPage />} />
          </Routes>
        </Layout>
      </BrowserRouter>
    </SWRConfig>
  );
}

export default App;

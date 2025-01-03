import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
//import './index.css'
import "./css/bootstrap.min.css"
import "./css/site.css"
import App from './App.tsx'
import { Catalogue } from "./Catalogue.tsx"
import { BrowserRouter, Route, Routes } from 'react-router-dom'

createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <BrowserRouter>
            <Routes>
                <Route path='/' element={<App />} />
                <Route path='/catalogue' element={<Catalogue/>} />
            </Routes>
        </BrowserRouter>,
  </StrictMode>,
)

import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
//import './index.css'
import "./css/bootstrap.min.css"
import "./css/site.css"
import App from './App.tsx'
import { Catalogue } from "./books/Catalogue.tsx"
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { BookDetails, BookDelete } from "./books/BookDetails.tsx"
import { BookEdit, BookCreate } from "./books/BookEdit.tsx"
import { LoginForm } from "./LoginComponent.tsx"
import { RegisterForm } from "./RegisterComponent.tsx"
import { AuthProvider } from "./shared/AuthProvider.tsx"


createRoot(document.getElementById('root')!).render(
    <StrictMode>
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<App />} />
                    <Route path='/catalogue' element={<Catalogue />} />
                    <Route path='/detailsBook/:bookId' element={<BookDetails />} />
                    <Route path='/editBook/:bookId' element={<BookEdit />} />
                    <Route path='/deleteBook/:bookId' element={<BookDelete />} />
                    <Route path='/createBook' element={<BookCreate />} />
                    <Route path='/login' element={<LoginForm />} />
                    <Route path='/registration' element={<RegisterForm />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
  </StrictMode>,
)

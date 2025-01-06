import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
//import './index.css'
import "./css/bootstrap.min.css"
import "./css/site.css"
import App from './App.tsx'
import { Catalogue } from "./Catalogue.tsx"
import { BrowserRouter, Route, Routes } from 'react-router-dom'
import { BookDetails, BookDelete } from "./BookCrud.tsx"
import { BookEdit, BookCreate } from "./BookEdit.tsx"
import { LoginForm } from "./LoginForm.tsx"
import { RegisterForm } from "./RegisterForm.tsx"

createRoot(document.getElementById('root')!).render(
    <StrictMode>
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
        </BrowserRouter>,
  </StrictMode>,
)
